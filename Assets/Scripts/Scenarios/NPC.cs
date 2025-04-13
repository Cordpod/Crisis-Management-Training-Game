using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string scenarioName; // Set this in the Inspector for each NPC
    public bool dialogueCompleted = false; // Tracking if dialogue has been done before
    public GameObject alertIcon; // Assign in inspector. shows up if havent completed
    public GameObject speechBubble;
    public float bubbleDuration = 2f;
    public TMP_Text speechText; // Assign TMP
    public bool dialogueWasSkipped = false;

    [TextArea(2, 3)] public string messageIfCompleted;
    [TextArea(2, 3)] public string messageIfSkipped;

    private void Start()
    {
        dialogueCompleted = SessionScenarioTracker.IsCompleted(scenarioName);

        if (alertIcon != null)
            alertIcon.SetActive(!dialogueCompleted);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !dialogueCompleted)
        {
            StartCoroutine(HighlightNPC());
            ScenarioCoordinator.instance.StartScenario(scenarioName, this);
            //Debug.Log("calling the scenario coordinator from NPC.cs");

            //DialogueEntry dialogue = DialogueManager.instance.GetDialogueById(dialogueId);
            //if (dialogue != null)
            //{
            //    Debug.Log("dialogue found");
            //    DialogueUI.instance.DisplayDialogue(dialogue);
            //}
            //else {
            //    Debug.Log("dialogue not found");  
            //}
        }
        else
        {
            if (speechBubble != null && speechText != null)
            {
                Debug.Log($"message skipped?: {dialogueWasSkipped}");
                bool wasSkipped = SessionScenarioTracker.WasSkipped(scenarioName);
                Debug.Log($"message skipped?: {wasSkipped}");
                string msg = wasSkipped ? messageIfSkipped : messageIfCompleted;
                StartCoroutine(ShowBubble(msg));
            }
        }
    }
        IEnumerator ShowBubble(string message)
        {
            speechText.text = message;
            speechBubble.SetActive(true);
            yield return new WaitForSeconds(bubbleDuration);
            speechBubble.SetActive(false);
        }

    public void MarkDialogueComplete()
    {
        dialogueCompleted = true;
        SessionScenarioTracker.MarkCompleted(scenarioName);
        dialogueWasSkipped = false;

        if (alertIcon != null)
            alertIcon.SetActive(false);
    }

    public void MarkDialogueSkipped()
    {
        dialogueCompleted = true; // Consider it done, but flagged as skipped
        SessionScenarioTracker.MarkCompleted(scenarioName);
        SessionScenarioTracker.MarkSkipped(scenarioName);
        dialogueWasSkipped = true;

        if (alertIcon != null)
            alertIcon.SetActive(false);
    }

    System.Collections.IEnumerator HighlightNPC()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        sr.color = Color.yellow; // Highlight color
        yield return new WaitForSeconds(0.2f);
        sr.color = originalColor; // Revert to original color
    }
}

