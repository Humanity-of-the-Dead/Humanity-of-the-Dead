using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { 
    Main,
    ShowText,
    BeforeBoss, //�{�X�O
    AfterBOss,//�{�X��
    GameOver,
}

public class GameMgr : MonoBehaviour
{
    public GameState enGameState;

    private void Start()
    {
        enGameState = GameState.Main;
    }

    //�X�e�[�g�`�F���W
    public void ChangeState(GameState newState)
    {
        enGameState = newState;
    }

}

