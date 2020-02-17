using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookeScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float k = 0.3f;
    public float dampening_factor = 0.95f;
    float velocity = 0.0f;

    // Update is called once per frame
    void Update()
    {
        float x = 1.0f - transform.localScale.x;
        float acceleration = k * x;
        velocity += acceleration;
        velocity *= dampening_factor;

        transform.localScale += Vector3.one * velocity;

        Debug();
    }

    void Debug()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Perturb();
        }
    }

    public void Perturb()
    {
        transform.localScale = Vector3.one * 0.75f;
    }
}
