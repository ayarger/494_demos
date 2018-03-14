using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Camera))]
public class WarpEffect : MonoBehaviour {

    /* Private Data */
    Material mat;
    public List<DistortionRing> distortion_rings = new List<DistortionRing>();
    const int MAX_NUM_RINGS = 146;
    const int NUM_DATA_MEMBERS_PER_RING = 7;

    /* Interface / Usage */
    public void SpawnDistortionRing(float x_pixel, float y_pixel, float strength, float ring_width, float travel_distance, float duration_sec)
    {
        distortion_rings.Add(new DistortionRing(x_pixel, y_pixel, strength, ring_width, travel_distance, duration_sec));
    }

    private void Awake()
    {
        // Load the material that uses our WarpEffect Shader.
        mat = Resources.Load<Material>("WarpEffect");
    }

    /* Responsible for serializing DistortionRing objects into a float array */
    /* Shaders cannot accept object arrays to my knowledge */
    float[] GetDistortionRingData()
    {
        float[] result = new float[MAX_NUM_RINGS * NUM_DATA_MEMBERS_PER_RING];

        for(int i = 0; i < MAX_NUM_RINGS * NUM_DATA_MEMBERS_PER_RING; i++)
        {
            result[i] = 0.0f;
        }

        for(int i = 0; i < distortion_rings.Count; i ++)
        {
            DistortionRing ring = distortion_rings[i];
            result[i * NUM_DATA_MEMBERS_PER_RING] = ring.x;
            result[i * NUM_DATA_MEMBERS_PER_RING + 1] = ring.y;
            result[i * NUM_DATA_MEMBERS_PER_RING + 2] = ring.strength;
            result[i * NUM_DATA_MEMBERS_PER_RING + 3] = ring.radius;
            result[i * NUM_DATA_MEMBERS_PER_RING + 4] = ring.spawn_time;
            result[i * NUM_DATA_MEMBERS_PER_RING + 5] = ring.distance;
            result[i * NUM_DATA_MEMBERS_PER_RING + 6] = ring.duration_sec;
        }

        return result;
    }

    private void Update()
    {
        // Remove Finished Rings
        for(int i = 0; i < distortion_rings.Count; i++)
        {
            DistortionRing d = distortion_rings[i];
            if(Time.time - d.spawn_time > d.duration_sec)
            {
                distortion_rings.RemoveAt(i);
                i--;
            }
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Obtain data required by the shader.
        float[] data = GetDistortionRingData();
        Debug.Assert(data.Length % NUM_DATA_MEMBERS_PER_RING == 0);

        // Send necessary data to shader.
        mat.SetInt("_number_data_members", NUM_DATA_MEMBERS_PER_RING);
        mat.SetInt("_NumDistortionRings", distortion_rings.Count);
        mat.SetFloat("_current_time", Time.time);
        mat.SetInt("_screen_resolution_x", Screen.width);
        mat.SetInt("_screen_resolution_y", Screen.height);
        mat.SetFloatArray("_DistortionRings", data);

        // Apply the shader to the final render that appears
        // on your screen.
        Graphics.Blit(source, destination, mat);
    }

    [System.Serializable]
    public class DistortionRing
    {
        public float x;
        public float y;
        public float strength;
        public float radius;
        public float distance;
        public float duration_sec;
        public float spawn_time;

        public DistortionRing(float x, float y, float strength, float radius, float distance, float duration_sec)
        {
            this.x = x;
            this.y = y;
            this.strength = strength;
            this.radius = radius;
            this.spawn_time = Time.time;
            this.distance = distance;
            this.duration_sec = duration_sec;
        }
    }
}
