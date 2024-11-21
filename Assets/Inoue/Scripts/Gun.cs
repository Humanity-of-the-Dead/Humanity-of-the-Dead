using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // 銃弾のプレハブ
    private Transform firePoint; // 発射位置のTransform

    void Start()
    {
        // Gunタグを持つオブジェクトの中からFirePointを探す
        firePoint = GameObject.FindGameObjectWithTag("Gun").transform;
    }

    public void Shoot(Vector2 direction)
    {

        // 銃弾を生成
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 銃弾の向きを設定
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bullet.GetComponent<Bullet>().speed;
    }
}

