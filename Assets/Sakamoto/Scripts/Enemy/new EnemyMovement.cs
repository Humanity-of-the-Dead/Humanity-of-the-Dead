using UnityEngine;

public class newEnemyMovement : MonoBehaviour
{
    // 移動を始める場所、終わりの場所、普段の移動速度、追跡中の移動速度、敵の索敵可能な範囲を設定

    [Header("エネミーの移動関連の設定")]
    [Tooltip("移動の絶対値")]
    [SerializeField] private float moveDistance = 5f;
    [Tooltip("普段の移動速度")]
    [SerializeField] private float normalSpeed = 2f;
    [Tooltip("追跡中の移動速度")]
    [SerializeField] private float chaseSpeed = 4f;
    [Tooltip("敵の索敵可能な範囲")]
    [SerializeField] private float chaseRange = 5f;
    [Tooltip("敵の攻撃可能な範囲")]
    [SerializeField] private float attackRange = 2f;
    [Tooltip("敵の攻撃待機時間")]
    [SerializeField] private float waitTime = 1f;

    [Header("エネミーの基本設定")]
    [Tooltip("各キャラクターが持っている上半身パーツ")]

    [SerializeField] private BodyPartsData upperPart;
    [Tooltip("各キャラクターが持っている下半身パーツ")]

    [SerializeField] private BodyPartsData lowerPart;
    private Gun gun;

    private EnemyMoveAnimation enemyMoveAnimation;

    private float pointA, pointB;//開始位置と終了位置


    private enum EnemyState { Search, Walk, Attack, Wait }

    private EnemyState enemyState = EnemyState.Search;

    private bool movingToPointB = false; // 進行方向
    private PlayerControl player; // プレイヤーの位置
    private float timer;//攻撃後の時間
    
    void Start()
    {
        // プレイヤーを探すやつ
        player = GameObject.Find("Player Variant").GetComponent<PlayerControl>();
        enemyMoveAnimation = GetComponent<EnemyMoveAnimation>();
        pointA = transform.position.x + moveDistance;
        pointB = transform.position.x - moveDistance;

        if (upperPart.sPartsName == "警察の上半身")
        {
            gun = GetComponent<Gun>();
        }
    }

    void Update()
    {
        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log(distanceToPlayer.ToString());
        switch (GameMgr.GetState())
        {
            case GameState.Main:
                switch (enemyState)
                {
                    case EnemyState.Search:
                        enemyMoveAnimation.WalkInstance();
                        // プレイヤーが追跡範囲内に入っているかどうか判断
                        if (distanceToPlayer < chaseRange)
                        {
                            enemyState = EnemyState.Walk;
                        }
                        else
                        {
                            // いつもの挙動
                            float Target = movingToPointB ? pointB : pointA;
                            Vector3 target = new Vector3(Target, transform.position.y, transform.position.z);
                            MoveTowards(target, normalSpeed);

                            // 敵が折り返し地点に到達したかどうか判断
                            if (transform.position == target)
                            {
                                // 到達したら回れ右
                                if (movingToPointB == true) enemyMoveAnimation.RightMove();
                                else enemyMoveAnimation.LeftMove();
                                movingToPointB = !movingToPointB;
                            }
                        }
                        break;
                    case EnemyState.Walk:
                        // プレイヤーを追跡
                        MoveTowards(player.transform.position, chaseSpeed);
                        if (PlayerPositionFromEnemy() != movingToPointB)
                        {
                            if (movingToPointB == true) enemyMoveAnimation.RightMove();
                            else enemyMoveAnimation.LeftMove();
                            movingToPointB = !movingToPointB;

                        }
                        // プレイヤーが攻撃範囲内に入っているかどうか判断
                        if ((distanceToPlayer < upperPart.AttackArea || distanceToPlayer < lowerPart.AttackArea)
                            && PlayerPositionFromEnemy() == movingToPointB)
                        {
                            enemyState = EnemyState.Wait;
                        }
                        break;
                    case EnemyState.Wait:
                        //moveAnimation.Upright();
                        if (timer > waitTime)
                        {
                            timer = 0;
                            if ((distanceToPlayer < upperPart.AttackArea || distanceToPlayer < lowerPart.AttackArea) && PlayerPositionFromEnemy() == movingToPointB)
                            {
                                enemyState = EnemyState.Attack;
                            }
                            else
                            {
                                enemyState = EnemyState.Search;
                            }
                            break;
                        }
                        timer += Time.deltaTime;
                        break;
                    case EnemyState.Attack:
                        if (distanceToPlayer < upperPart.AttackArea || distanceToPlayer < lowerPart.AttackArea &&
                            PlayerPositionFromEnemy() == movingToPointB)
                        {
                            //乱数を取得する
                            int num = Random.Range(0, 2);
                            if (num == 0)
                            {
                                //上半身攻撃
                                enemyMoveAnimation.PantieStart();

                                //攻撃者の上半身を確認
                                switch (upperPart.sPartsName)
                                {
                                    case "警察の上半身":
                                        Vector2 ShootMoveBector = new Vector2(0, 0);
                                        //子のplayerRCのローテーションYを持ってくる
                                        // y = 0のときは右向き、0 y = 180のときは左向き
                                        Debug.Log(transform.GetChild(0).
                                            transform.eulerAngles.y);
                                        if (transform.GetChild(0).
                                                transform.eulerAngles.y == 180)
                                        {
                                            ShootMoveBector.x = -1;
                                        }
                                        else
                                        {
                                            ShootMoveBector.x = 1;
                                        }

                                        Debug.Log(ShootMoveBector);
                                        gun.Shoot(ShootMoveBector, transform);
                                        //警察官の上半身で攻撃するSEを鳴らす
                                        MultiAudio.ins.PlaySEByName(
                                            "SE_policeofficer_attack_upper");

                                        Debug.Log("警官が上半身で攻撃");

                                        break;

                                    case "ボスの上半身":
                                        //ラスボス上半身の攻撃音のSEを鳴らす
                                        MultiAudio.ins.PlaySEByName(
                                            "SE_lastboss_attack_upper");

                                        Debug.Log("ボスが上半身で攻撃");

                                        break;

                                    case "雑魚敵の上半身":
                                        //主人公上半身の攻撃音のSEを鳴らす
                                        MultiAudio.ins.PlaySEByName(
                                            "SE_hero_attack_upper");

                                        Debug.Log("雑魚敵が上半身で攻撃");

                                        break;

                                    case "看護師の上半身":
                                        //ナース上半身の攻撃音のSEを鳴らす
                                        MultiAudio.ins.PlaySEByName(
                                            "SE_nurse_attack_upper");

                                        Debug.Log("看護師が上半身で攻撃");

                                        break;
                                }

                                UpperEnemyAttack((float)upperPart.iPartAttack);
                                //MultiAudio.ins.PlaySEByName("SE_common_hit_attack");
                            }
                            if (num == 1)
                            {
                                //下半身攻撃
                                enemyMoveAnimation.KickStart();

                                //攻撃者の下半身を確認
                                switch (lowerPart.sPartsName)
                                {
                                    case "警察の下半身":
                                        //警察官下半身の攻撃音のSEをならす
                                        MultiAudio.ins.PlaySEByName(
                                            "SE_policeofficer_attack_lower");

                                        Debug.Log("警官が下半身で攻撃");
                                        break;

                                    case "ボスの下半身":
                                        //ラスボス下半身の攻撃音のSEを鳴らす
                                        MultiAudio.ins.PlaySEByName(
                                            "SE_lastboss_attack_lower");

                                        Debug.Log("ボスが下半身で攻撃");

                                        break;

                                    case "雑魚敵の下半身":
                                        //主人公下半身の攻撃音のSEを鳴らす
                                        MultiAudio.ins.PlaySEByName(
                                            "SE_hero_attack_lower");

                                        Debug.Log("雑魚が下半身で攻撃");
                                        break;

                                    case "看護師の下半身":
                                        //ナース下半身の攻撃音のSEを鳴らす
                                        MultiAudio.ins.PlaySEByName(
                                            "SE_nurse_attack_lower");

                                        Debug.Log("看護師が下半身で攻撃");

                                        break;
                                }

                                LowerEnemyAttack((float)lowerPart.iPartAttack);
                            }
                        }
                     
                        enemyState = EnemyState.Search;
                        //moveAnimation.PlayerPantie();
                        break;

                }



                break;
            case GameState.ShowText:
                break;


        }
    }
    private void MoveTowards(Vector3 target, float moveSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log($"Collided with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

            //Debug.Log("敵同士が衝突し、回れ右");
            if (movingToPointB)
            {
                enemyMoveAnimation?.RightMove();
            }
            else
            {
                enemyMoveAnimation?.LeftMove();
            }
            movingToPointB = !movingToPointB;
        }
    }

    //PlayerPositionFromEnemy右向いてたら＋、左向いてたらー
    bool PlayerPositionFromEnemy()
    {
        float Direction = player.transform.position.x - gameObject.transform.position.x;
        if (Direction < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void UpperEnemyAttack(float upperDamage)
    {

        IDamageable damageable = PlayerParameter.Instance.GetComponent<IDamageable>();
        damageable?.TakeDamage(upperDamage, 0);


    }
    void LowerEnemyAttack(float lowerDamage)
    {
        IDamageable damageable = PlayerParameter.Instance.GetComponent<IDamageable>();
        damageable?.TakeDamage(lowerDamage, 1);
    }
}
// プレイヤーに向かって移動
//ゾンビ(仮)の画像を使っています。本来のオブジェクトにアタッチする。
//プレイヤー(仮)の画像を使っています。TagがPlayerになっていないと動かん
//テスト用にプレイヤーを見つけると移動速度変わるようにしてるけどインスペクターからカスタムできる

