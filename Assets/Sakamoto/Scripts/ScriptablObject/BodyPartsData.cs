using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum PartsType 
{
    Upper,
    Lower,
}

[CreateAssetMenu(menuName = "ScriptableObject/Create BodyPartsData")] 

public class BodyPartsData : ScriptableObject
{
    [Header("�p�[�c�̕���")]
    public PartsType enPartsType;
    [Header("�p�[�c�̏㔼�g�U��")]
    public UpperAttack upperAttack;
    [Header("�p�[�c�̉����g�U��")]
    public LowerAttack lowerAttack;

    [Header("�p�[�c�̖��O")]
    public string sPartsName;
    [Header("�A�^�b�N�͈�")]
    public float AttackArea;
    [Header("�p�[�c��HP")]
    public int iPartHp;
    [Header("�p�[�c�̍U����")]
    public int iPartAttack;
    [Header("�p�[�c�̑̉摜")]
    public Sprite spBody;
    [Header("�p�[�c�̉E�r�摜")]
    public Sprite spRightArm;
    [Header("�p�[�c�̉E��摜")]
    public Sprite spRightHand;
    [Header("�p�[�c�̍��r�摜")]
    public Sprite spLeftArm;
    [Header("�p�[�c�̍���摜")]
    public Sprite spLeftHand;
    [Header("�p�[�c�̍��摜")]
    public Sprite spWaist;
    [Header("�p�[�c�̉E�r�摜")]
    public Sprite spRightLeg;
    [Header("�p�[�c�̉E���摜")]
    public Sprite spRightFoot;
    [Header("�p�[�c�̍��r�摜")]
    public Sprite spLeftLeg;
    [Header("�p�[�c�̍����摜")]
    public Sprite spLeftFoot;

    //�㔼�g�̃h���b�v�p�[�c
    // ����A�v���C���[�̔s�k���o�Ƃ��Ẵh���b�v�ɂ̂ݎg�p
    [SerializeField, Header("�㔼�g�h���b�v�p�[�c")]
    private GameObject dropPartUpper;

    //�����g�̃h���b�v�p�[�c
    // ����A�v���C���[�̔s�k���o�Ƃ��Ẵh���b�v�ɂ̂ݎg�p
    [SerializeField, Header("�����g�h���b�v�p�[�c")]
    private GameObject dropPartLower;

    public GameObject DropPartUpper
    {
        get { return dropPartUpper; }
        set { dropPartUpper = value; }
    }
    public GameObject DropPartLower
    {
        get { return dropPartLower; }
        set { dropPartLower = value; }
    }
}
