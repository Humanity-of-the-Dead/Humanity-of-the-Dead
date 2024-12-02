using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class MultiAudio : MonoBehaviour
{



    public AudioClip[] audioClipsBGM; // Array for multiple BGM clips
    public AudioClip[] audioClipsSE;  // Array for multiple SE clips

    public AudioSource bgmSource;
    public AudioSource seSource;

    // Audio Mixer Groups to assign different mixer settings
    public AudioMixerGroup bgmMixerGroup;
    public AudioMixerGroup seMixerGroup;
    public AudioMixerGroup uiMixerGroup;
    //BGMのオーディオクリップ

    private Dictionary<string, AudioClip> sEClipDictionary;
    private Dictionary<string, AudioClip> BGMClipDictionary;

    //シングルトン
    public static MultiAudio ins;

    private void Awake()
    {
        //シングルトン化
        if (ins != null)
        {
            Destroy(gameObject);
        }
        else
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        //\シングルトン化
    }




    private void Start()
    {
        // bgmSourceの初期化確認
        bgmSource = GameObject.Find("AudioSet")?.transform.Find("BGM")?.GetComponent<AudioSource>();

        if (bgmSource == null)
        {
            Debug.LogError("BGM AudioSource is not assigned or found. Please ensure that 'AudioSet' and 'BGM' exist in the scene.");
        }

        // seSourceの初期化確認
        seSource = GameObject.FindWithTag("SE")?.GetComponent<AudioSource>();
        if (seSource == null)
        {
            Debug.LogError("SE AudioSource not found. Ensure the tag 'SE' is correctly set.");
        }

        // audioClipsBGMの初期化確認
        if (audioClipsBGM == null || audioClipsBGM.Length == 0)
        {
            Debug.LogError("audioClipsBGM array is not initialized or is empty.");
            return; // audioClipsBGMが空の場合、処理を中断
        }

        // BGMClipDictionaryの初期化
        BGMClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClipsBGM)
        {
            BGMClipDictionary[clip.name] = clip;
        }

        // SEクリップ辞書の初期化
        sEClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClipsSE)
        {
            if (clip != null)
            {
                sEClipDictionary[clip.name] = clip;
            }
        }
    }

    // Method to play a selected BGM by index
    public void ChooseSongs_BGM(int index)
    {
        if (index >= 0 && index < audioClipsBGM.Length)
        {
            bgmSource.clip = audioClipsBGM[index];
            if (bgmSource.clip != null)
            {
                bgmSource.Play();
                Debug.Log("Playing BGM: " + bgmSource.clip.name);
            }
            else
            {
                Debug.LogWarning("BGM clip not set.");
            }
        }
        else
        {
            Debug.LogWarning("BGM index out of range.");
        }
    }
    public void PlayBGM_ByName(string bgmName)
    {
        if (BGMClipDictionary == null)
        {
            Debug.LogError("BGMClipDictionary is not initialized.");
            return;
        }

        if (BGMClipDictionary.TryGetValue(bgmName, out var clip))
        {
            PlayBGM(clip);
            Debug.Log($"Playing BGM: {bgmName}");
        }
        else
        {
            Debug.LogWarning("BGM with name not found: " + bgmName);
        }
        // AudioSourceに設定して再生


    }
    public void PlaySEByName(string name)
    {
        if (sEClipDictionary.TryGetValue(name, out var clip))
        {
            PlaySE(clip);
        }
        else
        {
            Debug.LogWarning("SE with name not found: " + name);
        }
    }
    private void PlaySE(AudioClip clip)
    {
        if (clip != null)
        {
            seSource.clip = clip;

            if (clip.name.StartsWith("UI"))
            {
                seSource.outputAudioMixerGroup = uiMixerGroup;
            }
            else
            {
                seSource.outputAudioMixerGroup = seMixerGroup;
            }

            seSource.PlayOneShot(seSource.clip);
            Debug.Log("Playing SE: " + clip.name);
        }
        else
        {
            Debug.LogWarning("SE clip is null");
        }
    }
    private void PlayBGM(AudioClip clip)
    {
        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true; // ループ再生
            bgmSource.Play();

        }
        else
        {
            Debug.LogWarning("BGMs clip is null");
        }
    }
    // Method to play a selected SE by index
    public void ChooseSongs_SE(int index)
    {
        if (index >= 0 && index < audioClipsSE.Length)
        {
            PlaySE(audioClipsSE[index]);
        }
        else
        {
            Debug.LogWarning("SE index out of range");
        }
    }


}
