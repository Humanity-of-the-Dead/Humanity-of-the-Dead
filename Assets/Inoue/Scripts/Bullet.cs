using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f; // 銃弾の速度
    public Rigidbody2D rb;    // Rigidbody2Dでスプライトの向きを計算する

    void Start()
    {
        //// 銃弾の進行方向に速度を設定
        //rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // ターゲットに当たったかどうかを衝突したオブジェクトのタグによって判断する
        // たぶん銃を撃ったのがプレイヤーかエネミーか等、場合によって反応するタグを変える必要があるので未完成
        if (hitInfo.gameObject.CompareTag("Player") || hitInfo.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("当たった");
            Destroy(this.gameObject); // 銃弾を消す
        }
    }

    void Update()
    {
        // 銃弾が画面外に出たかどうかをチェック
        if (IsOutOfScreen())
        {
            Destroy(gameObject); // 銃弾を消す
        }
    }

    bool IsOutOfScreen()
    {
        // 銃弾が画面外に出たかどうかをチェックするやつ
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        return screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;
    }
}


