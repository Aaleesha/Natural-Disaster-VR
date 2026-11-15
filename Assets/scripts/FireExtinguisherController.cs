
using UnityEngine;

public class FireExtinguisherController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag your Particle System component here")]
    public ParticleSystem waterParticleSystem;

    [Tooltip("The Presser/Handle transform from your hierarchy")]
    public Transform presserTransform;

    [Tooltip("Drag the extinguisher spraying Audio Source here")]
    public AudioSource spraySound;

    [Header("States")]
    private bool isGrabbed = false;
    private bool isPinRemoved = true;
    private bool isPressed = false;

    void Start()
    {
        // Ensure water and sound start stopped
        if (waterParticleSystem != null)
        {
            waterParticleSystem.Stop();
        }

        if (spraySound != null)
        {
            spraySound.Stop();
        }
    }

    public void OnGrabbed()
    {
        isGrabbed = true;
    }

    public void OnReleased()
    {
        isGrabbed = false;
        StopWaterFlow();
    }

    public void SetPresserActive(bool pressed)
    {
        isPressed = pressed;
        EvaluateWaterFlow();
    }

    public void RemovePin()
    {
        isPinRemoved = true;
        EvaluateWaterFlow();
    }

    void EvaluateWaterFlow()
    {
        // Water and sound work only when all conditions are met
        if (isGrabbed && isPinRemoved && isPressed)
        {
            if (waterParticleSystem != null && !waterParticleSystem.isPlaying)
            {
                waterParticleSystem.Play();
            }

            if (spraySound != null && !spraySound.isPlaying)
            {
                spraySound.Play();
            }
        }
        else
        {
            StopWaterFlow();
        }
    }

    void StopWaterFlow()
    {
        if (waterParticleSystem != null && waterParticleSystem.isPlaying)
        {
            waterParticleSystem.Stop();
        }

        if (spraySound != null && spraySound.isPlaying)
        {
            spraySound.Stop();
        }
    }
}
