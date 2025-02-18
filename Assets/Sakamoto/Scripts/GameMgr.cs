
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Main,
    ShowOption,
    ShowText,
    Tutorial,
    
    

    Clear, //クリア表示
    BeforeBoss, // 新しく追加：ボス戦直前
    Hint,
    AfterBoss,//ボス後
    GameOver,
}

public class GameMgr : MonoBehaviour
{
    [SerializeField] Button OptionButton;
    [SerializeField] Button OptionReturnButton;
    [SerializeField] Button TitleButton;
    static GameState enGameState;
    static GameState previousGameState; // 前回のゲームステートを保存
    float timer = 0;
    private void Start()
    {
        enGameState = GameState.Main;
        previousGameState = enGameState; // 初期化

    }

    private void Update()
    {


        switch (GetState())
        {
            case GameState.BeforeBoss:

                //ForceEnemiesMoveLeft();  // 雑魚キャラを左移動させる

                break;
            case GameState.Main:
                if (Input.GetKeyDown(KeyCode.G))
                {
                    OptionButton.onClick.Invoke();

                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    //hintButton.onClick.Invoke();
                    TextDisplay textDisplay = FindAnyObjectByType<TextDisplay>();
                    textDisplay.ShowHintText();
                }
                break;
            case GameState.ShowOption:

                //時間停止
                Time.timeScale = 0.0f;

                TitleButton.onClick.AddListener(() =>
           SceneTransitionManager.instance.NextSceneButton(0));
                TitleButton.onClick.AddListener(() => ResetTime());
                ////TitleButton.onClick.AddListener(() => PlayerParameter.Instance.ResetPlayerData()
                //);
                if (Input.GetKeyDown(KeyCode.G))
                {
                    OptionReturnButton.onClick.Invoke();

                }

                break;
            case GameState.GameOver:

                if (timer > 1)
                {
                    SceneTransitionManager.instance.ReloadCurrentScene();
                    timer = 0;
                }
                timer += Time.deltaTime;

                break;

            case GameState.Hint:

                break;
        }
    }

    //ステートチェンジ
    public static void ChangeState(GameState newState)
    {
        previousGameState = enGameState; // 現在のステートを前回のステートとして保存

        enGameState = newState;
        Debug.Log(newState);
    }
    // ステートが変わったかを確認する関数
    public static bool HasStateChanged()
    {
        return enGameState != previousGameState;
    }

    public static GameState GetState()
    {
        return enGameState;
    }
    public void ResetTime()
    {
        Time.timeScale = 1.0f;

    }

    //private void OnGUI()
    //{
    //    GUI.skin.label.fontSize = 30;  // 例えば30に設定

    //    GUI.Label(new Rect(10.0f, 10.0f, Screen.width, Screen.height), enGameState.ToString());
    //}
}

