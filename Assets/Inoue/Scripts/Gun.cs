using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // �e�e�̃v���n�u
    //private Transform firePoint; // ���ˈʒu��Transform

    //void Start()
    //{
    //    // Player��Enemy�^�O�����I�u�W�F�N�g�̒�����FirePoint��T��
    //    // Enemy�ɂ͏e���g��Ȃ���ނ�����̂Ŕz��ɂȂ��Ă܂�
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    GameObject []enemies = GameObject.FindGameObjectsWithTag("Enemy");

    //    // ���ꂼ��̃I�u�W�F�N�g��FirePoint�����邩�m�F���Đݒ�
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
    //            if (firePoint != null) break; // FirePoint ���������烋�[�v�𔲂���
    //        }
    //    }

    //    if (firePoint == null)
    //    {
    //        Debug.LogWarning("FirePoint ��������܂���ł����B");
    //    }
    //}

    [SerializeField]
    public Vector2 offset = new Vector2(0, 0.1f); //�e�e���o��ʒu(Y��)�𒲐�

    public void Shoot(Vector2 direction, Transform firePoint)
    {
        // �e�e�̈ʒu�𒲐�
        Vector2 adjustedPosition = (Vector2)firePoint.position + offset;

        // �e�e�𐶐�
        GameObject bullet = Instantiate(bulletPrefab, (Vector3)adjustedPosition, firePoint.rotation); // Vector3�ɃL���X�g���Đ���

        // �e�e��Rigidbody2D���擾
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // ���˕����Ɋ�Â���velocity��ݒ�
        rb.velocity = direction * bullet.GetComponent<Bullet>().speed;

        // �e���E�ɔ�ԏꍇ
        if (rb.velocity.x > 0)
        {
            rb.rotation = 0; // ���ʌ����i�E�����j
        }
        // �e�����ɔ�ԏꍇ
        else if (rb.velocity.x < 0)
        {
            rb.rotation = 180; // ���]�i�������j
        }
    }
}

