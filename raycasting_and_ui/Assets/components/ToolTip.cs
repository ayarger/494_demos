using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {

    Text inner_text;
    RectTransform rt;
    public Transform target;
    public Vector3 offset = Vector3.zero;

	// Use this for initialization
	void Start () {
        if(target == null)
        {
            Debug.Log("Error: Null target.");
            Destroy(gameObject);
        }

        rt = GetComponent<RectTransform>();
        inner_text = GetComponentInChildren<Text>();
        inner_text.text = target.gameObject.name;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);

        Vector2 desired_pos = new Vector2(screenPos.x, screenPos.y);
        rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, desired_pos, 0.1f);
    }
}
