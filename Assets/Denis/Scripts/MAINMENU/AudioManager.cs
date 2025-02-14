using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer audioMixer; // Reference to the Audio Mixer
    public AudioSource musicSource; // Reference to the AudioSource for music
    public string[] musicScenes; // Scenes where music should play

    void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return; // Exit early to prevent further initialization
        }

        // Check initial scene
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (musicSource == null)
        {
            Debug.LogError("MusicSource is not assigned in the AudioManager.");
            return;
        }

        bool isMusicScene = false;
        foreach (string musicScene in musicScenes)
        {
            if (scene.name == musicScene)
            {
                isMusicScene = true;
                break;
            }
        }

        if (isMusicScene && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
        else if (!isMusicScene && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer is not assigned in the AudioManager.");
            return;
        }

        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
}
