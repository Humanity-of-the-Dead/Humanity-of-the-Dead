using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // 銃弾のプレハブ
    //private Transform firePoint; // 発射位置のTransform

    //void Start()
    //{
    //    // PlayerとEnemyタグを持つオブジェクトの中からFirePointを探す
    //    // Enemyには銃を使わない種類もいるので配列になってます
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    GameObject []enemies = GameObject.FindGameObjectsWithTag("Enemy");

    //    // それぞれのオブジェクトにFirePointがあるか確認して設定
    //    if (player != null && player.transform != null)
    //    {
    //        firePoint = player.transform.Find("FirePoint");
    //    }

    //    foreach (GameObject enemy in enemies)
    //    {
    //        sEnemyParameters enemyParams = enemy.GetComponent<sEnemyParameters>();
    //        if (firePoint == null && enemyParams != null && enemyParams.canShoot && enemy.transform != null)
    //        {
    //            firePoint = enemy.transform.Find("FirePoint");
    //            if (firePoint != null) break; // FirePoint を見つけたらループを抜ける
    //        }
    //    }

    //    if (firePoint == null)
    //    {
    //        Debug.LogWarning("FirePoint が見つかりませんでした。");
    //    }
    //}

    [SerializeField]
    public Vector2 offset = new Vector2(0, 0.1f); //銃弾が出る位置(Y軸)を調整

    public void Shoot(Vector2 direction, Transform firePoint)
    {
        // 銃弾の位置を調整
        Vector2 adjustedPosition = (Vector2)firePoint.position + offset;

        // 銃弾を生成
        GameObject bullet = Instantiate(bulletPrefab, (Vector3)adjustedPosition, firePoint.rotation); // Vector3にキャストして生成

        // 銃弾のRigidbody2Dを取得
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // 発射方向に基づいてvelocityを設定
        rb.velocity = direction * bullet.GetComponent<Bullet>().speed;

        // 弾が右に飛ぶ場合
        if (rb.velocity.x > 0)
        {
            rb.rotation = 0; // 正面向き（右方向）
        }
        // 弾が左に飛ぶ場合
        else if (rb.velocity.x < 0)
        {
            rb.rotation = 180; // 反転（左方向）
        }
    }
}

