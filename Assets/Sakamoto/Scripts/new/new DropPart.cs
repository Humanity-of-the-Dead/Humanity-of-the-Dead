using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class newDropPart : MonoBehaviour
{
    //パーツのデータ
    private BodyPartsData partsData;
    ////プレイヤーのmanager
    //PlayerParameter playerManager;

    //プレイヤー情報
    PlayerParameter scPlayerParameter;

    //クリアテキスト
    GameObject goTextBox;

    //ボスフラグ
    bool bBoss;
     SceneTransitionManager sceneTransitionManager;

    //ボタンオブジェクト
    [SerializeField] GameObject[] goButton;

    //お墓
    [SerializeField] GameObject goGrave;

    //ゲームクリアの標準
    GameObject goPanel;


    void Start()
    {
        //GameClearタグを持つゲームオブジェクトを取得
        goPanel = GameObject.Find("GameResult").gameObject;
        goPanel = goPanel.transform.Find("GameClear").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Jキーを押したら慰霊する
        if(Input.GetKeyUp(KeyCode.J) && goButton[1].activeSelf == true) {
            scPlayerParameter.comfort(10);
            MultiAudio.ins.PlaySEByName("SE_hero_action_irei");
            Debug.Log(this.transform.position);
            GameObject obj = Instantiate(goGrave);
            obj.transform.position = new Vector3(this.gameObject.transform.position.x,
                                                        0.5f, this.gameObject.transform.position.z);
            if (bBoss)
            {
                GameClear();
            }
            Destroy(this.gameObject);

        }
        //Lキーを押したら移植する
        if (Input.GetKeyDown(KeyCode.L) && goButton[0].activeSelf == true)
        {
            scPlayerParameter.transplant(partsData);
            MultiAudio.ins.PlaySEByName("SE_hero_action_ishoku");
            if (bBoss)
            {
                GameClear();
            }
            Destroy(this.gameObject);
        }
    }

    //パーツデータの取得
    public void getPartsData(BodyPartsData partsData)
    {
        this.partsData = partsData;
    }
    //アイテムの画像になる
    public void setImnage()
    {
        Image image = this.GetComponent<Image>();
        image.sprite = partsData.spBody; 
    }

    public void getPlayerManegerObjet(PlayerParameter scr)
    {
        scPlayerParameter = scr;
    }

    //テキストボックスの取得
    public void getTextBox(GameObject obj)
    {
        goTextBox = obj;
    }
    //ボスフラグ
    public void getBossf(bool flag)
    {
        bBoss = flag;
    }
    
    //移植
    public void getTransplant()
    {
        scPlayerParameter.transplant(partsData);
        Destroy(this.gameObject);
    }

    //慰霊
    public void getComfort()
    {
        scPlayerParameter.comfort(10);
        Destroy(this.gameObject);
    }
    public void getSceneTransition(SceneTransitionManager sceneTransitionManager)
    {
       this.sceneTransitionManager = sceneTransitionManager;    

    }

    //ゲームクリア処理
    private void GameClear()
    {
        //ゲームクリアを表示
        goPanel.SetActive(true);
        //goTextBox.GetComponent<GoalScript>().showText();

        //プレイヤーの状態を保持する
        scPlayerParameter.KeepBodyData();
        
        //現在のシーンの一つ先のシーンのインデックスを取得
        int iNextIndex = SceneTransitionManager.instance.sceneInformation.GetCurrentScene() + 1;
        //ステージが4の時
        if (iNextIndex == 4)
        {
            scPlayerParameter.DefaultBodyData();
        }
        //インデックスが上限に行ったらタイトルのインデックスを代入
        if (iNextIndex > 4)
        {
            //DontDestroyOnLoadになっているPlayerParameterオブジェクトを削除
            SceneManager.MoveGameObjectToScene(scPlayerParameter.gameObject, SceneManager.GetActiveScene());
            iNextIndex = 0;
        }
        GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().Stop();

        MultiAudio.ins.PlayBGM_ByName("BGM_clear");
        //テキストボックスの表示
        goTextBox.SetActive(true);
        //GameStateをAfterBOssに切り替える
        GameMgr.ChangeState(GameState.AfterBOss);
        //SceneTransitionManager.instance.NextSceneButton(iNextIndex);


    }
}
