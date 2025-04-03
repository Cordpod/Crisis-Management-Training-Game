using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDialogueManager : MonoBehaviour
{
    public static TrainingDialogueManager instance;
    private DialogueData dialogueData;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Note: We intentionally do not call DontDestroyOnLoad here so that training-specific objects are destroyed on scene transition.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadDialogueForScenario(string scenarioName)
    {
        Debug.Log("TrainingDialogueManager called");
        TextAsset jsonFile = Resources.Load<TextAsset>("Dialogue");
        if (jsonFile != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueData>(jsonFile.text);
            Debug.Log($"Loaded training dialogue for scenario: {scenarioName}");
        }
        else
        {
            Debug.LogError($"Dialogue file not found for training scenario: {scenarioName}");
        }
    }

    public DialogueEntry GetDialogueById(string id)
    {
        if (dialogueData == null || dialogueData.dialogue == null)
        {
            return null;
        }
        // For training, match the dialogue entry directly by id without any level prefix.
        DialogueEntry entry = dialogueData.dialogue.Find(d => d.id == id);
        return entry;
    }
}
