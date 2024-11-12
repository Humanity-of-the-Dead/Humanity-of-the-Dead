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
    }
    protected void LowerEnemyAttack(float damage)
    {
        scPlayerParameter.LowerHP -= damage; 
    }
}
