using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string text;
    public string nextId;
}

[System.Serializable]
public class DialogueLine
{
    public string text;
    public List<DialogueOption> options;
}

[System.Serializable]
public class DialogueEntry
{
    public string id;
    public string speaker;
    public List<DialogueLine> lines;
}

[System.Serializable]
public class DialogueData
{
    public List<DialogueEntry> dialogue;
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private DialogueData dialogueData;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void LoadDialogueForScenario(string scenarioName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Dialogue/{scenarioName}/dialogue");
        if (jsonFile != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueData>(jsonFile.text);
            Debug.Log($"Loaded dialogue for scenario: {scenarioName}");
        }
        else
        {
            Debug.LogError($"Dialogue file not found for scenario: {scenarioName}");
        }
    }

    public DialogueEntry GetDialogueById(string id)
    {
        return dialogueData?.dialogue.Find(d => d.id == id);
    }
}
