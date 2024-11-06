using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecibelTransformation : MonoBehaviour
{
    //ボリューム0~1
    [Header("ボリューム0~1")][SerializeField,Range(0f,1f)]float vol;
    //デシベル(ボリューム-80~0)
    [Header("デシベル")][SerializeField,Range(-80f,0f)]float dec;

    //ボリュームからデシベルに変換された値
    float volToDec_Dec;
    //デシベルからボリュームに変換された値
    float decToVol_Vol;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //ボリュームをデシベルに変換
        VolumeToDecibelTransformation();
        //デシベルをボリュームに変換
        DecibelToVolumeTransformation();

        Debug.Log("デシベル:" + volToDec_Dec + "\nボリューム:" + decToVol_Vol);
    }

    //ボリュームからデシベルに変換
    void VolumeToDecibelTransformation()
    {
        //ボリュームをデシベルに変換
        volToDec_Dec = Mathf.Clamp(Mathf.Log10(vol) * 20f, -80f, 0f);
    }

    //デシベルからボリュームに変換
    void DecibelToVolumeTransformation()
    {
        decToVol_Vol = Mathf.Clamp(Mathf.Pow(10f, dec / 20f), 0f, 1f);
    }
}
