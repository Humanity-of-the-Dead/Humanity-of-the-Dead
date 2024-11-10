using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class newEnemyMovement : EnemyAttack
{
    // 移動を始める場所、終わりの場所、普段の移動速度、追跡中の移動速度、敵の索敵可能な範囲を設定
    [SerializeField] private float pointA;
    [SerializeField] private float pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private BodyPartsData part;

    [SerializeField] EnemyMoveAnimation moveAnimation;

    enum EnemyState 
    {
        search,
        walk,
        attack,
        wait,
    }

    EnemyState enemystate;

    private bool movingToPointB = true; // 進行方向
    private Transform player; // プレイヤーの位置

    [SerializeField] GameMgr gamestate;

    void Start()
    {
        // プレイヤーを探すやつ
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        switch (gamestate.enGameState)
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
                                if (movingToPointB == true) moveAnimation.RightMove();
                                else moveAnimation.LeftMove();
                                movingToPointB = !movingToPointB;
                            }
                        }
                        break;
                    case EnemyState.walk:
                        // プレイヤーを追跡
                        MoveTowards(player.position, chaseSpeed);
                        // プレイヤーが攻撃範囲内に入っているかどうか判断
                        if (distanceToPlayer < attackRange)
                        {
                            enemystate = EnemyState.attack;
                        }
                        break;
                    case EnemyState.attack:
                        UpperEnemyAttack((float)part.iPartAttack);
                        break;
                    case EnemyState.wait:
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
}
// プレイヤーに向かって移動
//ゾンビ(仮)の画像を使っています。本来のオブジェクトにアタッチする。
//プレイヤー(仮)の画像を使っています。TagがPlayerになっていないと動かん
//テスト用にプレイヤーを見つけると移動速度変わるようにしてるけどインスペクターからカスタムできる

