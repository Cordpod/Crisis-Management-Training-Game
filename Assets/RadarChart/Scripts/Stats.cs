using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats {

    public event EventHandler OnStatsChanged;

    public static int STAT_MIN = 0;
    // public static int STAT_MAX = 24;  // since we have a factor of max 24 for the scoring among the categories
    private static Dictionary<Type, int> CategoryMaxScores = new Dictionary<Type, int>() {
        { Type.Infoclarity, 23 },
        { Type.Mentalmodels, 20 },
        { Type.Externalaid, 21 },
        { Type.Heuristics, 24 },
        { Type.Cognitiveload, 24 }
    };

    public enum Type {
        Heuristics,
        Mentalmodels,
        Infoclarity,
        Cognitiveload,
        Externalaid,
    }

    private SingleStat heuristicsStat;
    private SingleStat mentalmodelsStat;
    private SingleStat infoclarityStat;
    private SingleStat cognitiveloadStat;
    private SingleStat externalaidStat;

    // new added
    // public Stats() {
    //     heuristicsStat = new SingleStat(0);
    //     mentalmodelsStat = new SingleStat(0);
    //     infoclarityStat = new SingleStat(0);
    //     cognitiveloadStat = new SingleStat(0);
    //     externalaidStat = new SingleStat(0);
    // }

    public Stats() {
        heuristicsStat = new SingleStat(0, CategoryMaxScores[Type.Heuristics]);
        mentalmodelsStat = new SingleStat(0, CategoryMaxScores[Type.Mentalmodels]);
        infoclarityStat = new SingleStat(0, CategoryMaxScores[Type.Infoclarity]);
        cognitiveloadStat = new SingleStat(0, CategoryMaxScores[Type.Cognitiveload]);
        externalaidStat = new SingleStat(0, CategoryMaxScores[Type.Externalaid]);
    }

    private SingleStat GetSingleStat(Type statType){
        switch (statType){
            default:
            case Type.Heuristics: return heuristicsStat;
            case Type.Mentalmodels: return mentalmodelsStat;
            case Type.Infoclarity: return infoclarityStat;
            case Type.Cognitiveload: return cognitiveloadStat;
            case Type.Externalaid: return externalaidStat;
        }
    }
    public void SetStatAmount(Type statType, int statAmount) {
        Debug.Log($"Setting {statType} to {statAmount}");
        GetSingleStat(statType).SetStatAmount(statAmount);
        if (OnStatsChanged != null) OnStatsChanged(this, EventArgs.Empty);
    }

    // new added
    public void AddToStatAmount(Type statType, int amount) {
        SetStatAmount(statType, GetStatAmount(statType) + amount);
    }


    public int GetStatAmount(Type statType) {
        return GetSingleStat(statType).GetStatAmount();
    }

    public float GetStatAmountNormalized(Type statType) {
        // return GetSingleStat(statType).GetStatAmountNormalized();
        float normalizedValue = GetSingleStat(statType).GetStatAmountNormalized();
        Debug.Log($"{statType} normalized value: {normalizedValue}");
        return normalizedValue;
    }

    public int GetMaxScore(Type statType) {
        return CategoryMaxScores[statType];
    }


    private class SingleStat {

        private int stat;
        private int maxStat;

        // public SingleStat(int statAmount) {
        //     SetStatAmount(statAmount);
        // }

        public SingleStat(int statAmount, int maxStatAmount) {
            maxStat = maxStatAmount;
            SetStatAmount(statAmount);
        }

        // public void SetStatAmount(int statAmount) {
        //     stat = Mathf.Clamp(statAmount, STAT_MIN, STAT_MAX);
        // }

        public void SetStatAmount(int statAmount) {
            stat = Mathf.Clamp(statAmount, STAT_MIN, maxStat);
        }

        public int GetStatAmount() {
            return stat;
        }

        // public float GetStatAmountNormalized() {
        //     return (float)stat / (STAT_MAX * 2);
        // }
        public float GetStatAmountNormalized() {
            // Normalize based on category's max score
            return (float)stat / (maxStat * 2);
        }
    }
}
