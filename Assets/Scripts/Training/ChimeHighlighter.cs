using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChimeHighlighter : MonoBehaviour
{
    // Singleton instance so other scripts can easily access it.
    public static ChimeHighlighter instance;

    // List of TextMeshPro objects for the letters C, H, I, M, E.
    public List<TMP_Text> factorLetters;

    // Private variable that stores the current factor index.
    private int trainingDialogueIndex = -1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates.
        }
    }

    /// <summary>
    /// Updates the CHIME highlighting: resets all letters to black,
    /// then highlights the letter at trainingDialogueIndex red (if valid).
    /// </summary>
    public void UpdateLetterHighlight()
    {
        // Reset all letters to black.
        foreach (var letter in factorLetters)
        {
            letter.color = Color.black;
        }
        // Highlight the current letter if the index is valid.
        if (trainingDialogueIndex >= 0 && trainingDialogueIndex < factorLetters.Count)
        {
            factorLetters[trainingDialogueIndex].color = Color.red;
        }
    }

    /// <summary>
    /// Sets the current highlight based on the dialogue id.
    /// If the dialogue id matches a factor, trainingDialogueIndex is updated;
    /// if not, it is set to -1 (resetting the highlight).
    /// </summary>
    public void SetHighlightForDialogue(string dialogueId)
    {
        trainingDialogueIndex = GetFactorIndex(dialogueId);
        UpdateLetterHighlight();
    }

    /// <summary>
    /// Returns the factor index based on the dialogue id.
    /// For example:
    /// "TrainingScene9"  => 0 (Cognitive load -> C)
    /// "TrainingScene11" => 1 (Heuristics and biases -> H)
    /// "TrainingScene13" => 2 (Information clarity -> I)
    /// "TrainingScene15" => 3 (Mental models -> M)
    /// "TrainingScene17" => 4 (External aid -> E)
    /// Returns -1 if the dialogue id does not match any factor.
    /// </summary>
    private int GetFactorIndex(string dialogueId)
    {
        switch (dialogueId)
        {
            case "TrainingScene9":
                return 0; // Cognitive load -> C
            case "TrainingScene11":
                return 1; // Heuristics and biases -> H
            case "TrainingScene13":
                return 2; // Information clarity -> I
            case "TrainingScene15":
                return 3; // Mental models -> M
            case "TrainingScene17":
                return 4; // External aid -> E
            default:
                return -1;
        }
    }

    /// <summary>
    /// Resets the highlighting (i.e. no letter is highlighted).
    /// </summary>
    public void ResetHighlight()
    {
        trainingDialogueIndex = -1;
        UpdateLetterHighlight();
    }
}
