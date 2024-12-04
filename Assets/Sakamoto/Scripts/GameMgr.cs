using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { 
    Main,
    ShowText,
    BeforeBoss, //ボス前
    AfterBOss,//ボス後
    GameOver,
}

public class GameMgr : MonoBehaviour
{
    public GameState enGameState;

    private void Start()
    {
        enGameState = GameState.Main;
    }

    //ステートチェンジ
    public void ChangeState(GameState newState)
    {
        enGameState = newState;
    }

}

