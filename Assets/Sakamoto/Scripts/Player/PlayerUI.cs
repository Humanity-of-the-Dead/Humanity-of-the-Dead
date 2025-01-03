using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    float iHumanity;//�l�Ԑ�
    float iUpperHP;//�㔼�gHP
    float iLowerHP;//�����gHP

    [SerializeField] GameObject goHumanity_Bar;//�l�Ԑ���Bar
    [SerializeField] GameObject goUpperHP_Bar;//�㔼�gHP��Bar
    [SerializeField] GameObject goLowerHP_Bar;//�����gHP��Bar

    PlayerParameter scSlayerParameter;

    // Start is called before the first frame update
    void Start()
    {
        scSlayerParameter = PlayerParameter.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        iHumanity =  scSlayerParameter.Humanity;
        iUpperHP = scSlayerParameter.UpperHP;
        iLowerHP = scSlayerParameter.LowerHP;

        Image HumanityImage = goHumanity_Bar.GetComponent<Image>();�@//ImageComponent���擾
        HumanityImage.fillAmount = (float)iHumanity / scSlayerParameter.iHumanityMax;�@//fillAmount�̒l��ύX���ăQ�[�W������

        Image UpperHPImage = goUpperHP_Bar.GetComponent<Image>();�@//ImageComponent���擾
        UpperHPImage.fillAmount = (float)iUpperHP / scSlayerParameter.iUpperHPMax;�@//fillAmount�̒l��ύX���ăQ�[�W������

        Image LowerHPImage = goLowerHP_Bar.GetComponent<Image>();�@//ImageComponent���擾
        LowerHPImage.fillAmount = (float)iLowerHP / scSlayerParameter.iLowerHPMax;�@//fillAmount�̒l��ύX���ăQ�[�W������

    }
}
