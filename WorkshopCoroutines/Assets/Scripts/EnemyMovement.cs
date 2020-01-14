using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
	public float moveSpeed = 2f;
	Rigidbody2D rb;

	Vector2[] directions = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

	Coroutine currentMove;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D>();
		currentMove = StartCoroutine(Move());
	}
	
	IEnumerator Move() {
		while (true) {
			Vector2 dir = directions[Random.Range(0, directions.Length)];
			for (float moved = 0; moved < 1; moved += moveSpeed * Time.deltaTime) {
				rb.velocity = dir * moveSpeed;
				yield return null;
			}
		}
	}

	void OnCollisionStay2D(Collision2D collision) {
		StopCoroutine(currentMove);
		currentMove = StartCoroutine(Move());
	}
}
