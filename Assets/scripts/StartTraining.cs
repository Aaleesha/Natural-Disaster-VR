
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartTraining : MonoBehaviour
{
    public string nextSceneName = "Main Scene";

    public void StartGame()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
