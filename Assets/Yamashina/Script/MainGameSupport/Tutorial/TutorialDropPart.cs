using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDropPart : newDropPart
{
    private Tutorial tutorial;
    private string newLayerName = "Enemy"; // 変更したいレイヤー名

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

            GameObject GameMain = GameObject.Find("GameMain");

            GameMain = GameMain.transform.Find("Nomal_stg01 Variant_Add").gameObject;
            int newLayer = LayerMask.NameToLayer(newLayerName);
            SetLayerRecursively(GameMain, newLayer);
            Debug.Log(GameMain);
            GameMain.SetActive(true);


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
    // 再帰的にレイヤーを変更する関数
    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
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
