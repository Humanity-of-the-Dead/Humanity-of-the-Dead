using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStateChange : MonoBehaviour
{
    //ボタンを押したときゲームステートをShowOptionにする
    public void ChangeStateShowOption()
    {
        GameMgr.ChangeState(GameState.ShowOption);
    }
    //ボタンを押したときゲームステートをMainにする
    public void ChangeStateMain()
    {
        GameMgr.ChangeState(GameState.Main);
    }
}
