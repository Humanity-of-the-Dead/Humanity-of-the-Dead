using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyParameters : MonoBehaviour
{
    //���ʂ̑ϋv�l��ݒ�ł���
    [SerializeField]
    private int HP;

    //�e�X�g�p�@�G�ɗ^����_���[�W��ݒ�ł���
    [SerializeField]
    private int damage;

    //�h���b�v����摜��ݒ�ł���
    [SerializeField]
    private Image deathImage;

    void Update()
    {
        //�����ϋv�l��0�ɂȂ�����h���b�v����
        if (HP <= 0)
        {
            ShowDeathImage();
        }
    }
    public void TakeDamage(int damage)
    {
        //HP������d�g��
        //damage�̓e�X�g�p�̊֐�
        HP -= damage;
    }
    void ShowDeathImage()
    {
        //�����h���b�v�摜�ݒ肷��Ƃ�
        if (deathImage != null)
        {
            deathImage.enabled = true;
        }
    }

    //�h���b�v�̋�������ĂȂ������ʂɏo�邾���Ȃ̂Œ��߂���
    //�|���ꂽ��̂���������v���O�������K�v
    //���̎��_���Ɨ����h���b�v���Ă��܂��̂ŏC������
    //����Image�����邱�ƂɂȂ��Ă邯�ǁA������Sprite������悤�ɂ�����

    //���̃v���O�����̓������e�X�g�p�ɉ�������
}