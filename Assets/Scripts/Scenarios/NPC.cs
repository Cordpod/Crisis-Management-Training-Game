using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string scenarioName; // Set this in the Inspector for each NPC

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            StartCoroutine(HighlightNPC());
            ScenarioCoordinator.instance.StartScenario(scenarioName);
            Debug.Log("calling the scenario coordinator from NPC.cs");

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

    System.Collections.IEnumerator HighlightNPC()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        sr.color = Color.yellow; // Highlight color
        yield return new WaitForSeconds(0.2f);
        sr.color = originalColor; // Revert to original color
    }
}

