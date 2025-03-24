using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Testing : MonoBehaviour {

    [SerializeField] private UI_StatsRadarChart uiStatsRadarChart;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text commentText;

    private void Start() {
        if (StatsManager.instance == null)
        {
            Debug.LogError("StatsManager instance is null!");
            return;
        }

        // newly added
        Stats stats = StatsManager.instance.GetStats();

        float elapsedTime = PlayerPrefs.GetFloat("ElapsedTime", 0f);
        Debug.Log("Elapsed Time Retrieved: " + elapsedTime);
        if (elapsedTime <= 60f)
        {
            stats.AddToStatAmount(Stats.Type.Cognitiveload, 24);
        }
        else if (elapsedTime <= 90f)
        {
            stats.AddToStatAmount(Stats.Type.Cognitiveload, 16);
        }
        else if (elapsedTime <= 120f)
        {
            stats.AddToStatAmount(Stats.Type.Cognitiveload, 8);
        }
        else
        {
            stats.AddToStatAmount(Stats.Type.Cognitiveload, 0); 
        }

        StatsManager.instance.SaveStats(stats);
        string assessmentComment = GenerateAssessmentComment(stats);
        commentText.text = assessmentComment;

        Debug.Log("Cognitiveload Value After Update: " + stats.GetStatAmount(Stats.Type.Cognitiveload));

        if (uiStatsRadarChart == null) {
            Debug.LogError("uiStatsRadarChart is not assigned in Testing.cs!");
            return;
        }
        else{
            uiStatsRadarChart.SetStats(stats);
        }

        if (timeText != null)
        {
            timeText.text = elapsedTime.ToString("F2") + " s";
        }
        else
        {
            Debug.LogError("timeText UI is not assigned in Testing.cs!");
        }    
    }

    private string GenerateAssessmentComment(Stats stats) {

        List<string> expertFactors = new List<string>();
        List<string> intermediateFactors = new List<string>();
        List<string> beginnerFactors = new List<string>();

        int informationClarityScore = stats.GetStatAmount(Stats.Type.Infoclarity);
        int mentalModelsScore = stats.GetStatAmount(Stats.Type.Mentalmodels);
        int externalAidScore = stats.GetStatAmount(Stats.Type.Externalaid);
        int heuristicsBiasScore = stats.GetStatAmount(Stats.Type.Heuristics);
        int cognitiveLoadScore = stats.GetStatAmount(Stats.Type.Cognitiveload);

        int totalScore = informationClarityScore + mentalModelsScore + externalAidScore + heuristicsBiasScore + cognitiveLoadScore;

        if (informationClarityScore >= 16 && informationClarityScore <= 23) {
            expertFactors.Add("Information Clarity");
        } else if (informationClarityScore >= 8 && informationClarityScore <= 15) {
            intermediateFactors.Add("Information Clarity");
        } else {
            beginnerFactors.Add("Information Clarity");
        }

        if (mentalModelsScore >= 14 && mentalModelsScore <= 20) {
            expertFactors.Add("Mental Models");
        } else if (mentalModelsScore >= 7 && mentalModelsScore <= 13) {
            intermediateFactors.Add("Mental Models");
        } else {
            beginnerFactors.Add("Mental Models");
        }

        if (externalAidScore >= 15 && externalAidScore <= 21) {
            expertFactors.Add("External Aid");
        } else if (externalAidScore >= 8 && externalAidScore <= 14) {
            intermediateFactors.Add("External Aid");
        } else {
            beginnerFactors.Add("External Aid");
        }

        if (heuristicsBiasScore >= 17 && heuristicsBiasScore <= 24) {
            expertFactors.Add("Heuristics & Bias");
        } else if (heuristicsBiasScore >= 9 && heuristicsBiasScore <= 16) {
            intermediateFactors.Add("Heuristics & Bias");
        } else {
            beginnerFactors.Add("Heuristics & Bias");
        }

        if (cognitiveLoadScore == 24) {
            expertFactors.Add("Cognitive Load");
        } else if (cognitiveLoadScore == 16) {
            intermediateFactors.Add("Cognitive Load");
        } else {
            beginnerFactors.Add("Cognitive Load");
        }

        string level;
        if (totalScore >= 76) {
            level = "excellent";
        } else if (totalScore >= 38 && totalScore <= 75) {
            level = "good";
        } else {
            level = "poor";
        }

        string expertFactorsText = FormatFactorsList(expertFactors);
        string intermediateFactorsText = FormatFactorsList(intermediateFactors);
        string beginnerFactorsText = FormatFactorsList(beginnerFactors);

        string comment = $"Your crisis management assessment shows <b>{level}</b> performance: ";
        
        if (expertFactors.Count > 0) {
            comment += $"You excellent in your <b>{expertFactorsText}</b>. ";
        }
        
        if (beginnerFactors.Count > 0) {
            comment += $"Your <b>{beginnerFactorsText}</b> need significant development. ";
        }
        
        if (intermediateFactors.Count > 0) {
            comment += $"Your <b>{intermediateFactorsText}</b> are adequate but could be improved through more refined techniques and consistent practice.";
        }
        
        return comment;
    }
    
    private string FormatFactorsList(List<string> factors) {
        if (factors.Count == 0) {
            return "";
        } else if (factors.Count == 1) {
            return factors[0];
        } else if (factors.Count == 2) {
            return $"{factors[0]} and {factors[1]}";
        } else {
            string result = "";
            for (int i = 0; i < factors.Count - 1; i++) {
                result += factors[i];
                if (i < factors.Count - 2) {
                    result += ", ";
                }
            }
            result += $" and {factors[factors.Count - 1]}";
            return result;
        }
    }

}
