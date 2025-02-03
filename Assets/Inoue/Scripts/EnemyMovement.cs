using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyMovement : EnemyAttack
{
    // 移動を始める場所、終わりの場所、普段の移動速度、追跡中の移動速度、敵の索敵可能な範囲を設定
    [Header("移動の絶対値")]
    [SerializeField] float moveAbs;
    private float pointA;
    private float pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private BodyPartsData upperpart;
    [SerializeField] private BodyPartsData lowerpart;

    [SerializeField] EnemyMoveAnimation moveAnimation;

    enum EnemyState
    {
        search,
        walk,
        attack,
        wait,
    }

    EnemyState enemystate;

    private bool movingToPointB = false; // 進行方向
    private Transform player; // プレイヤーの位置

    public GameMgr gamestate;

    private float timer;
    [SerializeField] float waitTime; //攻撃後の後隙

    void Start()
    {
        // プレイヤーを探すやつ
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pointA = this.transform.position.x + moveAbs;
        pointB = this.transform.position.x - moveAbs;

    }

    void Update()
    {
        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        switch (GameMgr.GetState())
        {
            case GameState.Main:
                switch (enemystate)
                {
                    case EnemyState.search:
                        // プレイヤーが追跡範囲内に入っているかどうか判断
                        if (distanceToPlayer < chaseRange)
                        {
                            enemystate = EnemyState.walk;
                        }
                        else
                        {
                            // いつもの挙動
                            float Target = movingToPointB ? pointB : pointA;
                            Vector3 target = new Vector3(Target, this.transform.position.y, this.transform.position.z);
                            MoveTowards(target, speed);

                            // 敵が折り返し地点に到達したかどうか判断
                            if (transform.position == target)
                            {
                                // 到達したら回れ右
                                if (movingToPointB == true) moveAnimation.TurnToRight();
                                else moveAnimation.TurnToLeft();
                                movingToPointB = !movingToPointB;
                            }
                        }
                        break;
                    case EnemyState.walk:
                        // プレイヤーを追跡
                        MoveTowards(player.position, chaseSpeed);
                        // プレイヤーが攻撃範囲内に入っているかどうか判断
                        if (distanceToPlayer < upperpart.AttackArea || distanceToPlayer < lowerpart.AttackArea)
                        {
                            enemystate = EnemyState.wait;
                        }
                        break;
                    case EnemyState.attack:
                        if (distanceToPlayer < upperpart.AttackArea)
                        {
                            moveAnimation.PantieStart();
                            Debug.Log("パンチ");
                            UpperEnemyAttack((float)upperpart.iPartAttack * 0.1f);
                        }
                        else if (distanceToPlayer < lowerpart.AttackArea)
                        {
                            LowerEnemyAttack((float)lowerpart.iPartAttack * 0.1f);

                        }
                        enemystate = EnemyState.search;
                        //moveAnimation.PlayerPantie();
                        break;
                    case EnemyState.wait:
                        Debug.Log("直立");
                        if (timer > waitTime)
                        {
                            timer = 0;
                            enemystate = EnemyState.attack;
                            break;
                        }
                        timer += Time.deltaTime;
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
        Debug.Log("衝突イベント検知");
        Debug.Log(collision.gameObject.tag);
        Debug.Log(enemystate);
        if (collision.gameObject.CompareTag("Enemy") && enemystate == EnemyState.search)
        {
            Debug.Log("敵同士が衝突し、回れ右");
            if (movingToPointB)
            {
                moveAnimation.TurnToRight();
            }
            else
            {
                moveAnimation.TurnToLeft();
            }
            movingToPointB = !movingToPointB;
        }
    }
}
// プレイヤーに向かって移動
//ゾンビ(仮)の画像を使っています。本来のオブジェクトにアタッチする。
//プレイヤー(仮)の画像を使っています。TagがPlayerになっていないと動かん
//テスト用にプレイヤーを見つけると移動速度変わるようにしてるけどインスペクターからカスタムできる


