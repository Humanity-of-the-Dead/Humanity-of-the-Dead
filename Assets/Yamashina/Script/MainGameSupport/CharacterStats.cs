using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour, IDamageable
{
    [Header("Hit Effect Settings")]
    public GameObject hitGameObject;
    public float upperEffectXMin, upperEffectXMax;
    public float upperEffectYMin, upperEffectYMax;
    public float lowerEffectXMin, lowerEffectXMax;
    public float lowerEffectYMin, lowerEffectYMax;

    // インターフェースのメソッドを実装する
    public abstract void TakeDamage(float damage, int body = 0);
    public abstract void ShowHitEffects(int body);



}
