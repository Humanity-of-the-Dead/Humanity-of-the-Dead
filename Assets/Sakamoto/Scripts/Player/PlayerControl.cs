using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [System.Serializable]
    //スプライト関連を一つのクラスにまとめてしまって、そのデータを[System.Serializable]でインスペクタでも設定できる
    private class CharacterSprites
    {
        public SpriteRenderer head;
        public SpriteRenderer body;
        public SpriteRenderer armRight;
        public SpriteRenderer armLeft;
        public SpriteRenderer handRight;
        public SpriteRenderer handLeft;
        public SpriteRenderer waist;
        public SpriteRenderer legRight;
        public SpriteRenderer legLeft;
        public SpriteRenderer footRight;
        public SpriteRenderer footLeft;
    }
    // キャラクターパーツ (SpriteRenderer)
    [SerializeField, Header("キャラクターパーツ")]
    private CharacterSprites characterSprites;

    //モーションアニメスクリプト
    private PlayerMoveAnimation playerMoveAnimation;



    //private Rigidbody2D rigidbody2D;
    [SerializeField, Header("移動スピード")]
    private float playerSpeed;
    [SerializeField, Header("ジャンプ力")]
    private float playerJumpPower;
    //ジャンプできるかどうか
    private bool isJump = false;
    //連続ジャンプ
    private int jumpCount;


    //カメラ関連
    [SerializeField, Header("カメラを代入")] private Camera mainCamera;
    //高さ
    private float mainCameraHeight;
    //幅
    private float mainCameraWidth;

    //ターゲット
    [SerializeField, Header("ボスのオブジェクトを入れる\nボス以外はスポナーが勝手に生成")]
    public List<GameObject> enemyObject;
    //private float originalGravityScale;
    Rigidbody2D playerRigidBody2D;

    //[SerializeField] GameObject[] goObj;

    [SerializeField, Header("何秒以上放置しているか")] private float sleepThreshold = 30.0f; //sleepThreshold秒以上スリープ状態ならタイトルへ

    public static float lastInputTime;
    [SerializeField, Header("放置対策画面")] private GameObject SleepPanel;
    private GameObject SleepPanelInstance;
    private Gun Gun;
    //拳銃のショットフラグ
    private bool isShot;
    void Start()
    {
        playerRigidBody2D = GetComponent<Rigidbody2D>();

        SleepPanel = Resources.Load<GameObject>("SleepPanel");

        if(SleepPanelInstance != null ) { Destroy(SleepPanelInstance); }
        //これダメな奴
        //playerParameter = GameObject.FindAnyObjectByType<PlayerParameter>();
        //これいいやつ
        playerMoveAnimation = GetComponent<PlayerMoveAnimation>();
        Gun = GetComponent<Gun>();
        mainCamera = FindAnyObjectByType<Camera>();
        // カメラの高さ（orthographicSize）はカメラの中央から上下の距離を表す
        mainCameraHeight = 2f * mainCamera.orthographicSize;

        // カメラの幅はアスペクト比に基づいて計算する
        mainCameraWidth = mainCameraHeight * mainCamera.aspect;
        // 攻撃アニメーション終了時のコールバックを設定
        // 上半身攻撃アニメーション終了時のコールバックを設定
    }
    public void InstantiateSkipPanel()
    {
        if (SleepPanelInstance != null) { Destroy(SleepPanelInstance); }
        SleepPanelInstance = Instantiate(SleepPanel);
        ChangeStage1();
        ChangeTutorial();
    }
    private void ChangeStage1()
    {
        Button YesButton = SleepPanelInstance.transform.Find("YesButton").GetComponent<Button>();
        Debug.Log(YesButton);

        if (YesButton != null)
        {
            YesButton.onClick.RemoveAllListeners();
            YesButton.onClick.AddListener(() =>
            {
                SceneTransitionManager.instance.NextSceneButton(0);
                Destroy(SleepPanelInstance);
            });

        }
    }
    private void ChangeTutorial()
    {
        Button NoButton = SleepPanelInstance.transform.Find("NoButton").GetComponent<Button>();
        Debug.Log(NoButton);

        if (NoButton != null)
        {
            NoButton.onClick.RemoveAllListeners();
            NoButton.onClick.AddListener(() =>
            {

                Destroy(SleepPanelInstance);
            });

        }
    }
    // Update is called once per frame
    void Update()
    {

        //プレイヤーのY座標の制限
        //プレイヤーのY座標が8.0を超えたらリジッドボディのフォースを0にする

        switch (GameMgr.GetState())
        {
            case GameState.Main:
                //bShootFlagをfalseにする
                isShot = false;
                if (8.0f < transform.position.y)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1);
                }
                UpdateTimers();


                //攻撃アニメーション中でなければbShootFlagをtrueにする
                //Debug.Log(playerMoveAnimation.SetAttack());
                if (playerMoveAnimation.SetAttack() == false)
                {
                    isShot = true;
                }

                MainExecution();

                break;
            case GameState.ShowText:
                UpdateTimers();

                break;
            case GameState.ShowOption:

                break;

            case GameState.Tutorial:
                UpdateTimers();
                break;
            case GameState.Hint:
                Time.timeScale = 0.0f;
                //Debug.Log("プレイヤーが動いていないこと確認");

                break;
            default:
                //Debug.Log("プレイヤーが動いていないこと確認");
                break;
        }
        if (GameMgr.GetState() != GameState.ShowOption)
        {
            PlayerSleeping();

        }
    }

    private void PlayerSleeping()
    {
        if (Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 ||
        Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            lastInputTime = Time.time;
        }
        // 一定時間操作がなければ「放置」と判定
        if (Time.time - lastInputTime >= sleepThreshold)
        {
            InstantiateSkipPanel();

            Debug.Log("プレイヤーが一定時間操作していません");
        }



    }
    private void UpdateTimers()
    {
        playerMoveAnimation.timeWalk -= Time.deltaTime;
        playerMoveAnimation.timeAttack -= Time.deltaTime;
        Time.timeScale = 1.0f;

    }
    void Move()
    {
        //現在のポジションを取得
        Vector3 vPosition = transform.position;

        //カメラとの距離の絶対値が一定以下ならプレイヤーが動く　画面外に出ないための処置
        //移動
        Vector3 vPosFromCame = vPosition - mainCamera.transform.position; //カメラ基準のプレイヤーの位置

        if (!playerMoveAnimation.SetAttack())
        {
            //左移動
            if (Input.GetKey(KeyCode.A))
            {
                if (vPosFromCame.x > -mainCameraWidth / 2)
                {
                    if (isEnemyHit() == false)
                    {
                        Debug.Log("左に移動します");
                        vPosition.x -= Time.deltaTime * playerSpeed;
                    }
                }
                playerMoveAnimation.HandleWalk(PlayerMoveAnimation.SHAFT_DIRECTION_LEFT);
            }
            //右移動
            if (Input.GetKey(KeyCode.D))
            {
                if (mainCameraWidth / 2 > vPosFromCame.x)
                {
                    if (isEnemyHit() == false)
                    {
                        vPosition.x += Time.deltaTime * playerSpeed;
                    }
                }
                playerMoveAnimation.HandleWalk(PlayerMoveAnimation.SHAFT_DIRECTION_RIGHT);
            }
            if (Input.GetKey(KeyCode.W) && jumpCount < 1)
            {
                Vector2 upVector = Vector2.up;
                playerRigidBody2D.velocity = upVector;
                playerRigidBody2D.AddForce(transform.up * playerJumpPower, ForceMode2D.Force);
                Debug.Log(transform.position);
                MultiAudio.ins.PlaySEByName("SE_hero_action_jump");
                isJump = true;
                jumpCount++;

            }

            //楽に次のシーン行きたいならこの下のコードをコメントアウト解除　確認後コメントアウトしておいて

            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    SceneTransitionManager.instance.NextSceneButton(SceneTransitionManager.instance.sceneInformation.GetCurrentScene() + 1); 
            //}
            //ここまで
            //楽にボス戦行きたいなら以下のコードをコメント解除
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    vPosition = new Vector2(190.0f, -1.536416f);
            //}
            //ここまで

        }

        //体が回転しないようにするのオイラーを０で設定すればできる
        //自分のtransformを取得

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.position = vPosition;
    }

    //ゲームメインのエクスキュート
    void MainExecution()
    {
        if (Tutorial.GetState() == Tutorial_State.PlayerDoNotMove)
        {
            return;
        }
        Move();


        #region 山品変更
        //アニメーションをここで呼ぶため、追記
        //攻撃関連
        if (!playerMoveAnimation.SetAttack() && playerMoveAnimation.timeAttack < 0)
        {
            //上半身攻撃
            if (Input.GetKeyDown(KeyCode.I))
            {
                //Tutorial.NextState();       
                UpperAttack upperattack = PlayerParameter.Instance.UpperData.upperAttack;
                playerMoveAnimation.PantieStart();
                // 警察上半身は銃弾に当たり判定を持つ
                if (upperattack != UpperAttack.POLICE)
                {
                    OnUpperAttackAnimationFinished();
                }

                #endregion
                switch (upperattack)
                {
                    case UpperAttack.NORMAL:

                        MultiAudio.ins.PlaySEByName("SE_hero_attack_upper");

                        break;

                    case UpperAttack.POLICE:
                        Vector2 ShootMoveBector = new Vector2(0, 0);
                        //子のplayerRCのローテーションYを持ってくる
                        // y = 0のときは右向き、0 y = 180のときは左向き
                        Debug.Log(transform.GetChild(0).transform.eulerAngles.y);
                        if (transform.GetChild(0).transform.eulerAngles.y == 180)
                        {
                            ShootMoveBector.x = -1;
                        }
                        else
                        {
                            ShootMoveBector.x = 1;
                        }

                        Debug.Log(ShootMoveBector);
                        Debug.Log("shootFlagは" + isShot);

                        //isShotがtrueなら銃を発射する
                        if (isShot == true)
                        {
                            Debug.Log("弾発射");
                            Gun.Shoot(ShootMoveBector, transform, PlayerParameter.Instance.UpperData.iPartAttack);

                            MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_upper");

                            //bShootFlag = false;
                        }
                        break;

                    case UpperAttack.NURSE:
                        MultiAudio.ins.PlaySEByName("SE_nurse_attack_upper");
                        break;
                    case UpperAttack.BOSS:
                        MultiAudio.ins.PlaySEByName("SE_lastboss_attack_upper");

                        break;
                }





            }
            //下半身攻撃
            if (Input.GetKeyDown(KeyCode.K))
            {
                //Tutorial.NextState();

                #region 山品変更
                playerMoveAnimation.KickStart();
                OnLowerAttackAnimationFinished();

                #endregion
                switch (PlayerParameter.Instance.LowerData.lowerAttack)
                {
                    case LowerAttack.NORMAL:
                        MultiAudio.ins.PlaySEByName("SE_hero_attack_lower");

                        break;

                    case LowerAttack.POLICE:
                        MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_lower");
                        break;

                    case LowerAttack.NURSE:
                        MultiAudio.ins.PlaySEByName("SE_nurse_attack_lower");
                        break;

                    case LowerAttack.BOSS:
                        MultiAudio.ins.PlaySEByName("SE_lastboss_attack_lower");
                        break;
                }



            }
        }


    }

    //アニメーションが持っていた関数を移動（アニメーションそのものには関係ないため）
    /// <summary>
    /// 上半身のイメージ
    /// </summary>
    /// <param name="upperBody">画像データ集合体</param>
    public void ChangeUpperBody(BodyPartsData upperBody)
    {
        characterSprites.body.sprite = upperBody.spBody;
        characterSprites.armRight.sprite = upperBody.spRightArm;
        characterSprites.armLeft.sprite = upperBody.spLeftArm;
        characterSprites.handRight.sprite = upperBody.spRightHand;
        characterSprites.handLeft.sprite = upperBody.spLeftHand;
    }
    //アニメーションが持っていた関数を移動（アニメーションそのものには関係ないため）

    /// <summary>
    /// 下半身のイメージ
    /// </summary>
    /// <param name="underBody">画像データ集合体</param>
    public void ChangeUnderBody(BodyPartsData underBody)
    {
        characterSprites.waist.sprite = underBody.spWaist;
        characterSprites.footRight.sprite = underBody.spRightFoot;
        characterSprites.footLeft.sprite = underBody.spLeftFoot;
        characterSprites.legRight.sprite = underBody.spRightLeg;
        characterSprites.legLeft.sprite = underBody.spLeftLeg;
    }

    //private void OnDestroy()
    //{
    //    // コールバックの解除
    //    if (playerMoveAnimation != null)
    //    {
    //        playerMoveAnimation.OnUpperAttackAnimationFinished -= OnUpperAttackAnimationFinished;
    //        playerMoveAnimation.OnLowerAttackAnimationFinished -= OnLowerAttackAnimationFinished;
    //    }
    //}

    private void OnUpperAttackAnimationFinished()
    {
        // 上半身攻撃判定
        for (int i = 0; i < enemyObject.Count; i++)
        {
            UpperBodyAttack(i, enemyObject[i].transform.position, PlayerParameter.Instance.UpperData.AttackArea, PlayerParameter.Instance.UpperData.iPartAttack);
        }
    }

    private void OnLowerAttackAnimationFinished()
    {
        // 下半身攻撃判定
        for (int i = 0; i < enemyObject.Count; i++)
        {
            LowerBodyAttack(i, enemyObject[i].transform.position, PlayerParameter.Instance.LowerData.AttackArea, PlayerParameter.Instance.LowerData.iPartAttack);
        }
    }

    //上半身攻撃
    public void UpperBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        bool isAttackingToEnemy = IsFacingToTarget(transform.position.x, vTargetPos.x, playerMoveAnimation.isFacingToRight());

        float fAttackReach = Vector3.Distance(vTargetPos, transform.position);
        if (!isAttackingToEnemy || fAttackReach >= fReach)
        {
            return;
        }
        IDamageable damageable = enemyObject[EnemyNum].GetComponent<IDamageable>();
        damageable?.TakeDamage(iDamage, 0);
        Debug.Log("上半身攻撃ダメージ判断");

    }
    //下半身攻撃
    public void LowerBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        bool isAttackingToEnemy = IsFacingToTarget(transform.position.x, vTargetPos.x, playerMoveAnimation.isFacingToRight());

        float fAttackReach = Vector3.Distance(vTargetPos, transform.position);
        if (!isAttackingToEnemy || fAttackReach >= fReach)
        {
            return;
        }
        IDamageable damageable = enemyObject[EnemyNum].GetComponent<IDamageable>();
        damageable?.TakeDamage(iDamage, 1);
        Debug.Log("下半身攻撃ダメージ判断");

    }

    private bool IsFacingToTarget(float playerPosX, float targetPosX, bool isFacingToRight)
    {
        bool isFacingToRightTarget = playerPosX <= targetPosX && isFacingToRight;
        bool isFacingToLeftTarget = playerPosX >= targetPosX && !isFacingToRight;
        return isFacingToRightTarget || isFacingToLeftTarget;
    }


    public void AddListItem(GameObject obj) => enemyObject.Add(obj);
    public void RemoveListItem(GameObject obj) => enemyObject.Remove(obj);

    //床判定
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Car"))
        {
            isJump = false;
            jumpCount = 0;
        }


    }
    //private void OnCollisionExit2D(Collision2D other)
    //{
    //    if (other.gameObject.CompareTag("OnTheCar"))
    //    {
    //        GameMgr.ChangeState(GameState.Main); // ボス戦直前に状態変更

    //    }
    //}

    //敵の弾との当たり判定
    private void OnTriggerEnter2D(Collider2D playerCollision)
    {
        if (playerCollision.gameObject.CompareTag("EnemyShoot"))
        {
            int attack = playerCollision.gameObject.GetComponent<Bullet>().attack;
            if (0 > transform.position.y - playerCollision.transform.position.y)
            {
                PlayerParameter.Instance.UpperHP -= attack;
            }
            else
            {
                PlayerParameter.Instance.LowerHP -= attack;
            }
        }

    }

    public void SetEnabledPlayerRenderer(bool enabled)
    {
        var renderers = transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = enabled;
        }
    }

    //enemyと当たっているかどうか
    //return :: true->当たっている    false->当っていない
    bool isEnemyHit()
    {
        for (int i = 0; i < enemyObject.Count; i++)
        {
            //敵とのX距離
            float distanceX = Mathf.Abs(enemyObject[i].transform.position.x - this.transform.position.x);
            //敵とのY距離
            float distanceY = Mathf.Abs(enemyObject[i].transform.position.y - this.transform.position.y);
            //X距離がplayerとenemyのコライダーのXサイズの半分の和より小さく
            //Y距離がplayerとenemyのコライダーのYサイズの半分の和より小さいならif文に入る
            //Scaleの半分だと見た目との誤差でうまく働かないため半分よりも少し大きい値を取りたいため1.5とする
            if ((distanceX < this.GetComponent<BoxCollider2D>().size.x * this.transform.localScale.x / 1.5
                + enemyObject[i].GetComponent<BoxCollider2D>().size.x * enemyObject[i].transform.localScale.x / 1.5) &&
                (distanceY < this.GetComponent<BoxCollider2D>().size.y * this.transform.localScale.y / 2.5
                + enemyObject[i].GetComponent<BoxCollider2D>().size.y * enemyObject[i].transform.localScale.y / 2.5))
            {
                //playerが右を向いているかつenemyがplayerの右側にいるか
                //playerが左を向いているかつenemyがplayerの左側にいるなら当たっている
                if ((playerMoveAnimation.isFacingToRight() == true &&
                    0 < enemyObject[i].transform.position.x - this.transform.position.x) ||
                    (playerMoveAnimation.isFacingToRight() == false &&
                    enemyObject[i].transform.position.x - this.transform.position.x < 0))
                {
                    Debug.Log("当たってる");
                    //当たっている
                    return true;
                }
            }
        }
        //当っていない
        return false;
    }
}
