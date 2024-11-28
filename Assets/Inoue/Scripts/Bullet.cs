using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f; // �e�e�̑��x
    public Rigidbody2D rb;    // Rigidbody2D�ŃX�v���C�g�̌������v�Z����

    void Start()
    {
        //// �e�e�̐i�s�����ɑ��x��ݒ�
        //rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // �^�[�Q�b�g�ɓ����������ǂ������Փ˂����I�u�W�F�N�g�̃^�O�ɂ���Ĕ��f����
        // ���Ԃ�e���������̂��v���C���[���G�l�~�[�����A�ꍇ�ɂ���Ĕ�������^�O��ς���K�v������̂Ŗ�����
        if (hitInfo.gameObject.CompareTag("Player") || hitInfo.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("��������");
            Destroy(this.gameObject); // �e�e������
        }
    }

    void Update()
    {
        // �e�e����ʊO�ɏo�����ǂ������`�F�b�N
        if (IsOutOfScreen())
        {
            Destroy(gameObject); // �e�e������
        }
    }

    bool IsOutOfScreen()
    {
        // �e�e����ʊO�ɏo�����ǂ������`�F�b�N������
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        return screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;
    }
}


