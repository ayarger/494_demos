using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

	int points;

	public delegate void OnPointAdd(int points);
	public event OnPointAdd onPointAdd = delegate{};

	void Start() {
		UIManager uiManager = FindObjectOfType<UIManager>();
		if (uiManager)
			onPointAdd += uiManager.UpdatePointsText;
	}

	public void AddPoint() {
		points += 1;
		onPointAdd(points);
	}


}
