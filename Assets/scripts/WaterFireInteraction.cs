using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WaterFireInteraction : MonoBehaviour
{
    [Header("Fire")]
    public ParticleSystem fireParticles;

    [Header("Extinguishing Settings")]
    public float extinguishDuration = 5f;

    private ParticleSystem waterParticles;
    private List<ParticleSystem.Particle> particlesInside =
        new List<ParticleSystem.Particle>();

    private float contactTimer = 0f;
    private bool touchingFire = false;
    private bool fireExtinguished = false;

    public XRGrabInteractable fireExtinguisherInteractable;     // Fire Extinguisher
    public GameObject mySecondImage; //ui image

    void Awake()
    {
        waterParticles = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger()
    {
        if (fireExtinguished) return;

        int count = waterParticles.GetTriggerParticles(
            ParticleSystemTriggerEventType.Inside,
            particlesInside
        );

        if (count > 0)
        {
            touchingFire = true;
        }
    }

    void Update()
    {
        if (fireExtinguished) return;

        if (touchingFire)
        {
            contactTimer += Time.deltaTime;

            if (contactTimer >= extinguishDuration)
            {
                ExtinguishFire();
            }
        }
        else
        {
            contactTimer = 0f;
        }

        touchingFire = false;
    }

    void ExtinguishFire()
    {
        fireExtinguished = true;

        fireParticles.Stop(
            true,
            ParticleSystemStopBehavior.StopEmitting
        );
        fireParticles.gameObject.SetActive(false);

        Debug.Log("Fire Extinguished!");
    }

    public void FireExtinguisherGrabbable()
    {
        fireExtinguisherInteractable.enabled = true;
         mySecondImage.SetActive(true);  //Enable FIre UI
    }
}