using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats {

    public event EventHandler OnStatsChanged;

    public static int STAT_MIN = 0;
    public static int STAT_MAX = 20;

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

    public Stats(int heuristicsStatAmount, int mentalmodelsStatAmount, int infoclarityStatAmount, int cognitiveloadStatAmount, int externalaidStatAmount) {
        heuristicsStat = new SingleStat(heuristicsStatAmount);
        mentalmodelsStat = new SingleStat(mentalmodelsStatAmount);
        infoclarityStat = new SingleStat(infoclarityStatAmount);
        cognitiveloadStat = new SingleStat(cognitiveloadStatAmount);
        externalaidStat = new SingleStat(externalaidStatAmount);
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
        GetSingleStat(statType).SetStatAmount(statAmount);
        if (OnStatsChanged != null) OnStatsChanged(this, EventArgs.Empty);
    }

    public void IncreaseStatAmount(Type statType) {
        SetStatAmount(statType, GetStatAmount(statType)+1);
    }

    public void DecreaseStatAmount(Type statType) {
        SetStatAmount(statType, GetStatAmount(statType)-1);
    }

    public int GetStatAmount(Type statType) {
        return GetSingleStat(statType).GetStatAmount();
    }

    public float GetStatAmountNormalized(Type statType) {
        return GetSingleStat(statType).GetStatAmountNormalized();
    }



    // /*
    //  * Represents a Single Stat of any Type
    //  * */
    private class SingleStat {

        private int stat;

        public SingleStat(int statAmount) {
            SetStatAmount(statAmount);
        }

        public void SetStatAmount(int statAmount) {
            stat = Mathf.Clamp(statAmount, STAT_MIN, STAT_MAX);
            
        }

        public int GetStatAmount() {
            return stat;
        }

        public float GetStatAmountNormalized() {
            return (float)stat / STAT_MAX;
        }
    }
}
