using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sEnemyParameters : MonoBehaviour
{

    //���ʂ̑ϋv�l��ݒ�ł���
    [SerializeField]
    private int UpperHP;

    [SerializeField]
    private int LowerHP;

    //�e�X�g�p�@�G�ɗ^����_���[�W��ݒ�ł���
    [SerializeField]
    private int damage;

    ////�h���b�v����摜��ݒ�ł���
    //[SerializeField]
    //private Image deathImage;

    //�{�f�B�p�[�c
    [SerializeField]
    private BodyPartsData Upperbodypart;

    [SerializeField]
    private BodyPartsData Lowerbodypart;

    //�v���n�u�̃p�[�c
    [SerializeField]
    private GameObject prePart;

    GameObject drop;

    //�v���C���[�p�����[�^-
    public GameObject PlayerParameter;
    //�v���C���[�R���g���[��
    public GameObject PlayerControl;

    //�{�X�t���O
    [SerializeField]
    bool Boss;

    //�N���A�e�L�X�g
    [SerializeField]
    GameObject textBox;


    //�G�l�~�[���e�������ǂ����������t���O
    public bool canShoot = false;

    // FirePoint �̎Q��
    public Transform firePoint;

    private void Start()
    {

        // canShoot �� true �̏ꍇ�̂� FirePoint ��T��
        if (canShoot)
        {
            firePoint = transform.Find("FirePoint");

            if (firePoint == null)
            {
                Debug.LogWarning("FirePoint ��������܂���ł����B");
            }
        }
    }

    void Update()
    {
        // �����ϋv�l��0�ɂȂ�����h���b�v����
        if (UpperHP <= 0)
        {
            PlayerControl.GetComponent<PlayerControl>().RemoveListItem(this.gameObject);
            Debug.Log("�����g����������");
            Drop(Lowerbodypart);
        }
        if (LowerHP <= 0)
        {
            PlayerControl.GetComponent<PlayerControl>().RemoveListItem(this.gameObject);
            Debug.Log("�㔼�g����������");
            Drop(Upperbodypart);
        }
    }

    public void TakeDamage(int damage, int body = 0)
    {
        // HP������d�g��
        // damage�̓e�X�g�p�̊֐�
        if (body == 0)
        {
            UpperHP -= damage;
        }

        if (body == 1)
        {
            LowerHP -= damage;
        }
    }

    void ShowDeathImage()
    {
        ////�����h���b�v�摜�ݒ肷��Ƃ�
        //if (deathImage != null)
        //{
        //    deathImage.enabled = true;
        //}
    }

    public void Drop(BodyPartsData part)
    {
        // �v���n�u���C���X�^���X��
        drop = Instantiate(prePart);

        // ���������p�[�c�����g�̏ꏊ�Ɏ����Ă���
        drop.transform.position = this.transform.position;

        //// �v���C���[�p�����[�^�[��n��
        //drop.GetComponent<newDropPart>().getPlayerManegerObjet(PlayerParameter);

        // �e�L�X�g�{�b�N�X��n��
        drop.GetComponent<newDropPart>().getTextBox(textBox);

        // �{�X�t���O��n��
        drop.GetComponent<newDropPart>().getBossf(Boss);

        // �f�[�^�ƃV�[���J�ڃ}�l�[�W���[��n��
        drop.GetComponent<newDropPart>().getPartsData(part);

        // �����̃Q�[���I�u�W�F�N�g������
        this.gameObject.SetActive(false);
    }
}

