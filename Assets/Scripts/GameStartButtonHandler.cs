using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartButtonHandler : MonoBehaviour
{
    public string gameSceneName; // Set this in the Inspector for each button

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => LoadGameScene(gameSceneName));
        }
    }

    public void LoadGameScene(string startScene)
    {
        // If the scene is NOT the training scene, start the timer.
        if (startScene != "TrainingScene")
        {
            if (TimeManager.instance == null)
            {
                Debug.LogError("TimeManager is NULL! Cannot start the timer.");
                return;
            }
            TimeManager.instance.StartTimer(); // Start timer for normal gameplay
        }
        else
        {
            Debug.Log("Loading training scene – skipping timer start.");
        }

        // Load the desired scene
        SceneManager.LoadScene(startScene);
    }
}
