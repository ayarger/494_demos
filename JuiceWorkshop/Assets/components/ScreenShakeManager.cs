using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    static ScreenShakeManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void Perturb()
    {
        instance.transform.localPosition = UnityEngine.Random.onUnitSphere * instance.amplitude;
    }

    public float amplitude = 0.5f;
    public float k = 0.3f;
    public float dampening_factor = 0.95f;
    Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        Vector3 displacement = Vector3.zero - transform.localPosition;
        Vector3 acceleration = k * displacement;
        velocity += acceleration;
        velocity *= dampening_factor;

        transform.localPosition += velocity;

        Debug();
    }

    void Debug()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Perturb();
        }
    }
}
