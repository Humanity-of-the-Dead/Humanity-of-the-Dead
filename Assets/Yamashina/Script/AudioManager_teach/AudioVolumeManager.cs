using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioVolumeManager : MonoBehaviour
{
    public AudioMixer audioMixer; // MainAudioMixerを割り当て
    public Slider bgmSlider;
    public Slider seSlider;
    public Slider uiSlider;

    private void Start()
    {
        // スライダーの初期値をPlayerPrefsから取得またはデフォルト1.0fで設定
        bgmSlider.value = PlayerPrefs.GetFloat("BGM_Volume", 1.0f);
        seSlider.value = PlayerPrefs.GetFloat("SE_Volume", 1.0f);
        uiSlider.value = PlayerPrefs.GetFloat("UI_Volume", 1.0f);

        // スライダー変更時に音量を更新し、保存
        bgmSlider.onValueChanged.AddListener(value => SetVolume("BGM_Volume", value));
        seSlider.onValueChanged.AddListener(value => SetVolume("SE_Volume", value));
        uiSlider.onValueChanged.AddListener(value => SetVolume("UI_Volume", value));
    }

    // ボリュームをdBに変換してミキサーに設定
    private void SetVolume(string exposedParam, float volume)
    {
        float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dbVolume);

        // ボリューム設定を保存
        PlayerPrefs.SetFloat(exposedParam, volume);
    }
    private void Update()
    {
        Debug.Log(uiSlider.value);
        Debug.Log(seSlider.value);


    }

    private void OnDestroy()
    {
        // スライダーのリスナー解除
        bgmSlider.onValueChanged.RemoveListener(value => SetVolume("BGM_Volume", value));
        seSlider.onValueChanged.RemoveListener(value => SetVolume("SE_Volume", value));
        uiSlider.onValueChanged.RemoveListener(value => SetVolume("UI_Volume", value));
    }
}


