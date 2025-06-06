using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class newDropPart : MonoBehaviour
{
    //パーツのデータ
    private BodyPartsData partsData;

    private bool bBoss;

    [SerializeField, Header("人間性の回復量")]
    protected int humanityRecoveryAmount = 10;
    //ボタンオブジェクト
    [SerializeField] protected GameObject[] goButton;

    //お墓
    [SerializeField] protected GameObject goGrave;

    [SerializeField, Header("親友の身体か")]
    private bool isFriendBothParts = false;

    private PlayerControl playerControl;

    //ゲームクリアの標準
    GameObject goPanel;


    protected virtual void Start()
    {
        //GameClearタグを持つゲームオブジェクトを取得
        goPanel = GameObject.Find("GameResult").gameObject;
        goPanel = goPanel.transform.Find("GameClear").gameObject;
        playerControl = GameObject.Find("Player Variant").GetComponent<PlayerControl>();
        if (playerControl != null)
        {
            Collider2D playerCollider = playerControl.GetComponent<Collider2D>();
            Collider2D thisCollider = GetComponent<Collider2D>();
            if (playerCollider != null && thisCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, thisCollider);
            }
        }
        else
        {
            Debug.LogError(playerControl);
        }

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch (GameMgr.GetState())
        {
            case GameState.Main:


                DoComfort();
                DoTransplant();
               
                break;

            default:
                //Debug.Log("プレイヤーが動いていないこと確認");
                break;
        }
    }



    //パーツデータの取得
    public void getPartsData(BodyPartsData partsData)
    {
        this.partsData = partsData;
    }
    //アイテムの画像になる


    //テキストボックスの取得
    //public void getTextBox(GameObject obj)
    //{
    //    goTextBox = obj;
    //}
    //ボスフラグ
    public void getBossf(bool flag)
    {
        bBoss = flag;
    }

    //移植
    public void getTransplant()
    {
        PlayerParameter.Instance.transplant(partsData);
        Destroy(gameObject);
    }

    //慰霊
    public void getComfort()
    {
        PlayerParameter.Instance.comfort(humanityRecoveryAmount);
        Destroy(gameObject);
    }


    protected virtual void DoComfort()
    {
        // Jキーを押したら慰霊する
        if (Input.GetKeyUp(KeyCode.J) && goButton.Length > 0 && goButton[0] != null && goButton[0].activeSelf)
        {
            PlayerParameter.Instance.comfort(humanityRecoveryAmount);
            MultiAudio.ins.PlaySEByName("SE_hero_action_irei");
            GameObject obj = Instantiate(goGrave);
            obj.transform.position = new Vector3(this.gameObject.transform.position.x, 0.5f, this.gameObject.transform.position.z);
            DestroyImmediate(gameObject);

            if (bBoss)
            {
                GameClear();
            }
        }
    }
    protected virtual void DoTransplant()
    {
        // Lキーを押したら移植する
        if (Input.GetKeyDown(KeyCode.L) && goButton.Length > 1 && goButton[1] != null && goButton[1].activeSelf)
        {
            if (isFriendBothParts)
            {
                PlayerParameter.Instance.transplantFriendBoth();
            }
            else
            {
                PlayerParameter.Instance.transplant(partsData);
            }
            MultiAudio.ins.PlaySEByName("SE_hero_action_ishoku");
            DestroyImmediate(gameObject);

            if (bBoss)
            {
                GameClear();
            }
        }
    }
    //ゲームクリア処理
    protected virtual void GameClear()
    {
        ////ゲームクリアを表示
        //goPanel.SetActive(true);
        //goTextBox.GetComponent<GoalScript>().showText();
        //テキストボックスの表示
        //goTextBox.SetActive(true);
        //GameStateをAfterBossに切り替える
        GameMgr.ChangeState(GameState.AfterBoss);
        TextDisplay textDisplay = FindAnyObjectByType<TextDisplay>();
        textDisplay.TextArea.SetActive(true);

        textDisplay.UpdateText();
        //SceneTransitionManager.instance.NextSceneButton(iNextIndex);

        //プレイヤーの状態を保持する
        PlayerParameter.Instance.KeepBodyData();

        //現在のシーンの一つ先のシーンのインデックスを取得
        string sceneName = SceneManager.GetActiveScene().name;

        //ステージが4の時
        if (sceneName == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageThreeDotFive))
        {
            PlayerParameter.Instance.SetBadyForStageFour();
        }
        //インデックスが上限に行ったらタイトルのインデックスを代入
        if (sceneName == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageFour))
        {

            SceneManager.MoveGameObjectToScene(PlayerParameter.Instance.gameObject, SceneManager.GetActiveScene());

        }


    }

    /// <summary>
    /// 床と接した時
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Car"))
        {
            var rigidbody2D = GetComponent<Rigidbody2D>();
            rigidbody2D.velocity = Vector2.zero; // 速度をリセット
            rigidbody2D.angularVelocity = 0f; // 回転速度をリセット
            rigidbody2D.gravityScale = 0f; // 重力を無効化
        }
    }
}
