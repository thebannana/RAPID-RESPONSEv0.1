using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip buttonClickSound; // Sound to play when the button is clicked

    void Start()
    {
        // Ensure that an AudioSource component is assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Ensure that an AudioClip is assigned
        if (buttonClickSound == null)
        {
            Debug.LogError("Button click sound is not assigned!");
        }
    }

    // Method to play the button click sound
    public void PlayButtonClickSound()
    {
        // Play the sound
        audioSource.PlayOneShot(buttonClickSound);
    }
}

