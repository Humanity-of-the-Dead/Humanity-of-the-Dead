using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    float iHumanity;//人間性
    float iUpperHP;//上半身HP
    float iLowerHP;//下半身HP

    [SerializeField] private GameObject goHumanity_Bar;//人間性のBar
    [SerializeField] private GameObject goUpperHP_Bar;//上半身HPのBar
    [SerializeField] private GameObject goLowerHP_Bar;//下半身HPのBar


    

    // Update is called once per frame
    void Update()
    {
        iHumanity =  PlayerParameter.Instance.Humanity;
        iUpperHP = PlayerParameter.Instance.UpperHP;
        iLowerHP = PlayerParameter.Instance.LowerHP;

        Image HumanityImage = goHumanity_Bar.GetComponent<Image>();　//ImageComponentを取得
        HumanityImage.fillAmount = (float)iHumanity / PlayerParameter.Instance.iHumanityMax;　//fillAmountの値を変更してゲージを減少

        Image UpperHPImage = goUpperHP_Bar.GetComponent<Image>();　//ImageComponentを取得
        UpperHPImage.fillAmount = (float)iUpperHP / PlayerParameter.Instance.iUpperHPMax;　//fillAmountの値を変更してゲージを減少
            
        Image LowerHPImage = goLowerHP_Bar.GetComponent<Image>();　//ImageComponentを取得
        LowerHPImage.fillAmount = (float)iLowerHP / PlayerParameter.Instance.iLowerHPMax;　//fillAmountの値を変更してゲージを減少

    }
}
