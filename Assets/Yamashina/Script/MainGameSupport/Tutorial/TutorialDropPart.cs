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
            isComforted = true; 

        }


    }
    protected override void DoTransplant()
    {
        base.DoTransplant();
        if(!isTransplant&&tutorial.IsTextFullyDisplayed())
        {
            TutorialShowText();
            isTransplant = true;    
        }
    }

    void TutorialShowText()
    {

        GameMgr.ChangeState(GameState.ShowText);    //GameState��ShowText�ɕς��
        tutorial.UpdateText();
        //�e�L�X�g�\�����\����
        tutorial.TextArea.SetActive(true);
    }
    protected override void GameClear()
    {
        base.GameClear();
    }
}
