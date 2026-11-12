using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class VRMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName = "Office";

    private UIDocument uiDocument;
    private VisualElement root;

    private Button startButton;
    private Button settingsButton;
    private Button exitButton;
    private Button backButton;

    private VisualElement settingsPanel;

    private Slider volumeSlider;
    private Toggle subtitlesToggle;
    private Toggle comfortToggle;

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found!");
            return;
        }

        root = uiDocument.rootVisualElement;

        // Find buttons
        startButton = root.Q<Button>("StartButton");
        settingsButton = root.Q<Button>("SettingsButton");
        exitButton = root.Q<Button>("ExitButton");
        backButton = root.Q<Button>("BackButton");

        // Find settings panel
        settingsPanel = root.Q<VisualElement>("SettingsPanel");

        // Find settings controls
        volumeSlider = root.Q<Slider>("MasterVolumeSlider");
        subtitlesToggle = root.Q<Toggle>("SubtitlesToggle");
        comfortToggle = root.Q<Toggle>("ComfortToggle");

        // Connect button functions
        if (startButton != null)
            startButton.clicked += StartGame;

        if (settingsButton != null)
            settingsButton.clicked += OpenSettings;

        if (exitButton != null)
            exitButton.clicked += ExitGame;

        if (backButton != null)
            backButton.clicked += CloseSettings;

        // Connect settings controls
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.RegisterValueChangedCallback(OnVolumeChanged);
        }

        if (subtitlesToggle != null)
        {
            subtitlesToggle.value =
                PlayerPrefs.GetInt("Subtitles", 1) == 1;

            subtitlesToggle.RegisterValueChangedCallback(OnSubtitlesChanged);
        }

        if (comfortToggle != null)
        {
            comfortToggle.value =
                PlayerPrefs.GetInt("ComfortMode", 0) == 1;

            comfortToggle.RegisterValueChangedCallback(OnComfortChanged);
        }

        // Hide settings when menu starts
        if (settingsPanel != null)
            settingsPanel.style.display = DisplayStyle.None;
    }

    // START BUTTON
    public void StartGame()
    {
        Debug.Log("Starting VR Experience...");

        SceneManager.LoadScene(nextSceneName);
    }

    // SETTINGS BUTTON
    public void OpenSettings()
    {
        Debug.Log("Opening Settings...");

        if (settingsPanel != null)
            settingsPanel.style.display = DisplayStyle.Flex;
    }

    // BACK BUTTON
    public void CloseSettings()
    {
        Debug.Log("Closing Settings...");

        if (settingsPanel != null)
            settingsPanel.style.display = DisplayStyle.None;
    }

    // EXIT BUTTON
    public void ExitGame()
    {
        Debug.Log("Exiting Application...");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // MASTER VOLUME
    private void OnVolumeChanged(ChangeEvent<float> evt)
    {
        AudioListener.volume = evt.newValue;
        PlayerPrefs.SetFloat("MasterVolume", evt.newValue);
    }

    // SUBTITLES
    private void OnSubtitlesChanged(ChangeEvent<bool> evt)
    {
        PlayerPrefs.SetInt("Subtitles", evt.newValue ? 1 : 0);
    }

    // COMFORT MODE
    private void OnComfortChanged(ChangeEvent<bool> evt)
    {
        PlayerPrefs.SetInt("ComfortMode", evt.newValue ? 1 : 0);
    }

    private void OnDisable()
    {
        if (startButton != null)
            startButton.clicked -= StartGame;

        if (settingsButton != null)
            settingsButton.clicked -= OpenSettings;

        if (exitButton != null)
            exitButton.clicked -= ExitGame;

        if (backButton != null)
            backButton.clicked -= CloseSettings;

        if (volumeSlider != null)
            volumeSlider.UnregisterValueChangedCallback(OnVolumeChanged);

        if (subtitlesToggle != null)
            subtitlesToggle.UnregisterValueChangedCallback(OnSubtitlesChanged);

        if (comfortToggle != null)
            comfortToggle.UnregisterValueChangedCallback(OnComfortChanged);
    }
}