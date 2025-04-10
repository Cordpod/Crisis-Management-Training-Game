using UnityEngine;

public class Level0Initializer : MonoBehaviour
{
    void Start()
    {
        // Loads dialogue for the "Level0" scenario (make sure your JSON dialogue IDs use the prefix "Level0")
        DialogueManager.instance.LoadDialogueForScenario("Level0");

        // Get your Level0 starting dialogue entry by its full ID (like "Level0_InfoClarityTest")
        DialogueEntry entry = DialogueManager.instance.GetDialogueById("Level0_InfoClarityTest");

        // Display the dialogue using DialogueUI.
        DialogueUI.instance.DisplayDialogue(entry);
    }
}
