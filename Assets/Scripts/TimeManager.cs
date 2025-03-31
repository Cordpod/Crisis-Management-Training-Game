using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    // private float startTime;
    private float accumulatedTime = 0f;
    private bool isTimerRunning = false;
    private float startGameTime = 0f;


    public TMP_Text CountdownTimer;  
    private float countdownTime = 300f;  
    private float timeRemaining;


    private bool isBlinking = false;
    private float blinkInterval = 0.5f; // Time between color changes in seconds
    private float blinkTimer = 0;
    private Color originalColor;
    private Color blinkColor = Color.red;
    private bool isRed = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
        accumulatedTime = PlayerPrefs.GetFloat("AccumulatedTime", 0f);
        timeRemaining = countdownTime;
    }

    void Start()
    {
        FindCountdownTimerUI();
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    public void RecordStartGameTime()
    {
        startGameTime = Time.time;
        isTimerRunning = true;
    }

    public float GetTotalGameTime()
    {
        return Time.time - startGameTime;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                StopTimer();
            }

            accumulatedTime += Time.deltaTime;

            UpdateTimerUI();  

            if (timeRemaining <= 15f && !isBlinking)
            {
                StartBlinking();
            }
            else if (timeRemaining > 15f && isBlinking)
            {
                StopBlinking();
            }
            if (isBlinking)
            {
                UpdateBlinkEffect();
            }
        }

        
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        CountdownTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        //Debug.Log("Timer Updated: " + CountdownTimer.text);
    }

    private void StartBlinking()
    {
        if (CountdownTimer != null)
        {
            isBlinking = true;
            originalColor = CountdownTimer.color;
            blinkTimer = 0;
        }
    }

    private void StopBlinking()
    {
        if (CountdownTimer != null)
        {
            isBlinking = false;
            CountdownTimer.color = originalColor;
        }
    }

    private void UpdateBlinkEffect()
    {
        if (CountdownTimer == null) return;
        
        blinkTimer += Time.deltaTime;
        
        if (blinkTimer >= blinkInterval)
        {
            blinkTimer = 0;
            isRed = !isRed;
            
            CountdownTimer.color = isRed ? blinkColor : originalColor;
        }
    }


    public void StartTimer()
    {
        // startTime = Time.time;
        isTimerRunning = true;
    }

    // public void StopTimer()
    // {
    //     float elapsedTime = 0f;

    //     if (isTimerRunning)
    //     {
    //         elapsedTime = Time.time - startTime;
    //         PlayerPrefs.SetFloat("ElapsedTime", elapsedTime); 
    //         PlayerPrefs.Save(); 
    //         isTimerRunning = false;

    //         if (isBlinking)
    //         {
    //             StopBlinking();
    //         }
    //     }
    //     Debug.Log("Total Time Taken: " + elapsedTime + " seconds");
    // }
    public void StopTimer()
    {
        if (isTimerRunning)
        {
            isTimerRunning = false;

            // Save accumulated time to PlayerPrefs
            PlayerPrefs.SetFloat("AccumulatedTime", accumulatedTime);
            PlayerPrefs.Save();

            if (isBlinking)
            {
                StopBlinking();
            }
        }
    }

    public float GetAccumulatedTime()
    {
        return accumulatedTime;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCountdownTimerUI(); 
    }

    private void FindCountdownTimerUI()
    {
        if (CountdownTimer == null)
        {
            GameObject timerObject = GameObject.FindGameObjectWithTag("CountdownTimer"); 
            if (timerObject != null)
            {
                CountdownTimer = timerObject.GetComponent<TMP_Text>();
                UpdateTimerUI(); 

                if (CountdownTimer != null)
                {
                    originalColor = CountdownTimer.color;
                    UpdateTimerUI();

                    if (timeRemaining <= 15f && isTimerRunning)
                    {
                        StartBlinking();
                    }
                }
            }
            else
            {
                Debug.LogWarning("Countdown Timer UI not found in scene.");
            }
        }   
    }
}
