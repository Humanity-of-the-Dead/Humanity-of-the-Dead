using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager_V2: MonoBehaviour
{
    //AudioMixer
    [SerializeField] AudioMixer audMix;

    //BGM�X���C�_
    [SerializeField] Slider bGMSli;
    //SESlider
    [SerializeField]Slider sESli;

    //BGM�̌��݃{�����[��(����0~100)
    float bGMVol;
    //SE�̌��݃{�����[��(����0~100)
    float sEVol;

    //BGM�̃f�V�x��
    float bGMDec;
    //SE�̃f�V�x��
    float sEDec;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    //BGM�X���C�_�̒l���ς���ꂽ�Ƃ�
    public void OnValueChangedBGM()
    {
        //�{�����[���̎擾
        //0~100�̐�������0.00~1.00�ɂ���
        bGMVol = bGMSli.value/100;

        DecibelConversion(true);
    }

    //SE�X���C�_�̒l���ς���ꂽ�Ƃ�
    public void OnValueChangedSE()
    {
        //�{�����[���̎擾
        //0~100�̐�������0.00~1.00�ɂ���
        sEVol = sESli.value/100;

        DecibelConversion(false);
    }

    void DecibelConversion(bool isBGM)
    {
        //BGM�X���C�_�̒l��ς����Ƃ�
        if (isBGM) {
            //�f�V�x���ϊ�
            bGMDec = Mathf.Clamp(Mathf.Log10(bGMVol) * 20f, -80f, 0f);
        }
        //SE�̃X���C�_�̒l��ς����Ƃ�
        else
        {
            //�f�V�x���ϊ�
            sEDec = Mathf.Clamp(Mathf.Log10(sEVol) * 20f, -80f, 0f);
        }
    }
}
