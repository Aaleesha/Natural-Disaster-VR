
using UnityEngine;

public class SweepDetector : MonoBehaviour
{
    [Header("References")]
    public ExtinguisherPASSController passController;

    [Header("Sweep Settings")]
    public float minimumSweepDistance = 0.35f;
    public float sweepDuration = 1.5f;

    private Vector3 previousPosition;
    private float sweepTimer;
    private float totalDistance;
    private bool sweepCompleted;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        if (sweepCompleted || passController == null)
            return;

        if (!passController.CanSpray())
        {
            ResetSweep();
            return;
        }

        Vector3 movement = transform.position - previousPosition;

        // Measure horizontal movement only.
        Vector3 horizontalMovement =
            Vector3.ProjectOnPlane(movement, Vector3.up);

        totalDistance += horizontalMovement.magnitude;

        previousPosition = transform.position;

        if (totalDistance >= minimumSweepDistance)
        {
            sweepTimer += Time.deltaTime;
        }

        if (sweepTimer >= sweepDuration)
        {
            sweepCompleted = true;

            passController.OnSweepCompleted();

            Debug.Log("Side-to-side sweep completed!");
        }
    }

    void ResetSweep()
    {
        sweepTimer = 0f;
        totalDistance = 0f;
        previousPosition = transform.position;
    }
}
