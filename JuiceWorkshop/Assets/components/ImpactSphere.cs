using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSphere : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 1.0f)
        {
            GetComponent<GenerateView>().GetCurrentView().GetComponent<HookeScale>().Perturb();
            ScreenShakeManager.Perturb();
            ParticleSystemManager.RequestParticlesAtPositionAndDirection(collision.contacts[0].point, collision.contacts[0].normal);
        }
    }
}
