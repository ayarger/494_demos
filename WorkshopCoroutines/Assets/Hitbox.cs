using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

	Renderer renderer;

	public float knockbackTime = .5f;
	public float knockbackSpeed = 5f;
	bool invulnerable = false;
	public float invulnTime = 1f;

	void Start() {
		renderer = this.GetComponent<Renderer>();
	}

	public void OnHit(Vector2 direction) {
		if (invulnerable)
			return;
		StartCoroutine(Knockback(direction));
		StartCoroutine(Invincibility());
	}

	IEnumerator Knockback(Vector2 direction) {
		for (float t = 0; t < knockbackTime; t += Time.deltaTime) {
			this.transform.position += (Vector3)direction * knockbackSpeed * Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}
	}


	IEnumerator Invincibility() {
		invulnerable = true;
		for (float endTime = Time.time + invulnTime; Time.time < endTime; ) {
			renderer.enabled = false;
			yield return new WaitForSeconds(.1f);
			renderer.enabled = true;
			yield return new WaitForSeconds(.1f);
		}
		invulnerable = false;
	}
}
