using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Camera))]
[DisallowMultipleComponent]
public class SceneTransitionEffect : MonoBehaviour {

    /* Private Data */
    Material mat;
    float progress = 0.0f;
    Vector2 center_position = Vector2.zero;
    Texture fade_shape;
    float texture_size_factor = 4.0f;
    SceneTransitionEffectMode mode = SceneTransitionEffectMode.CIRCLE;
    float maximum_size_factor = 4.0f; // Controls how large the fade_image gets when disappearing. Tweak to make different images work.

    /* Interface / Usage */
    private void Start()
    {
        // Load the material that uses our SceneTransition Shader.
        SetModeMaterial(mode);
        center_position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        progress = 0.0f;
    }

    Material GetMaterial()
    {
        string material_name = "SceneTransitionEffect";
        if (mode == SceneTransitionEffectMode.IMAGE)
            material_name = "SceneTransitionEffectImage";

        return Resources.Load<Material>(material_name);
    }

    public void SetProgress(float p)
    {
        progress = p;
    }

    public void SetMaximumSizeFactor(float msf)
    {
        maximum_size_factor = msf;
    }

    void SetModeMaterial(SceneTransitionEffectMode m)
    {
        mode = m;
        mat = GetMaterial();
    }

    public void SetFadeShape(Texture t)
    {
        fade_shape = t;
        if(fade_shape != null)
            SetModeMaterial(SceneTransitionEffectMode.IMAGE);
        else
            SetModeMaterial(SceneTransitionEffectMode.CIRCLE);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Send necessary data to shader.
        mat.SetTexture("_Symbol", fade_shape);
        mat.SetFloat("_Progress", Mathf.Clamp01(1.0f - progress));
        mat.SetFloat("_x_pixel_center", center_position.x);
        mat.SetFloat("_y_pixel_center", center_position.y);
        mat.SetInt("_screen_resolution_x", Screen.width);
        mat.SetInt("_screen_resolution_y", Screen.height);
        mat.SetFloat("maximum_size_factor", maximum_size_factor);

        // Apply the shader to the final render that appears
        // on your screen.
        Graphics.Blit(source, destination, mat);
    }
}

public enum SceneTransitionEffectMode { CIRCLE, IMAGE };