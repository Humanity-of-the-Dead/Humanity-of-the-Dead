using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeManager : MonoBehaviour
{
    //BGM�X���C�_
    [SerializeField] Slider bGMSli;
    //SESlider
    [SerializeField]Slider sESli;

    //BGM�̌��݃{�����[��(0~100)
    int bGMVol;
    //SE�̌��݃{�����[��(0~100)
    int sEVol;

    //BGM�̃f�V�x��
    float bGMDec;
    //SE�̃f�V�x��
    float sEDec;

    /*�{�����[����0���ǂ���
    0:0�łȂ�
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
    
    //BGM�X���C�_�̒l���ς���ꂽ�Ƃ�
    public void OnValueChangedBGM()
    {
        //�{�����[���̎擾
        bGMVol = (int)bGMSli.value;

        DecibelConversion(true);
    }

    //SE�X���C�_�̒l���ς���ꂽ�Ƃ�
    public void OnValueChangedSE()
    {
        //�{�����[���̎擾
        sEVol = (int)sESli.value;

        DecibelConversion(false);
    }

    void DecibelConversion(bool isBGM)
    {
        //BGM�X���C�_�̒l��ς����Ƃ�
        if (isBGM) {
            //�{�����[����0���傫���Ƃ�
            if ( bGMVol> 0)
            {
                //�������珑��
                bGMDec = Mathf.Clamp(bGMVol, -80f, 0f);
            }
        }
        //SE�̃X���C�_�̒l��ς����Ƃ�
        else
        {

        }
    }
}
