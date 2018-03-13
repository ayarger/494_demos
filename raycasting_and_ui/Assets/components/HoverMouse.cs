using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMouse : MonoBehaviour {

    Transform hovered_object = null;
    Color stored_color = Color.red;

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 50.0f))
        {

            if(hit.collider.gameObject.name == "tiled_cube")
            {
                if(hovered_object != null)
                {
                    hovered_object.GetComponent<Renderer>().material.color = stored_color;
                }

                hovered_object = hit.collider.transform;
                stored_color = hit.collider.GetComponent<Renderer>().material.color;
                hovered_object.GetComponent<Renderer>().material.color = Color.black;
            }
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
