using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private float iHumanity;
    private float iUpperHP;
    private float iLowerHP;

    [SerializeField, Header("�l�Ԑ��̃o-�̃I�u�W�F�N�g(Humanity_Bar)������")] private GameObject goHumanity_Bar;
    [SerializeField, Header("�㔼�gHP�̃o-�̃I�u�W�F�N�g(UpperHP_Bar)������")] private GameObject goUpperHP_Bar;
    [SerializeField, Header("�����gHP�̃o-�̃I�u�W�F�N�g(LowerHP_Bar)������")] private GameObject goLowerHP_Bar;

    [SerializeField, Header("�Q�[�W����%�ȉ��ɂȂ�����x�����J�n���邩")] private float alertLevel = 0.3f;

    [SerializeField, Header("�ʏ�̐l�Ԑ��o�[�̐F")] private Color normalColor = Color.green;
    [SerializeField, Header("�ʏ��HP�o�[�̐F�i�㔼�g�E�����g�j")] private Color normalColor_Isyoku = Color.cyan;

    [SerializeField, Header("�Q�[�W���댯�ȏ�ԂɂȂ����Ƃ��̈�ڂ̐F")] private Color warningFlashColor1 = new Color(1f, 0.2f, 0.2f);
    [SerializeField, Header("�Q�[�W���댯�ȏ�ԂɂȂ����Ƃ��̓�ڂ̐F")] private Color warningFlashColor2 = new Color(0.6f, 0f, 0f);
    [SerializeField, Header("���邳�ω��̑��x")] private float warningFlashSpeed = 2f;

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
