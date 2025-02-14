using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCameraOnClick : MonoBehaviour
{
    public Camera newCamera1; // Reference to the first camera
    public Camera newCamera2; // Reference to the second camera

    private Camera previousCamera; // Reference to the previously active camera

    void Start()
    {
        // Disable all cameras except the one named "Camera"
        foreach (Camera camera in Camera.allCameras)
        {
            if (camera.name != "Camera")
            {
                camera.gameObject.SetActive(false);
            }
            else
            {
                camera.gameObject.SetActive(true);
            }
        }
    }

    void OnMouseDown()
    {
        // Check line of sight before toggling between cameras
        if (HasLineOfSight())
        {
            // Disable the previous camera if it exists
            if (previousCamera != null)
            {
                previousCamera.tag = "Untagged"; // Change the tag of the previous camera to "Untagged"
                previousCamera = null; // Clear the reference to the previous camera
            }

            // Toggle between the cameras
            if (newCamera1.gameObject.activeSelf)
            {
                // Disable the first camera
                newCamera1.gameObject.SetActive(false);

                // Enable the second camera
                newCamera2.gameObject.SetActive(true);
                newCamera2.tag = "MainCamera"; // Set the tag of the second camera to "MainCamera"
            }
            else
            {
                // Disable the second camera
                newCamera2.gameObject.SetActive(false);

                // Enable the first camera
                newCamera1.gameObject.SetActive(true);
                newCamera1.tag = "MainCamera"; // Set the tag of the first camera to "MainCamera"
            }
        }
    }

    bool HasLineOfSight()
    {
        RaycastHit hit;
        Vector3 direction = transform.position - Camera.main.transform.position;

        if (Physics.Raycast(Camera.main.transform.position, direction, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true; // Object clicked has line of sight
            }
        }

        return false; // Object clicked does not have line of sight
    }
}



