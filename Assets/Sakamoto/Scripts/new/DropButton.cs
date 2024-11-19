using UnityEngine;

public class DropButton : MonoBehaviour
{
    [SerializeField] private GameObject[] goButton;

    void Start()
    {
        goButton[0].SetActive(false);
        goButton[1].SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log($"OnTriggerStay2D���Ăяo����܂���: {collision.gameObject.name}");

        if (collision.CompareTag("Player"))
        {
            collision.attachedRigidbody?.WakeUp(); // �X���[�v��Ԃ�����
            goButton[0].SetActive(true);
            goButton[1].SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�v���C���[���g���K�[�͈͊O�ɏo�܂���");
            goButton[0].SetActive(false);
            goButton[1].SetActive(false);
        }
    }
}
