using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour, IDamageable
{


    // インターフェースのメソッドを実装する
    public abstract void TakeDamage(float damage, int body = 0);
    //public abstract void ShowHitEffects(int body);



}
