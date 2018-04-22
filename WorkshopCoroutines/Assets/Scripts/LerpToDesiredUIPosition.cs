/* A component for linearly interpolating (easing) a UI object to a 2D position over time. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToDesiredUIPosition : MonoBehaviour
{
    /* Inspector Tunables */
    public float ease_factor = 0.1f;

    /* Private Data */
    private RectTransform rt;

    bool hidden = false;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        ProcessControls();

        Vector2 desired_ui_position = Vector2.zero;

        if (hidden)
            desired_ui_position = new Vector2(-272, 0);

        // By jumping a particular fraction (ease_factor) of the way to our destination
        // every frame, we re-enact Zeno's Dichotomy paradox (https://youtu.be/EfqVnj-sgcc), 
        // achieving smooth movement.
        // - AY
        rt.anchoredPosition += 
            (desired_ui_position - rt.anchoredPosition) * ease_factor;
    }

    void ProcessControls()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            hidden = !hidden;
    }
}
