using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioVolumeManager : MonoBehaviour
{
    public AudioMixer audioMixer; // MainAudioMixer�����蓖��
    public Slider bgmSlider;
    public Slider seSlider;
    public Slider uiSlider;

    private void Start()
    {
        // �X���C�_�[�̏����l��PlayerPrefs����擾�܂��̓f�t�H���g1.0f�Őݒ�
        bgmSlider.value = PlayerPrefs.GetFloat("BGM_Volume", 1.0f);
        seSlider.value = PlayerPrefs.GetFloat("SE_Volume", 1.0f);
        uiSlider.value = PlayerPrefs.GetFloat("UI_Volume", 1.0f);

        // �X���C�_�[�ύX���ɉ��ʂ��X�V���A�ۑ�
        bgmSlider.onValueChanged.AddListener(value => SetVolume("BGM_Volume", value));
        seSlider.onValueChanged.AddListener(value => SetVolume("SE_Volume", value));
        uiSlider.onValueChanged.AddListener(value => SetVolume("UI_Volume", value));
    }

    // �{�����[����dB�ɕϊ����ă~�L�T�[�ɐݒ�
    private void SetVolume(string exposedParam, float volume)
    {
        float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dbVolume);

        // �{�����[���ݒ��ۑ�
        PlayerPrefs.SetFloat(exposedParam, volume);
    }
    private void Update()
    {
        Debug.Log(uiSlider.value);
        Debug.Log(seSlider.value);


    }

    private void OnDestroy()
    {
        // �X���C�_�[�̃��X�i�[����
        bgmSlider.onValueChanged.RemoveListener(value => SetVolume("BGM_Volume", value));
        seSlider.onValueChanged.RemoveListener(value => SetVolume("SE_Volume", value));
        uiSlider.onValueChanged.RemoveListener(value => SetVolume("UI_Volume", value));
    }
}


