using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    static ParticleSystemManager instance;

    ParticleSystem ps;

    static float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        instance = this;
    }

    public static void RequestParticlesAtPositionAndDirection(Vector3 pos, Vector3 normal)
    {
        instance.transform.position = pos;
        instance.transform.forward = normal;
        instance.ps.Play();
        timer = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0.0f && ps.isPlaying)
        {
            ps.Stop();
        }
    }
}
