using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // �e�e�̃v���n�u
    private Transform firePoint; // ���ˈʒu��Transform

    void Start()
    {
        // Gun�^�O�����I�u�W�F�N�g�̒�����FirePoint��T��
        firePoint = GameObject.FindGameObjectWithTag("Gun").transform;
    }

    public void Shoot(Vector2 direction)
    {

        // �e�e�𐶐�
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // �e�e�̌�����ݒ�
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bullet.GetComponent<Bullet>().speed;
    }
}

