
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

    [SerializeField, Header("�Q�[�W����%�ȉ��ɂȂ�����x�����J�n���邩")] private float alertLevel = 0.3f; // 30% �ȉ��ŐԂ�����

    [SerializeField, Header("�ʏ��HP�o�[�̐F")] private Color normalColor = Color.green;

    [SerializeField, Header("�Q�[�W���댯�ȏ�ԂɂȂ����Ƃ��̈�ڂ̐F\n�F�̖��邳�����̎����ŕω������邽�߂ɕK�v")] private Color warningFlashColor1 = new Color(1f, 0.2f, 0.2f); // ���邢��

    [SerializeField, Header("�Q�[�W���댯�ȏ�ԂɂȂ����Ƃ��̓�ڂ̐F\n�F�̖��邳�����̎����ŕω������邽�߂ɕK�v")] private Color warningFlashColor2 = new Color(0.6f, 0f, 0f); // �Â���

    [SerializeField, Header("���邳�ω��̑��x")] private float warningFlashSpeed = 2f; // ���邳�ω��̑��x

    void Update()
    {
        //���݂̃X�e�[�^�X���擾
        iHumanity = PlayerParameter.Instance.Humanity;

        iUpperHP = PlayerParameter.Instance.UpperHP;

        iLowerHP = PlayerParameter.Instance.LowerHP;

        //HP�o�[���X�V
        UpdateBar(goHumanity_Bar, iHumanity, PlayerParameter.Instance.iHumanityMax);

        UpdateBar(goUpperHP_Bar, iUpperHP, PlayerParameter.Instance.iUpperHPMax);

        UpdateBar(goLowerHP_Bar, iLowerHP, PlayerParameter.Instance.iLowerHPMax);
    }

    void UpdateBar(GameObject bar, float currentValue, float maxValue)
    {
        Image barImage = bar.GetComponent<Image>();

        //���݂�HP�̒l��HP�̍ő�l����C���[�W��fillAmount�������v�Z
        float fillAmount = currentValue / maxValue;

        // �C���[�W��fillAmount�Ɋ�������
        barImage.fillAmount = fillAmount;

        if (fillAmount <= alertLevel)
        {
            // ���邳�̕ω�������i0 �`length)�̎w�肵���͈͂����� 
            float flashRatio = Mathf.PingPong(Time.time * warningFlashSpeed, 1f);

            //flashRatio�̒l�ɉ�����warningFlashColor1����warningFlashColor2�֊��炩�ɕω�������
            barImage.color = Color.Lerp(warningFlashColor1, warningFlashColor2, flashRatio);

        }
        else
        {
            barImage.color = normalColor;
        }
    }
}


