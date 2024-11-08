using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeManager : MonoBehaviour
{
    //BGMスライダ
    [SerializeField] Slider bGMSli;
    //SESlider
    [SerializeField]Slider sESli;

    //BGMの現在ボリューム(0~100)
    int bGMVol;
    //SEの現在ボリューム(0~100)
    int sEVol;

    //BGMのデシベル
    float bGMDec;
    //SEのデシベル
    float sEDec;

    /*ボリュームが0かどうか
    0:0でない
    1:0
    10:BGM
    01:SE*/
    int vol0Flag=0;

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
        bGMVol = (int)bGMSli.value;

        DecibelConversion(true);
    }

    //SEスライダの値が変えられたとき
    public void OnValueChangedSE()
    {
        //ボリュームの取得
        sEVol = (int)sESli.value;

        DecibelConversion(false);
    }

    void DecibelConversion(bool isBGM)
    {
        //BGMスライダの値を変えたとき
        if (isBGM) {
            //ボリュームが0より大きいとき
            if ( bGMVol> 0)
            {
                //ここから書く
                bGMDec = Mathf.Clamp(bGMVol, -80f, 0f);
            }
        }
        //SEのスライダの値を変えたとき
        else
        {

        }
    }
}
