using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioVolumeManager : MonoBehaviour
{
    public AudioMixer audioMixer; // Reference to the main AudioMixer
    public Slider bgmSlider;
    public Slider seSlider;
    public Slider uiSlider;
    private AudioSource BGM;
    private AudioSource SE;

    [Header("初期BGMのスライダーの値")]
    [Tooltip("Floatの小数点第１まで入力、0.0～1.0まで")]
    public float initial_BGM = 0.5f;
    [Header("初期SEのスライダーの値")]
    [Tooltip("Floatの小数点第１まで入力、0.0～1.0まで")]
    public float initial_SE = 0.5f;
    [Header("初期UIのスライダーの値")]
    [Tooltip("Floatの小数点第１まで入力、0.0～1.0まで")]
    public float initial_UI = 0.5f;

    private const string BGM_PREF_KEY = "BGM_Volume";
    private const string SE_PREF_KEY = "SE_Volume";
    private const string UI_PREF_KEY = "UI_Volume";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // このオブジェクトをシーン間で保持
    }

    private void Start()
    {
        // AudioSourceの取得
        BGM = GameObject.FindWithTag("BGM")?.GetComponent<AudioSource>();
        SE = GameObject.FindWithTag("SE")?.GetComponent<AudioSource>();

        // PlayerPrefsから音量を取得してスライダーとAudioSourceに反映
        float savedBGMVolume = PlayerPrefs.GetFloat(BGM_PREF_KEY, initial_BGM);
        float savedSEVolume = PlayerPrefs.GetFloat(SE_PREF_KEY, initial_SE);
        float savedUIVolume = PlayerPrefs.GetFloat(UI_PREF_KEY, initial_UI);

        bgmSlider.value = savedBGMVolume;
        seSlider.value = savedSEVolume;
        uiSlider.value = savedUIVolume;

        if (BGM != null) BGM.volume = savedBGMVolume;
        if (SE != null) SE.volume = savedSEVolume;

        // スライダー変更時のリスナーを設定
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        seSlider.onValueChanged.AddListener(SetSEVolume);
        uiSlider.onValueChanged.AddListener(SetUIVolume);

        // シーン遷移時のイベントを登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void SetBGMVolume(float value)
    {
        SetVolume(BGM_PREF_KEY, "BGM_Volume", value);
        if (BGM != null) BGM.volume = value;
    }

    private void SetSEVolume(float value)
    {
        SetVolume(SE_PREF_KEY, "SE_Volume", value);
        if (SE != null) SE.volume = value;
    }

    private void SetUIVolume(float value)
    {
        SetVolume(UI_PREF_KEY, "UI_Volume", value);
    }

    private void SetVolume(string prefKey, string exposedParam, float volume)
    {
        // AudioMixerに設定
        float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dbVolume);

        // PlayerPrefsに保存
        PlayerPrefs.SetFloat(prefKey, volume);
        PlayerPrefs.Save();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーン遷移後にAudioSourceを再取得
        BGM = GameObject.FindWithTag("BGM")?.GetComponent<AudioSource>();
        SE = GameObject.FindWithTag("SE")?.GetComponent<AudioSource>();

        // 再取得したAudioSourceに音量を適用
        if (BGM != null)
        {
            float savedBGMVolume = PlayerPrefs.GetFloat(BGM_PREF_KEY, initial_BGM);
            BGM.volume = savedBGMVolume;
        }

        if (SE != null)
        {
            float savedSEVolume = PlayerPrefs.GetFloat(SE_PREF_KEY, initial_SE);
            SE.volume = savedSEVolume;
        }
    }

    private void OnDestroy()
    {
        // リスナーを解除してメモリリークを防止
        bgmSlider.onValueChanged.RemoveListener(SetBGMVolume);
        seSlider.onValueChanged.RemoveListener(SetSEVolume);
        uiSlider.onValueChanged.RemoveListener(SetUIVolume);

        // シーンロードイベントを解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
