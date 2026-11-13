using UnityEngine;

public class FireExtinguisherSpray : MonoBehaviour
{
    [Header("Particle System")]
    public ParticleSystem sprayParticles;

    [Header("Fire Target")]
    public FireExtinguisherTarget fireTarget;

    [Header("Extinguishing Settings")]
    public float requiredTime = 5f;
    public float collisionGracePeriod = 0.2f;

    private float extinguishTimer = 0f;
    private float lastCollisionTime = -10f;

    private bool isComplete = false;

    void Update()
    {
        if (isComplete || fireTarget == null)
            return;

        if (fireTarget.IsExtinguished())
            return;

        // Only count time while the extinguisher is spraying
        if (sprayParticles == null || !sprayParticles.isEmitting)
        {
            extinguishTimer = 0f;
            return;
        }

        // Check whether spray particles are hitting the fire
        if (Time.time - lastCollisionTime <= collisionGracePeriod)
        {
            extinguishTimer += Time.deltaTime;

            Debug.Log("Extinguishing: " +
                extinguishTimer.ToString("F1") + " / " +
                requiredTime + " seconds");

            // Fire is completely extinguished after 5 seconds
            if (extinguishTimer >= requiredTime)
            {
                isComplete = true;
                fireTarget.ExtinguishFire();

                Debug.Log("SUCCESS! FIRE EXTINGUISHED!");
            }
        }
        else
        {
            // Reset if the spray stops hitting the fire
            extinguishTimer = 0f;
        }
    }

    // Called when spray particles collide with another object
    void OnParticleCollision(GameObject other)
    {
        if (isComplete || fireTarget == null)
            return;

        // Check whether the collision is with the fire
        if (other.transform == fireTarget.transform ||
            other.transform.IsChildOf(fireTarget.transform))
        {
            lastCollisionTime = Time.time;
        }
    }
}