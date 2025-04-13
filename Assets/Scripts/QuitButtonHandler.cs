using UnityEngine;

public class QuitButtonHandler : MonoBehaviour
{
    // This method quits the game when called.
    public void QuitGame()
    {
        Debug.Log("Quit button pressed. Exiting game...");

        // If running in the Unity Editor, stop play mode
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // In a built game, Application.Quit() terminates the application
            Application.Quit();
        #endif
    }
}
