using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
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
            if (bGMVol > 0)
            {
                //デシベル変換
                bGMDec = Mathf.Clamp(Mathf.Log10((float)bGMVol / 100f) * 20f, -80f, 0f);
                //BGMのボリュームが0というフラグを下げる
                vol0Flag &= 1;
            }
            else
            {
                //BGMのボリュームが0というフラグを立てる
                vol0Flag |= 2;
            }
        }
        //SEのスライダの値を変えたとき
        else
        {
            //ボルームが0より大きいとき
            if (sEVol > 0)
            {
                //デシベル変換
                sEDec = Mathf.Clamp(Mathf.Log10((float)sEVol / 100f) * 20f, -80f, 0f);
                //SEのボリュームが0というフラグを下げる
                vol0Flag &= 2;
            }
            else
            {
                //SEのボリュームが0というフラグを立てる
                vol0Flag |= 1;
            }
        }

        switch (vol0Flag)
        {
            case 0:
                Debug.Log("デシベル BGM:" + bGMDec + "\n" +
                    "\tSE:" + sEDec);
                break;

            case 1:
                Debug.Log("デシベル BGM:" + bGMDec + "\n" +
                    "\tSE:ボリューム0");
                break;

            case 2:
                Debug.Log("デシベル BGM:ボリューム0\n" +
                    "\tSE:" + sEDec);
                break;

            case 3:
                Debug.Log("デシベル BGM:ボリューム0\n" +
                    "\tSE:ボリューム0");
                break;
        }
    }
}
