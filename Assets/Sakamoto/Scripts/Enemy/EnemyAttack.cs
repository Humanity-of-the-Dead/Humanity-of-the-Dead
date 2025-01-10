using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //プレイヤーパラメーター
    public PlayerParameter scPlayerParameter;

    protected void UpperEnemyAttack(float damage)
    {
        scPlayerParameter.UpperHP -= damage;
        MultiAudio.ins.PlaySEByName("SE_common_hit_attack");
    }
    protected void LowerEnemyAttack(float damage)
    {
        scPlayerParameter.LowerHP -= damage;
        MultiAudio.ins.PlaySEByName("SE_common_hit_attack");
    }
}
