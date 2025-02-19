using UnityEngine;
using UnityEngine.EventSystems;

public class PersistentEventSystem : MonoBehaviour
{
    private static PersistentEventSystem instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep EventSystem across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate EventSystems
        }
    }
}
