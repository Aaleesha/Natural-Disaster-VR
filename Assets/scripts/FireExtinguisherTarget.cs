using UnityEngine;

public class FireExtinguisherTarget : MonoBehaviour
{
    [Header("Fire Particle Systems")]
    public ParticleSystem[] fireParticles;

    [Header("Fire Settings")]
    public GameObject fireVisuals;

    private bool isExtinguished = false;

    public bool IsExtinguished()
    {
        return isExtinguished;
    }

    public void ExtinguishFire()
    {
        if (isExtinguished)
            return;

        isExtinguished = true;

        // Stop all fire particle systems
        foreach (ParticleSystem fire in fireParticles)
        {
            if (fire != null)
            {
                fire.Stop(
                    true,
                    ParticleSystemStopBehavior.StopEmittingAndClear
                );
            }
        }

        // Hide remaining fire visuals
        if (fireVisuals != null)
        {
            fireVisuals.SetActive(false);
        }

        Debug.Log("FIRE COMPLETELY EXTINGUISHED!");
    }
}