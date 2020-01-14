using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	public GameObject sword;

	bool canAttack = true;

	void Start() {
		sword.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.X) && canAttack) {
			StartCoroutine(Attack());
		}
	}

	IEnumerator Attack() {
		canAttack = false;
		sword.SetActive(true);
		yield return new WaitForSeconds(.3f);
		sword.SetActive(false);
		yield return new WaitForSeconds(.3f);
		canAttack = true;
	}
}
