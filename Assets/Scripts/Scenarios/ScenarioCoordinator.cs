using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioCoordinator : MonoBehaviour
{
    public static ScenarioCoordinator instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void StartScenario(string scenarioName)
    {
        Debug.Log($"Starting scenario: {scenarioName}");
        DialogueManager.instance.LoadDialogueForScenario(scenarioName);
        TriggerScenarioSpecificAssets(scenarioName);
    }

    private void TriggerScenarioSpecificAssets(string scenarioName)
    {
        // Placeholder for animations and sound effects
        if (scenarioName == "HeartAttack")
        {
            // Trigger heart attack animation and sound
        }
        else if (scenarioName == "Fire")
        {
            // Trigger fire-related visuals and audio
        }
    }
}

