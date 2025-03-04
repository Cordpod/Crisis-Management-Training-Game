using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    // Singleton instance
    public static DialogueUI instance;

    public TMP_Text dialogueText;
    public GameObject optionsContainer;
    public Button optionButtonPrefab;

    private DialogueEntry currentDialogue; // For current ref dialogue
    public static bool isDialogueActive = false; // Global flag

    // Holds references to your "C, H, I, M, E" TextMeshPro objects in order
    public List<TMP_Text> factorLetters;

    // Start at -1 so that no letter is highlighted initially
    private int trainingDialogueIndex = -1;

    // Flag to indicate training mode
    public bool isTrainingMode = false;

    private void Start()
    {
        // Hide dialogue box on start
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        // Singleton pattern to ensure only one DialogueUI exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    public void DisplayDialogue(DialogueEntry dialogue)
    {
        gameObject.SetActive(true); // Show dialogue box
        isDialogueActive = true;
        currentDialogue = dialogue; // Store the current dialogue

        if (dialogueText == null)
        {
            Debug.LogError("dialogueText is NULL! Check if it's assigned.");
            return;
        }

        // Display the first line of this dialogue entry
        dialogueText.text = dialogue.lines[0].text;
        Debug.Log($"Displaying current dialogue: {dialogueText.text}");

        // In training mode, always disable option buttons
        if (isTrainingMode)
        {
            optionsContainer.SetActive(false);
        }
        else
        {
            // Only create option buttons if they exist
            if (dialogue.lines[0].options.Count > 0)
            {
                // Clear previous buttons
                foreach (Transform child in optionsContainer.transform)
                {
                    Destroy(child.gameObject);
                    Debug.Log($"destroyed: {child.name}");
                }

                optionsContainer.SetActive(true);
                foreach (var option in dialogue.lines[0].options)
                {
                    Button btn = Instantiate(optionButtonPrefab, optionsContainer.transform);
                    TMP_Text btnText = btn.GetComponentInChildren<TMP_Text>();
                    if (btnText != null)
                    {
                        btnText.text = option.text;
                    }
                    else
                    {
                        Debug.LogError("TMP text not found in button prefab");
                    }
                    Debug.Log($"option text for button: {option.text}, option nextId for button {option.nextId}");
                    btn.onClick.AddListener(() => ContinueDialogue(option.nextId));
                }
            }
            else
            {
                optionsContainer.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // Handles screen taps/clicks to move on dialogue
        if (Input.GetMouseButtonDown(0) ||
           (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Debug.Log("Screen Clicked");

            // Only advance if no choices are active (or if training mode has no choices)
            if (optionsContainer.activeSelf == false)
            {
                // If there's a current dialogue with no branching
                if (currentDialogue != null && currentDialogue.lines[0].options.Count == 0)
                {
                    // If training mode, increment the highlight index before continuing
                    if (isTrainingMode)
                    {
                        trainingDialogueIndex++;
                        UpdateLetterHighlight();
                    }

                    // Advance to the next dialogue line
                    ContinueDialogue(currentDialogue.lines[0].nextId);
                }
            }
        }
    }

    void ContinueDialogue(string nextId)
    {
        Debug.Log($"Calling ContinueDialogue, Moving to Next Dialogue Line Id:{nextId}");

        var nextDialogue = DialogueManager.instance.GetDialogueById(nextId);

        if (nextDialogue != null)
        {
            DisplayDialogue(nextDialogue);
        }
        else
        {
            Debug.Log("Dialogue Ended");
            gameObject.SetActive(false); // Hide dialogue box when finished
            isDialogueActive = false;
        }
    }

    public void CloseDialogue()
    {
        Debug.Log("Dialogue closed.");
        isDialogueActive = false; // Allow player to move again
        gameObject.SetActive(false); // Hide dialogue box
    }

    /// <summary>
    /// Resets all letters to black, then highlights the current index red if valid.
    /// </summary>
    private void UpdateLetterHighlight()
    {
        // Reset all letters to black
        foreach (var letter in factorLetters)
        {
            letter.color = Color.black;
        }

        // Highlight the current letter if it's in range
        if (trainingDialogueIndex >= 0 && trainingDialogueIndex < factorLetters.Count)
        {
            factorLetters[trainingDialogueIndex].color = Color.red;
        }
    }
}
