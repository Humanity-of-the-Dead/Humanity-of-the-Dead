using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Floor : MonoBehaviour
{
    //プレイヤーパラメーター
    private PlayerParameter scPlayerParameter;
    
    [SerializeField] private float damage = 0.01f;

    private void Start()
    {
        scPlayerParameter= GameObject.FindAnyObjectByType<PlayerParameter>().GetComponent<PlayerParameter>();
    }

    protected void UpperEnemyAttack(float damage)
    {
        scPlayerParameter.UpperHP -= damage;
    }
    protected void LowerEnemyAttack(float damage)
    {
        scPlayerParameter.LowerHP -= damage;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            UpperEnemyAttack(damage);
            LowerEnemyAttack(damage);
        }
    }
}
