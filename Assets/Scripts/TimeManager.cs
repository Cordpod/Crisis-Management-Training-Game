using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    // Timing variables
    private float accumulatedTime = 0f;
    private bool isTimerRunning = false;
    private float startGameTime = 0f;
    public TMP_Text CountdownTimer;
    private float countdownTime = 300f;
    private float timeRemaining;

    // Blinking variables
    private bool isBlinking = false;
    private float blinkInterval = 0.5f; // Time between color changes in seconds
    private float blinkTimer = 0;
    private Color originalColor;
    private Color blinkColor = Color.red;
    private bool isRed = false;

    // We’ll track if we’ve already initialized the timer once in a main game scene.
    private bool hasStartedGameTimer = false;

    // List of scenes that are NOT part of the main game
    private readonly List<string> nonGameScenes = new List<string>
    {
        "MainScene",     // Menu scene
        "SampleScene",
        "StatsScene",
        "WaitingForMRT" // Training scene
    };

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
        if (CountdownTimer == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        CountdownTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        if (isTimerRunning)
        {
            isTimerRunning = false;
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

        // If scene is in the non-game list, stop the timer and reset flags
        if (nonGameScenes.Contains(scene.name))
        {
            StopTimer();
            hasStartedGameTimer = false;  // So that next time we go back to main game, it resets properly
            return;
        }

        // Otherwise, we treat this as a main-game scene
        // If we've NOT started the timer yet (i.e. first main-game scene, typically "GameSceneStart"), reset the time
        if (!hasStartedGameTimer)
        {
            // Reset the time only on the first main-game scene
            timeRemaining = countdownTime;
            accumulatedTime = 0f;

            // Start the timer
            RecordStartGameTime();
            hasStartedGameTimer = true;
        }
        else
        {
            // Already started the timer in a previous main-game scene
            // Just continue running
            isTimerRunning = true;
        }
    }

    private void FindCountdownTimerUI()
    {
        // If the reference is already found, no need to look again
        if (CountdownTimer != null)
            return;

        GameObject timerObject = GameObject.FindGameObjectWithTag("CountdownTimer");
        if (timerObject != null)
        {
            CountdownTimer = timerObject.GetComponent<TMP_Text>();
            UpdateTimerUI();

            if (CountdownTimer != null)
            {
                originalColor = CountdownTimer.color;
                UpdateTimerUI();

                // If the timer is already running and time is low, resume blinking
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
