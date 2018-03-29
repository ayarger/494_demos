using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OutlineEffect : MonoBehaviour {

    /* Private Data */
    Material mat;
  
    private void Awake()
    {
        // Load the material that uses our WarpEffect Shader.
        mat = Resources.Load<Material>("GameobjectOutline");
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Obtain data required by the shader.
        /*float[] data = GetDistortionRingData();
        Debug.Assert(data.Length % NUM_DATA_MEMBERS_PER_RING == 0);

        // Send necessary data to shader.
        mat.SetInt("_number_data_members", NUM_DATA_MEMBERS_PER_RING);
        mat.SetInt("_NumDistortionRings", distortion_rings.Count);
        mat.SetFloat("_current_time", Time.time);
        mat.SetInt("_screen_resolution_x", Screen.width);
        mat.SetInt("_screen_resolution_y", Screen.height);
        mat.SetFloatArray("_DistortionRings", data);*/

        // Apply the shader to the final render that appears
        // on your screen.
        Graphics.Blit(source, destination, mat);
    }
}
