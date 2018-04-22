using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rupee : MonoBehaviour, ICollectable {

	public void TryCollect(GameObject other) {
		RupeeHolder isRupeeHolder = other.GetComponent<RupeeHolder>();
		if (isRupeeHolder) {
			isRupeeHolder.rupees += 1;
			Destroy(this.gameObject);
		}
	}

}
