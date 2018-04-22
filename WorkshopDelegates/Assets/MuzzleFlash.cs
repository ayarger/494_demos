using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {

	public float flashTime = .1f;

	Light myLight;

	float maxIntensity;

	void Awake() {
		myLight = this.GetComponent<Light>();
		maxIntensity = myLight.intensity;
		myLight.intensity = 0;
	}

	public void Flash() {
		StartCoroutine(RunFlash());
	}

	IEnumerator RunFlash() {
		for (float t = 0; t <= flashTime; t+= Time.deltaTime) {
			float p = 1 - t / flashTime;
			myLight.intensity = p * maxIntensity;
			yield return null;
		}
		myLight.intensity = 0;
	}
}
