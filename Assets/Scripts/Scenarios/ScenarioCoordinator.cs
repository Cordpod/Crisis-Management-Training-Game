using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioCoordinator : MonoBehaviour
{
    public static ScenarioCoordinator instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartScenario(string scenarioName)
    {
        Debug.Log($"Starting scenario: {scenarioName}");
        DialogueManager.instance.LoadDialogueForScenario(scenarioName);
        StartFirstDialogue(scenarioName);
        TriggerScenarioSpecificAssets(scenarioName);
    }

    private void TriggerScenarioSpecificAssets(string scenarioName)
    {
        // Placeholder for animations and sound effects
        if (scenarioName == "TestScene")
        {
            // Trigger heart attack animation and sound
        }
        else if (scenarioName == "Fire")
        {
            // Trigger fire-related visuals and audio
        }
    }

    private void StartFirstDialogue(string scenarioName)
    {

        DialogueEntry firstDialogue = DialogueManager.instance.GetDialogueById(scenarioName);

        if (firstDialogue != null)
        {
            Debug.Log($"Starting First Dialogue for: {scenarioName}");
            DialogueUI.instance.DisplayDialogue(firstDialogue);
        }
        else
        {
            Debug.LogError("First dialogue entry not found in JSON.");
        }
    }
}

