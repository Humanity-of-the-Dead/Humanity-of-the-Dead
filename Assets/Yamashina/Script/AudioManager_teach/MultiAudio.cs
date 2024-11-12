using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;




public class MultiAudio : MonoBehaviour
{
    public AudioClip[] audioClipsBGM; // BGM音源
    public AudioClip[] audioClipSE;   // SE音源

    private AudioSource bgmSource;
    private AudioSource seSource;

    // Audio MixerをInspectorからアタッチ
    public AudioMixerGroup bgmMixerGroup;
    public AudioMixerGroup seMixerGroup;
    public AudioMixerGroup uiMixerGroup;

    private void Start()
    {
        // BGMおよびSEのAudioSourceを取得
        bgmSource = GameObject.FindWithTag("BGM").GetComponent<AudioSource>();
        seSource = GameObject.FindWithTag("SE").GetComponent<AudioSource>();

        // 各AudioSourceにデフォルトのミキサーグループを割り当てる
        if (bgmSource != null)
        {
            bgmSource.outputAudioMixerGroup = bgmMixerGroup;
        }

        if (seSource != null)
        {
            seSource.outputAudioMixerGroup = seMixerGroup;
        }
        
    }

    public void ChooseSongs_BGM(int num)
    {
        AudioSource bgmSource = GameObject.FindWithTag("BGM").GetComponent<AudioSource>();

        // 音源の選択
        switch (num)
        {
            case 0:
                Debug.Log("Selecting BGM 0: " + audioClipsBGM[0].name);
                bgmSource.clip = audioClipsBGM[0];
                break;
            case 1:
                Debug.Log("Selecting BGM 1: " + audioClipsBGM[1].name);
                bgmSource.clip = audioClipsBGM[1];
                break;
            case 2:
                Debug.Log("Selecting BGM 2: " + audioClipsBGM[2].name);
                bgmSource.clip = audioClipsBGM[2];
                break;
        }

        // 音源が設定されたことを確認
        if (bgmSource.clip != null)
        {
            Debug.Log("BGM clip set: " + bgmSource.clip.name);
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("Failed to set BGM clip.");
        }
    
}

    public void ChooseSongs_SE(int num)
    {
        if (num >= 0 && num < audioClipSE.Length)
        {

            // SE音源をセット
            seSource.clip = audioClipSE[num];

            // UIカテゴリの音源か判別
            if (audioClipSE[num].name.Contains("UI"))
            {
                // UIカテゴリであればUIミキサーグループに設定
                seSource.outputAudioMixerGroup = uiMixerGroup;
                Debug.Log(seSource.outputAudioMixerGroup);

            }
            else
            {
                // 通常のSEカテゴリであればSEミキサーグループに設定
                seSource.outputAudioMixerGroup = seMixerGroup;
                Debug.Log(seSource.outputAudioMixerGroup);
            }

            seSource.PlayOneShot(audioClipSE[num]);
        }
    }
}


