using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPickup : MonoBehaviour, ICollectable {

	public GameObject arrowPrefab;

	public void TryCollect(GameObject other) {
		BowOwner isBowOwner = other.GetComponent<BowOwner>();
		if (!isBowOwner) {
			other.AddComponent<BowOwner>().arrowPrefab = arrowPrefab;
			Destroy(this.gameObject);
		}
	}

}
