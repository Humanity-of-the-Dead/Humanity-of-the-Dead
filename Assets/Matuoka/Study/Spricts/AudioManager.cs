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
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    //BGMスライダの値が変えられたとき
    public void OnValueChangedBGM()
    {
        //ボリュームの取得
        //0~100の整数から0.00~1.00にする
        bGMVol = bGMSli.value/100;

        DecibelConversion(true);
    }

    //SEスライダの値が変えられたとき
    public void OnValueChangedSE()
    {
        //ボリュームの取得
        //0~100の整数から0.00~1.00にする
        sEVol = sESli.value/100;

        DecibelConversion(false);
    }

    void DecibelConversion(bool isBGM)
    {
        //BGMスライダの値を変えたとき
        if (isBGM) {
            //デシベル変換
            bGMDec = Mathf.Clamp(Mathf.Log10(bGMVol) * 20f, -80f, 0f);
        }
        //SEのスライダの値を変えたとき
        else
        {
            //デシベル変換
            sEDec = Mathf.Clamp(Mathf.Log10(sEVol) * 20f, -80f, 0f);
        }
    }
}
