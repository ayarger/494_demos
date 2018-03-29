using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour {

    /* Singleton */
    static SceneTransitionController singleton;

    /* Private Data */
    SceneTransitionEffect effect;
    SceneTransitionState _current_state = SceneTransitionState.NOT_TRANSITIONING;
    static AsyncOperation loading_operation;

    /* Public Usage Interface */
    public static bool RequestSceneTransition(string scene_name, float duration_sec, SceneTransitionCallback callback, Texture fade_shape, AnimationCurve ease_curve=null, float max_fade_image_size_factor = 4.0f)
    {
        Debug.Log("Requested Transition");
        if (singleton._current_state != SceneTransitionState.NOT_TRANSITIONING)
            return false;

        singleton.effect.SetFadeShape(fade_shape);
        singleton.effect.SetMaximumSizeFactor(max_fade_image_size_factor);

        singleton._current_state = SceneTransitionState.LEAVING_SCENE;
        callback(singleton._current_state, SceneManager.GetActiveScene().name);

        singleton.StartCoroutine(_TransitionScene(scene_name, duration_sec, callback, ease_curve));
        return true;
    }

    public static float GetLoadingProgress()
    {
        return loading_operation.progress;
    }

    /* Implementation */
    private void Awake()
    {
        if(singleton != null && singleton != this)
        {
            Destroy(gameObject);
            return;
        } else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start () {
        RefreshEffectComponentOnCamera();
	}
	
    void RefreshEffectComponentOnCamera()
    {
        effect = Camera.main.GetComponent<SceneTransitionEffect>();
        if (effect == null)
            effect = Camera.main.gameObject.AddComponent<SceneTransitionEffect>();
    }

	// Update is called once per frame
	void Update () {
        if (effect == null)
        {
            RefreshEffectComponentOnCamera();
            return;
        }
    }

    static IEnumerator _TransitionScene(string scene_name, float duration_sec, SceneTransitionCallback callback, AnimationCurve ease_curve = null)
    {
        // Transition out of the current scene.
        float ending_time = Time.time + duration_sec * 0.5f;
        while(Time.time < ending_time)
        {
            float progress = Mathf.Clamp01(1.0f - (ending_time - Time.time) / (duration_sec * 0.5f));
            if (ease_curve != null)
                progress = ease_curve.Evaluate(progress);
            singleton.effect.SetProgress(progress);

            yield return null;
        }

        // Load new scene.
        singleton._current_state = SceneTransitionState.LOADING_SCENE;
        loading_operation = SceneManager.LoadSceneAsync(scene_name);
        callback(singleton._current_state, scene_name);
        float loading_start_time = Time.time;
        float minimum_loading_duration_sec = 1.0f;
        while (!loading_operation.isDone || Time.time - loading_start_time < minimum_loading_duration_sec)
        {
            singleton.effect.SetProgress(1.0f);
            yield return null;
        }

        // Transition into the current scene.
        singleton._current_state = SceneTransitionState.ENTERING_SCENE;
        callback(singleton._current_state, scene_name);
        
        ending_time = Time.time + duration_sec * 0.5f;
        while (Time.time < ending_time)
        {
            float progress = Mathf.Clamp01((ending_time - Time.time) / (duration_sec * 0.5f));
            if (ease_curve != null)
                progress = ease_curve.Evaluate(progress);
            singleton.effect.SetProgress(progress);

            yield return null;
        }

        singleton.effect.SetProgress(0.0f);
        singleton._current_state = SceneTransitionState.NOT_TRANSITIONING;
        callback(singleton._current_state, scene_name);
    }
}

/* Helpful Structures */
public enum SceneTransitionState { NOT_TRANSITIONING, LEAVING_SCENE, LOADING_SCENE, ENTERING_SCENE };
public delegate void SceneTransitionCallback(SceneTransitionState current_transition_state, string current_scene=null);
