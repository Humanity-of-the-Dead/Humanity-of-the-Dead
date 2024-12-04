using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { 
    Main,
    ShowText,
    BeforeBoss, //ボス前
    AfterBOss,//ボス後
    GameOver,
}

public class GameMgr : MonoBehaviour
{
    [SerializeField] Button OptionButton;
    [SerializeField] Button OptionReturnButton;

    public GameState enGameState;

    private void Start()
    {
        enGameState = GameState.Main;
    }

    private void Update()
    {
        if(enGameState == GameState.Main && Input.GetKeyDown(KeyCode.G))
        {
            OptionButton.onClick.Invoke();
            enGameState = GameState.ShowText;
        }
        else if (enGameState == GameState.ShowText && Input.GetKeyDown(KeyCode.G))
        {
            OptionReturnButton.onClick.Invoke();
            enGameState = GameState.Main;
        }
    }

    //ステートチェンジ
    public void ChangeState(GameState newState)
    {
        enGameState = newState;
    }

}

