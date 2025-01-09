using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerParameter : MonoBehaviour
{
    //ゲームマネージャー
    [SerializeField]GameObject scGameMgr;

    //移植時のモザイク
    GameObject goMosaic;

    public static PlayerParameter Instance;

    [Header("1減少するのにかかる時間")]
    [SerializeField] int iDownTime;

    public  int iHumanityMax;     //人間性の最大値
    public  int iUpperHPMax;      //上半身のHPの最大値
    public  int iLowerHPMax;      //下半身のHPの最大値

    private float iHumanity;     //人間性
    private float iUpperHP;      //上半身のHP
    private float iLowerHP;      //下半身のHP
    // Start is called before the first frame update

    [Header("プレイヤーオブジェクト")]
    [SerializeField] GameObject goPlayer;

    //上半身のパーツデータ
    public BodyPartsData UpperData;
    //下半身のパーツデータ
    public BodyPartsData LowerData;
    //キャラのイメージ取得用
    PlayerMoveAnimation scPlayerMoveAnimation;
    //上半身のパーツデータ(保存用)
    private BodyPartsData upperIndex;
    //下半身のパーツデータ(保存用)
    private BodyPartsData lowerIndex;
    //上半身のパーツデータ(ステージ4用)
    private BodyPartsData upperPlayer;
    //下半身のパーツデータ(ステージ4用)
    private BodyPartsData lowerPlayer;



    //ゲームオーバーの標準
    GameObject goPanel;
    SceneTransitionManager sceneTransitionManager;


    public void Awake()
    {
        CheckInstance();
    }
    private void Start()
    {
        upperIndex = UpperData;
        lowerIndex = LowerData;
        upperPlayer = UpperData;
        lowerPlayer = LowerData;
        InitializeReferences();

        //コンポーネント取得
        scPlayerMoveAnimation = goPlayer.GetComponent<PlayerMoveAnimation>();

        //シーン遷移で破棄されない
        DontDestroyOnLoad(gameObject);

        // シーンがロードされた後に参照を再取得
        SceneManager.sceneLoaded += OnSceneLoaded;
        sceneTransitionManager = GameObject.FindAnyObjectByType<SceneTransitionManager>();
    }
    private void Update()
    {
       string SceneName = SceneManager.GetActiveScene().name;
        if(!(SceneName == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.Title)))
        {
            switch (GameMgr.GetState())
            {
                case GameState.Main:
                    //パラメータの値をiDownTime秒で1減少させる
                    iHumanity -= Time.deltaTime / iDownTime/* *dgbScale*/;
                    iUpperHP -= Time.deltaTime / iDownTime;
                    iLowerHP -= Time.deltaTime / iDownTime;
                    Debug.Log(iDownTime);
                    if (iHumanity < 0 || iUpperHP < 0 || iLowerHP < 0)
                    {

                        Debug.Log("リロードを開始します"); // デバッグログで確認

                        GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().Stop();
                        


                        //GameOverのBGM鳴らす箇所
                        MultiAudio.ins.PlayBGM_ByName("BGM_defeated");
                        #region 山品変更
                        ////パラメータの全回復
                        //iHumanity = iHumanityMax;
                        //iUpperHP = iUpperHPMax;
                        //iLowerHP = iLowerHPMax;

                        #endregion
                        //プレイヤーを初期化
                        UpperData = upperIndex;
                        LowerData = lowerIndex;

                        //ゲームオーバーの標準
                        goPanel.SetActive(true);
                        GameMgr.ChangeState(GameState.GameOver);

                    }

                    ////シーン移動
                    //if (Input.GetKeyDown(KeyCode.M))
                    //{
                    //    SceneManager.LoadScene("Stage2");
                    //}
                    break;
            }
        }
    }

    //慰霊
    //人間性を引数分回復する
    public void comfort(int iRecovery)
    {
        iHumanity += iRecovery;
        //回復した値が最大値を超えていたら最大値にする
        if (iHumanity > iHumanityMax)
        {
            iHumanity = iHumanityMax;
        }

    }
   
    //移植
    //パーツの画像とパラメータを入れ替える
    //BodyPartsData partsData : 入れ替えるパーツのスクリプタブルオブジェクト
    //テスト段階では引数はnullでいい
    public void transplant(BodyPartsData partsData)
    {
        //移植時にモザイクを表示させる
        //モザイク自体が時間差で消えるから表示だけでいい
        goMosaic.SetActive(true);

        //partsData = partsData ?? DefaultData;



        switch (partsData.enPartsType)
        {
            case PartsType.Upper:
                //パーツデータのHPをMaxに代入
                iUpperHPMax = partsData.iPartHp;
                iUpperHP = iUpperHPMax;
                //partDataの上書き
                UpperData = partsData;
                /*
                //SpriteRendererのSpriteにパーツデータのSpriteを挿入
                spriteRenderer.sprite = partsData.spBody;
                */
                //見た目変更関数待ち
                scPlayerMoveAnimation.ChangeUpperBody(partsData);
                //攻撃モーションの変更
                scPlayerMoveAnimation.ChangeUpperMove(partsData.upperAttack);
                break;
            case PartsType.Lower:
                //パーツデータのHPをMax代入
                iLowerHPMax = partsData.iPartHp;
                iLowerHP = iLowerHPMax;
                //partDataの上書き
                LowerData = partsData;
                /*
                //SpriteRendererのSpriteにパーツデータのSpriteを挿入
                spriteRenderer.sprite = partsData.spWaist;
                */
                //見た目変更関数待ち
                scPlayerMoveAnimation.ChangeUnderBody(partsData);
                //攻撃モーションの変更
                scPlayerMoveAnimation.ChangeLowerMove(partsData.lowerAttack);
                break;
        }

    }


    //人間性の取得
    public float Humanity
    {
        get { return iHumanity; }
        set { iHumanity = value; }
    }
    //上半身HPの取得
    public float UpperHP
    {
        get { return iUpperHP; }
        set { iUpperHP = value; }
    }
    //下半身HPの取得
    public float LowerHP
    {
        get { return iLowerHP; }
        set { iLowerHP = value; }
    }

    //シングルトンのチェック
    void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void InitializeReferences()
    {
        // シーン遷移後に必要なオブジェクトを再取得
        scGameMgr = GameObject.FindGameObjectWithTag("GameManager");
        goMosaic = GameObject.Find("Player Variant").gameObject;
        goMosaic = goMosaic.transform.Find("Mosaic").gameObject;
        goPlayer = GameObject.Find("Player Variant").gameObject;
        goPanel = GameObject.FindGameObjectWithTag("GameOver");

        //コンポーネント取得
        scPlayerMoveAnimation = goPlayer.GetComponent<PlayerMoveAnimation>();

        //最大値を設定
        iUpperHPMax = UpperData.iPartHp;
        iLowerHPMax = LowerData.iPartHp;
        //パラメータの初期化
        iHumanity = iHumanityMax;
        iUpperHP = iUpperHPMax;
        iLowerHP = iLowerHPMax;

        scPlayerMoveAnimation.ChangeUpperBody(UpperData);
        scPlayerMoveAnimation.ChangeUpperMove(UpperData.upperAttack);
        scPlayerMoveAnimation.ChangeUnderBody(LowerData);
        scPlayerMoveAnimation.ChangeLowerMove(LowerData.lowerAttack);

        if (scGameMgr == null || goMosaic == null || goPanel == null)
        {
            Debug.LogWarning("必要なオブジェクトが見つかりません");
        }

    }

    /// <summary>
    /// ステージクリア時プレイヤーの状態を保持する
    /// DropPartに呼んでもらう
    /// </summary>
    public void KeepBodyData()
    {
        upperIndex = UpperData;
        lowerIndex = LowerData;
    }

    /// <summary>
    /// ステージクリア4の時デフォルトの状態にする
    /// DropPartに呼んでもらう
    /// </summary>
    public void DefaultBodyData()
    {
        UpperData = upperPlayer;
        LowerData = lowerPlayer;
        upperIndex = upperPlayer;
        lowerIndex = lowerPlayer;
    }
    private void OnEnable()
    {
        // シーンがロードされた後に参照を再取得
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // イベントの解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーン遷移後に参照を再取得
        InitializeReferences();
        //upperIndex = UpperData;
        //lowerIndex = LowerData;
        Debug.Log($"シーン {scene.name} がロードされました");
    }
}
