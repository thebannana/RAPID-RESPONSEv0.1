using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnHover : MonoBehaviour
{
    public AudioClip hoverSound;
    public AudioClip hoverEndSound;

    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to the GameObject
        audioSource = GetComponent<AudioSource>();
        // Make sure to disable play on awake
        audioSource.playOnAwake = false;
    }

    void OnMouseEnter()
    {
        // Play the hover sound
        audioSource.clip = hoverSound;
        audioSource.Play();
    }

    void OnMouseExit()
    {
        // Play the hover end sound
        audioSource.clip = hoverEndSound;
        audioSource.Play();
    }
}
