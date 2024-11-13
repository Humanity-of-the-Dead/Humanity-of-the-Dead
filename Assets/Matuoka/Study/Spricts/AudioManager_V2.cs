using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager_V2: MonoBehaviour
{
    //AudioMixer
    [SerializeField] AudioMixer audMix;

    //BGMスライダ
    [SerializeField] Slider bGMSli;
    //SESlider
    [SerializeField]Slider sESli;

    //SE
    [SerializeField]AudioSource sE;

    //BGMの現在ボリューム(整数0~100)
    float bGMVol;
    //SEの現在ボリューム(整数0~100)
    float sEVol;

    //BGMのデシベル
    float bGMDec;
    //SEのデシベル
    float sEDec;

    // Start is called before the first frame update
    void Start()
    {
        //BGM・SEのボリュームの初期化
        bGMVol = 50;
        sEVol = 50;
        DecibelConversion(true);
        DecibelConversion(false);
        //\BGM・SEのボリュームの初期化
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    //BGMスライダの値が変えられたとき
    public void OnValueChangedBGM()
    {
        DecibelConversion(true);
    }

    //SEスライダの値が変えられたとき
    public void OnValueChangedSE()
    {
        DecibelConversion(false);

        //SEを流す
        sE.PlayOneShot(sE.clip);
    }

    void DecibelConversion(bool isBGM)
    {
        //BGMスライダの値を変えたとき
        if (isBGM) {
            //ボリュームの取得
            //0~100の整数から0.00~1.00にする
            bGMVol = bGMSli.value / 100;

            //デシベル変換
            bGMDec = Mathf.Clamp(Mathf.Log10(bGMVol) * 20f, -80f, 0f);
            
            //AudioMixerに代入
            audMix.SetFloat("BGM",bGMDec);
        }
        //SEのスライダの値を変えたとき
        else
        {
            //ボリュームの取得
            //0~100の整数から0.00~1.00にする
            sEVol = sESli.value / 100;

            //デシベル変換
            sEDec = Mathf.Clamp(Mathf.Log10(sEVol) * 20f, -80f, 0f);

            //AudioMixerに代入
            audMix.SetFloat("SE", sEDec);
        }
    }
}
