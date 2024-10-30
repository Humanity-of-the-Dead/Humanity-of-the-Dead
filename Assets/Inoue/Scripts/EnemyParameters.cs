using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyParameters : MonoBehaviour
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

    void Update()
    {
        //�����ϋv�l��0�ɂȂ�����h���b�v����
        if (UpperHP <= 0)
        {
           
            Drop(Lowerbodypart);
        }
        if (LowerHP <= 0)
        {
           
            Drop(Upperbodypart);
        }
    }
    //body�ɂ�0��1��������Ă͂����Ȃ��@BA//GU/RU
    //body : 0->�㔼�g�Ƀ_���[�W
    //body : 1->�����g�Ƀ_���[�W

    public void TakeDamage(int damage, int body = 0)
    {
        //HP������d�g��
        //damage�̓e�X�g�p�̊֐�
#if body
    
       UpperHP -= damage;
#else
       LowerHP -= damage;

#endif
    }
    void ShowDeathImage()
    {
        ////�����h���b�v�摜�ݒ肷��Ƃ�
        //if (deathImage != null)
        //{
        //    deathImage.enabled = true;
        //}
    }
    public  void Drop(BodyPartsData part)
    {
        //�v���n�u���C���X�^���X��
        drop = Instantiate(prePart);

        //���������p�[�c�����g�̏ꏊ�Ɏ����Ă���
        drop.transform.position = this.transform.position;

        //
        drop.GetComponent<DropPart>().getPartsData(part);


        //�����̃Q�[���I�u�W�F�N�g������
        Destroy(this.gameObject);
    }


    //�h���b�v�̋�������ĂȂ������ʂɏo�邾���Ȃ̂Œ��߂���
    //�|���ꂽ��̂���������v���O�������K�v
    //���̎��_���Ɨ����h���b�v���Ă��܂��̂ŏC������
    //����Image�����邱�ƂɂȂ��Ă邯�ǁA������Sprite������悤�ɂ�����

    //���̃v���O�����̓������e�X�g�p�ɉ�������

    //�_���[�W��get��set

}