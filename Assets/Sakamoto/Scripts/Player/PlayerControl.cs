using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //モーションアニメスクリプト
    [SerializeField] private PlayerMoveAnimation playerMoveAnimation;
    // キャラクターパーツ (SpriteRenderer)
    [Header("キャラクターパーツ")]
    [SerializeField] private CharacterSprites characterSprites;
    //ゲームマネージャー


    private Rigidbody2D rigidbody2D;
    [Header("移動スピード")]
    [SerializeField] private float fSpeed;
    [Header("ジャンプ力")]
    [SerializeField] private float fJmpPower;
    private bool bJump = false;
    //連続ジャンプ
    [SerializeField] private int Jmpconsecutive;


    //カメラ関連
    [SerializeField] private Camera goCamera;
    //高さ
    private float fCameraHeight;
    //幅
    private float fCameraWidth;

    //ターゲット
    [SerializeField] private List<GameObject> liObj;
    //[SerializeField] GameObject[] goObj;

    //プレイヤーパラメーターの取得
    private PlayerParameter playerParameter;



    [SerializeField] private Gun Gun;
    //拳銃のショットフラグ
    private bool bShootFlag;
    void Start()
    {


        //これダメな奴
        //playerParameter = GameObject.FindAnyObjectByType<PlayerParameter>();
        //これいいやつ
        playerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();
        playerMoveAnimation = GetComponent<PlayerMoveAnimation>();
        Gun = GetComponent<Gun>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        // カメラの高さ（orthographicSize）はカメラの中央から上下の距離を表す
        fCameraHeight = 2f * goCamera.orthographicSize;

        // カメラの幅はアスペクト比に基づいて計算する
        fCameraWidth = fCameraHeight * goCamera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーのY座標の制限
        //プレイヤーのY座標が8.0を超えたらリジッドボディのフォースを0にする
        if (8.0f < transform.position.y)
        {
            rigidbody2D.velocity = new Vector2(0.0f, -1);
        }

        switch (GameMgr.GetState())
        {
            case GameState.Main:
                //bShootFlagをfalseにする
                bShootFlag = false;
                //攻撃アニメーション中でなければbShootFlagをtrueにする
                //Debug.Log(playerMoveAnimation.SetAttack());
                if (playerMoveAnimation.SetAttack() == false)
                {
                    bShootFlag = true;
                    MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = true;

                }

                MainExecution();

                break;
        }
    }

    //ゲームメインのエクスキュート
    void MainExecution()
    {
        //現在のポジションを取得
        Vector3 vPosition = transform.position;

        //カメラとの距離の絶対値が一定以下ならプレイヤーが動く　画面外に出ないための処置
        //移動
        Vector3 vPosFromCame = vPosition - goCamera.transform.position; //カメラ基準のプレイヤーの位置

        if (!playerMoveAnimation.SetAttack())
        {
            //左移動
            if (Input.GetKey(KeyCode.A))
            {
                if (vPosFromCame.x > -fCameraWidth / 2)
                {
                    vPosition.x -= Time.deltaTime * fSpeed;
                }
            }
            //右移動
            if (Input.GetKey(KeyCode.D))
            {
                if (fCameraWidth / 2 > vPosFromCame.x)
                {
                    vPosition.x += Time.deltaTime * fSpeed;
                }
            }

            //ジャンプ

            if (Input.GetKey(KeyCode.W) && Jmpconsecutive < 1)
            {
                rigidbody2D.AddForce(transform.up * fJmpPower);
                MultiAudio.ins.PlaySEByName("SE_hero_action_jump");
                bJump = true;
                Jmpconsecutive++;
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




        #region 山品変更
        //アニメーションをここで呼ぶため、追記
        //攻撃関連
        if (!playerMoveAnimation.SetAttack() && playerMoveAnimation.timeAttack < 0)
        {
            //上半身攻撃
            if (Input.GetKeyDown(KeyCode.I))
            {
                playerMoveAnimation.PantieStart();
                #endregion
                switch (playerParameter.UpperData.upperAttack)
                {
                    case UpperAttack.NORMAL:
                        if (MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay)
                        {
                            MultiAudio.ins.PlaySEByName("SE_hero_attack_upper");
                            MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = false;
                        }
                        break;

                    case UpperAttack.POLICE:
                        Debug.Log("ここに銃弾の発射のプログラムをかいでね");
                        //この下
                        Vector2 ShootMoveBector = new Vector2(0, 0);
                        //子のplayerRCのローテーションYを持ってくる
                        // y = 0のときは右向き、0 y = 180のときは左向き
                        Debug.Log(gameObject.transform.GetChild(0).transform.eulerAngles.y);
                        if (gameObject.transform.GetChild(0).transform.eulerAngles.y == 180)
                        {
                            ShootMoveBector.x = -1;
                        }
                        else
                        {
                            ShootMoveBector.x = 1;
                        }

                        Debug.Log(ShootMoveBector);
                        Debug.Log("shootFlagは" + bShootFlag);

                        //bShootFlagがtrueなら銃を発射する
                        if (bShootFlag == true)
                        {
                            Debug.Log("弾発射");
                            Gun.Shoot(ShootMoveBector, transform);
                            if (MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay)
                            {
                                MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_upper");
                                MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = false;

                            }
                            //bShootFlag = false;
                        }
                        break;

                    case UpperAttack.NURSE:
                        if (MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay)
                        {
                            MultiAudio.ins.PlaySEByName("SE_nurse_attack_upper");
                            MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = false;
                        }
                        break;
                }

                if (playerParameter.UpperData.sPartsName == "ボスの上半身")
                {
                    MultiAudio.ins.PlaySEByName("SE_lastboss_attack_upper");
                }

                for (int i = 0; i < liObj.Count; i++)
                {
                    //Debug.Log(liObj[i].gameObject.transform.position);
                    //Debug.Log(playerParameter.UpperData.AttackArea);
                    //仮引数
                    UpperBodyAttack(i, liObj[i].transform.position, playerParameter.UpperData.AttackArea, playerParameter.UpperData.iPartAttack);
                }
            }
            //下半身攻撃
            if (Input.GetKeyDown(KeyCode.K))
            {
                #region 山品変更
                playerMoveAnimation.KickStart();
                #endregion
                switch (playerParameter.LowerData.lowerAttack)
                {
                    case LowerAttack.NORMAL:
                        if (MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay)
                        {
                            MultiAudio.ins.PlaySEByName("SE_hero_attack_lower");
                            MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = false;
                        }

                        break;

                    case LowerAttack.POLICE:
                        if (MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay)
                        {
                            MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_lower");
                            MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = false;
                        }
                        break;

                    case LowerAttack.NURSE:
                        if (MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay)
                        {
                            MultiAudio.ins.PlaySEByName("SE_nurse_attack_lower");
                            MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = false;

                        }
                        break;
                }
                for (int i = 0; i < liObj.Count; i++)
                {
                    //仮引数
                    LowerBodyAttack(i, liObj[i].transform.position, playerParameter.LowerData.AttackArea, playerParameter.LowerData.iPartAttack);
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

    //床判定
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Car"))
        {
            bJump = false;
            Jmpconsecutive = 0;
        }
    }

    //上半身攻撃
    public void UpperBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        float fAttackReach = Vector3.Distance(vTargetPos, transform.position);
        if (fAttackReach < fReach)
        {

            liObj[EnemyNum].GetComponent<newEnemyParameters>().TakeDamage(iDamage, 0);

            Debug.Log("上半身攻撃成功");

        }
        else
        {
            Debug.Log("上半身攻撃失敗");
        }
    }
    //下半身攻撃
    public void LowerBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        float fAttackReach = Vector3.Distance(vTargetPos, transform.position);
        if (fAttackReach < fReach)
        {
            liObj[EnemyNum].GetComponent<newEnemyParameters>().TakeDamage(iDamage, 1);
            Debug.Log("下半身攻撃成功");
        }
        else
        {
            Debug.Log("下半身攻撃失敗");
        }
    }

    
    public void AddListItem(GameObject obj) => liObj.Add(obj);
    public void RemoveListItem(GameObject obj) => liObj.Remove(obj);




    //敵の弾都の当たり判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyShoot"))
        {
            if (0 > transform.position.y - collision.transform.position.y)
            {
                playerParameter.UpperHP -= 1;
            }
            else
            {
                playerParameter.LowerHP -= 1;
            }
        }

    }
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
}
