using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Camera))]
public class HeatDistortionEffect : MonoBehaviour
{
    [Header("To config the heat effect, alter the HeatDistortionEffect material.")]
    public bool dummy;

    /* Private Data */
    Material mat;

    private void Awake()
    {
        // Load the material that uses our WarpEffect Shader.
        mat = Resources.Load<Material>("HeatDistortionEffect");
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Apply the shader to the final render that appears
        // on your screen.
        Graphics.Blit(source, destination, mat);
    }
}
