using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
[System.Serializable]




public class AudioSettings
{

    [Header("BGM音量")]
    [Tooltip("Floatで入力")]

    public float BGMVolume;

    [Header("SE音量")]
    [Tooltip("Floatで入力")]

    public float SEVolume;

    [Header("UI音量")]
    [Tooltip("Floatで入力")]

    public float UIVolume;
    public float GetVolume(string volumeType)
    {
        switch (volumeType)
        {
            case "BGM": return BGMVolume;
            case "SE": return SEVolume;
            case "UI": return UIVolume;
            default: throw new ArgumentException("Invalid volume type");
        }
    }

    public void SetVolume(string volumeType, float value)
    {
        switch (volumeType)
        {
            case "BGM":
                BGMVolume = value;
                break;
            case "SE":
                SEVolume = value;
                break;
            case "UI":
                UIVolume = value;
                break;
            default:
                throw new ArgumentException("Invalid volume type");
        }
    }
}
[System.Serializable]



public class A_OptionManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;
    [SerializeField] Slider UIVolumeSlider;

    [Header("初期音量設定")]

    public AudioSettings initialAudioSettings;

    private const string BGMVolumeKey = "BGMVolume";
    private const string UIVolumeKey = "UIVolume";
    private const string SEVolumeKey = "SEVolume";




    private void Start()
    {
        // AudioSourceの初期化
        Audiovolume.InitializeAudioSources();

        // 初期設定の音量をAudioSourceに反映
        float savedBGMVolume = PlayerPrefs.GetFloat(BGMVolumeKey, initialAudioSettings.GetVolume("BGM"));
        SetVolume("BGM", savedBGMVolume);

        float savedSEVolume = PlayerPrefs.GetFloat(SEVolumeKey, initialAudioSettings.GetVolume("SE"));
        SetVolume("SE", savedSEVolume);

        float savedUIVolume = PlayerPrefs.GetFloat(UIVolumeKey, initialAudioSettings.GetVolume("UI"));
        SetVolume("UI", savedUIVolume);

       
        //Debug.Log("Initial Display Speed: " + T_ScenarioManager.displaySpeed);

        InitializeSliders(); // スライダーの初期化
    }

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetSettings();
        }
    }

    private void InitializeAudioSources()
    {
        Audiovolume.InitializeAudioSources();
    }

    public void ResetSettings()
    {
        // 設定をリセット
        PlayerPrefs.DeleteKey(BGMVolumeKey);
        PlayerPrefs.DeleteKey(UIVolumeKey);
        PlayerPrefs.DeleteKey(SEVolumeKey);
        PlayerPrefs.Save(); // 変更を保存

        // スライダーをデフォルト値に戻す
        BGMSlider.value = initialAudioSettings.GetVolume("BGM");
        UIVolumeSlider.value = initialAudioSettings.GetVolume("UI");
        SESlider.value = initialAudioSettings.GetVolume("SE");

        // オーディオと表示速度のデフォルト設定を適用
        SetVolume("BGM", initialAudioSettings.GetVolume("BGM"));
        SetVolume("UI", initialAudioSettings.GetVolume("UI"));
        SetVolume("SE", initialAudioSettings.GetVolume("SE"));
    }

    private void InitializeSliders()
    {
        // BGMスライダー
        float BGMVolume = PlayerPrefs.GetFloat(BGMVolumeKey, initialAudioSettings.GetVolume("BGM"));
        BGMSlider.value = BGMVolume;
        BGMSlider.onValueChanged.RemoveAllListeners();
        BGMSlider.onValueChanged.AddListener(value => SetVolume("BGM", value));

        // UIVolumeスライダー
        float UIVolume = PlayerPrefs.GetFloat(UIVolumeKey, initialAudioSettings.GetVolume("UI"));
        UIVolumeSlider.value = UIVolume;
        UIVolumeSlider.onValueChanged.RemoveAllListeners();
        UIVolumeSlider.onValueChanged.AddListener(value => SetVolume("UI", value));

        // SEスライダー
        float SEVolume = PlayerPrefs.GetFloat(SEVolumeKey, initialAudioSettings.GetVolume("SE"));
        SESlider.value = SEVolume;
        SESlider.onValueChanged.RemoveAllListeners();
        SESlider.onValueChanged.AddListener(value => SetVolume("SE", value));

           }

    // 汎用音量設定メソッド
    public void SetVolume(string volumeType, float volume)
    {
        float dbVolume = ChangeVolumeToDB(volume);
        audioMixer.SetFloat(volumeType + "Volume", dbVolume); // AudioMixerに設定

        // AudioSourceに設定を反映
        if (volumeType == "BGM")
        {
            Audiovolume.SetBgmVolume(volume);
        }
        else if (volumeType == "SE")
        {
            Audiovolume.SetSeVolume(volume);
        }
        else if (volumeType == "UI")
        {
            Audiovolume.SetUIVolume(volume);    
        }

            initialAudioSettings.SetVolume(volumeType, volume); // AudioSettingsにも保存
        PlayerPrefs.SetFloat(volumeType + "Volume", volume); // PlayerPrefsに保存
        PlayerPrefs.Save();
    }

    // テキスト表示速度の変更

    // 音量をデシベルに変換
    private float ChangeVolumeToDB(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        return Mathf.Log10(volume) * 20f;
    }
}


