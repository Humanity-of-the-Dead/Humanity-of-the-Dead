using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    float iHumanity;//�l�Ԑ�
    float iUpperHP;//�㔼�gHP
    float iLowerHP;//�����gHP

    [SerializeField] private GameObject goHumanity_Bar;//�l�Ԑ���Bar
    [SerializeField] private GameObject goUpperHP_Bar;//�㔼�gHP��Bar
    [SerializeField] private GameObject goLowerHP_Bar;//�����gHP��Bar


    

    // Update is called once per frame
    void Update()
    {
        iHumanity =  PlayerParameter.Instance.Humanity;
        iUpperHP = PlayerParameter.Instance.UpperHP;
        iLowerHP = PlayerParameter.Instance.LowerHP;

        Image HumanityImage = goHumanity_Bar.GetComponent<Image>();�@//ImageComponent���擾
        HumanityImage.fillAmount = (float)iHumanity / PlayerParameter.Instance.iHumanityMax;�@//fillAmount�̒l��ύX���ăQ�[�W������

        Image UpperHPImage = goUpperHP_Bar.GetComponent<Image>();�@//ImageComponent���擾
        UpperHPImage.fillAmount = (float)iUpperHP / PlayerParameter.Instance.iUpperHPMax;�@//fillAmount�̒l��ύX���ăQ�[�W������
            
        Image LowerHPImage = goLowerHP_Bar.GetComponent<Image>();�@//ImageComponent���擾
        LowerHPImage.fillAmount = (float)iLowerHP / PlayerParameter.Instance.iLowerHPMax;�@//fillAmount�̒l��ύX���ăQ�[�W������

    }
}
