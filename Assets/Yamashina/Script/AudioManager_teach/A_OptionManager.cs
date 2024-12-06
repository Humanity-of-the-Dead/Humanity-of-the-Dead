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

    [Header("Master音量")]
    [Tooltip("Floatで入力")]

    public float MasterVolume;
    public float GetVolume(string volumeType)
    {
        switch (volumeType)
        {
            case "BGM": return BGMVolume;
            case "SE": return SEVolume;
            case "Master": return MasterVolume;
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
            case "Master":
                MasterVolume = value;
                break;
            default:
                throw new ArgumentException("Invalid volume type");
        }
    }
}
[System.Serializable]

public class DisplaySpeedSettings
{
    [Header("テキスト表示速度の設定")]
    [Tooltip("テキスト表示速度: 低速\nfloatで設定、おそらく0.3で適正")]
    [Range(0, 2)]

    public float SlowSpeed = 0.3f;

    [Tooltip("テキスト表示速度: 中速\nfloatで設定,おそらく0.2で適正")]
    [Range(0, 2)]

    public float MediumSpeed = 0.2f;

    [Tooltip("テキスト表示速度: 高速\nfloatで設定,0.05～0.1の間が適正")]
    [Range(0, 2)]

    public float FastSpeed = 0.1f;

}


public class A_OptionManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;
    [SerializeField] Slider MasterVolumeSlider;
    [SerializeField] Slider DisplaySpeedSlider;

    public DisplaySpeedSettings displaySpeedSettings;
    [Header("初期音量設定")]

    public AudioSettings initialAudioSettings;

    private const string BGMVolumeKey = "BGMVolume";
    private const string MasterVolumeKey = "MasterVolume";
    private const string SEVolumeKey = "SEVolume";
    private const string DisplaySpeedKey = "DisplaySpeed";




    private void Start()
    {
        // AudioSourceの初期化
        Audiovolume.InitializeAudioSources();

        // 初期設定の音量をAudioSourceに反映
        float savedBGMVolume = PlayerPrefs.GetFloat(BGMVolumeKey, initialAudioSettings.GetVolume("BGM"));
        SetVolume("BGM", savedBGMVolume);

        float savedSEVolume = PlayerPrefs.GetFloat(SEVolumeKey, initialAudioSettings.GetVolume("SE"));
        SetVolume("SE", savedSEVolume);

        float savedMasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, initialAudioSettings.GetVolume("Master"));
        SetVolume("Master", savedMasterVolume);

        // 表示速度も初期設定に反映
        int savedSpeedIndex = PlayerPrefs.GetInt(DisplaySpeedKey, 1);
        //T_ScenarioManager.displaySpeed = displaySpeedSettings.MediumSpeed; // デフォルト値設定
        SetDisplaySpeed(savedSpeedIndex); // スライダーの値を使って設定

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
        PlayerPrefs.DeleteKey(MasterVolumeKey);
        PlayerPrefs.DeleteKey(SEVolumeKey);
        PlayerPrefs.DeleteKey(DisplaySpeedKey);
        PlayerPrefs.Save(); // 変更を保存

        // スライダーをデフォルト値に戻す
        BGMSlider.value = initialAudioSettings.GetVolume("BGM");
        MasterVolumeSlider.value = initialAudioSettings.GetVolume("Master");
        SESlider.value = initialAudioSettings.GetVolume("SE");
        DisplaySpeedSlider.value = 1;  // 中速がデフォルト

        // オーディオと表示速度のデフォルト設定を適用
        SetVolume("BGM", initialAudioSettings.GetVolume("BGM"));
        SetVolume("Master", initialAudioSettings.GetVolume("Master"));
        SetVolume("SE", initialAudioSettings.GetVolume("SE"));
        SetDisplaySpeed(1); // 中速
    }

    private void InitializeSliders()
    {
        // BGMスライダー
        float BGMVolume = PlayerPrefs.GetFloat(BGMVolumeKey, initialAudioSettings.GetVolume("BGM"));
        BGMSlider.value = BGMVolume;
        BGMSlider.onValueChanged.RemoveAllListeners();
        BGMSlider.onValueChanged.AddListener(value => SetVolume("BGM", value));

        // MasterVolumeスライダー
        float MasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, initialAudioSettings.GetVolume("Master"));
        MasterVolumeSlider.value = MasterVolume;
        MasterVolumeSlider.onValueChanged.RemoveAllListeners();
        MasterVolumeSlider.onValueChanged.AddListener(value => SetVolume("Master", value));

        // SEスライダー
        float SEVolume = PlayerPrefs.GetFloat(SEVolumeKey, initialAudioSettings.GetVolume("SE"));
        SESlider.value = SEVolume;
        SESlider.onValueChanged.RemoveAllListeners();
        SESlider.onValueChanged.AddListener(value => SetVolume("SE", value));

        // 表示速度スライダー
        DisplaySpeedSlider.minValue = 0;
        DisplaySpeedSlider.maxValue = 2;
        DisplaySpeedSlider.wholeNumbers = true;
        DisplaySpeedSlider.value = PlayerPrefs.GetInt(DisplaySpeedKey, 1); // デフォルトは1（中速）
        DisplaySpeedSlider.onValueChanged.RemoveAllListeners();
        DisplaySpeedSlider.onValueChanged.AddListener(SetDisplaySpeed);
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

        initialAudioSettings.SetVolume(volumeType, volume); // AudioSettingsにも保存
        PlayerPrefs.SetFloat(volumeType + "Volume", volume); // PlayerPrefsに保存
        PlayerPrefs.Save();
    }

    // テキスト表示速度の変更
    public void SetDisplaySpeed(float sliderValue)
    {
        int speedIndex = Mathf.RoundToInt(sliderValue);
        float speed = speedIndex switch
        {
            0 => displaySpeedSettings.SlowSpeed,
            1 => displaySpeedSettings.MediumSpeed,
            2 => displaySpeedSettings.FastSpeed,
            _ => displaySpeedSettings.MediumSpeed
        };

        //T_ScenarioManager.displaySpeed = speed;
        PlayerPrefs.SetInt(DisplaySpeedKey, speedIndex);
        PlayerPrefs.Save();
    }

    // 音量をデシベルに変換
    private float ChangeVolumeToDB(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        return Mathf.Log10(volume) * 20f;
    }
}


