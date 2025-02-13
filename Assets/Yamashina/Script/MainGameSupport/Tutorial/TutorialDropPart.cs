using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDropPart : newDropPart
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
    protected override void DoComfort()
    {
        if (Input.GetKeyUp(KeyCode.J) && goButton.Length > 0 && goButton[0] != null && goButton[0].activeSelf && Tutorial.GetState() == Tutorial.Tutorial_State.PlayerComfort)
        {

            Tutorial.ChangeState(Tutorial.Tutorial_State.EnemyDrop);
            TutorialShowText();


        }
        base.DoComfort();



    }
    protected override void DoTransplant()
    {
        if (Input.GetKeyDown(KeyCode.L) && goButton.Length > 1 && goButton[1] != null && goButton[1].activeSelf&&Tutorial.GetState()==Tutorial.Tutorial_State.PlayerTransplant)
        {
            Tutorial.ChangeState(Tutorial.Tutorial_State.Option);
            TutorialShowText();

        }
        base.DoTransplant();




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
