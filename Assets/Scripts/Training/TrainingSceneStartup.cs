using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingSceneStartup : MonoBehaviour
{
    void Start() 
    { 
        TrainingScenarioCoordinator.instance.StartTrainingScenario("TrainingScene", null); 
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            // Unsubscribe from the event and destroy this object
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
    }
}
