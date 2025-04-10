using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StatPair
{
    //public Stats.Type key;
    public string key;
    public int value;

    public Stats.Type GetParsedKey()
    {
        if (System.Enum.TryParse(key, true, out Stats.Type parsedType))
        {
            return parsedType;
        }
        else
        {
            Debug.LogError($"Failed to parse '{key}' to Stats.Type. Defaulting to Heuristics.");
            return Stats.Type.Heuristics; // Default to avoid errors
        }
    }
}

[System.Serializable]
public class DialogueOption
{
    public string text;
    public string image;
    public string nextId;
    public List<StatPair> stats;
}

[System.Serializable]
public class DialogueLine
{
    public string text;
    public List<DialogueOption> options;
    public string nextId;
    public string trigger;
    public string sound;
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
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { 
            Destroy(gameObject); 
        }
    }

    public void LoadDialogueForScenario(string scenarioName)
    {
        Debug.Log("dialogueManager called");
        // TextAsset jsonFile = Resources.Load<TextAsset>($"Dialogue/{scenarioName}/dialogue");
        TextAsset jsonFile = Resources.Load<TextAsset>("Dialogue");
        if (jsonFile != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueData>(jsonFile.text);
            Debug.Log($"Loaded dialogue for scenario: {scenarioName}");
            // Debugging: Print entire parsed dialogue
            //foreach (var entry in dialogueData.dialogue)
            //{
            //    Debug.Log($"Dialogue ID: {entry.id}, Speaker: {entry.speaker}");

            //    foreach (var line in entry.lines)
            //    {
            //        Debug.Log($"  Text: {line.text}, NextId: {line.nextId}");

            //        if (line.options != null)
            //        {
            //            foreach (var option in line.options)
            //            {
            //                Debug.Log($"    Option Text: {option.text}, NextId: {option.nextId}");

            //                if (option.stats != null)
            //                {
            //                    foreach (var stat in option.stats)
            //                    {
            //                        Debug.Log($"      Stat: {stat.GetParsedKey()}, Value: {stat.value}");
            //                    }
            //                }
            //                else
            //                {
            //                    Debug.Log("      Stats: None");
            //                }
            //            }
            //        }
            //        else
            //        {
            //            Debug.Log("    No options available.");
            //        }
            //    }
            //}
        }
        else
        {
            Debug.LogError($"Dialogue file not found for scenario: {scenarioName}");
        }
    }

    public DialogueEntry GetDialogueById(string id)
    {
        // If the active scene is Level0, ignore the extra prefix
        if (SceneManager.GetActiveScene().name == "Level0")
        {
            return dialogueData?.dialogue.Find(d => d.id == id);
        }

        string levelPrefix = GameStateManager.instance?.GetLevelPrefix() ?? "L1";

        // Priority: Check for level-specific override
        DialogueEntry entry = dialogueData?.dialogue.Find(d => d.id == $"{levelPrefix}{id}");

        // Fallback to generic (non-prefixed) dialogue
        if (entry == null)
            entry = dialogueData?.dialogue.Find(d => d.id == id);

        return entry;
    }
}
