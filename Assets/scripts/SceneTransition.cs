
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName;

    private bool sceneLoading = false;

    private void OnTriggerEnter(Collider other)
    {
        if (sceneLoading)
            return;

        if (other.CompareTag("Player"))
        {
            sceneLoading = true;

            Debug.Log("Player entered the door. Loading next scene...");

            SceneManager.LoadScene(nextSceneName);
        }
    }
}
