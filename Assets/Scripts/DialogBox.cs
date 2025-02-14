using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [System.Serializable]
    public class Dialog
    {
        public string text;
        public List<string> choices; // Choices for the player
    }

    public List<Dialog> dialogs; // Add dialog data in the Inspector
    public TextMeshProUGUI dialogText; // Link to the Text UI element
    public GameObject[] choiceButtons; // Link to the buttons

    private int currentDialogIndex = 0;

    void Start()
    {
        ShowDialog(currentDialogIndex);
    }

    public void ShowDialog(int dialogIndex)
    {
        if (dialogIndex < dialogs.Count)
        {
            dialogText.text = dialogs[dialogIndex].text;

            for (int i = 0; i < choiceButtons.Length; i++)
            {
                if (i < dialogs[dialogIndex].choices.Count)
                {
                    choiceButtons[i].SetActive(true);
                    choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = dialogs[dialogIndex].choices[i];
                    int choiceIndex = i; // Prevent closure issue
                    choiceButtons[i].GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
                    choiceButtons[i].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnChoiceSelected(choiceIndex));
                }
                else
                {
                    choiceButtons[i].SetActive(false);
                }
            }
        }
    }

    public void OnChoiceSelected(int choiceIndex)
    {
        // Handle the player's choice
        Debug.Log("Player selected choice: " + choiceIndex);

        // Progress to the next dialog or end the dialog
        currentDialogIndex++;
        if (currentDialogIndex < dialogs.Count)
        {
            ShowDialog(currentDialogIndex);
        }
        else
        {
            Debug.Log("Dialog sequence complete!");
        }
    }
}
