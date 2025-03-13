using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartButtonHandler : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(LoadGameScene);
        }
    }
    void LoadGameScene()
    {
        if (TimeManager.instance == null)
        {
            Debug.LogError("TimeManager is NULL! Cannot start the timer.");
            return;
        }
        TimeManager.instance.StartTimer(); // Start timer
        SceneManager.LoadScene("GameSceneStart");
    }

}
