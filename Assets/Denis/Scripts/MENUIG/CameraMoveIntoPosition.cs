using UnityEngine;

public class SmoothCameraMovement : MonoBehaviour
{
    public GameObject evidenceBoard; // Reference to the evidence board GameObject
    public Transform targetLocation; // Reference to the target location where the camera should move
    public GameObject cameraHolder; // Reference to the GameObject holding the camera
    public float movementSpeed = 5f; // Movement speed

    private Vector3 originalCameraPosition; // Original position of the camera
    private Vector3 targetCameraPosition; // Target position for smooth movement
    private bool isMoving = false; // Flag to track if the camera is currently moving
    private bool evidenceBoardPressed = false; // Flag to track if the evidence board has been pressed

    private const float distanceThreshold = 0.1f; // Threshold distance for considering arrival at target

    void Start()
    {
        // Save the original position of the camera
        originalCameraPosition = cameraHolder.transform.position;
    }

    void Update()
    {
        // Check if the camera is moving
        if (isMoving)
        {
            MoveCamera();
        }
        else
        {
            HandleInput();
        }
    }

    void MoveCamera()
    {
        // Calculate the distance and direction to the target location
        Vector3 direction = (targetCameraPosition - cameraHolder.transform.position).normalized;
        float distance = Vector3.Distance(cameraHolder.transform.position, targetCameraPosition);

        // Move the camera towards the target location
        cameraHolder.transform.position += direction * movementSpeed * Time.deltaTime;

        // Check if the camera has reached the target location
        if (distance <= distanceThreshold)
        {
            isMoving = false;
        }
    }

    void HandleInput()
    {
        // Check for input to start moving the camera towards the evidence board
        if (!evidenceBoardPressed && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check if the ray hits a collider
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the collider belongs to the evidence board GameObject
                if (hit.collider.gameObject == evidenceBoard)
                {
                    MoveCameraToTarget(targetLocation.position);
                    evidenceBoardPressed = true; // Evidence board has been pressed
                }
            }
        }

        // Check for ESC key to smoothly return the camera to its original position
        if (evidenceBoardPressed && Input.GetKeyDown(KeyCode.Escape))
        {
            MoveCameraToTarget(originalCameraPosition);
            evidenceBoardPressed = false; // Reset the evidence board pressed flag
        }
    }

    void MoveCameraToTarget(Vector3 targetPosition)
    {
        isMoving = true;
        targetCameraPosition = targetPosition;
    }
}
