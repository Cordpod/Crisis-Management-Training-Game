using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;
    private Stats stats;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            stats = new Stats(); // Initialize stats
            // Debug.Log("StatsManager instance initialized.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveStats(Stats newStats)
    {
        stats = newStats;
    }

    public Stats GetStats()
    {
        return stats;
    }
}
