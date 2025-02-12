using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyParameter : newEnemyParameters
{
    private Tutorial tutorial;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        tutorial = GameObject.FindFirstObjectByType<Tutorial>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();  
    }

    public override void Drop(BodyPartsData part, bool typ = true)
    {
        base.Drop(part, typ);
        GameMgr.ChangeState(GameState.ShowText);    //GameStateがShowTextに変わる
        tutorial.UpdateText();
        //テキスト表示域を表示域
        tutorial.TextArea.SetActive(true);
    }
}
