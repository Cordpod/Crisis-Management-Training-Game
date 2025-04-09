using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    // References to the AudioSource components for different music tracks.
    public AudioSource mainGameAudio;
    public AudioSource trainingAudio;

    private void Awake()
    {
        // Singleton pattern: ensure only one instance of SoundManager exists.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object persistent between scene loads.
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called each time a new scene is loaded.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check the scene name and play the appropriate audio.
        if (scene.name == "TrainingScene" || scene.name == "WaitingForMRT")
        {
            // For the training scene, stop the main game audio if it's playing,
            // and play the training audio if not already playing.
            if (mainGameAudio.isPlaying)
            {
                mainGameAudio.Stop();
            }
            if (!trainingAudio.isPlaying)
            {
                trainingAudio.Play();
            }
        }
        else if (scene.name == "MainScene" || scene.name == "AnotherMainGameScene")  // Extend these conditions as needed.
        {
            // For the main game scene, stop the training audio if it's playing,
            // and play the main game audio if not already playing.
            if (trainingAudio.isPlaying)
            {
                trainingAudio.Stop();
            }
            if (!mainGameAudio.isPlaying)
            {
                mainGameAudio.Play();
            }
        }
    }

}
