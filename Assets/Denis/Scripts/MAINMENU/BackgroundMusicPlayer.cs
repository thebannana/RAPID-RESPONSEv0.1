using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusicClip; // The background music audio clip
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Ensure that an AudioSource component is attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        // Check if an AudioSource component doesn't exist, create one and attach it
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the background music clip to the AudioSource
        audioSource.clip = backgroundMusicClip;

        // Set the audio to loop
        audioSource.loop = true;

        // Play the background music
        audioSource.Play();
    }
}
