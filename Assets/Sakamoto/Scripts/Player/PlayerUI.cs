using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    float iHumanity;//人間性
    float iUpperHP;//上半身HP
    float iLowerHP;//下半身HP

    [SerializeField] GameObject goHumanity_Bar;//人間性のBar
    [SerializeField] GameObject goUpperHP_Bar;//上半身HPのBar
    [SerializeField] GameObject goLowerHP_Bar;//下半身HPのBar

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

        Image HumanityImage = goHumanity_Bar.GetComponent<Image>();　//ImageComponentを取得
        HumanityImage.fillAmount = (float)iHumanity / scSlayerParameter.iHumanityMax;　//fillAmountの値を変更してゲージを減少

        Image UpperHPImage = goUpperHP_Bar.GetComponent<Image>();　//ImageComponentを取得
        UpperHPImage.fillAmount = (float)iUpperHP / scSlayerParameter.iUpperHPMax;　//fillAmountの値を変更してゲージを減少

        Image LowerHPImage = goLowerHP_Bar.GetComponent<Image>();　//ImageComponentを取得
        LowerHPImage.fillAmount = (float)iLowerHP / scSlayerParameter.iLowerHPMax;　//fillAmountの値を変更してゲージを減少

    }
}
