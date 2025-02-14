using UnityEngine;

public class EmissionChanger : MonoBehaviour
{
    public Renderer objectRenderer; // Reference to the object's renderer
    public float changeSpeed = 1.0f; // Speed at which the color changes

    private Material objectMaterial;
    private Color startColor = Color.black;
    private Color endColor = Color.gray;

    void Start()
    {
        // Get the material of the object
        objectMaterial = objectRenderer.material;
        // Ensure the shader supports emission
        objectMaterial.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // Calculate t using a sine wave for smooth back and forth transition
        float t = (Mathf.Sin(Time.time * changeSpeed) + 1.0f) / 2.0f;

        // Lerp the emission color
        Color emissionColor = Color.Lerp(startColor, endColor, t);
        objectMaterial.SetColor("_EmissionColor", emissionColor);

        // Update the emission property
        DynamicGI.SetEmissive(objectRenderer, emissionColor);
    }
}
