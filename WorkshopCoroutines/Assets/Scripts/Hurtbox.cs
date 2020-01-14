using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		Hitbox hitbox = other.gameObject.GetComponent<Hitbox>();
		if (hitbox) {
			Vector2 direction = (other.transform.position - this.transform.position).normalized;
			hitbox.OnHit(direction);
		}
	}
}
