using UnityEngine;

public class NormalModeSetter : MonoBehaviour
{
    void Start()
    {
        if (DialogueUI.instance != null)
        {
            DialogueUI.instance.isTrainingMode = false;
            Debug.Log("Normal mode activated.");
        }
    }
}
