
using UnityEngine;

public class ExtinguisherPASSController : MonoBehaviour
{
    [Header("PASS UI")]
    public PASSInstructionUI passUI;

    private bool pinRemoved;
    private bool nozzleAimed;
    private bool handlePressed;
    private bool sweepCompleted;

    public void OnPinRemoved()
    {
        if (pinRemoved) return;

        pinRemoved = true;
        Debug.Log("PASS: Pin removed");

        if (passUI != null)
            passUI.CompleteStep();
    }

    public void OnNozzleAimed()
    {
        if (!pinRemoved || nozzleAimed) return;

        nozzleAimed = true;
        Debug.Log("PASS: Nozzle aimed");

        if (passUI != null)
            passUI.CompleteStep();
    }

    public void OnHandlePressed()
    {
        if (!nozzleAimed || handlePressed) return;

        handlePressed = true;
        Debug.Log("PASS: Handle pressed");

        if (passUI != null)
            passUI.CompleteStep();
    }

    public void OnSweepCompleted()
    {
        if (!handlePressed || sweepCompleted) return;

        sweepCompleted = true;
        Debug.Log("PASS: Sweep completed");

        if (passUI != null)
            passUI.CompleteStep();
    }

    public bool CanSpray()
    {
        return pinRemoved && nozzleAimed && handlePressed;
    }
}
