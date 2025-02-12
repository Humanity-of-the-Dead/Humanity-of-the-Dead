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
        base.Update();
    }

    protected override IEnumerator ShowHPBarAndDestroy(Image hpBar, BodyPartsData part, bool typ)
    {
       
        return base.ShowHPBarAndDestroy(hpBar, part, typ);

    }
    protected override IEnumerator FlashObject(int body = 0)
    {
        if (!hasDroped)
        {
            GameMgr.ChangeState(GameState.Main);
        }
        return base.FlashObject(body);
    }

    public override void TakeDamage(float damage, int body = 0)
    {
        base.TakeDamage(damage, body);
        if(Tutorial.GetState()==Tutorial.Tutorial_State.EnemyDrop)
        {
            Tutorial.NextState();

        }

    }

}
