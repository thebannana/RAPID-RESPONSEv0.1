using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseEmissionOnHover : MonoBehaviour
{
    public Renderer objectRenderer; // Reference to the Renderer component of the object
    public float desiredEmission = 1.0f; // Desired emission intensity when the mouse hovers over the object

    private Material material; // Reference to the material of the object
    private float originalEmission; // Original emission intensity of the material

    private void Start()
    {
        // Ensure objectRenderer is assigned
        if (objectRenderer == null)
        {
            Debug.LogError("Object Renderer not assigned in IncreaseEmissionOnHover script.");
            return;
        }

        // Get the material from the object's renderer
        material = objectRenderer.material;

        // Store the original emission intensity
        originalEmission = material.GetColor("_EmissionColor").r;
    }

    private void OnMouseEnter()
    {
        // Increase the emission intensity when the mouse hovers over the object
        SetEmissionIntensity(desiredEmission);
    }

    private void OnMouseExit()
    {
        // Reset the emission intensity when the mouse exits the object
        SetEmissionIntensity(originalEmission);
    }

    private void SetEmissionIntensity(float intensity)
    {
        // Set the emission intensity of the material
        Color newEmission = new Color(intensity, intensity, intensity);
        material.SetColor("_EmissionColor", newEmission);

        // Enable emission on the material
        material.EnableKeyword("_EMISSION");
    }
}
