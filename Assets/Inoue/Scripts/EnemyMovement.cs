using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // 移動を始める場所、終わりの場所、普段の移動速度、追跡中の移動速度、敵の索敵可能な範囲を設定
    [SerializeField] private Vector3 pointA;
    [SerializeField] private Vector3 pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float chaseRange = 5f;

    private bool movingToPointB = true; // 進行方向
    private Transform player; // プレイヤーの位置

    void Start()
    {
        // プレイヤーを探すやつ
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // プレイヤーが追跡範囲内に入っているかどうか判断
        if (distanceToPlayer < chaseRange)
        {
            // プレイヤーを追跡
            MoveTowards(player.position, chaseSpeed);
        }
        else
        {
            // いつもの挙動
            Vector3 target = movingToPointB ? pointB : pointA;
            MoveTowards(target, speed);

            // 敵が折り返し地点に到達したかどうか判断
            if (transform.position == target)
            {
                // 到達したら回れ右
                movingToPointB = !movingToPointB;
            }
        }
    }

    // プレイヤーに向かって移動
    private void MoveTowards(Vector3 target, float moveSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }
}

//ゾンビ(仮)の画像を使っています。本来のオブジェクトにアタッチする。
//プレイヤー(仮)の画像を使っています。TagがPlayerになっていないと動かん
//テスト用にプレイヤーを見つけると移動速度変わるようにしてるけどインスペクターからカスタムできる

