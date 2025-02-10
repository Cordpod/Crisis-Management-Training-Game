using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 targetPosition;
    private bool isMoving = false;

    public static PlayerMovement instance; // Singleton Pattern

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the player across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    void Update()
    {
        HandleInput();
        MoveCharacter();
    }

    void HandleInput()
    {
        // For Mouse (PC Testing)
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DetermineDirection(clickPosition);
        }

        // For Touch (Android)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                DetermineDirection(touchPosition);
            }
        }
    }

    void DetermineDirection(Vector2 inputPosition)
    {
        // Check if the tap/click is on the left or right half of the screen
        if (inputPosition.x < 0)
            targetPosition = transform.position + Vector3.left;  // Move Left
        else
            targetPosition = transform.position + Vector3.right; // Move Right

        isMoving = true;
    }

    void MoveCharacter()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if ((Vector2)transform.position == targetPosition)
                isMoving = false;
        }
    }

    public void ResetMovement(Vector2 newPosition)
    {
        transform.position = newPosition;  // Move the player to the spawn point
        targetPosition = newPosition;      // Reset the target position
        isMoving = false;                  // Stop any ongoing movement
    }

}
