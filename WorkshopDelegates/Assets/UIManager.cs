using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[SerializeField] Text pointsText;

	public void UpdatePointsText(int amount) {
		pointsText.text = "Fired: " + amount;
	}

}
