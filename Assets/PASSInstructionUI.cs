
using UnityEngine;
using TMPro;

public class PASSInstructionUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text progressText;

    [Header("Optional Panels")]
    public GameObject instructionPanel;
    public GameObject successPanel;

    private int currentStep = 0;
    private bool trainingComplete = false;

    private readonly string[] titles =
    {
        "1. PULL THE PIN",
        "2. AIM",
        "3. SQUEEZE",
        "4. SWEEP SIDE TO SIDE"
    };

    private readonly string[] descriptions =
    {
        "Remove the safety pin to unlock the extinguisher.",
        "Point the nozzle towards the base of the fire.",
        "Press the handle to release the extinguishing agent.",
        "Move the nozzle from side to side across the base of the fire."
    };

    void Start()
    {
        ShowStep(0);

        if (successPanel != null)
            successPanel.SetActive(false);
    }

    public void CompleteStep()
    {
        if (trainingComplete || currentStep >= 4)
            return;

        currentStep++;

        if (currentStep < 4)
        {
            ShowStep(currentStep);
        }
        else
        {
            CompleteTraining();
        }
    }

    public void ShowStep(int step)
    {
        currentStep = Mathf.Clamp(step, 0, 3);

        titleText.text = titles[currentStep];
        descriptionText.text = descriptions[currentStep];
        progressText.text = "STEP " + (currentStep + 1) + " / 4";
    }

    public void CompleteTraining()
    {
        trainingComplete = true;

        if (instructionPanel != null)
            instructionPanel.SetActive(false);

        if (successPanel != null)
            successPanel.SetActive(true);

        Debug.Log("PASS training completed!");
    }

    public int CurrentStep => currentStep;
}
