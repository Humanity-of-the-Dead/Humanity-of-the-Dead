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
    [Header("�p�[�c�̘r�摜")]
    public Sprite spArm;
    [Header("�p�[�c�̎�摜")]
    public Sprite spHand;
    [Header("�p�[�c�̍��摜")]
    public Sprite spWaist;
    [Header("�p�[�c�̋r�摜")]
    public Sprite spLeg;
    [Header("�p�[�c�̑��摜")]
    public Sprite spFoot;
}
