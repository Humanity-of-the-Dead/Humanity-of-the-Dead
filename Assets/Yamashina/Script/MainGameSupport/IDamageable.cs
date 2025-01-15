using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, int body = 0);
    void ShowHitEffects(int body);
}

