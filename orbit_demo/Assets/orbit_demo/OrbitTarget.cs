using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitTarget : MonoBehaviour {

	public Transform target;
	public float rate = 4.0f;
	public float radius = 4.0f;
	
	// Update is called once per frame
	void Update () {
		transform.position = target.position + new Vector3 (Mathf.Cos (Time.time * rate), Mathf.Sin (Time.time * rate), 0) * radius;
	}
}
