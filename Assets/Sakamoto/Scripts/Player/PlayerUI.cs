
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

    [SerializeField, Header("ゲージが何%以下になったら警告を開始するか")] private float alertLevel = 0.3f; // 30% 以下で赤くする

    [SerializeField, Header("通常のHPバーの色")] private Color normalColor = Color.green;

    [SerializeField, Header("ゲージが危険な状態になったときの一個目の色\n色の明るさを一定の周期で変化させるために必要")] private Color warningFlashColor1 = new Color(1f, 0.2f, 0.2f); // 明るい赤

    [SerializeField, Header("ゲージが危険な状態になったときの二個目の色\n色の明るさを一定の周期で変化させるために必要")] private Color warningFlashColor2 = new Color(0.6f, 0f, 0f); // 暗い赤

    [SerializeField, Header("明るさ変化の速度")] private float warningFlashSpeed = 2f; // 明るさ変化の速度

    void Update()
    {
        //現在のステータスを取得
        iHumanity = PlayerParameter.Instance.Humanity;

        iUpperHP = PlayerParameter.Instance.UpperHP;

        iLowerHP = PlayerParameter.Instance.LowerHP;

        //HPバーを更新
        UpdateBar(goHumanity_Bar, iHumanity, PlayerParameter.Instance.iHumanityMax);

        UpdateBar(goUpperHP_Bar, iUpperHP, PlayerParameter.Instance.iUpperHPMax);

        UpdateBar(goLowerHP_Bar, iLowerHP, PlayerParameter.Instance.iLowerHPMax);
    }

    void UpdateBar(GameObject bar, float currentValue, float maxValue)
    {
        Image barImage = bar.GetComponent<Image>();

        //現在のHPの値とHPの最大値からイメージのfillAmount割合を計算
        float fillAmount = currentValue / maxValue;

        // イメージのfillAmountに割合を代入
        barImage.fillAmount = fillAmount;

        if (fillAmount <= alertLevel)
        {
            // 明るさの変化をつける（0 ～length)の指定した範囲を往復 
            float flashRatio = Mathf.PingPong(Time.time * warningFlashSpeed, 1f);

            //flashRatioの値に応じてwarningFlashColor1からwarningFlashColor2へ滑らかに変化させる
            barImage.color = Color.Lerp(warningFlashColor1, warningFlashColor2, flashRatio);

        }
        else
        {
            barImage.color = normalColor;
        }
    }
}


