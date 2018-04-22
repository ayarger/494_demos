using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public float speed;

	Rigidbody2D rb;

	public GameObject arrow;

	private void Awake() {
		rb = this.GetComponent<Rigidbody2D>();
	}

	void Update() {
		rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;
	}

	void OnTriggerEnter2D(Collider2D other) {
	}
}
