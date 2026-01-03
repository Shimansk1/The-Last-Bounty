using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Reference")]
    public Dropdown qualityDropdown;
    public Slider musicSlider;
    public Toggle fullscreenToggle;

    [Header("Audio")]
    public AudioMixer audioMixer;

    private void Start()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string> { "Low", "Medium", "High", "Ultra" });

        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        float volume;
        audioMixer.GetFloat("MusicVolume", out volume);
        musicSlider.value = Mathf.InverseLerp(-80f, 0f, volume); 

        fullscreenToggle.isOn = Screen.fullScreen;

        qualityDropdown.onValueChanged.AddListener(SetQuality);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Kvalita zmìnìna na: " + qualityIndex);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float volume = Mathf.Lerp(-40f, 0f, sliderValue); 
        audioMixer.SetFloat("MusicVolume", volume);
        Debug.Log("Hlasitost hudby: " + volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log("Fullscreen: " + isFullscreen);
    }
}
