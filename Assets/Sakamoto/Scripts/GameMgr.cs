using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { 
    Main,
    ShowOption,
    ShowText,
    Clear, //クリア表示
    AfterBOss,//ボス後
    GameOver,
}

public class GameMgr : MonoBehaviour
{
    [SerializeField] Button OptionButton;
    [SerializeField] Button OptionReturnButton;

    static GameState enGameState;

    float timer = 0;
    private void Start()
    {
        enGameState = GameState.Main;
    }

    private void Update()
    {
        if(enGameState == GameState.Main && Input.GetKeyDown(KeyCode.G))
        {
            OptionButton.onClick.Invoke();
            //enGameState = GameState.ShowText;
        }
        else if (enGameState == GameState.ShowOption && Input.GetKeyDown(KeyCode.G))
        {
            OptionReturnButton.onClick.Invoke();
            //enGameState = GameState.Main;
        }

        if(enGameState == GameState.GameOver)
        {
            if(timer > 1)
            {
                SceneTransitionManager.instance.ReloadCurrentScene();
                timer = 0;
            }
            timer += Time.deltaTime;
        }
    }

    //ステートチェンジ
    public static void ChangeState(GameState newState)
    {
        enGameState = newState;
    }
    public static GameState GetState()
    {
        return enGameState;
    }
}

