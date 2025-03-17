using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Testing : MonoBehaviour {

    [SerializeField] private UI_StatsRadarChart uiStatsRadarChart;
    [SerializeField] private TMP_Text timeText;

    private void Start() {

        // newly added
        Stats stats = new Stats();
        stats.SetStatAmount(Stats.Type.Heuristics, PlayerPrefs.GetInt("Heuristics", 0));
        stats.SetStatAmount(Stats.Type.Mentalmodels, PlayerPrefs.GetInt("Mentalmodels", 0));
        stats.SetStatAmount(Stats.Type.Infoclarity, PlayerPrefs.GetInt("Infoclarity", 0));
        stats.SetStatAmount(Stats.Type.Cognitiveload, PlayerPrefs.GetInt("Cognitiveload", 0));
        stats.SetStatAmount(Stats.Type.Externalaid, PlayerPrefs.GetInt("Externalaid", 0));

        uiStatsRadarChart.SetStats(stats);

        if (uiStatsRadarChart == null) {
            Debug.LogError("uiStatsRadarChart is not assigned in Testing.cs!");
            return;
        }

        float elapsedTime = PlayerPrefs.GetFloat("ElapsedTime", 0f);

        if (timeText != null)
        {
            // float elapsedTime = TimeManager.instance.GetElapsedTime();
            timeText.text = elapsedTime.ToString("F2") + " s";
        }
        else
        {
            Debug.LogError("timeText UI is not assigned in Testing.cs!");
        }


            
            
    }

}
