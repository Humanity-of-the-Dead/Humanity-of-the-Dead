
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Main,
    ShowOption,
    ShowText,
    Clear, //クリア表示
    BeforeBoss, // 新しく追加：ボス戦直前

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
                break;
            case GameState.ShowOption:
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
        }
    }

        //ステートチェンジ
        public static void ChangeState(GameState newState)
        {
            enGameState = newState;
        Debug.Log(newState);
    }
        public static GameState GetState()
        {
            return enGameState;
        }
        private void ForceEnemiesMoveLeft()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                enemy.transform.Translate(Vector3.left * 5f); // 任意の速度で左移動
                EnemyMoveAnimation moveAnimation = GameObject.FindAnyObjectByType<EnemyMoveAnimation>();
                moveAnimation.LeftMove();
            }

        }
        private void OnGUI()
        {
            GUI.skin.label.fontSize = 30;  // 例えば30に設定

            GUI.Label(new Rect(10.0f, 10.0f, Screen.width, Screen.height), enGameState.ToString());
        }
    }

