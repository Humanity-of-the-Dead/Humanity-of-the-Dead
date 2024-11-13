using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioVolumeManager : MonoBehaviour
{

    public AudioMixer audioMixer; // Reference to the main AudioMixer
    public Slider bgmSlider;
    public Slider seSlider;
    public Slider uiSlider;

    // Keys for saving volume preferences
    private const string BGM_PREF_KEY = "BGM_Volume";
    private const string SE_PREF_KEY = "SE_Volume";
    private const string UI_PREF_KEY = "UI_Volume";

    private void Start()
    {
        // Set slider values from saved preferences or default to 1.0f
        bgmSlider.value = PlayerPrefs.GetFloat(BGM_PREF_KEY, 1.0f);
        seSlider.value = PlayerPrefs.GetFloat(SE_PREF_KEY, 1.0f);
        uiSlider.value = PlayerPrefs.GetFloat(UI_PREF_KEY, 1.0f);

        // Set up slider listeners to update and save volume changes
        bgmSlider.onValueChanged.AddListener(value => SetVolume(BGM_PREF_KEY, "BGM_Volume", value));
        seSlider.onValueChanged.AddListener(value => SetVolume(SE_PREF_KEY, "SE_Volume", value));
        uiSlider.onValueChanged.AddListener(value => SetVolume(UI_PREF_KEY, "UI_Volume", value));
    }

    // Method to set volume and save to PlayerPrefs
    private void SetVolume(string prefKey, string exposedParam, float volume)
    {
        float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dbVolume);
        PlayerPrefs.SetFloat(prefKey, volume);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // Remove listeners to prevent memory leaks
        bgmSlider.onValueChanged.RemoveListener(value => SetVolume(BGM_PREF_KEY, "BGM_Volume", value));
        seSlider.onValueChanged.RemoveListener(value => SetVolume(SE_PREF_KEY, "SE_Volume", value));
        uiSlider.onValueChanged.RemoveListener(value => SetVolume(UI_PREF_KEY, "UI_Volume", value));
    }
}



