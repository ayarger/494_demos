using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
	public float moveTime = 2;
	public float distance = 3;
	public bool startRight = true;

	void Start() {
		StartCoroutine(Move());
	}


	IEnumerator Move() {
		Vector3 a = this.transform.position + Vector3.left * distance;
		Vector3 b = this.transform.position + Vector3.right * distance;

		if (!startRight) {
			Vector3 tmp = a;
			a = b;
			b = tmp;
		}
		while (true) {
			for (float t = 0; t < moveTime; t += Time.deltaTime) {
				this.transform.position = Vector3.Lerp(a, b, t / moveTime);
				yield return null;
			}
			for (float t = 0; t < moveTime; t += Time.deltaTime) {
				this.transform.position = Vector3.Lerp(b, a, t / moveTime);
				yield return null;
			}
		}
	}
}
