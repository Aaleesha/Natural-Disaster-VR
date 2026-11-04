using UnityEngine;

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
                nonBrokenGlass.SetActive(false);
                glassShatteredAudio.PlayOneShot(glassShatteredAudioClip);
            }
        }
    }
}
