using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�X���C�_���g���̂ɕK�v
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    //�X���C�_
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        //�X���C�_���擾
        slider = GetComponent<Slider>();

        //�X���C�_�̍ő�l�A���ݒl
        float maxVal,nowVal;

        maxVal = 100f;
        nowVal = 40;

        //�X���C�_�̍ő�l�ݒ�
        slider.maxValue = maxVal;

        //�X���C�_�̌��ݒl�ݒ�
        slider.value= nowVal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�X���C�_�̒l���ύX����邽�тɎ��s
    public void Method()
    {
        //�X���C�_�̒l���o��
        Debug.Log("���ݒn:"+slider.value);

        if(slider.value >= 50)
        {
            Debug.Log("50�ȏ�ł�");
        }
    }
}
