using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    // private float timer = 0f;
    private float startTime;

    private bool isTimerRunning = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    // void Update()
    // {
    //     if (isTimerRunning)
    //     {
    //         timer += Time.deltaTime;
    //     }
    // }

    public void StartTimer()
    {
        // timer = 0f; // Reset timer

        startTime = Time.time;
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        float elapsedTime = 0f;

        if (isTimerRunning)
        {
            elapsedTime = Time.time - startTime;
            PlayerPrefs.SetFloat("ElapsedTime", elapsedTime); // Save time in PlayerPrefs
            PlayerPrefs.Save(); // Ensure it gets stored
            isTimerRunning = false;
        }
        Debug.Log("Total Time Taken: " + elapsedTime + " seconds");
    }

    // public float GetElapsedTime()
    // {
    //     // return timer;
    //     return PlayerPrefs.GetFloat("ElapsedTime", 0f);
    // }
}
