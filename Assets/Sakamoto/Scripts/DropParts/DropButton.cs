using UnityEngine;

public class DropButton : MonoBehaviour
{
    [SerializeField] private GameObject[] goButton;
    private bool isTriggeredByPlayer = false; // プレイヤーと接触中かどうか
    private Rigidbody2D Rigidbody2D;
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        if (Rigidbody2D == null)
        {
            Debug.LogError("Rigidbody2Dがアタッチされていません！");
            return;
        }

        // Rigidbody2Dの設定
        Rigidbody2D.bodyType = RigidbodyType2D.Dynamic; // 動的物理挙動
        Rigidbody2D.gravityScale = 0f; // 重力の影響を無効化
        Rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // 高速移動時のすり抜け防止
        Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation; // 回転を固定
                                                                // ボタンを非表示にする
        if (goButton != null)
        {
            foreach (var button in goButton)
            {
                button.SetActive(false);
            }
        }
         // プレイヤーとの衝突を無視
        
    }
    private void Update()
    {
        // 万が一、壁をすり抜けた場合の処理
        if (transform.position.y < -10f) // 床下など意図しない範囲外に出た場合
        {
            Debug.LogWarning("アイテムが範囲外に出たため、位置をリセットします。");
            ResetPosition();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggeredByPlayer)
        {
            Debug.Log("プレイヤーがトリガー範囲内にいます。");
            isTriggeredByPlayer = true; // プレイヤーとの接触状態を記録
            foreach (var button in goButton)
            {
                button.SetActive(true); // ボタンを有効化
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isTriggeredByPlayer)
        {
            Debug.Log("プレイヤーがトリガー範囲外に出ました。");
            isTriggeredByPlayer = false; // プレイヤーとの接触状態を解除
            foreach (var button in goButton)
            {
                button.SetActive(false); // ボタンを非表示
            }
        }
    }
    // アイテムの位置をリセットする関数
    private void ResetPosition()
    {
        transform.position = new Vector3(0f, 0.5f, transform.position.z); // 初期位置に戻す
        Rigidbody2D.velocity = Vector2.zero; // 速度リセット
        Rigidbody2D.angularVelocity = 0f; // 回転速度リセット
    }
}
