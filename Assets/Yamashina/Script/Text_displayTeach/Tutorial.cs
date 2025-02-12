using UnityEngine;
using UnityEngine.UI;
public class Tutorial : TextDisplay
{
    private Tutorial_spown tutorial_Spawn;
    public enum Tutorial_State
    {
        PlayerMove,
        PlayerGauge,
        PlayerAttack,
        PlayerTransplant,
        EnemyDrop,
        PlayerComfort,
        Option
    }
    static Tutorial_State enGameState;
    static Tutorial_State previousGameState; // 前回のゲームステートを保存
    protected override void Start()
    {
        base.Start();
        tutorial_Spawn = FindAnyObjectByType<Tutorial_spown>();
    }
    public static void ChangeState(Tutorial_State newState)
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
    public static Tutorial_State GetState()
    {
        return enGameState;
    }
    protected override void Update()
    {

        switch (GameMgr.GetState())
        {

            case GameState.Main:
                base.Update();

                break;
            case GameState.ShowText:
                base.Update();
                if (!TextArea.activeSelf)
                {
                    GameMgr.ChangeState(GameState.Tutorial);
                    tutorial_Spawn.SpawnTutorial();
                    ChangeState(Tutorial_State.PlayerMove);


                }
                break;
            case GameState.Tutorial:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    tutorial_Spawn.DestroyCanvasWithImage();
                }
                break;
            case GameState.Hint:
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
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 30;  // 例えば30に設定
        GUI.skin.label.normal.textColor = Color.black;
        GUI.skin.label.fontStyle = FontStyle.Bold;  
        GUI.Label(new Rect(10.0f,400.0f, Screen.width, Screen.height), enGameState.ToString());
    }
}
