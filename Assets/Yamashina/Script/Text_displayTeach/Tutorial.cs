using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public enum Tutorial_State
{
    PlayerMove,
    PlayerGauge,
    PlayerDoNotMove,
    PlayerAttack,
    PlayerComfort,
    EnemyDrop,
    PlayerTransplant,
    Option
}
public class Tutorial : TextDisplay
{
    private Tutorial_spown tutorial_Spawn;

    private const float POSITION_DONOT_MOVE = 21;

    [SerializeField, Header("チュートリアル画像を消すまでの時間")] private float tutorialDelete;

    private float tutorialTimer = 0;
    public static void NextState()
    {
        int nextIndex = (int)enGameState + 1; // 次のインデックス
        if (nextIndex < System.Enum.GetValues(typeof(Tutorial_State)).Length)
        {
            ChangeState((Tutorial_State)nextIndex);
        }
        else
        {
            Debug.Log("Tutorial finished!");
        }

        Debug.Log("Next State: " + enGameState);
    }

    static Tutorial_State enGameState = Tutorial_State.PlayerMove;
    static Tutorial_State previousGameState; // 前回のゲームステートを保存
    protected override void Start()
    {
        enGameState = Tutorial_State.PlayerMove;
        base.Start();
        tutorial_Spawn = FindAnyObjectByType<Tutorial_spown>();
    }
    public static void ChangeState(Tutorial_State newState)
    {
        previousGameState = enGameState; // 現在のステートを前回のステートとして保存

        enGameState = newState;
        Debug.Log("ChangeState" + newState);
    }
    // ステートが変わったかを確認する関数
    public static bool HasStateChanged()
    {
        return enGameState != previousGameState;
    }
    public static Tutorial_State GetState()
    {
        return enGameState;
    }
    private void ChangeStateToDoNotMoveIfNeeded()
    {
        if (enGameState == Tutorial_State.PlayerGauge && Player.transform.position.x > POSITION_DONOT_MOVE)
        {
            ChangeState(Tutorial_State.PlayerDoNotMove);
        }
    }
    protected override void Update()
    {

        switch (GameMgr.GetState())
        {

            case GameState.Main:
                base.Update();
                ChangeStateToDoNotMoveIfNeeded();

                break;
            case GameState.ShowText:
                base.Update();
                if (!TextArea.activeSelf)
                {
                    GameMgr.ChangeState(GameState.Tutorial);
                    tutorial_Spawn.SpawnTutorial();
                    Debug.Log(GameMgr.GetState().ToString());

                }

                break;
            case GameState.Tutorial:
                if (tutorialTimer > tutorialDelete)
                {
                    Debug.Log(tutorial_Spawn.newImageObject.transform.Find("EnterUI").gameObject);

                    tutorial_Spawn.newImageObject.transform.Find("EnterUI").gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.Space))
                    {



                        tutorial_Spawn.DestroyCanvasWithImage();

                        tutorialTimer = 0;
                    }
                }
                tutorialTimer += Time.deltaTime;

                if (GetState() == Tutorial_State.Option && tutorial_Spawn.canvasObject == null)
                {
                    ShowGameClearUI();
                }

                break;
            case GameState.Hint:
                base.Update(); break;
            case GameState.AfterBoss:
                base.Update(); break;

            case GameState.Clear:
                base.Update();
                break;




        }
    }

    public override void ShowHintText()
    {
        base.ShowHintText();
    }

    protected override void FinishTextHint()
    {
        base.FinishTextHint();
    }
    public override void FinishTextShowText()
    {
        base.FinishTextShowText();
    }
    protected override void initCurrentTextDisplay()
    {
        base.initCurrentTextDisplay();
    }
    protected override void UpdateHintText()
    {
        base.UpdateHintText();
    }
    public override void ShowTextChange()
    {

        for (int i = 0; i < Position.Length; i++)
        {
            if (Player.transform.position.x > Position[i] && Flag[i] == false)
            {
                Flag[i] = true;

                GameMgr.ChangeState(GameState.ShowText);    //GameStateがShowTextに変わる
                UpdateText();
                //テキスト表示域を表示域
                TextArea.SetActive(true);
                if (i != 0)
                {
                    NextState();
                }
            }


        }
    }
    //private void OnGUI()
    //{
    //    GUI.skin.label.fontSize = 30;  // 例えば30に設定
    //    GUI.skin.label.normal.textColor = Color.black;
    //    GUI.skin.label.fontStyle = FontStyle.Bold;
    //    GUI.Label(new Rect(10.0f, 400.0f, Screen.width, Screen.height), enGameState.ToString());
    //}
}
