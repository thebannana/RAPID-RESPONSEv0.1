using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityControl : MonoBehaviour
{
    public Light targetLight; // Reference to the light source
    public float minIntensity = 1.5f; // Minimum intensity
    public float maxIntensity = 3f; // Maximum intensity
    public float pulseSpeed = 1f; // Speed of the pulsation

    private bool isHovering = false; // Flag to track if the mouse is hovering over the object
    private bool isPulsating = true; // Flag to track if pulsation is allowed

    void OnMouseEnter()
    {
        isHovering = true;
    }

    void OnMouseExit()
    {
        isHovering = false;
        // Reset the light intensity to the minimum when exiting
        targetLight.intensity = minIntensity;
    }

    void Update()
    {
        // Check if the mouse is hovering over the object and pulsation is allowed
        if (isHovering && isPulsating)
        {
            // Calculate the intensity using SmoothStep for pulsating effect
            float t = Mathf.PingPong(Time.time * pulseSpeed, 1f); // PingPong time between 0 and 1
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, t); // Smoothly interpolate between min and max intensity
            targetLight.intensity = intensity;
        }

        // Check for mouse click
        if (Input.GetMouseButtonDown(0) && isHovering)
        {
            // Stop pulsating when clicked
            isPulsating = false;
        }

        // Check for ESC key press (only if pulsation was previously stopped by clicking)
        if (Input.GetKeyDown(KeyCode.Escape) && !isPulsating)
        {
            // Allow pulsating again
            isPulsating = true;
        }
    }
}





