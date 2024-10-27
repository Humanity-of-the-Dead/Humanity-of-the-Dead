using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    int iHumanity;//人間性
    int iUpperHP;//上半身HP
    int iLowerHP;//下半身HP

    [SerializeField] GameObject goHumanity_Bar;//人間性のBar
    [SerializeField] GameObject goUpperHP_Bar;//上半身HPのBar
    [SerializeField] GameObject goLowerHP_Bar;//下半身HPのBar

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

        Image HumanityImage = goHumanity_Bar.GetComponent<Image>();　//ImageComponentを取得
        HumanityImage.fillAmount = (float)iHumanity / 10f;　//fillAmountの値を変更してゲージを減少

        Image UpperHPImage = goUpperHP_Bar.GetComponent<Image>();　//ImageComponentを取得
        UpperHPImage.fillAmount = (float)iUpperHP / 10f;　//fillAmountの値を変更してゲージを減少

        Image LowerHPImage = goLowerHP_Bar.GetComponent<Image>();　//ImageComponentを取得
        LowerHPImage.fillAmount = (float)iLowerHP / 10f;　//fillAmountの値を変更してゲージを減少

    }
}
