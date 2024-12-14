using UnityEngine;

public class DropButton : MonoBehaviour
{
    [SerializeField] private GameObject[] goButton;

    void Start()
    {
        for(int i = 0; i < goButton.Length; i++)
        {
            goButton[i].SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log($"OnTriggerStay2D���Ăяo����܂���: {collision.gameObject.name}");

        if (collision.CompareTag("Player"))
        {
            collision.attachedRigidbody?.WakeUp(); // �X���[�v��Ԃ�����
            for(int i = 0; i < goButton.Length;i++)
            {
                goButton[i].SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�v���C���[���g���K�[�͈͊O�ɏo�܂���");
            for(int i = 0; i < goButton.Length; i++)
            {
                goButton[i].SetActive(false);
            }
        }
    }
}
