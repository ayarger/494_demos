using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDistortionOnClick : MonoBehaviour {

    /* Tunables */
    public float _strength;
    public float _radius;
    public float _distance;
    public float _duration;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            SpawnWithClickPosition((int)Input.mousePosition.x, (int)Input.mousePosition.y);
        }
	}

    void SpawnWithClickPosition(int x_pixel, int y_pixel)
    {
        WarpEffect.SpawnDistortionRing(x_pixel, y_pixel, _strength, _radius, _distance, _duration);
    }
}
