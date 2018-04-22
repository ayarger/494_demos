using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start() {
		Destroy(this, 10);
		this.GetComponent<Rigidbody2D>().velocity = Vector3.up * speed;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		Killable killable = collision.gameObject.GetComponent<Killable>();
		if (killable) {
			killable.Damage(1);
			Destroy(this.gameObject);
		}
	}
}
