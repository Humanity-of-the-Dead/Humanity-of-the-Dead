
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Fire_Floor : MonoBehaviour
{
#if DEBUG

#endif
    //プレイヤーパラメーター
    //private PlayerParameter scPlayerParameter;
    
    [SerializeField] private float damage = 0.01f;

    float time;

    private void Start()
    {
        time = 0;
    }

    protected void UpperEnemyAttack(float damage)
    {
        PlayerParameter.Instance.UpperHP -= damage;

    }
    protected void LowerEnemyAttack(float damage)
    {
        PlayerParameter.Instance.LowerHP -= damage;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(time > 0.5)
        {
            if (collision.gameObject.tag == "Player")
            {

                UpperEnemyAttack(damage);
                LowerEnemyAttack(damage);
                MultiAudio.ins.PlaySEByName("SE_hero_hit_fire");
                time = 0;
            }
        }
        time += Time.deltaTime;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        time = 1;
    }
}
