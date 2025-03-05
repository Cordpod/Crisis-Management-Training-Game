using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField] private UI_StatsRadarChart uiStatsRadarChart;

    private void Start() {
        // newly deleted
        // Stats stats = new Stats(10, 10, 10, 10, 2);

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

        // int heuristics = PlayerPrefs.GetInt("Heuristics", 0);
        // int mentalModels = PlayerPrefs.GetInt("MentalModels", 0);
        // int infoClarity = PlayerPrefs.GetInt("InformationClarity", 0);
        // int cognitiveLoad = PlayerPrefs.GetInt("CognitiveLoad", 0);
        // int externalAid = PlayerPrefs.GetInt("ExternalAid", 0);

        // Stats stats = new Stats(heuristics, mentalModels, infoClarity, cognitiveLoad, externalAid);
        // uiStatsRadarChart.SetStats(stats);
            
            
    }

}
