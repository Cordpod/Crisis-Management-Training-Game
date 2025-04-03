using UnityEngine;

public class TrainingModeSetter : MonoBehaviour
{
    void Start()
    {
        if (TrainingDialogueUI.instance != null)
        {
            TrainingDialogueUI.instance.gameObject.SetActive(true);
            TrainingDialogueUI.instance.gameObject.SetActive(true); // Make sure it shows up
            Debug.Log("Training mode activated and Training Dialogue UI is visible.");
        }
        else
        {
            Debug.LogWarning("TrainingDialogueUI instance not found.");
        }
    }
}
