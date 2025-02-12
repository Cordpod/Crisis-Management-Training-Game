using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    // Singleton instance
    public static DialogueUI instance;

    public Text dialogueText;
    public GameObject optionsContainer;
    public Button optionButtonPrefab;

    private void Awake()
    {
        // Singleton pattern to ensure only one DialogueUI exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates if multiple DialogueUI instances exist
        }
    }

    public void DisplayDialogue(DialogueEntry dialogue)
    {
        dialogueText.text = dialogue.lines[0].text;

        // Clear previous options
        foreach (Transform child in optionsContainer.transform)
            Destroy(child.gameObject);

        // Create buttons for options
        foreach (var option in dialogue.lines[0].options)
        {
            Button btn = Instantiate(optionButtonPrefab, optionsContainer.transform);
            btn.GetComponentInChildren<Text>().text = option.text;
            btn.onClick.AddListener(() => OnOptionSelected(option.nextId));
        }
    }

    void OnOptionSelected(string nextId)
    {
        var nextDialogue = DialogueManager.instance.GetDialogueById(nextId);
        if (nextDialogue != null)
        {
            DisplayDialogue(nextDialogue);
        }
        else
        {
            // Hide dialogue UI if there's no next dialogue
            dialogueText.text = "";
            optionsContainer.SetActive(false);
        }
    }
}
