using UnityEngine;

public class LightColorAndIntensityChanger : MonoBehaviour
{
    public Light targetLight;      // Reference to the light to be modified
    public float maxIntensity = 1f; // The maximum intensity
    public float cycleDuration = 2f; // Duration of one complete cycle from red to blue and back to red

    private float cycleTimer = 0f;

    void Update()
    {
        if (targetLight == null) return;

        // Update the cycle timer
        cycleTimer += Time.deltaTime;
        if (cycleTimer > cycleDuration) cycleTimer -= cycleDuration;

        // Calculate the normalized cycle position (0 to 1)
        float cyclePosition = cycleTimer / cycleDuration;

        // Interpolate color from red to blue and back
        if (cyclePosition < 0.5f)
        {
            // From 0 to 0.5, interpolate from red to blue
            targetLight.color = Color.Lerp(Color.red, Color.blue, cyclePosition * 2f);
        }
        else
        {
            // From 0.5 to 1, interpolate from blue back to red
            targetLight.color = Color.Lerp(Color.blue, Color.red, (cyclePosition - 0.5f) * 2f);
        }

        // Interpolate intensity from 0 to maxIntensity and back
        if (cyclePosition < 0.5f)
        {
            // From 0 to 0.5, increase intensity from 0 to maxIntensity
            targetLight.intensity = Mathf.Lerp(0f, maxIntensity, cyclePosition * 2f);
        }
        else
        {
            // From 0.5 to 1, decrease intensity from maxIntensity back to 0
            targetLight.intensity = Mathf.Lerp(maxIntensity, 0f, (cyclePosition - 0.5f) * 2f);
        }
    }
}
