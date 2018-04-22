using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour {

	// This uses the builtin UnityAction delegate, but you could define your own like this
	//public delegate void OnInput();
	//public OnFire onInput;

	// Set it equal to delegate { } to ensure that even with no added functions, this won't throw an error.
	// Add event to make sure other classes can only add and remove to the list, not reset it
	public event UnityAction onInput = delegate { };

	void Start() {
		Gun gun = this.GetComponentInChildren<Gun>();
		if (gun)
			onInput += gun.Fire;
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			onInput();
		}
	}
}
