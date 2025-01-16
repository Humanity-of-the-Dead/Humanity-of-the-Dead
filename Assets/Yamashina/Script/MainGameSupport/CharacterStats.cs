using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour, IDamageable
{

    [SerializeField, Header("�G�t�F�N�g�֘A")]
    [Tooltip("�G�t�F�N�g�̃v���n�u����")]
    public GameObject hitGameObject;
    [Tooltip("�G�t�F�N�g���㔼�g�ɏo��͈́iX���W),�ő�ƍŏ�")]

    public float upperEffectXMin, upperEffectXMax;
    [Tooltip("�G�t�F�N�g���㔼�g�ɏo��͈́iY���W),�ő�ƍŏ�")]

    public float upperEffectYMin, upperEffectYMax;
    [Tooltip("�G�t�F�N�g�������g�ɏo��͈́iX���W),�ő�ƍŏ�")]

    public float lowerEffectXMin, lowerEffectXMax;
    [Tooltip("�G�t�F�N�g�������g�ɏo��͈́iY���W),�ő�ƍŏ�")]

    public float lowerEffectYMin, lowerEffectYMax;

    // �C���^�[�t�F�[�X�̃��\�b�h����������
    public abstract void TakeDamage(float damage, int body = 0);
    public abstract void ShowHitEffects(int body);



}
