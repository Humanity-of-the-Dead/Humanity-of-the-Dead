
using UnityEngine;

public class BossAreaTrigger : MonoBehaviour
{
    private PlayerControl targetPosition;


    // 許容範囲（ターゲット位置との距離）
    [SerializeField,Header("プレイヤーがボス前のエリア地点にどれだけ近づいたら到達とみなすかを決める値")] private float threshold = 1.0f;
    private bool isDelayedStateChange = false;

    private void Start()
    {
        targetPosition = GameObject.Find("Player Variant").GetComponent<PlayerControl>();

    }
    void Update()
    {
        //Debug.Log(IsPlayerAtTarget());
        // プレイヤーが指定の位置に到達したかをチェック
        if (GameMgr.GetState() == GameState.Main && IsPlayerAtTarget())
        {
            GameMgr.ChangeState(GameState.BeforeBoss);
        }
    }

    // プレイヤーがターゲット位置に到達しているかを判定
    private bool IsPlayerAtTarget()
    {
        float distance = Vector3.Distance(new Vector2(transform.position.x, transform.position.z),
    new Vector2(targetPosition.transform.position.x, targetPosition.transform.position.z));
        //Debug.Log(transform.position + "ボス前エリア ");
        //Debug.Log(targetPosition.transform.position + "プレイヤーの位置 " );

        return distance <= threshold;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameMgr.ChangeState(GameState.BeforeBoss); // ボス戦直前に状態変更
        }
    }
}
