using UnityEngine;

public class TrainingModeSetter : MonoBehaviour
{
    void Start()
    {
        if (DialogueUI.instance != null)
        {
            DialogueUI.instance.isTrainingMode = true;
            DialogueUI.instance.gameObject.SetActive(true); // Make sure it shows up
            Debug.Log("Training mode activated and Dialogue UI is visible.");
        }
        else
        {
            Debug.LogWarning("DialogueUI instance not found.");
        }
    }
}
