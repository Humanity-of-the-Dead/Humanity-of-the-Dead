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

        //銃弾を生成する位置を調整
        Vector2 adjustedPosition = (Vector2)firePoint.position + offset;

        // 銃弾を生成
        GameObject bullet = Instantiate(bulletPrefab, (Vector3)adjustedPosition, firePoint.rotation); // Vector3にキャストして生成

        // 銃弾の向きを設定
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bullet.GetComponent<Bullet>().speed;
        if (rb.velocity.x > 0)
        {
            Quaternion rotate = rb.transform.rotation;
            rotate.y = 180;
            rb.transform.rotation = rotate;
        }
    }
}

