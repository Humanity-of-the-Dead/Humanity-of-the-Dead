using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEnemyParameter : newEnemyParameters
{
    private Tutorial tutorial;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        tutorial = FindFirstObjectByType<Tutorial>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        // 部位が破壊された際にHPバーを一瞬表示
        if ((UpperHP <= 0 || LowerHP <= 0) && newEnemyMovement.GetEnemyState() != newEnemyMovement.EnemyState.IsDead&&Tutorial.GetState()==Tutorial_State.PlayerAttack)
        {
            GameMgr.ChangeState(GameState.ShowText);    //GameStateがShowTextに変わる
            tutorial.UpdateText();
            Tutorial.ChangeState(Tutorial_State.PlayerComfort);

            //テキスト表示域を表示域
            tutorial.TextArea.SetActive(true);
        }

        if ((UpperHP <= 0 || LowerHP <= 0) && newEnemyMovement.GetEnemyState() != newEnemyMovement.EnemyState.IsDead && Tutorial.GetState() == Tutorial_State.EnemyDrop)
        {
            GameMgr.ChangeState(GameState.ShowText);    //GameStateがShowTextに変わる
            tutorial.UpdateText();
            Tutorial.ChangeState(Tutorial_State.PlayerTransplant);

            //テキスト表示域を表示域
            tutorial.TextArea.SetActive(true);
        }
       
        base.Update();
    }

    protected override IEnumerator ShowHPBarAndDestroy(Image hpBar, BodyPartsData part, bool typ)
    {

        return base.ShowHPBarAndDestroy(hpBar, part, typ);

    }
    protected override IEnumerator FlashObject(int body = 0)
    {
       
        return base.FlashObject(body);
    }

    public override void TakeDamage(float damage, int body = 0)
    {
        base.TakeDamage(damage, body);
       
       

    }

}
