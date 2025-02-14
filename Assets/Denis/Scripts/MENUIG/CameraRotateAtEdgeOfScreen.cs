using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float maxRotationAngle = 20f; // Maximum rotation angle in degrees
    public float rotationSpeed = 2f; // Rotation speed

    private float minRotationAngle; // Minimum rotation angle in degrees

    void Start()
    {
        // Calculate the minimum rotation angle by subtracting maxRotationAngle from 360
        minRotationAngle = 360f - maxRotationAngle;
    }

    void Update()
    {
        // Get the horizontal position of the mouse relative to the screen width (-1 to 1)
        float mouseXNormalized = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;

        // Calculate the target rotation angle based on the mouse position within the range
        float targetRotationAngle = Mathf.Lerp(-maxRotationAngle, maxRotationAngle, mouseXNormalized);

        // Clamp the rotation angle within the specified range (-30 to 30 degrees)
        targetRotationAngle = Mathf.Clamp(targetRotationAngle, -30f, 30f);

        // Smoothly rotate the object towards the target rotation angle
        float currentRotationAngle = transform.localEulerAngles.y;
        float newRotationAngle = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, Time.deltaTime * rotationSpeed);

        // Apply the new rotation to the object
        transform.localEulerAngles = new Vector3(0f, newRotationAngle, 0f);
    }
}









