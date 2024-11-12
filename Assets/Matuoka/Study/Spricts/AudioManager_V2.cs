using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
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
            if (bGMVol > 0)
            {
                //�f�V�x���ϊ�
                bGMDec = Mathf.Clamp(Mathf.Log10((float)bGMVol / 100f) * 20f, -80f, 0f);
                //BGM�̃{�����[����0�Ƃ����t���O��������
                vol0Flag &= 1;
            }
            else
            {
                //BGM�̃{�����[����0�Ƃ����t���O�𗧂Ă�
                vol0Flag |= 2;
            }
        }
        //SE�̃X���C�_�̒l��ς����Ƃ�
        else
        {
            //�{���[����0���傫���Ƃ�
            if (sEVol > 0)
            {
                //�f�V�x���ϊ�
                sEDec = Mathf.Clamp(Mathf.Log10((float)sEVol / 100f) * 20f, -80f, 0f);
                //SE�̃{�����[����0�Ƃ����t���O��������
                vol0Flag &= 2;
            }
            else
            {
                //SE�̃{�����[����0�Ƃ����t���O�𗧂Ă�
                vol0Flag |= 1;
            }
        }

        switch (vol0Flag)
        {
            case 0:
                Debug.Log("�f�V�x�� BGM:" + bGMDec + "\n" +
                    "\tSE:" + sEDec);
                break;

            case 1:
                Debug.Log("�f�V�x�� BGM:" + bGMDec + "\n" +
                    "\tSE:�{�����[��0");
                break;

            case 2:
                Debug.Log("�f�V�x�� BGM:�{�����[��0\n" +
                    "\tSE:" + sEDec);
                break;

            case 3:
                Debug.Log("�f�V�x�� BGM:�{�����[��0\n" +
                    "\tSE:�{�����[��0");
                break;
        }
    }
}
