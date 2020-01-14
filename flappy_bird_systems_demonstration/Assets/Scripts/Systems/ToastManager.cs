/* ToastManager.cs
 * 
 * This file houses the Toast system-- a singleton system responsible for implementing little pop-up "toast" messages for the user.
 * 
 * The Toast system is made accessible throughout the codebase via one public static method-- "Toast()".
 * Unfortunately, this fact couples other systems to the existence of the Toast system (but it sure is convenient in a small project!)
 * A superior solution might be to create a global "request-pool", where individual systems can emit all of their requests to (without regard to who's listening for them).
 * With this approach, the Toast system could simply listen for "toast" requests appearing in the request-pool, and execute toasts when necessary.
 * The benefit-- no system would need to know about the Toast system's existence or how it works, and the Toast system wouldn't care about other systems either! Isolation! Independence!
 * Perhaps this may be experimented with in a larger project.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastManager : MonoBehaviour {

    // Singleton static instance variable. When one ToastManager "claims" this variable, all the others go away.
    static ToastManager instance;

    // The two places the toast UI panel alternates between.
    Vector3 hidden_pos;
    Vector3 visible_pos;

    public RectTransform toast_panel;
    public Text toast_text;

    // These inspector-accessible variables control how the toast UI panel moves between the hidden and visible positions.
    public AnimationCurve ease;
    public AnimationCurve ease_out;

    // Duration controls.
    public float ease_duration = 0.5f;
    public float show_duration = 2.0f;

    // We don't want to discard toast requests that come in while we are already toasting. What if the message is critical?
    // The queue keeps a rolling data store of work we still need to do.
    Queue<ToastRequest> requests = new Queue<ToastRequest>();

    // Use this for initialization
    void Awake()
    {
        // Typical singleton initialization code.
        if(instance != null && instance != this)
        {
            // If there already exists a ToastManager, we need to go away.
            Destroy(gameObject);
            return;
        } else
        {
            // If we are the first ToastManager, we claim the "instance" variable so others go away.
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Init positions
        hidden_pos = new Vector3(0, 60, 0);
        visible_pos = new Vector3(0, -30, 0);
    }

    // "public static" makes this function accessible from anywhere.
    // note that it does not actually launch a toast operation-- it just throws it on the queue for later execution.
    public static void Toast(string msg)
    {
        instance.requests.Enqueue(new ToastRequest(msg));
    }

    // The Update function is responsible for monitoring the queue and executing requests
    void Update()
    {
        // If a request exists on the queue, and we're not busy servicing an earlier request, we service the next one on the queue.
        if (!toasting && requests.Count > 0)
        {
            ToastRequest new_request = requests.Dequeue();
            toasting = true;

            instance.toast_text.text = new_request.message;
            instance.StartCoroutine(DoToast(instance.ease_duration, instance.show_duration));
        }
    }

    static IEnumerator DoToast(float duration_ease_sec, float duration_show_sec)
    {
        // Ease In the UI panel
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / duration_ease_sec;

        while(progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_ease_sec;
            float eased_progress = instance.ease.Evaluate(progress);
            instance.toast_panel.anchoredPosition = Vector3.LerpUnclamped(instance.hidden_pos, instance.visible_pos, eased_progress);

            yield return null;
        }

        // Show the UI Panel for "duration_show_sec" seconds.
        yield return new WaitForSeconds(duration_show_sec);

        // Ease Out the UI panel
        initial_time = Time.time;
        progress = 0.0f;
        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_ease_sec;
            float eased_progress = instance.ease_out.Evaluate(progress);
            instance.toast_panel.anchoredPosition = Vector3.LerpUnclamped(instance.hidden_pos, instance.visible_pos, 1.0f - eased_progress);

            yield return null;
        }

        // When we're done toasting, we tell the "Update" function that we're ready for more requests.
        instance.toasting = false;
    }

    bool toasting = false;
}

// A simple data structure for holding information about a toast request.
// This could be expanded to store additional request parameters like sound effects, colors, end-of-toast callbacks, etc.
public class ToastRequest
{
    public string message;

    public ToastRequest(string s)
    {
        message = s;
    }
}
