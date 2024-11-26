using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeManager_Matsuoka : MonoBehaviour
{
    //AudioMixer
    [SerializeField] AudioMixer audMix;

    //BGMスライダ
    [SerializeField] Slider bGMSli;
    //SESlider
    [SerializeField]Slider sESli;
    //UISlider
    [SerializeField]Slider uISli;

    //SE
    [SerializeField]AudioSource sE;

    //BGMの現在ボリューム(整数0~100)
    float bGMVol;
    //SEの現在ボリューム(整数0~100)
    float sEVol;
    //UIの現在ボリューム(整数0~100)
    float uIVol;

    //BGMのデシベル
    float bGMDec;
    //SEのデシベル
    float sEDec;
    //UIのデシベル
    float uIDec;

    // Start is called before the first frame update
    void Start()
    {
        //BGM・SEのボリュームの初期化
        bGMVol = 50;
        sEVol = 50;
        DecibelConversion(0);
        DecibelConversion(1);
        DecibelConversion(2);
        //\BGM・SEのボリュームの初期化
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    //BGMスライダの値が変えられたとき
    public void OnValueChangedBGM()
    {
        DecibelConversion(0);
    }

    //SEスライダの値が変えられたとき
    public void OnValueChangedSE()
    {
        DecibelConversion(1);

        //SEを流す
        sE.PlayOneShot(sE.clip);
    }

    //UIスライダの値が変えられたとき
    public void OnValueChangedUI()
    {
        DecibelConversion(2);

        //SEを流す
        sE.PlayOneShot(sE.clip);
    }

    void DecibelConversion(int audMixGroFlag)
    {
        switch (audMixGroFlag) {
            //BGMスライダの値を変えたとき
            case 0:
                //ボリュームの取得
                //0~100の整数から0.00~1.00にする
                bGMVol = bGMSli.value / 100;

                //デシベル変換
                bGMDec = Mathf.Clamp(Mathf.Log10(bGMVol) * 20f, -80f, 0f);

               //AudioMixerに代入
                audMix.SetFloat("BGM", bGMDec);

                break;

            //SEのスライダの値を変えたとき
            case 1:
                //ボリュームの取得
                //0~100の整数から0.00~1.00にする
                sEVol = sESli.value / 100;

                //デシベル変換
                sEDec = Mathf.Clamp(Mathf.Log10(sEVol) * 20f, -80f, 0f);

                //AudioMixerに代入
                audMix.SetFloat("SE", sEDec);
                
                break;

            //UIのスライダを変えたとき
            case 2:
                //ボリュームの取得
                //0~100の整数から0.00~1.00にする
                uIVol = uISli.value / 100;

                //デシベル変換
                uIDec = Mathf.Clamp(Mathf.Log10(uIVol) * 20f, -80f, 0f);

                //AudioMixerに代入
                audMix.SetFloat("UI", uIDec);

                break;
        }
    }
}
