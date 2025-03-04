using UnityEngine;

public class TrainingSceneStartup : MonoBehaviour
{
    void Start()
    {
        ScenarioCoordinator.instance.StartScenario("TrainingScene");
    }
}
