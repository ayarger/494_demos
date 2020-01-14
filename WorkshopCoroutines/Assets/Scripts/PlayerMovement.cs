using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	Rigidbody2D rb;

	public float horizontalSpeed = 5f;
	public float jumpPower = 15f;
	public float ascentAcc;

	bool grounded = false;

	void Awake() {
		rb = this.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
		Vector2 newVelocity = rb.velocity;

		float horInput = Input.GetAxisRaw("Horizontal");

		newVelocity.x = horInput * horizontalSpeed;

		if (horInput > 0)
			this.transform.localScale = new Vector3(1, 1, 1);
		if (horInput < 0)
			this.transform.localScale = new Vector3(-1, 1, 1);

		if (Input.GetKeyDown(KeyCode.Z) && grounded) {
			newVelocity.y = jumpPower;
			grounded = false;
			StartCoroutine(WatchJump());
		}

		rb.velocity = newVelocity;
	}

	IEnumerator WatchJump() {
		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();
		while (rb.velocity.y > 0 && grounded == false) {
			rb.velocity -= Vector2.up * ascentAcc * Time.deltaTime;
			if (!Input.GetKey(KeyCode.Z)) {
				rb.velocity = new Vector2(rb.velocity.x, 0);
			}
			yield return null;
		}
	}

	void FixedUpdate() {
		grounded = false;
	}

	void OnCollisionStay2D(Collision2D collision) {
		if (Vector2.Dot(collision.contacts[0].normal, Vector2.up) > .95f) {
			grounded = true;
		}
	}
}
