using UnityEngine;

public class DropButton : MonoBehaviour
{
    [SerializeField] private GameObject[] goButton;
    private bool isTriggeredByPlayer = false; // �v���C���[�ƐڐG�����ǂ���
    private Rigidbody2D Rigidbody2D;
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        if (Rigidbody2D == null)
        {
            Debug.LogError("Rigidbody2D���A�^�b�`����Ă��܂���I");
            return;
        }

        // Rigidbody2D�̐ݒ�
        Rigidbody2D.bodyType = RigidbodyType2D.Dynamic; // ���I��������
        Rigidbody2D.gravityScale = 0f; // �d�͂̉e���𖳌���
        Rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // �����ړ����̂��蔲���h�~
        Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation; // ��]���Œ�
                                                                // �{�^�����\���ɂ���
        if (goButton != null)
        {
            foreach (var button in goButton)
            {
                button.SetActive(false);
            }
        }
         // �v���C���[�Ƃ̏Փ˂𖳎�
        
    }
    private void Update()
    {
        // ������A�ǂ����蔲�����ꍇ�̏���
        if (transform.position.y < -10f) // �����ȂǈӐ}���Ȃ��͈͊O�ɏo���ꍇ
        {
            Debug.LogWarning("�A�C�e�����͈͊O�ɏo�����߁A�ʒu�����Z�b�g���܂��B");
            ResetPosition();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggeredByPlayer)
        {
            Debug.Log("�v���C���[���g���K�[�͈͓��ɂ��܂��B");
            isTriggeredByPlayer = true; // �v���C���[�Ƃ̐ڐG��Ԃ��L�^
            foreach (var button in goButton)
            {
                button.SetActive(true); // �{�^����L����
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isTriggeredByPlayer)
        {
            Debug.Log("�v���C���[���g���K�[�͈͊O�ɏo�܂����B");
            isTriggeredByPlayer = false; // �v���C���[�Ƃ̐ڐG��Ԃ�����
            foreach (var button in goButton)
            {
                button.SetActive(false); // �{�^�����\��
            }
        }
    }
    // �A�C�e���̈ʒu�����Z�b�g����֐�
    private void ResetPosition()
    {
        transform.position = new Vector3(0f, 0.5f, transform.position.z); // �����ʒu�ɖ߂�
        Rigidbody2D.velocity = Vector2.zero; // ���x���Z�b�g
        Rigidbody2D.angularVelocity = 0f; // ��]���x���Z�b�g
    }
}
