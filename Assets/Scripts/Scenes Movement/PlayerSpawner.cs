using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;

    void Start()
    {
        if (GameManager.instance != null && PlayerMovement.instance != null)
        {
            if (GameManager.instance.entryDirection == "Left")
                PlayerMovement.instance.ResetMovement(leftSpawnPoint.position);
            else
                PlayerMovement.instance.ResetMovement(rightSpawnPoint.position);
        }
    }
}
