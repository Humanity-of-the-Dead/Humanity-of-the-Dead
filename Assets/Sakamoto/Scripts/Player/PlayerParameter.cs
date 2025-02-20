using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerParameter : CharacterStats
{
    //移植時のモザイク
    private GameObject goMosaic;

    public static PlayerParameter Instance;

    [SerializeField, Header("1減少するのにかかる時間")]
    protected float iDownTime;
    [SerializeField, Header("ステージ１の減少速度の調整用")]

    protected float slowFactor;
    [SerializeField, Header("人間性の最大値")]
    public int iHumanityMax;     //人間性の最大値
    [SerializeField, Header("上半身のHPの最大値,パーツが変わる度に\n数値が変わるため設定はそのままでOK")]

    public int iUpperHPMax;      //上半身のHPの最大値
    [SerializeField, Header("下半身のHPの最大値,パーツが変わる度に\n数値が変わるため設定はそのままでOK")]

    public int iLowerHPMax;      //下半身のHPの最大値

    protected float iHumanity;     //人間性
    protected float iUpperHP;      //上半身のHP
    protected float iLowerHP;      //下半身のHP

    [Header("プレイヤーコントロールスクリプト")]
    [SerializeField] private PlayerControl playerControl;

    //上半身のパーツデータ
    public BodyPartsData UpperData;
    //下半身のパーツデータ
    public BodyPartsData LowerData;
    //上半身のパーツデータ
    public BodyPartsData upperDataDefault;
    //下半身のパーツデータ
    public BodyPartsData lowerDataDefault;
    //上半身のパーツデータ(ステージ4用)
    public BodyPartsData UpperDataForStageFour;
    //下半身のパーツデータ(ステージ4用)
    public BodyPartsData LowerDataForStageFour;
    //キャラのイメージ取得用
    private PlayerMoveAnimation playerMoveAnimation;
    //上半身のパーツデータ(保存用)
    private BodyPartsData upperIndex;
    //下半身のパーツデータ(保存用)
    private BodyPartsData lowerIndex;
    ////上半身のパーツデータ(ステージ4用)
    //private BodyPartsData upperPlayer;
    ////下半身のパーツデータ(ステージ4用)
    //private BodyPartsData lowerPlayer;

    private EnemyMoveAnimation enemyMoveAnimation;


    //ゲームオーバーの標準
    private GameObject goPanel;

    private bool hasDroped = false;

    private const float GAMEOVER_ZOMBIEWALK_TIMEMAX = 0.3f;
    private const float GAMEOVER_ZOMBIEWALK_SPEED = 0.2f;


    protected virtual void Awake()
    {
        CheckInstance();
        InitBodyIndex();
        //コンポーネント取得
        InitializeReferences();
    }
    protected virtual void Start()
    {
        enemyMoveAnimation = FindObjectOfType<EnemyMoveAnimation>();


        //シーン遷移で破棄されない
        DontDestroyOnLoad(gameObject);

    }

    protected virtual void Update()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        if (!(SceneName == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.Title)))
        {
            switch (GameMgr.GetState())
            {
                case GameState.Main:
                    //パラメータの値をiDownTime秒で1減少させる
                    DecreasingHP();
                    //Debug.Log(iDownTime);
                    if (iHumanity < 0 || iUpperHP < 0 || iLowerHP < 0)
                    {

                        Debug.Log("リロードを開始します"); // デバッグログで確認

                        AudioSource BGM = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
                        MultiAudio.ins.PlayBGM_ByName("BGM_defeated");

                        BGM.loop = false;


                        #region 山品変更
                        ////パラメータの全回復
                        //iHumanity = iHumanityMax;
                        //iUpperHP = iUpperHPMax;
                        //iLowerHP = iLowerHPMax;

                        #endregion
                        //プレイヤーを初期化
                        //ゲームオーバーの標準
                        goPanel.SetActive(true);
                        GameMgr.ChangeState(GameState.GameOver);

                    }
                    break;



                ////シーン移動
                //if (Input.GetKeyDown(KeyCode.M))
                //{
                //    SceneManager.LoadScene("Stage2");
                //}

                case GameState.GameOver:
                    if (iHumanity < 0)
                    {
                        // 左方向へ移動
                        // ゾンビ歩きアニメーション
                        playerMoveAnimation.SetTimeMax(GAMEOVER_ZOMBIEWALK_TIMEMAX);
                        playerMoveAnimation.HandleWalk(PlayerMoveAnimation.SHAFT_DIRECTION_LEFT, true);
                        // 移動
                        Vector3 vPosition = playerControl.transform.position;
                        vPosition.x -= Time.deltaTime * GAMEOVER_ZOMBIEWALK_SPEED;
                        playerControl.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        playerControl.transform.position = vPosition;
                    }
                    else if (iUpperHP < 0)
                    {
                        DropAndRemovePlayerOnce(false);
                    }
                    else if (iLowerHP < 0)
                    {
                        DropAndRemovePlayerOnce(true);
                    }
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
                playerControl.ChangeUpperBody(partsData);
                //攻撃モーションの変更
                playerMoveAnimation.ChangeUpperMove(partsData.upperAttack);
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
                playerControl.ChangeUnderBody(partsData);
                //攻撃モーションの変更
                playerMoveAnimation.ChangeLowerMove(partsData.lowerAttack);
                break;
        }

    }

    /// <summary>
    /// 上下両方を一括移植
    /// </summary>
    public void transplantBoth(BodyPartsData upperPart, BodyPartsData lowerPart)
    {
        // モザイクを表示させる
        goMosaic.SetActive(true);

        // パーツデータのHPをMaxに代入
        iUpperHPMax = upperPart.iPartHp;
        iUpperHP = iUpperHPMax;
        iLowerHPMax = lowerPart.iPartHp;
        iLowerHP = iLowerHPMax;
        // 部位データの上書き
        UpperData = upperPart;
        LowerData = lowerPart;
        // 見た目変更関数待ち
        playerControl.ChangeUpperBody(upperPart);
        playerControl.ChangeUnderBody(lowerPart);
        // 攻撃モーションの変更
        playerMoveAnimation.ChangeUpperMove(upperPart.upperAttack);
        playerMoveAnimation.ChangeLowerMove(lowerPart.lowerAttack);
    }

    /// <summary>
    /// 親友の身体を上下とも移植
    /// </summary>
    public void transplantFriendBoth()
    {
        transplantBoth(UpperDataForStageFour, LowerDataForStageFour);
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

    /// <summary>
    /// upperIndexとlowerIndexを初期化
    /// </summary>
    public void InitBodyIndex()
    {
        upperIndex = upperDataDefault;
        lowerIndex = lowerDataDefault;
    }

    /// <summary>
    /// シーン読み込み時の初期化処理
    /// upperIndex/upperIndexは初期化されない
    /// </summary>
    protected virtual void InitializeReferences()
    {
        // シーン遷移後に必要なオブジェクトを再取得
        goMosaic = GameObject.Find("Player Variant");
        goMosaic = goMosaic.transform.Find("Mosaic").gameObject;
        goPanel = GameObject.Find("GameResult");
        goPanel = goPanel.transform.Find("GameOver").gameObject;
        playerControl = GameObject.Find("Player Variant").GetComponent<PlayerControl>();
        //コンポーネント取得
        playerMoveAnimation = playerControl.GetComponent<PlayerMoveAnimation>();

        UpperData = upperIndex;
        LowerData = lowerIndex;
        Debug.Log($"upperIndexは{upperIndex}");
        Debug.Log($"lowerIndexは{lowerIndex}");

        //最大値を設定
        iUpperHPMax = UpperData.iPartHp;
        iLowerHPMax = LowerData.iPartHp;
        //パラメータの初期化
        iHumanity = iHumanityMax;
        iUpperHP = iUpperHPMax;
        iLowerHP = iLowerHPMax;
        //Debug.Log(hitGameObject);


        hasDroped = false;

        playerControl.ChangeUpperBody(UpperData);
        playerMoveAnimation.ChangeUpperMove(UpperData.upperAttack);
        playerControl.ChangeUnderBody(LowerData);
        playerMoveAnimation.ChangeLowerMove(LowerData.lowerAttack);

        if (goMosaic == null || goPanel == null)
        {
            Debug.LogWarning("必要なオブジェクトが見つかりません");
        }

    }

    /// <summary>
    /// ステージ遷移時プレイヤーの状態を保持する
    /// 雑魚ステージ時はTextDisplayに呼んでもらう
    /// ボスステージ時はDropPartに呼んでもらう
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
    public void SetBadyForStageFour()
    {
        UpperData = UpperDataForStageFour;
        LowerData = LowerDataForStageFour;
        upperIndex = UpperDataForStageFour;
        lowerIndex = LowerDataForStageFour;
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
    public override void TakeDamage(float damage, int body = 0)
    {
        //HPが減る仕組み
        //damageはテスト用の関数
        if (body == 0)
        {
            //上半身のHPを減らす
            UpperHP -= damage;
            enemyMoveAnimation.ShowHitEffects(body, playerControl.transform.position);
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");

            //Debug.Log(UpperHP);

        }

        if (body == 1)
        {
            //下半身のHPを減らす
            LowerHP -= damage;
            enemyMoveAnimation.ShowHitEffects(body, playerControl.transform.position);
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");

            //Debug.Log(LowerHP);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = SceneTransitionManager.instance.sceneInformation.GetCurrentSceneName();

        // シーンが Title または End でない場合に InitializeReferences を実行
        if (sceneName != SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.Title) &&
            sceneName != SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.End))
        {
            InitializeReferences();
        }

        Debug.Log($"シーン {scene.name} がロードされました");
    }

    private void DropAndRemovePlayerOnce(bool dropsUpper)
    {
        if (!hasDroped)
        {
            hasDroped = true;

            GameObject bodyPart = dropsUpper ? UpperData.DropPartUpper : LowerData.DropPartLower;
            GameObject drop = Instantiate(bodyPart);
            drop.transform.position = playerControl.transform.position;
            // イショクイレイボタン非表示
            drop.GetComponentInChildren<DropButton>().ShowsButton = false;

            // プレイヤーを非表示
            playerControl.SetEnabledPlayerRenderer(false);

            MultiAudio.ins.PlaySEByName("SE_common_breakbody");
        }
    }

    protected virtual void DecreasingHP()
    {
        string sceneName = SceneTransitionManager.instance.sceneInformation.GetCurrentSceneName();

        if (sceneName != SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageOne))
        {
            iHumanity -= Time.deltaTime / iDownTime/* *dgbScale*/;
            iUpperHP -= Time.deltaTime / iDownTime;
            iLowerHP -= Time.deltaTime / iDownTime;


        }
        else
        {
            iHumanity -= (Time.deltaTime / iDownTime) * slowFactor;
            iUpperHP -= Time.deltaTime / iDownTime * slowFactor;
            iLowerHP -= Time.deltaTime / iDownTime * slowFactor;
        }

    }
}
