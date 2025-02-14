using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.Audio; // Import Audio namespace

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown GraphicsQuality;
    public Slider MasterVolume;
    public Slider MusicVolume;
    public Slider SfxVolume;
    public AudioMixer audioMixer; // Reference to the Audio Mixer
    public Toggle FullscreenToggle; // Reference to the fullscreen toggle

    void Start()
    {
        // Load settings when the game starts
        LoadSettings();

        // Add listeners to update settings when the user changes values
        MasterVolume.onValueChanged.AddListener(delegate { OnMasterVolumeChanged(); });
        MusicVolume.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(); });
        SfxVolume.onValueChanged.AddListener(delegate { OnSfxVolumeChanged(); });
        GraphicsQuality.onValueChanged.AddListener(delegate { OnGraphicsQualityChanged(); });
        FullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggleChanged(); });
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume.value);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume.value);
        PlayerPrefs.SetFloat("SfxVolume", SfxVolume.value);
        PlayerPrefs.SetInt("GraphicsQuality", GraphicsQuality.value);
        PlayerPrefs.SetInt("Fullscreen", FullscreenToggle.isOn ? 1 : 0);

        PlayerPrefs.Save(); // Ensure settings are saved immediately
        Debug.Log("Settings saved.");
    }

    public void LoadSettings()
    {
        // Load the values from PlayerPrefs
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            MasterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
            ApplyMasterVolume(MasterVolume.value); // Apply immediately
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
            ApplyMusicVolume(MusicVolume.value); // Apply immediately
        }
        if (PlayerPrefs.HasKey("SfxVolume"))
        {
            SfxVolume.value = PlayerPrefs.GetFloat("SfxVolume");
            ApplySfxVolume(SfxVolume.value); // Apply immediately
        }
        if (PlayerPrefs.HasKey("GraphicsQuality"))
        {
            GraphicsQuality.value = PlayerPrefs.GetInt("GraphicsQuality");
            ApplyGraphicsQuality(GraphicsQuality.value); // Apply immediately
        }
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            FullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
            ApplyFullscreen(FullscreenToggle.isOn); // Apply immediately
        }

        // Apply settings once loaded
        ApplySettings();

        Debug.Log("Settings loaded.");
    }

    public void OnMasterVolumeChanged()
    {
        ApplyMasterVolume(MasterVolume.value);
        SaveSettings();
    }

    public void OnMusicVolumeChanged()
    {
        ApplyMusicVolume(MusicVolume.value);
        SaveSettings();
    }

    public void OnSfxVolumeChanged()
    {
        ApplySfxVolume(SfxVolume.value);
        SaveSettings();
    }

    public void OnGraphicsQualityChanged()
    {
        ApplyGraphicsQuality(GraphicsQuality.value);
        SaveSettings();
    }

    public void OnFullscreenToggleChanged()
    {
        ApplyFullscreen(FullscreenToggle.isOn);
        SaveSettings();
    }

    void ApplyMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        Debug.Log("Master volume changed to: " + volume);
    }

    void ApplyMusicVolume(float volume)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMusicVolume(volume);
        }
        Debug.Log("Music volume changed to: " + volume);
    }

    void ApplySfxVolume(float volume)
    {
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(volume) * 20);
        Debug.Log("SFX volume changed to: " + volume);
    }

    void ApplyGraphicsQuality(int qualityLevel)
    {
        QualitySettings.SetQualityLevel(qualityLevel);
        Debug.Log("Graphics quality changed to: " + qualityLevel);
    }

    void ApplyFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log("Fullscreen mode changed to: " + isFullscreen);
    }

    void ApplySettings()
    {
        // Apply all settings after loading
        ApplyMasterVolume(MasterVolume.value);
        ApplyMusicVolume(MusicVolume.value);
        ApplySfxVolume(SfxVolume.value);
        ApplyGraphicsQuality(GraphicsQuality.value);
        ApplyFullscreen(FullscreenToggle.isOn);
    }
}
