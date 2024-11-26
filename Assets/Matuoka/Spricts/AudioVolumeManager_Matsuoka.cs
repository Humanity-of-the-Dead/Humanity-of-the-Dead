using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeManager_Matsuoka : MonoBehaviour
{
    //AudioMixer
    [SerializeField] AudioMixer audMix;

    //BGM�X���C�_
    [SerializeField] Slider bGMSli;
    //SESlider
    [SerializeField]Slider sESli;
    //UISlider
    [SerializeField]Slider uISli;

    //SE
    [SerializeField]AudioSource sE;

    //BGM�̌��݃{�����[��(����0~100)
    float bGMVol;
    //SE�̌��݃{�����[��(����0~100)
    float sEVol;
    //UI�̌��݃{�����[��(����0~100)
    float uIVol;

    //BGM�̃f�V�x��
    float bGMDec;
    //SE�̃f�V�x��
    float sEDec;
    //UI�̃f�V�x��
    float uIDec;

    // Start is called before the first frame update
    void Start()
    {
        //BGM�ESE�̃{�����[���̏�����
        bGMVol = 50;
        sEVol = 50;
        DecibelConversion(0);
        DecibelConversion(1);
        DecibelConversion(2);
        //\BGM�ESE�̃{�����[���̏�����
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    //BGM�X���C�_�̒l���ς���ꂽ�Ƃ�
    public void OnValueChangedBGM()
    {
        DecibelConversion(0);
    }

    //SE�X���C�_�̒l���ς���ꂽ�Ƃ�
    public void OnValueChangedSE()
    {
        DecibelConversion(1);

        //SE�𗬂�
        sE.PlayOneShot(sE.clip);
    }

    //UI�X���C�_�̒l���ς���ꂽ�Ƃ�
    public void OnValueChangedUI()
    {
        DecibelConversion(2);

        //SE�𗬂�
        sE.PlayOneShot(sE.clip);
    }

    void DecibelConversion(int audMixGroFlag)
    {
        switch (audMixGroFlag) {
            //BGM�X���C�_�̒l��ς����Ƃ�
            case 0:
                //�{�����[���̎擾
                //0~100�̐�������0.00~1.00�ɂ���
                bGMVol = bGMSli.value / 100;

                //�f�V�x���ϊ�
                bGMDec = Mathf.Clamp(Mathf.Log10(bGMVol) * 20f, -80f, 0f);

               //AudioMixer�ɑ��
                audMix.SetFloat("BGM", bGMDec);

                break;

            //SE�̃X���C�_�̒l��ς����Ƃ�
            case 1:
                //�{�����[���̎擾
                //0~100�̐�������0.00~1.00�ɂ���
                sEVol = sESli.value / 100;

                //�f�V�x���ϊ�
                sEDec = Mathf.Clamp(Mathf.Log10(sEVol) * 20f, -80f, 0f);

                //AudioMixer�ɑ��
                audMix.SetFloat("SE", sEDec);
                
                break;

            //UI�̃X���C�_��ς����Ƃ�
            case 2:
                //�{�����[���̎擾
                //0~100�̐�������0.00~1.00�ɂ���
                uIVol = uISli.value / 100;

                //�f�V�x���ϊ�
                uIDec = Mathf.Clamp(Mathf.Log10(uIVol) * 20f, -80f, 0f);

                //AudioMixer�ɑ��
                audMix.SetFloat("UI", uIDec);

                break;
        }
    }
}
