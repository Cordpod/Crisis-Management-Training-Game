using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class SessionScenarioTracker
{
    private static HashSet<string> completedScenarios = new HashSet<string>();
    private static HashSet<string> skippedScenarios = new HashSet<string>();


    public static void MarkCompleted(string scenarioName)
    {
        completedScenarios.Add(scenarioName);
    }

    public static void MarkSkipped(string scenarioName)
    {
        skippedScenarios.Add(scenarioName);
    }

    public static bool WasSkipped(string scenarioName)
    {
        return skippedScenarios.Contains(scenarioName);
    }


    public static bool IsCompleted(string scenarioName)
    {
        return completedScenarios.Contains(scenarioName);
    }

    public static void ResetAll()
    {
        completedScenarios.Clear();
    }
}