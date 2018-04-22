using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		ICollectable collectable = other.GetComponent<ICollectable>();
		if (collectable != null) {
			collectable.TryCollect(this.gameObject);
		}
	}
}
