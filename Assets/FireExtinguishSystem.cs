
using UnityEngine;
using System.Collections.Generic;

public class FireExtinguishSystem : MonoBehaviour
{
    [Header("Fire Components")]
    public ParticleSystem fireParticles;
    public ParticleSystem smokeParticles;
    public Light fireLight;

    [Header("Settings")]
    public float extinguishDuration = 5f;

    private ParticleSystem sprayParticles;
    private float extinguishTimer;
    private bool fireExtinguished;

    private readonly List<ParticleSystem.Particle> insideParticles =
        new List<ParticleSystem.Particle>();

    private bool sprayInsideFire;

    public float ExtinguishProgress =>
        Mathf.Clamp01(extinguishTimer / extinguishDuration);

    void Awake()
    {
        sprayParticles = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger()
    {
        if (fireExtinguished || sprayParticles == null)
            return;

        int count = sprayParticles.GetTriggerParticles(
            ParticleSystemTriggerEventType.Inside,
            insideParticles
        );

        sprayInsideFire = count > 0;
    }

    void Update()
    {
        if (fireExtinguished)
            return;

        if (sprayInsideFire)
        {
            extinguishTimer += Time.deltaTime;

            if (extinguishTimer >= extinguishDuration)
            {
                ExtinguishFire();
            }
        }
        else
        {
            extinguishTimer = 0f;
        }
    }

    public void ExtinguishFire()
    {
        if (fireExtinguished)
            return;

        fireExtinguished = true;

        if (fireParticles != null)
            fireParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (smokeParticles != null)
            smokeParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (fireLight != null)
            fireLight.enabled = false;

        Debug.Log("Fire extinguished successfully!");
    }
}
