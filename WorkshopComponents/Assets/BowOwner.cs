using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowOwner : MonoBehaviour {

	public GameObject arrowPrefab;

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Instantiate(arrowPrefab, this.transform.position + Vector3.up, Quaternion.identity);
		}
	}
}
