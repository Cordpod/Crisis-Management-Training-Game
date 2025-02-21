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

    private DialogueEntry currentDialogue; //For current ref dialogue
    public static bool isDialogueActive = false; // Global flag

    private void Start()
    {
        gameObject.SetActive(false); // Hide dialogue box on start
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
            Destroy(gameObject); // Prevent duplicates if multiple DialogueUI instances exist
        }
    }

    public void DisplayDialogue(DialogueEntry dialogue)
    {

        gameObject.SetActive(true); // Dialogue box appear
        isDialogueActive=true;
        currentDialogue = dialogue; // Storing the current dialogue here

        dialogueText.text = dialogue.lines[0].text;
        Debug.Log($"Displaying current dialogue: {dialogueText.text}");

        if (dialogueText == null) // Checking that text was got successfully
        {
            Debug.LogError("dialogueText is NULL! Check if it's assigned.");
            return;
        }

        if (dialogue.lines[0].options.Count > 0)
        {

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
                if (btnText != null) { btnText.text = option.text; }
                else { Debug.LogError("TMP text not found in button prefab"); }
                Debug.Log($"option text for button: {option.text}, option nextId for button {option.nextId} " );
                btn.onClick.AddListener(() => ContinueDialogue(option.nextId));
            }
        }
        else
        {
            optionsContainer.SetActive(false); // Hide buttons when no choices exist
        } 
    }

    private void Update() // Handles screen clicks to move on dialogue
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Debug.Log("Screen Clicked");
            //ContinueDialogue();

            if (optionsContainer.activeSelf == false) // Only advance if no choices exist
            {
                if (currentDialogue != null && currentDialogue.lines[0].options.Count == 0)
                {
                    ContinueDialogue(currentDialogue.lines[0].nextId); // Retrieve nextId from the current dialogue
                }
            }
        }
    }
    void ContinueDialogue(string nextId)
    {
        Debug.Log($"Calling ContinueDialogue, Moving to Next Dialogue Line Id:{nextId}");

        var nextDialogue = DialogueManager.instance.GetDialogueById(nextId); 
        //Debug.Log($"Getting nextDialogue: {nextDialogue}" );

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

}
