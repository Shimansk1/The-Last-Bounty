using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public string instruction;
        public string stepKey;
        public GameObject[] activateObjects;
        public GameObject[] deactivateObjects;
    }

    public Text instructionText;
    public List<TutorialStep> steps = new List<TutorialStep>();

    private int currentStep = 0;

    void Start()
    {
        ShowStep(currentStep);
    }

    public void MarkStepComplete(string key)
    {
        if (currentStep < steps.Count && steps[currentStep].stepKey == key)
        {
            NextStep();
        }
    }

    public void NextStep()
    {
        if (currentStep + 1 < steps.Count)
        {
            currentStep++;
            ShowStep(currentStep);
        }
        else
        {
            instructionText.text = "Tutorial splnen! uz jsi pripraven na skutecnou hru! zmackni f4 aby ses vratil do menu!";
        }
    }

    void ShowStep(int stepIndex)
    {
        var step = steps[stepIndex];
        instructionText.text = step.instruction;

        foreach (var obj in step.activateObjects)
            if (obj != null) obj.SetActive(true);

        foreach (var obj in step.deactivateObjects)
            if (obj != null) obj.SetActive(false);
    }
}
