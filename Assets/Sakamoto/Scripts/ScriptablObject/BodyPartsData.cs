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
    [Header("パーツの部位")]
    public PartsType enPartsType;
    [Header("パーツの名前")]
    public string sPartsName;
    [Header("アタック範囲")]
    public float AttackArea;
    [Header("パーツのHP")]
    public int iPartHp;
    [Header("パーツの攻撃力")]
    public int iPartAttack;
    [Header("パーツの体画像")]
    public Sprite spBody;
    [Header("パーツの腕画像")]
    public Sprite spArm;
    [Header("パーツの手画像")]
    public Sprite spHand;
    [Header("パーツの腰画像")]
    public Sprite spWaist;
    [Header("パーツの脚画像")]
    public Sprite spLeg;
    [Header("パーツの足画像")]
    public Sprite spFoot;
}
