using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRumble : MonoBehaviour {

	public float rumbleTime = .2f;
	public float rumbleDist = 1;

	public void Rumble() {
		StartCoroutine(RunRumble());
	}

	IEnumerator RunRumble () {
		Vector3 start = this.transform.position;
		for (float t = 0; t < rumbleTime; t += Time.deltaTime) {
			float p = 1 - t / rumbleTime;
			this.transform.position = start + Random.onUnitSphere * rumbleDist * p;
			yield return null;
		}
		this.transform.position = start;
	}
}
