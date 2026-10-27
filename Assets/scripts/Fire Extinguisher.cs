using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRFireExtinguisher : MonoBehaviour
{
    [Header("Grab Interactables")]
    [SerializeField] private XRGrabInteractable extinguisherBody;
    [SerializeField] private XRGrabInteractable nozzleGrab;
    [SerializeField] private XRGrabInteractable safetyPinGrab;

    [Header("Controller References")]
    [SerializeField] private Transform leftController;
    [SerializeField] private Transform rightController;

    [Header("Analog Trigger Actions")]
    [SerializeField] private InputActionReference leftTriggerAction;
    [SerializeField] private InputActionReference rightTriggerAction;

    [Header("Safety Pin")]
    [SerializeField, Min(0.01f)]
    private float pinPullDistance = 0.12f;

    [SerializeField] private bool hidePinAfterRemoval = true;

    [Header("Spray")]
    [SerializeField] private ParticleSystem sprayParticles;
    [SerializeField, Min(0f)] private float maxEmissionRate = 250f;
    [SerializeField, Range(0f, 1f)] private float minimumTriggerToSpray = 0.05f;

    [Header("Optional Spray Feedback")]
    [SerializeField] private AudioSource sprayAudio;
    [SerializeField] private Light sprayIndicatorLight;
    [SerializeField, Range(0f, 1f)] private float minimumLightIntensity = 0.1f;
    [SerializeField] private float maximumLightIntensity = 2f;

    public bool IsPinRemoved { get; private set; }
    public float CurrentPressure { get; private set; }

    private Vector3 initialPinPosition;
    private bool pinPositionRecorded;

    private void Awake()
    {
        if (safetyPinGrab != null)
        {
            initialPinPosition = safetyPinGrab.transform.position;
            pinPositionRecorded = true;
        }

        StopSpray();
    }

    private void OnEnable()
    {
        if (leftTriggerAction != null)
            leftTriggerAction.action.Enable();

        if (rightTriggerAction != null)
            rightTriggerAction.action.Enable();
    }

    private void OnDisable()
    {
        StopSpray();

        if (leftTriggerAction != null)
            leftTriggerAction.action.Disable();

        if (rightTriggerAction != null)
            rightTriggerAction.action.Disable();
    }

    private void Update()
    {
        CheckSafetyPin();

        // Every condition must be satisfied before spraying.
        if (IsPinRemoved &&
            extinguisherBody != null &&
            extinguisherBody.isSelected &&
            nozzleGrab != null &&
            nozzleGrab.isSelected &&
            AreHeldByOppositeHands())
        {
            UpdateSpray();
        }
        else
        {
            StopSpray();
        }
    }

    private void CheckSafetyPin()
    {
        if (IsPinRemoved || safetyPinGrab == null)
            return;

        if (!safetyPinGrab.isSelected || !pinPositionRecorded)
            return;

        float distance = Vector3.Distance(
            safetyPinGrab.transform.position,
            initialPinPosition
        );

        if (distance >= pinPullDistance)
        {
            IsPinRemoved = true;

            Debug.Log("Safety pin removed. Extinguisher armed.");

            if (hidePinAfterRemoval)
            {
                safetyPinGrab.gameObject.SetActive(false);
            }
        }
    }

    private bool AreHeldByOppositeHands()
    {
        if (extinguisherBody.firstInteractorSelecting == null ||
            nozzleGrab.firstInteractorSelecting == null)
        {
            return false;
        }

        Transform bodyInteractor =
            extinguisherBody.firstInteractorSelecting.transform;

        Transform nozzleInteractor =
            nozzleGrab.firstInteractorSelecting.transform;

        int bodyHand = GetHand(bodyInteractor);
        int nozzleHand = GetHand(nozzleInteractor);

        // 0 = left, 1 = right, -1 = unknown.
        return bodyHand >= 0 &&
               nozzleHand >= 0 &&
               bodyHand != nozzleHand;
    }

    private int GetHand(Transform interactor)
    {
        if (leftController == null || rightController == null)
            return -1;

        float leftDistance = Vector3.Distance(
            interactor.position, leftController.position);

        float rightDistance = Vector3.Distance(
            interactor.position, rightController.position);

        // Allow for small offsets between the controller and interactor.
        const float maximumDistance = 0.5f;

        if (Mathf.Min(leftDistance, rightDistance) > maximumDistance)
            return -1;

        return leftDistance <= rightDistance ? 0 : 1;
    }

    private void UpdateSpray()
    {
        InputActionReference triggerAction = GetNozzleHandTrigger();

        if (triggerAction == null)
        {
            StopSpray();
            return;
        }

        // Analog controller trigger: 0 = released, 1 = fully pressed.
        float triggerValue = Mathf.Clamp01(
            triggerAction.action.ReadValue<float>());

        if (triggerValue < minimumTriggerToSpray)
        {
            StopSpray();
            return;
        }

        CurrentPressure = Mathf.InverseLerp(
            minimumTriggerToSpray, 1f, triggerValue);

        SetSprayStrength(CurrentPressure);
    }

    private InputActionReference GetNozzleHandTrigger()
    {
        if (nozzleGrab == null ||
            nozzleGrab.firstInteractorSelecting == null)
            return null;

        int nozzleHand = GetHand(
            nozzleGrab.firstInteractorSelecting.transform);

        if (nozzleHand == 0)
            return leftTriggerAction;

        if (nozzleHand == 1)
            return rightTriggerAction;

        return null;
    }

    private void SetSprayStrength(float pressure)
    {
        pressure = Mathf.Clamp01(pressure);
        CurrentPressure = pressure;

        if (sprayParticles != null)
        {
            if (!sprayParticles.isPlaying)
                sprayParticles.Play();

            var emission = sprayParticles.emission;
            emission.rateOverTime = maxEmissionRate * pressure;
        }

        if (sprayAudio != null)
        {
            if (!sprayAudio.isPlaying)
                sprayAudio.Play();

            sprayAudio.volume = pressure;
        }

        if (sprayIndicatorLight != null)
        {
            sprayIndicatorLight.enabled = true;
            sprayIndicatorLight.intensity = Mathf.Lerp(
                minimumLightIntensity,
                maximumLightIntensity,
                pressure);
        }
    }

    private void StopSpray()
    {
        CurrentPressure = 0f;

        if (sprayParticles != null && sprayParticles.isPlaying)
            sprayParticles.Stop(
                true, ParticleSystemStopBehavior.StopEmitting);

        if (sprayAudio != null && sprayAudio.isPlaying)
            sprayAudio.Stop();

        if (sprayIndicatorLight != null)
            sprayIndicatorLight.enabled = false;
    }
}