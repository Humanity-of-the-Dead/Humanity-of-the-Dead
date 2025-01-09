using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // モーションアニメスクリプト
    [SerializeField] PlayerMoveAnimation scPlayerMoveAnimation;
    // ゲームマネージャー
    [SerializeField] GameMgr scGameMgr;
    private Rigidbody2D rbody2D;

    [Header("移動スピード")]
    [SerializeField] float fSpeed;

    [Header("ジャンプ力")]
    [SerializeField] float fJmpPower;

    bool bJump = false;
    [SerializeField] int Jmpconsecutive;

    // カメラ関連
    [SerializeField] Camera goCamera;
    float fCameraHeight;
    float fCameraWidth;

    // ターゲット
    [SerializeField] List<GameObject> liObj;

    PlayerParameter playerParameter;

    [SerializeField] SceneTransitionManager sceneTransitionManager;

    [SerializeField] Gun Juu;

    private UpperAttack upperAttack;
    private LowerAttack lowerAttack;

    bool bShootFlag;

    // Start is called before the first frame update
    void Start()
    {
        playerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();
        rbody2D = GetComponent<Rigidbody2D>();

        fCameraHeight = 2f * goCamera.orthographicSize;
        fCameraWidth = fCameraHeight * goCamera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        bool debug = true;

#if UNITY_EDITOR

        
            AutoPlayController autoPlayController = gameObject.GetComponent<AutoPlayController>();
            autoPlayController.enabled = true;

#else
            autoPlayController.enabled = false;


#endif

        // プレイヤーのY座標制限
        if (this.transform.position.y > 8.0f)
        {
            rbody2D.velocity = new Vector2(0.0f, -1);
        }

        switch (GameMgr.GetState())
        {
            case GameState.Main:
                bShootFlag = false;
                if (!scPlayerMoveAnimation.SetAttack())
                {
                    bShootFlag = true;
                    GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = true;
                }

                MainExecution();
                break;
        }
    }

    // ゲームメインのエクスキュート
    void MainExecution()
    {
        Vector2 vPosition = this.transform.position;
        Vector3 vPosFromCame = this.transform.position - goCamera.transform.position;

        if (!scPlayerMoveAnimation.SetAttack())
        {
            // 左移動
            if (Input.GetKey(KeyCode.A))
            {
                if (vPosFromCame.x > -fCameraWidth / 2)
                {
                    vPosition.x -= Time.deltaTime * fSpeed;
                }
            }
            // 右移動
            if (Input.GetKey(KeyCode.D))
            {
                if (fCameraWidth / 2 > vPosFromCame.x)
                {
                    vPosition.x += Time.deltaTime * fSpeed;
                }
            }

            // ジャンプ
            if (Input.GetKey(KeyCode.W) && Jmpconsecutive < 1)
            {
                rbody2D.AddForce(this.transform.up * fJmpPower);
                MultiAudio.ins.PlaySEByName("SE_hero_action_jump");
                bJump = true;
                Jmpconsecutive++;
            }
        }

        // 体の回転防止
        Quaternion quaternion = GetComponent<Transform>().rotation;
        quaternion.z = 0.0f;
        transform.rotation = quaternion;

        this.transform.position = vPosition;

        // 攻撃処理
        if (!scPlayerMoveAnimation.SetAttack() && scPlayerMoveAnimation.timeAttack < 0)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                UpperBodyAttack();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                LowerBodyAttack();
            }
        }
    }

    // 上半身攻撃
    public void UpperBodyAttack()
    {
        scPlayerMoveAnimation.PantieStart();

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
                Vector2 ShootMoveBector = new Vector2(0, 0);
                if (this.gameObject.transform.GetChild(0).gameObject.transform.eulerAngles.y == 180)
                {
                    ShootMoveBector.x = -1;
                }
                else
                {
                    ShootMoveBector.x = 1;
                }

                if (bShootFlag)
                {
                    Juu.Shoot(ShootMoveBector, this.transform);
                    if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                    {
                        MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_upper");
                        GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = false;
                    }
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

        for (int i = 0; i < liObj.Count; i++)
        {
            UpperBodyAttack(i, liObj[i].gameObject.transform.position, playerParameter.UpperData.AttackArea, playerParameter.UpperData.iPartAttack);
        }
    }

    // 下半身攻撃
    public void LowerBodyAttack()
    {
        scPlayerMoveAnimation.KickStart();

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
            LowerBodyAttack(i, liObj[i].gameObject.transform.position, playerParameter.LowerData.AttackArea, playerParameter.LowerData.iPartAttack);
        }
    }

    // 上半身攻撃のダメージ判定
    public void UpperBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        float fAttackReach = Vector3.Distance(vTargetPos, this.transform.position);
        if (fAttackReach < fReach)
        {
            liObj[EnemyNum].GetComponent<newEnemyParameters>().TakeDamage(iDamage, 0);
        }
    }

    // 下半身攻撃のダメージ判定
    public void LowerBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        float fAttackReach = Vector3.Distance(vTargetPos, this.transform.position);
        if (fAttackReach < fReach)
        {
            liObj[EnemyNum].GetComponent<newEnemyParameters>().TakeDamage(iDamage, 1);
        }
    }

    // ジャンプの判定
    public bool CanJump()
    {
        return Jmpconsecutive < 1 && !scPlayerMoveAnimation.SetAttack();
    }

    // ジャンプ
    public void Jump()
    {
        rbody2D.AddForce(this.transform.up * fJmpPower);
        MultiAudio.ins.PlaySEByName("SE_hero_action_jump");
        bJump = true;
        Jmpconsecutive++;
    }

    // 左移動
    public void MoveLeft()
    {
        Vector2 vPosition = this.transform.position;
        vPosition.x -= Time.deltaTime * fSpeed;
        this.transform.position = vPosition;
    }

    // 右移動
    public void MoveRight()
    {
        Vector2 vPosition = this.transform.position;
        vPosition.x += Time.deltaTime * fSpeed;
        this.transform.position = vPosition;
    }

    // 敵弾との当たり判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyShoot"))
        {
            if (0 > this.transform.position.y - collision.gameObject.transform.position.y)
            {
                playerParameter.UpperHP -= 1;
            }
            else
            {
                playerParameter.LowerHP -= 1;
            }
        }
    }

    // リストアイテムの追加
    public void AddListItem(GameObject obj)
    {
        liObj.Add(obj);
    }

    // リストアイテムの削除
    public void RemoveListItem(GameObject obj)
    {
        liObj.Remove(obj);
    }
    public bool CanAttack()
    {
        // 攻撃アニメーションが実行中でない場合
        if (scPlayerMoveAnimation.SetAttack() == false && scPlayerMoveAnimation.timeAttack < 0)
        {
            return true;
        }
        return false;
    }
    // 床判定
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Car"))
        {
            bJump = false;
            Jmpconsecutive = 0;
        }
    }
}
