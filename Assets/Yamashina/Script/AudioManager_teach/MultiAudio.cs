using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class MultiAudio : MonoBehaviour
{



    public AudioClip[] audioClipsBGM; // Array for multiple BGM clips
    public AudioClip[] audioClipsSE;  // Array for multiple SE clips

    private AudioSource bgmSource;
    private AudioSource seSource;

    // Audio Mixer Groups to assign different mixer settings
    public AudioMixerGroup bgmMixerGroup;
    public AudioMixerGroup seMixerGroup;
    public AudioMixerGroup uiMixerGroup;
    //BGMのオーディオクリップ

    private Dictionary<string, AudioClip> sEClipDictionary;
    private Dictionary<string , AudioClip> BGMClipDictionary;   

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
        bgmSource = GameObject.FindWithTag("BGM").GetComponent<AudioSource>();
        seSource = GameObject.FindWithTag("SE").GetComponent<AudioSource>();

        // Assign mixer groups to the audio sources
        if (bgmSource != null) bgmSource.outputAudioMixerGroup = bgmMixerGroup;
        if (seSource != null) seSource.outputAudioMixerGroup = seMixerGroup;
        // SEクリップを辞書化して名前でアクセス可能に
        sEClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClipsSE)
        {
            sEClipDictionary[clip.name] = clip;
        }
        BGMClipDictionary = new Dictionary<string , AudioClip>();
        foreach (var clip in audioClipsBGM)
        {
            BGMClipDictionary[clip.name] = clip;
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
        if (BGMClipDictionary.TryGetValue(bgmName, out var clip))
        {
            PlayBGM(clip);
        }
        else
        {
            Debug.LogWarning("BGM with name not found: " + bgmName);
        }
        // AudioSourceに設定して再生
      

        Debug.Log($"Playing BGM: {bgmName}");
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
