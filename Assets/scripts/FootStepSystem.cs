
using UnityEngine;

public class FootStepSystem : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip concrete;
    [SerializeField] private AudioClip fire;

    [Header("Ground Detection")]
    [SerializeField] private Transform rayStart;
    [SerializeField] private float groundCheckDistance = 1.5f;
    [SerializeField] private LayerMask groundLayer = ~0;

    [Header("Footstep Timing")]
    [SerializeField] private float stepInterval = 0.45f;
    [SerializeField] private float minimumMoveSpeed = 0.15f;
    [SerializeField] private float teleportThreshold = 2f;

    private Vector3 previousPosition;
    private float stepTimer;
    private bool wasGrounded;

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 movement = currentPosition - previousPosition;

        // Ignore vertical movement when calculating walking speed.
        movement.y = 0f;
        float horizontalSpeed = movement.magnitude / Mathf.Max(Time.deltaTime, 0.001f);

        // Avoid footsteps when teleporting or making a large position jump.
        bool teleported = movement.magnitude > teleportThreshold;

        previousPosition = currentPosition;

        if (teleported)
        {
            stepTimer = 0f;
            return;
        }

        bool grounded = CheckGrounded();

        if (!grounded)
        {
            stepTimer = 0f;
            wasGrounded = false;
            return;
        }

        // Avoid an immediate footstep when landing.
        if (!wasGrounded)
        {
            wasGrounded = true;
            stepTimer = 0f;
        }

        // Only play footsteps when moving horizontally.
        if (horizontalSpeed < minimumMoveSpeed)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer += Time.deltaTime;

        if (stepTimer >= stepInterval)
        {
            PlaySurfaceSound();
            stepTimer = 0f;
        }
    }

    private bool CheckGrounded()
    {
        if (rayStart == null)
            return false;

        return Physics.Raycast(
            rayStart.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );
    }

    private void PlaySurfaceSound()
    {
        if (audioSource == null || rayStart == null)
            return;

        if (Physics.Raycast(
            rayStart.position,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag("concrete"))
            {
                PlayClip(concrete);
            }
            else if (hit.collider.CompareTag("fire"))
            {
                PlayClip(fire);
            }
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.pitch = Random.Range(0.9f, 1.05f);
        audioSource.PlayOneShot(clip);
    }

    private void OnDrawGizmosSelected()
    {
        if (rayStart == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            rayStart.position,
            rayStart.position + Vector3.down * groundCheckDistance
        );
    }
}
