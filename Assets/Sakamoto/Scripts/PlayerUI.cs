using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    int iHumanity;//�l�Ԑ�
    int iUpperHP;//�㔼�gHP
    int iLowerHP;//�����gHP

    [SerializeField] GameObject goHumanity_Bar;//�l�Ԑ���Bar
    [SerializeField] GameObject goUpperHP_Bar;//�㔼�gHP��Bar
    [SerializeField] GameObject goLowerHP_Bar;//�����gHP��Bar

    [SerializeField] PlayerParameter scSlayerParameter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        iHumanity =  scSlayerParameter.getHumanity();
        iUpperHP = scSlayerParameter.getUpperHP();
        iLowerHP = scSlayerParameter.getLowerHP();

        Image HumanityImage = goHumanity_Bar.GetComponent<Image>();�@//ImageComponent���擾
        HumanityImage.fillAmount = (float)iHumanity / 10f;�@//fillAmount�̒l��ύX���ăQ�[�W������

        Image UpperHPImage = goUpperHP_Bar.GetComponent<Image>();�@//ImageComponent���擾
        UpperHPImage.fillAmount = (float)iUpperHP / 10f;�@//fillAmount�̒l��ύX���ăQ�[�W������

        Image LowerHPImage = goLowerHP_Bar.GetComponent<Image>();�@//ImageComponent���擾
        LowerHPImage.fillAmount = (float)iLowerHP / 10f;�@//fillAmount�̒l��ύX���ăQ�[�W������

    }
}
