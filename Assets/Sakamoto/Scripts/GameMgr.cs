using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { 
    Main,
    ShowText,
}

public class GameMgr : MonoBehaviour
{
    public GameState enGameState;

    private void Start()
    {
        enGameState = GameState.ShowText;
    }

    //�X�e�[�g�`�F���W
    public void ChangeState(GameState newState)
    {
        enGameState = newState;
    }

}

