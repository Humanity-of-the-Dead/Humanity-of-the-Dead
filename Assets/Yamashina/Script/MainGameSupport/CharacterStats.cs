using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour, IDamageable
{

    [SerializeField, Header("エフェクト関連")]
    [Tooltip("エフェクトのプレハブを代入")]
    public GameObject hitGameObject;
    [Tooltip("エフェクトが上半身に出る範囲（X座標),最大と最小")]

    public float upperEffectXMin, upperEffectXMax;
    [Tooltip("エフェクトが上半身に出る範囲（Y座標),最大と最小")]

    public float upperEffectYMin, upperEffectYMax;
    [Tooltip("エフェクトが下半身に出る範囲（X座標),最大と最小")]

    public float lowerEffectXMin, lowerEffectXMax;
    [Tooltip("エフェクトが下半身に出る範囲（Y座標),最大と最小")]

    public float lowerEffectYMin, lowerEffectYMax;

    // インターフェースのメソッドを実装する
    public abstract void TakeDamage(float damage, int body = 0);
    public abstract void ShowHitEffects(int body);



}
