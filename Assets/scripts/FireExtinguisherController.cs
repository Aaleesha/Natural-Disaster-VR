using UnityEngine;

public class FireExtinguisherController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag your Particle System component here")]
    public ParticleSystem waterParticleSystem;

    [Tooltip("The Presser/Handle transform from your hierarchy")]
    public Transform presserTransform;

    [Header("States")]
    private bool isGrabbed = false;
    private bool isPinRemoved = true;  // Set to false if you want a pin-pulling mechanic first
    private bool isPressed = false;

    void Start()
    {
        // Ensure water starts stopped
        if (waterParticleSystem != null)
        {
            waterParticleSystem.Stop();
        }
    }

    // Hook this up to XR Grab Interactable -> OnSelectEntered (Event)
    public void OnGrabbed()
    {
        isGrabbed = true;
    }

    // Hook this up to XR Grab Interactable -> OnSelectExited (Event)
    public void OnReleased()
    {
        isGrabbed = false;
        StopWaterFlow();
    }

    // Call this method when the user presses the interaction button for the handle
    public void SetPresserActive(bool pressed)
    {
        isPressed = pressed;
        EvaluateWaterFlow();
    }

    // Call this if you implement a mechanic to pull the safety pin
    public void RemovePin()
    {
        isPinRemoved = true;
        EvaluateWaterFlow();
    }

    void EvaluateWaterFlow()
    {
        // Water flows only if grabbed, pin is out, and handle is pressed
        if (isGrabbed && isPinRemoved && isPressed)
        {
            if (waterParticleSystem != null && !waterParticleSystem.isPlaying)
            {
                waterParticleSystem.Play();
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
    }
}