
using UnityEngine;

public class ExitSimulation : MonoBehaviour
{
    public void ExitExperience()
    {
        Debug.Log("Exiting VR Simulation...");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
