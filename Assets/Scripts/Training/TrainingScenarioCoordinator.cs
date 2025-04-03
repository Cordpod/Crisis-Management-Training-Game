using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingScenarioCoordinator : MonoBehaviour
{
    public static TrainingScenarioCoordinator instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Starts a training scenario using the training dialogue system
    public void StartTrainingScenario(string scenarioName, NPC npcRef)
    {
        Debug.Log($"Starting training scenario: {scenarioName}");
        // Load the dialogue data for the training scenario using TrainingDialogueManager
        TrainingDialogueManager.instance.LoadDialogueForScenario(scenarioName);
        StartFirstTrainingDialogue(scenarioName);
        if (npcRef != null)
        {
            StartCoroutine(HandleDialogue(npcRef, scenarioName));
        }
    }

    private IEnumerator HandleDialogue(NPC npcRef, string scenarioName)
    {
        // Wait until the training dialogue UI is no longer active
        yield return new WaitUntil(() => TrainingDialogueUI.instance.gameObject.activeSelf == false);
        npcRef.MarkDialogueComplete();
    }

    private void StartFirstTrainingDialogue(string scenarioName)
    {
        // Get the dialogue entry from TrainingDialogueManager instead of DialogueManager.
        DialogueEntry firstDialogue = TrainingDialogueManager.instance.GetDialogueById(scenarioName);
        if (firstDialogue != null)
        {
            Debug.Log($"Starting first training dialogue for: {scenarioName}");
            TrainingDialogueUI.instance.DisplayDialogue(firstDialogue);
        }
        else
        {
            Debug.LogError("First training dialogue entry not found in JSON.");
        }
    }
}
