using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;




public class MultiAudio : MonoBehaviour
{
    public AudioClip[] audioClipsBGM; // BGM����
    public AudioClip[] audioClipSE;   // SE����

    private AudioSource bgmSource;
    private AudioSource seSource;

    // Audio Mixer��Inspector����A�^�b�`
    public AudioMixerGroup bgmMixerGroup;
    public AudioMixerGroup seMixerGroup;
    public AudioMixerGroup uiMixerGroup;

    private void Start()
    {
        // BGM�����SE��AudioSource���擾
        bgmSource = GameObject.FindWithTag("BGM").GetComponent<AudioSource>();
        seSource = GameObject.FindWithTag("SE").GetComponent<AudioSource>();

        // �eAudioSource�Ƀf�t�H���g�̃~�L�T�[�O���[�v�����蓖�Ă�
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

        // �����̑I��
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

        // �������ݒ肳�ꂽ���Ƃ��m�F
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

            // SE�������Z�b�g
            seSource.clip = audioClipSE[num];

            // UI�J�e�S���̉���������
            if (audioClipSE[num].name.Contains("UI"))
            {
                // UI�J�e�S���ł����UI�~�L�T�[�O���[�v�ɐݒ�
                seSource.outputAudioMixerGroup = uiMixerGroup;
                Debug.Log(seSource.outputAudioMixerGroup);

            }
            else
            {
                // �ʏ��SE�J�e�S���ł����SE�~�L�T�[�O���[�v�ɐݒ�
                seSource.outputAudioMixerGroup = seMixerGroup;
                Debug.Log(seSource.outputAudioMixerGroup);
            }

            seSource.PlayOneShot(audioClipSE[num]);
        }
    }
}


