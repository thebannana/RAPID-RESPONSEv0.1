using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public Image fadeImage; // Reference to the UI Image used for fading
    public float fadeSpeed = 1.0f; // Speed of the fade-in effect

    private void Start()
    {
        // Start the fading effect
        StartCoroutine(FadeInEffect());
    }

    private IEnumerator FadeInEffect()
    {
        // Make sure the fade image is enabled
        fadeImage.gameObject.SetActive(true);

        // Set the initial alpha of the image to fully opaque
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1.0f);

        // Fade the image alpha to fully transparent over time
        while (fadeImage.color.a > 0)
        {
            // Reduce the alpha of the image over time
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImage.color.a - (fadeSpeed * Time.deltaTime));

            // Wait for the next frame
            yield return null;
        }

        // Disable the fade image when the fade-in effect is finished
        fadeImage.gameObject.SetActive(false);
    }
}

