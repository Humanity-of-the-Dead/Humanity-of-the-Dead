using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class AudioVolumeManager : MonoBehaviour
{

    public AudioMixer audioMixer; // Reference to the main AudioMixer
    public Slider bgmSlider;
    public Slider seSlider;
    public Slider uiSlider;
    public AudioSource BGM;
    public AudioSource SE;

    public float initial_BGM = 0.5f;
    public float initial_SE = 0.5f;
    public float initial_UI= 0.5f;

    private const string BGM_PREF_KEY = "BGM_Volume";
    private const string SE_PREF_KEY = "SE_Volume";
    private const string UI_PREF_KEY = "UI_Volume";

    private void Start()
    {
        BGM = GameObject.FindWithTag("BGM").GetComponent<AudioSource>();
        SE = GameObject.FindWithTag("SE").GetComponent<AudioSource>();

        // Set slider values from saved preferences or default to 1.0f
        bgmSlider.value = PlayerPrefs.GetFloat(BGM_PREF_KEY, initial_BGM);
        seSlider.value = PlayerPrefs.GetFloat(SE_PREF_KEY, initial_SE);
        uiSlider.value = PlayerPrefs.GetFloat(UI_PREF_KEY, initial_UI);

        // Set up slider listeners to update and save volume changes
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        seSlider.onValueChanged.AddListener(SetSEVolume);
        uiSlider.onValueChanged.AddListener(SetUIVolume);
        BGM.volume=bgmSlider.value; 
        SE.volume = seSlider.value;

    }
    private void SetBGMVolume(float value) => SetVolume(BGM_PREF_KEY, "BGM_Volume", value);
    private void SetSEVolume(float value) => SetVolume(SE_PREF_KEY, "SE_Volume", value);
    private void SetUIVolume(float value) => SetVolume(UI_PREF_KEY, "UI_Volume", value);

    // Method to set volume and save to PlayerPrefs
    public void SetVolume(string prefKey, string exposedParam, float volume)
    {
        float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dbVolume);
        PlayerPrefs.SetFloat(prefKey, volume);
        PlayerPrefs.Save();

        // BGM 音量を設定

        if (BGM != null && PlayerPrefs.HasKey(BGM_PREF_KEY))
        {
            BGM.volume = PlayerPrefs.GetFloat(BGM_PREF_KEY);
        }

        // SE 音量を設定
      if(SE != null && PlayerPrefs.HasKey(UI_PREF_KEY)&&SE.outputAudioMixerGroup == MultiAudio.ins.uiMixerGroup)
        {
            SE.volume= PlayerPrefs.GetFloat(UI_PREF_KEY);
        }

        if (SE != null && PlayerPrefs.HasKey(SE_PREF_KEY) && SE.outputAudioMixerGroup == MultiAudio.ins.seMixerGroup)
        {
            SE.volume = PlayerPrefs.GetFloat(SE_PREF_KEY);
        }

    }

    private void OnDestroy()
    {
        // Remove listeners to prevent memory leaks
        bgmSlider.onValueChanged.RemoveListener(value => SetVolume(BGM_PREF_KEY, "BGM_Volume", value));
        seSlider.onValueChanged.RemoveListener(value => SetVolume(SE_PREF_KEY, "SE_Volume", value));
        uiSlider.onValueChanged.RemoveListener(value => SetVolume(UI_PREF_KEY, "UI_Volume", value));
    }
}



