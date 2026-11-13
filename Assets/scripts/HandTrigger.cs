using UnityEngine;
using UnityEngine.XR.Interaction;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HandTrigger : MonoBehaviour
{
    public GameObject nonBrokenGlass;
    public GameObject brokenGlass;
    public AudioSource glassShatteredAudio;
    public AudioClip glassShatteredAudioClip;
    [SerializeField]
    public static int hitCounter; 
    public AudioClip glassHitOne;
    public AudioClip glassHitTwo;

    public XRGrabInteractable fireExtinguisherPinInteractable;  // Pin

    public GameObject myImage; //ui image


    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hands"))
        {
            hitCounter++;
            if(hitCounter == 1)
            {
                glassShatteredAudio.PlayOneShot(glassHitOne);
            }
             if(hitCounter == 2)
            {
                glassShatteredAudio.PlayOneShot(glassHitTwo);
            }
            if (hitCounter==3)
            {
                Debug.Log("Breaking.....");
                brokenGlass.SetActive(true);
                
                glassShatteredAudio.PlayOneShot(glassShatteredAudioClip);
                fireExtinguisherPinInteractable.enabled = true;
                myImage.SetActive(true);       // Enable UI Image for Pin Remove
                nonBrokenGlass.SetActive(false);
            }
        }
    }

   
}
