using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour, ICollectable {

	public void TryCollect(GameObject other) {
		Killable isKillable = other.GetComponent<Killable>();
		if (isKillable) {
			isKillable.health += 1;
			Destroy(this.gameObject);
		}
	}

}
