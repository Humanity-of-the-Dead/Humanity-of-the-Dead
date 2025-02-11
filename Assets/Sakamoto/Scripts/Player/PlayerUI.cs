using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private float iHumanity;
    private float iUpperHP;
    private float iLowerHP;

    [SerializeField, Header("人間性のバ-のオブジェクト(Humanity_Bar)を入れる")] private GameObject goHumanity_Bar;
    [SerializeField, Header("上半身HPのバ-のオブジェクト(UpperHP_Bar)を入れる")] private GameObject goUpperHP_Bar;
    [SerializeField, Header("下半身HPのバ-のオブジェクト(LowerHP_Bar)を入れる")] private GameObject goLowerHP_Bar;

    [SerializeField, Header("ゲージが何%以下になったら警告を開始するか")] private float alertLevel = 0.3f;

    [SerializeField, Header("通常の人間性バーの色")] private Color normalColor = Color.green;
    [SerializeField, Header("通常のHPバーの色（上半身・下半身）")] private Color normalColor_Isyoku = Color.cyan;

    [SerializeField, Header("ゲージが危険な状態になったときの一個目の色")] private Color warningFlashColor1 = new Color(1f, 0.2f, 0.2f);
    [SerializeField, Header("ゲージが危険な状態になったときの二個目の色")] private Color warningFlashColor2 = new Color(0.6f, 0f, 0f);
    [SerializeField, Header("明るさ変化の速度")] private float warningFlashSpeed = 2f;

    void Update()
    {
        iHumanity = PlayerParameter.Instance.Humanity;
        iUpperHP = PlayerParameter.Instance.UpperHP;
        iLowerHP = PlayerParameter.Instance.LowerHP;

        UpdateBar(goHumanity_Bar, iHumanity, PlayerParameter.Instance.iHumanityMax, normalColor);
        UpdateBar(goUpperHP_Bar, iUpperHP, PlayerParameter.Instance.iUpperHPMax, normalColor_Isyoku);
        UpdateBar(goLowerHP_Bar, iLowerHP, PlayerParameter.Instance.iLowerHPMax, normalColor_Isyoku);
    }

    void UpdateBar(GameObject bar, float currentValue, float maxValue, Color normalBarColor)
    {
        Image barImage = bar.GetComponent<Image>();
        float fillAmount = currentValue / maxValue;
        barImage.fillAmount = fillAmount;

        if (fillAmount <= alertLevel)
        {
            float flashRatio = Mathf.PingPong(Time.time * warningFlashSpeed, 1f);
            barImage.color = Color.Lerp(warningFlashColor1, warningFlashColor2, flashRatio);
        }
        else
        {
            barImage.color = normalBarColor;
        }
    }
}
