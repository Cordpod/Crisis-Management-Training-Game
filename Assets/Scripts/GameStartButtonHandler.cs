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
        if (TimeManager.instance == null)
        {
            Debug.LogError("TimeManager is NULL! Cannot start the timer.");
            return;
        }
        TimeManager.instance.StartTimer(); // Start timer
        SceneManager.LoadScene(startScene);
    }

}
