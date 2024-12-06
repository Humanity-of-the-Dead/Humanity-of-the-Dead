using UnityEngine;

public class Audiovolume : MonoBehaviour
{
    public static AudioSource audioSourceBGM; // 現在のシーンのBGM
    public static AudioSource audioSourceSE;  // 現在のシーンのSE (汎用)

    // 文字送りSE用のオーディオソース
    public static AudioSource audioSourceTextSE;

    public static float BGM
    {
        get { return PlayerPrefs.GetFloat(BGM_PREF_KEY, 1.0f); }  // デフォルト値 1.0f
        set { SetBgmVolume(value); }  // プロパティに値を設定すると自動的に音量も更新
    }
    public static float SE
    {
        get { return PlayerPrefs.GetFloat(SE_PREF_KEY, 1.0f); }  // デフォルト値 1.0f
        set { SetSeVolume(value); }  // プロパティに値を設定すると自動的に音量も更新
    }

    // PlayerPrefs に保存されるキー名
    private const string BGM_PREF_KEY = "BGM_VOLUME";
    private const string SE_PREF_KEY = "SE_VOLUME";

    // シーンが読み込まれたときにオーディオソースを初期化
    private void Start()
    {
        InitializeAudioSources();
        // PlayerPrefs から保存されている音量を読み込み適用する
        LoadVolumeSettings();
    }

    // オーディオソースの初期化
    public static void InitializeAudioSources()
    {
        // BGM、SE 用のオーディオソースを取得
        audioSourceBGM = GameObject.FindWithTag("BGM")?.GetComponent<AudioSource>();
        audioSourceSE = GameObject.FindWithTag("SE")?.GetComponent<AudioSource>();

        // 文字送り用SEと草をかき分けるSE用のオーディオソースを取得
        audioSourceTextSE = GameObject.FindWithTag("TextSE")?.GetComponent<AudioSource>();
        // 取得したBGM、SEオーディオソースに音量を適用

        ApplySavedVolumes();
    }

    // BGM音量を変更
    public static void SetBgmVolume(float bgmVolume)
    {
        if (audioSourceBGM != null)
        {
            audioSourceBGM.volume = bgmVolume;
        }
        // 音量を PlayerPrefs に保存

        PlayerPrefs.SetFloat(BGM_PREF_KEY, bgmVolume);
        PlayerPrefs.Save();
        Debug.Log($"BGM Volume set to: {bgmVolume}");
    }

    // SE音量を変更
    public static void SetSeVolume(float seVolume)
    {
        if (audioSourceSE != null)
        {
            audioSourceSE.volume = seVolume;
        }

        if (audioSourceTextSE != null)
        {
            audioSourceTextSE.volume = seVolume;
        }
        // 音量を PlayerPrefs に保存

        PlayerPrefs.SetFloat(SE_PREF_KEY, seVolume);
        PlayerPrefs.Save();
        Debug.Log($"SE Volume set to: {seVolume}");
    }

    // PlayerPrefs から音量設定を読み込み、オーディオソースに適用
    private void LoadVolumeSettings()
    {
        // BGM 音量の読み込み

        if (PlayerPrefs.HasKey(BGM_PREF_KEY))
        {
            float bgmVolume = PlayerPrefs.GetFloat(BGM_PREF_KEY);
            SetBgmVolume(bgmVolume);
        }
        // SE 音量の読み込み

        if (PlayerPrefs.HasKey(SE_PREF_KEY))
        {
            float seVolume = PlayerPrefs.GetFloat(SE_PREF_KEY);
            SetSeVolume(seVolume);
        }
    }

    // 保存された音量設定をオーディオソースに適用
    public static void ApplySavedVolumes()
    {
        // BGM 音量を設定

        if (audioSourceBGM != null && PlayerPrefs.HasKey(BGM_PREF_KEY))
        {
            audioSourceBGM.volume = PlayerPrefs.GetFloat(BGM_PREF_KEY);
        }

        // SE 音量を設定

        if (audioSourceSE != null && PlayerPrefs.HasKey(SE_PREF_KEY))
        {
            audioSourceSE.volume = PlayerPrefs.GetFloat(SE_PREF_KEY);
        }

        //TextSE 音量を設定

        if (audioSourceTextSE != null && PlayerPrefs.HasKey(SE_PREF_KEY))
        {
            audioSourceTextSE.volume = PlayerPrefs.GetFloat(SE_PREF_KEY);
        }

        
    }

    // 文字送りSEを再生
    public static void PlayTextSE()
    {
        if (audioSourceTextSE != null)
        {
           
            if (audioSourceTextSE.isPlaying)
            {
                audioSourceTextSE.Stop();
            }
            else
            {
                audioSourceTextSE.PlayOneShot(audioSourceTextSE.clip);

            }
            Debug.Log(audioSourceSE.isPlaying); 
        }
    }

   
}
