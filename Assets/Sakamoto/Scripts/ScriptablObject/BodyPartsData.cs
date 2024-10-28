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
    [Header("パーツの部位")]
    public PartsType enPartsType;
    [Header("パーツの名前")]
    public string sPartsName;
    [Header("パーツのHP")]
    public int iPartHp;
    [Header("パーツの攻撃力")]
    public int iPartAttack;
    [Header("パーツの画像")]
    public Sprite PartSprite;
}
