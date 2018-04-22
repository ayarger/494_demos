using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	Rigidbody2D rigid;

	[Header("Horizontal Properties")]
	public float maxSpeed = 5f;
	public float acceleration = 35f;
	public AnimationCurve accelerationMultiplierByDist;

	[Header("Vertical Properties")]
	public float jumpTime = .5f;
	public float jumpHeight = 2.5f;
	public float maxFallSpeed = 5f;
	public AnimationCurve gravityModifierBySpeed;
	public float variableJumpCutoffSpeed = 1f;

	[Header("Misc Properties")]
	public float groundedSlope = .75f;

	public float maxGroundedBuffer = .2f;
	float groundedBuffer = 0f;

	public float maxJumpBuffer = .2f;
	float jumpBuffer = 0;


	void Awake() {
		rigid = this.GetComponent<Rigidbody2D>();
	}

	void Update() {
		if (Input.GetButtonDown("Jump")) {
			jumpBuffer = maxJumpBuffer;
		}
	}

	void FixedUpdate() {
		Vector3 velocity = rigid.velocity;

		// Horizontal
		float xInput = Input.GetAxisRaw("Horizontal");
		float targetSpeed = xInput * maxSpeed;
		float xDiff = targetSpeed - velocity.x;
		float thisAcceleration = acceleration * accelerationMultiplierByDist.Evaluate(Mathf.Abs(xDiff / maxSpeed));
		float xStep = Mathf.Sign(xDiff) *
			Mathf.Min(Mathf.Abs(xDiff), thisAcceleration * Time.deltaTime);
		velocity.x += xStep;

		// Gravity
		float jumpPower = 2 * jumpHeight / jumpTime;
		float gravity = -2 * jumpHeight / (jumpTime * jumpTime);
		gravity *= gravityModifierBySpeed.Evaluate(velocity.y / jumpPower);
		if (!Input.GetButton("Jump") && velocity.y > variableJumpCutoffSpeed) {
			gravity *= 5;
		}
		velocity.y += gravity * Time.deltaTime;
		velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);

		// Jump
		if (jumpBuffer > 0 && groundedBuffer > 0) {
			velocity.y = jumpPower;
			jumpBuffer = 0;
			groundedBuffer = 0;
		}

		rigid.velocity = velocity;

		jumpBuffer -= Time.deltaTime;
		groundedBuffer -= Time.deltaTime;
	}

	void OnCollisionStay2D(Collision2D collision) {
		if (Vector3.Dot(collision.contacts[0].normal, Vector3.up) > groundedSlope && rigid.velocity.y <= 0) {
			groundedBuffer = maxGroundedBuffer;
		}
	}
}
