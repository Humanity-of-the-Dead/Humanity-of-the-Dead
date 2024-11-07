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
    [Header("パーツの右腕画像")]
    public Sprite spRightArm;
    [Header("パーツの右手画像")]
    public Sprite spRightHand;
    [Header("パーツの左腕画像")]
    public Sprite spLeftArm;
    [Header("パーツの左手画像")]
    public Sprite spLeftHand;
    [Header("パーツの腰画像")]
    public Sprite spWaist;
    [Header("パーツの右脚画像")]
    public Sprite spRightLeg;
    [Header("パーツの右足画像")]
    public Sprite spRightFoot;
    [Header("パーツの左脚画像")]
    public Sprite spLeftLeg;
    [Header("パーツの左足画像")]
    public Sprite spLeftFoot;
}
