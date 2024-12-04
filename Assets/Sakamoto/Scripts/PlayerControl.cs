using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //モーションアニメスクリプト
    [SerializeField] PlayerMoveAnimation scPlayerMoveAnimation;

    //ゲームマネージャー
    [SerializeField] GameMgr scGameMgr;

    private Rigidbody2D rbody2D;
    [Header("移動スピード")]
    [SerializeField] float fSpeed;
    [Header("ジャンプ力")]
    [SerializeField] float fJmpPower;
    bool bJump = false;


    //カメラ関連
    [SerializeField] Camera goCamera;
    //高さ
    float fCameraHeight;
    //幅
    float fCameraWidth;

    //ターゲット
    [SerializeField] List<GameObject> liObj;
    //[SerializeField] GameObject[] goObj;

    //プレイヤーパラメーターの取得
    PlayerParameter playerParameter;


    [SerializeField] SceneTransitionManager sceneTransitionManager;

    [SerializeField] Gun Juu;

    private UpperAttack upperAttack;
    LowerAttack lowerAttack;

    //拳銃のショットフラグ
    bool bShootFlag;
    void Start()
    {


        //これダメな奴
        //playerParameter = GameObject.FindAnyObjectByType<PlayerParameter>();
        //これいいやつ
        playerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();

        rbody2D = GetComponent<Rigidbody2D>();

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
        if (8.0f < this.transform.position.y)
        {
            this.rbody2D.velocity = new Vector2(0.0f, -1);
        }

        switch (scGameMgr.enGameState)
        {
            case GameState.Main:
                //bShootFlagをfalseにする
                bShootFlag = false;
                //攻撃アニメーション中でなければbShootFlagをtrueにする
                Debug.Log(scPlayerMoveAnimation.SetAttack());
                if (scPlayerMoveAnimation.SetAttack() == false)
                {
                    bShootFlag = true;
                    GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = true;

                }

                MainExecution();
                break;
        }
    }

    //ゲームメインのエクスキュート
    void MainExecution()
    {
        //現在のポジションを取得
        Vector2 vPosition = this.transform.position;

        //カメラとの距離の絶対値が一定以下ならプレイヤーが動く　画面外に出ないための処置
        //移動
        Vector3 vPosFromCame = this.transform.position - goCamera.transform.position; //カメラ基準のプレイヤーの位置
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

        if (Input.GetKey(KeyCode.W) && bJump == false)
        {
            this.rbody2D.AddForce(this.transform.up * fJmpPower);
            MultiAudio.ins.PlaySEByName("SE_hero_action_jump");
            bJump = true;
        }

        //体が回転しないようにする
        //自分のtransformを取得
        Quaternion quaternion = GetComponent<Transform>().rotation;
        quaternion.z = 0.0f;
        transform.rotation = quaternion;


        //移動後のポジションを代入
        this.transform.position = vPosition;

        //攻撃関連
        //上半身攻撃
        if (Input.GetKeyDown(KeyCode.I))
        {

            switch (playerParameter.UpperData.upperAttack)
            {
                case UpperAttack.NORMAL:
                    if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                    {
                        MultiAudio.ins.PlaySEByName("SE_hero_attack_upper");
                        GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = false;
                    }
                    break;

                case UpperAttack.POLICE:
                    Debug.Log("ここに銃弾の発射のプログラムをかいでね");
                    //この下
                    Vector2 ShootMoveBector = new Vector2(0, 0);
                    //子のplayerRCのローテーションYを持ってくる
                    // y = 0のときは右向き、0 y = 180のときは左向き
                    Debug.Log(this.gameObject.transform.GetChild(0).gameObject.transform.eulerAngles.y);
                    if (this.gameObject.transform.GetChild(0).gameObject.transform.eulerAngles.y == 180)
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
                        Juu.Shoot(ShootMoveBector, this.transform);
                        if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                        {
                            MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_upper");
                            GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = false;

                        }
                        //bShootFlag = false;
                    }
                    break;

                case UpperAttack.NURSE:
                    if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                    {
                        MultiAudio.ins.PlaySEByName("SE_nurse_attack_upper");
                        GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = false;
                    }
                    break;
            }

            if (playerParameter.UpperData.sPartsName == "ボスの上半身")
            {
                MultiAudio.ins.PlaySEByName("SE_lastboss_attack_upper");
            }

            for (int i = 0; i < liObj.Count; i++)
            {
                Debug.Log(liObj[i].gameObject.transform.position);
                Debug.Log(playerParameter.UpperData.AttackArea);
                //仮引数
                UpperBodyAttack(i, liObj[i].gameObject.transform.position, playerParameter.UpperData.AttackArea, playerParameter.UpperData.iPartAttack);
            }
        }
        //下半身攻撃
        if (Input.GetKeyDown(KeyCode.K))
        {
            switch (playerParameter.LowerData.lowerAttack)
            {
                case LowerAttack.NORMAL:
                    if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                    {
                        MultiAudio.ins.PlaySEByName("SE_hero_attack_lower");
                        GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = false;
                    }

                    break;

                case LowerAttack.POLICE:
                    if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                    {
                        MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_lower");
                        GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = false;
                    }
                    break;

                case LowerAttack.NURSE:
                    if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                    {
                        MultiAudio.ins.PlaySEByName("SE_nurse_attack_lower");
                    }
                    break;
            }
            for (int i = 0; i < liObj.Count; i++)
            {
                //仮引数
                LowerBodyAttack(i, liObj[i].gameObject.transform.position, playerParameter.LowerData.AttackArea, playerParameter.LowerData.iPartAttack);
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
        }

    }

    //床判定
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Car"))
        {
            bJump = false;
        }
    }

    //上半身攻撃
    public void UpperBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        float fAttackReach = Vector3.Distance(vTargetPos, this.transform.position);
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
        float fAttackReach = Vector3.Distance(vTargetPos, this.transform.position);
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

    public void AddListItem(GameObject obj)
    {
        liObj.Add(obj);
    }
    public void RemoveListItem(GameObject obj)
    {
        liObj.Remove(obj);
    }


    //敵の弾都の当たり判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyShoot"))
        {
            if (0 < this.transform.position.y - collision.gameObject.transform.position.y)
            {
                playerParameter.UpperHP -= 3;
            }
            else
            {
                playerParameter.LowerHP -= 3;
            }
        }

    }
}
