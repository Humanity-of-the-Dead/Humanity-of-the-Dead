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
        Debug.Log($"OnTriggerStay2Dが呼び出されました: {collision.gameObject.name}");

        if (collision.CompareTag("Player"))
        {
            collision.attachedRigidbody?.WakeUp(); // スリープ状態を解除
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
            Debug.Log("プレイヤーがトリガー範囲外に出ました");
            for(int i = 0; i < goButton.Length; i++)
            {
                goButton[i].SetActive(false);
            }
        }
    }
}
