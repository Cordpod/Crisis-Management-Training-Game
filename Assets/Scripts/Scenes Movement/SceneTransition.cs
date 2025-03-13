using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string targetScene; // Set this in the Inspector
    public string entryDirection; //"Left" or "Right" only

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // Make sure the player GameObject is tagged as "Player"
        {
            GameManager.instance.entryDirection = entryDirection;
            if (targetScene == "StatsScene") // Stop timer only when transitioning to StatsScene
            {
                TimeManager.instance.StopTimer();
            }
            SceneManager.LoadScene(targetScene);
        }
    }
}
