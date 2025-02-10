using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void ChangetoMainScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
