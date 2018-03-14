using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDistortionOnCollision: MonoBehaviour
{
    /* Tunables */
    public float _radius;
    public float _distance;
    public float _duration;

    /* Private Data */
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void SpawnWithWorldPositionAndStrength(Vector3 pos, float strength)
    {
        WarpEffect we = Camera.main.GetComponent<WarpEffect>();
        if (we == null)
            return;

        Vector3 screen_point = Camera.main.WorldToScreenPoint(pos);

        we.SpawnDistortionRing(screen_point.x, screen_point.y, strength, _radius, _distance, _duration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        float velocity_at_collision = collision.relativeVelocity.magnitude;
        SpawnWithWorldPositionAndStrength(collision.contacts[0].point, velocity_at_collision * 0.5f);
    }
}
