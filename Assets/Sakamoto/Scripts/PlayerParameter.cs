using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerParameter : MonoBehaviour
{
    [Header("1��������̂ɂ����鎞��")]
    [SerializeField] int iDownTime;

    public  int iHumanityMax;     //�l�Ԑ��̍ő�l
    public  int iUpperHPMax;      //�㔼�g��HP�̍ő�l
    public  int iLowerHPMax;      //�����g��HP�̍ő�l

    private float iHumanity;     //�l�Ԑ�
    private float iUpperHP;      //�㔼�g��HP
    private float iLowerHP;      //�����g��HP
    // Start is called before the first frame update

    [Header("�v���C���[�I�u�W�F�N�g")]
    [SerializeField] GameObject goPlayer;

    //�f�t�H���g�̃p�[�c�f�[�^
    [SerializeField] BodyPartsData DefaultData;

    private void Start()
    {
        //�p�����[�^�̏�����
        iHumanity = iHumanityMax;
        iUpperHP = iUpperHPMax;
        iLowerHP = iLowerHPMax;
    }
    private void Update()
    {
        //�p�����[�^�̒l��iDownTime�b��1����������
        iHumanity -= Time.deltaTime / iDownTime;
        iUpperHP -= Time.deltaTime / iDownTime;
        iLowerHP -= Time.deltaTime / iDownTime;
    }

    //�ԗ�
    //�l�Ԑ����������񕜂���
    public void comfort(int iRecovery)
    {
        iHumanity += iRecovery;
        //�񕜂����l���ő�l�𒴂��Ă�����ő�l�ɂ���
        if(iHumanity > iHumanityMax)
        {
            iHumanity = iHumanityMax;
        }
    }
    //�ڐA
    //�p�[�c�̉摜�ƃp�����[�^�����ւ���
    //BodyPartsData partsData : ����ւ���p�[�c�̃X�N���v�^�u���I�u�W�F�N�g
    //�e�X�g�i�K�ł͈�����null�ł���
    public void transplant(BodyPartsData partsData)
    {
        partsData = partsData ?? DefaultData;

        //�L�����̃C���[�W�擾�p
        SpriteRenderer spriteRenderer;

        switch (partsData.enPartsType)
        {          
            case PartsType.Head:
                break;
            case PartsType.Arm:
                //�p�[�c�f�[�^��HP��Max�ɑ��
                iUpperHPMax = partsData.iPartHp;
                iUpperHP = iUpperHPMax;
                //SpriteRenderer�R���|�[�l���g�擾
                spriteRenderer = goPlayer.transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
                //SpriteRenderer��Sprite�Ƀp�[�c�f�[�^��Sprite��}��
                spriteRenderer.sprite = partsData.PartSprite;
                break;
            case PartsType.Leg:
                //�p�[�c�f�[�^��HP��Max���
                iLowerHPMax = partsData.iPartHp;
                iLowerHP = iLowerHPMax;
                //SpriteRenderer�R���|�[�l���g�擾
                spriteRenderer = goPlayer.transform.GetChild(1).transform.GetComponent<SpriteRenderer>();
                //SpriteRenderer��Sprite�Ƀp�[�c�f�[�^��Sprite��}��
                spriteRenderer.sprite = partsData.PartSprite;
                break;
        }

    }


    //�l�Ԑ��̎擾
    public float Humanity
    {
        get { return iHumanity; }
        set { iHumanity = value; }
    }
    //�㔼�gHP�̎擾
    public float UpperHP
    {
        get { return iUpperHP; }
        set { iUpperHP = value; }
    }
    //�����gHP�̎擾
    public float LowerHP
    {
        get { return iLowerHP; }
        set { iLowerHP = value; }
    }

}
