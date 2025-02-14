using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnHover : MonoBehaviour
{
    public GameObject targetObject; // Reference to the object to be rotated
    public float rotationSpeed = 45f; // Speed of rotation in degrees per second
    public float targetRotation = 90f; // Target rotation angle when the mouse hovers over the object

    private Quaternion originalRotation; // Original rotation of the target object
    private Quaternion targetRotationQuaternion; // Target rotation quaternion when the mouse hovers over the object
    private bool isRotating = false; // Flag to indicate if the object is currently rotating

    private void Start()
    {
        // Store the original rotation of the target object
        originalRotation = targetObject.transform.rotation;

        // Calculate the target rotation quaternion
        targetRotationQuaternion = Quaternion.Euler(0, targetRotation, 0) * originalRotation;
    }

    private void OnMouseEnter()
    {
        // Start rotating the target object smoothly when the mouse hovers over the empty object
        isRotating = true;
    }

    private void OnMouseExit()
    {
        // Stop rotating the target object smoothly when the mouse exits the empty object
        isRotating = false;
    }

    private void Update()
    {
        // Smoothly rotate the target object if the isRotating flag is true
        if (isRotating)
        {
            RotateObjectSmoothly();
        }
        else
        {
            // If not rotating, smoothly rotate back to the original rotation
            targetObject.transform.rotation = Quaternion.RotateTowards(targetObject.transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void RotateObjectSmoothly()
    {
        // Smoothly rotate the target object towards the target rotation quaternion
        targetObject.transform.rotation = Quaternion.RotateTowards(targetObject.transform.rotation, targetRotationQuaternion, rotationSpeed * Time.deltaTime);
    }
}












