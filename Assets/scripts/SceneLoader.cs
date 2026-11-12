using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "YourTargetSceneName";
    [SerializeField] private string startButtonName = "Start"; // Must match the Name in UI Builder

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null) return;

        VisualElement root = uiDocument.rootVisualElement;
        Button startButton = root.Q<Button>(startButtonName);

        if (startButton != null)
        {
            startButton.clicked += OnStartClicked;
        }
    }

    private void OnDisable()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null) return;

        VisualElement root = uiDocument.rootVisualElement;
        Button startButton = root.Q<Button>(startButtonName);

        if (startButton != null)
        {
            startButton.clicked -= OnStartClicked;
        }
    }

    private void OnStartClicked()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}