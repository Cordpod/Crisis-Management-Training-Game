using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string dialogueId; // Set this in the Inspector for each NPC

    private void Update()
    {
        // Handle Mouse Click (PC Testing)
        if (Input.GetMouseButtonDown(0))
        {
            CheckInteraction(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        // Handle Touch Input (Mobile)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CheckInteraction(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
        }
    }

    void CheckInteraction(Vector2 clickPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            Debug.Log("NPC clicked: " + gameObject.name);
            TriggerDialogue();
        }
    }

    void TriggerDialogue()
    {
        // Highlight the NPC briefly
        StartCoroutine(HighlightNPC());

        DialogueEntry dialogue = DialogueManager.instance.GetDialogueById(dialogueId);
        if (dialogue != null)
        {
            DialogueUI.instance.DisplayDialogue(dialogue);
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

