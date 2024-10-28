using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum PartsType 
{
    Head,
    Arm,
    Leg,
}

[CreateAssetMenu(menuName = "ScriptableObject/Create BodyPartsData")] 

public class BodyPartsData : ScriptableObject
{
    [Header("�p�[�c�̕���")]
    public PartsType enPartsType;
    [Header("�p�[�c�̖��O")]
    public string sPartsName;
    [Header("�p�[�c��HP")]
    public int iPartHp;
    [Header("�p�[�c�̍U����")]
    public int iPartAttack;
    [Header("�p�[�c�̉摜")]
    public Sprite PartSprite;
}
