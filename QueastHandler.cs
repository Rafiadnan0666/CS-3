using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    [System.Serializable]
    public class Quest
    {
        public string questName;
        public string[] steps;
        public int currentStep = 0;
        public bool isCompleted = false;
    }

    public Quest currentQuest;
    public GameObject globalQuestTrigger; // Trigger for the communication tower
    public GameObject[] stepTriggers; // Triggers for each step

    public void InitializeQuest(string questName, string[] questSteps)
    {
        currentQuest = new Quest
        {
            questName = questName,
            steps = questSteps
        };

        Debug.Log($"Quest Initialized: {questName}");
        ActivateStepTrigger(currentQuest.currentStep);
    }

    public void CompleteCurrentStep()
    {
        if (currentQuest != null && !currentQuest.isCompleted)
        {
            Debug.Log($"Step Completed: {currentQuest.steps[currentQuest.currentStep]}");
            currentQuest.currentStep++;

            if (currentQuest.currentStep >= currentQuest.steps.Length)
            {
                currentQuest.isCompleted = true;
                Debug.Log("Quest Completed!");
                ActivateGlobalQuest();
            }
            else
            {
                ActivateStepTrigger(currentQuest.currentStep);
            }
        }
    }

    private void ActivateStepTrigger(int stepIndex)
    {
        foreach (var trigger in stepTriggers)
        {
            trigger.SetActive(false);
        }

        if (stepIndex < stepTriggers.Length)
        {
            stepTriggers[stepIndex].SetActive(true);
        }
    }

    private void ActivateGlobalQuest()
    {
        if (globalQuestTrigger != null)
        {
            globalQuestTrigger.SetActive(true);
        }
    }
}
