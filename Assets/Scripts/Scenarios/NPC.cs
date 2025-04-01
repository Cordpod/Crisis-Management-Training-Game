using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string scenarioName; // Set this in the Inspector for each NPC
    public bool dialogueCompleted = false; // Tracking if dialogue has been done before
    public GameObject alertIcon; // Assign in inspector. shows up if havent completed

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
    }
    public void MarkDialogueComplete()
    {
        dialogueCompleted = true;
        SessionScenarioTracker.MarkCompleted(scenarioName);

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

