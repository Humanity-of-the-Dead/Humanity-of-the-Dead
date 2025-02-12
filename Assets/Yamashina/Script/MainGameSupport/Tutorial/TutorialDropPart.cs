using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDropPart : newDropPart
{
    private Tutorial tutorial;
    private bool isComforted = false;
    private bool isTransplant = false;

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
    protected override void DoComfort()
    {
        base.DoComfort();
        if (!isComforted)
        {

            TutorialShowText();
            Tutorial.NextState();

            isComforted = true; 

        }


    }
    protected override void DoTransplant()
    {
        base.DoTransplant();
        if(!isTransplant)
        {
            TutorialShowText();
            Tutorial.NextState();

            isTransplant = true;    
        }
    }

    void TutorialShowText()
    {

        GameMgr.ChangeState(GameState.ShowText);    //GameStateがShowTextに変わる
        tutorial.UpdateText();
      
        //テキスト表示域を表示域
        tutorial.TextArea.SetActive(true);
    }
    protected override void GameClear()
    {
        base.GameClear();
    }
}
