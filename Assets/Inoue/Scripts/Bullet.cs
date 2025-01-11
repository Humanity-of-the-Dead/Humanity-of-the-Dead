using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f; // 銃弾の速度

    #region 山品変更
    /// <summary>
    /// 変更理由：Gunスクリプトで弾のリジットボディーを取得しているためここで変数を宣言する必要なし+下のコードでも最終的に一度も使っていない
    /// </summary>    

    //Gunスクリプトからの引用
    //Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();→GetComponentでbulletのオブジェクトのプレハブについているRigidbody2Dを取得する
    #endregion


    //private Rigidbody2D rb;    // Rigidbody2Dでスプライトの向きを計算する

    void Start()
    {
        //// 銃弾の進行方向に速度を設定
        //rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // ターゲットに当たったかどうかを衝突したオブジェクトのタグによって判断する
        // たぶん銃を撃ったのがプレイヤーかエネミーか等、場合によって反応するタグを変える必要があるので未完成
        if (hitInfo.gameObject.CompareTag("Player") || hitInfo.gameObject.CompareTag("Enemy") || hitInfo.gameObject.CompareTag("Car"))
        {
            Debug.Log("当たった");
            Debug.Log(hitInfo.gameObject.tag);
            Destroy(this.gameObject); // 銃弾を消す
        }
    }

    void Update()
    {
        // 銃弾が画面外に出たかどうかをチェック
        if (IsOutOfScreen())
        {
            Debug.Log("銃弾が画面外に出ました");
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


