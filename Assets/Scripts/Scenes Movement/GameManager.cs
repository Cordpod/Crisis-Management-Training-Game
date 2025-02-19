using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string entryDirection = "Right"; // Default direction

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
