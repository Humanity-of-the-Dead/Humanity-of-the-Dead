using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecibelTransformation : MonoBehaviour
{
    //�{�����[��0~1
    [Header("�{�����[��0~1")][SerializeField,Range(0f,1f)]float vol;
    //�f�V�x��(�{�����[��-80~0)
    [Header("�f�V�x��")][SerializeField,Range(-80f,0f)]float dec;

    //�{�����[������f�V�x���ɕϊ����ꂽ�l
    float volToDec_Dec;
    //�f�V�x������{�����[���ɕϊ����ꂽ�l
    float decToVol_Vol;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //�{�����[�����f�V�x���ɕϊ�
        VolumeToDecibelTransformation();
        //�f�V�x�����{�����[���ɕϊ�
        DecibelToVolumeTransformation();

        Debug.Log("�f�V�x��:" + volToDec_Dec + "\n�{�����[��:" + decToVol_Vol);
    }

    //�{�����[������f�V�x���ɕϊ�
    void VolumeToDecibelTransformation()
    {
        //�{�����[�����f�V�x���ɕϊ�
        volToDec_Dec = Mathf.Clamp(Mathf.Log10(vol) * 20f, -80f, 0f);
    }

    //�f�V�x������{�����[���ɕϊ�
    void DecibelToVolumeTransformation()
    {
        decToVol_Vol = Mathf.Clamp(Mathf.Pow(10f, dec / 20f), 0f, 1f);
    }
}
