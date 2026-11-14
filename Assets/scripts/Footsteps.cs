
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource audioSource;
    public float stepInterval = 0.5f;

    private Vector3 lastPosition;
    private float stepTimer;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 movement = transform.position - lastPosition;
        movement.y = 0;

        if (movement.magnitude > 0.001f)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                audioSource.Play();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }

        lastPosition = transform.position;
    }
}
