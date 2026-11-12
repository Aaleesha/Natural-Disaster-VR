
using UnityEngine;

public class RaycastDetection : MonoBehaviour
{
    public float rayDistance = 10f;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            Debug.Log("Hit Object: " + hit.collider.name);
        }

        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
    }
}