using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour
{
    [SerializeField]
    float TextSpeed = 1.0f;
    void Start()
    {
        // �R���[�`���̊J�n
        StartCoroutine(TextCoroutine());
    }

    IEnumerator TextCoroutine()
    {
        int Text = 9;

        for (int i = 0; i < Text; i++)
        {
            Debug.Log("�R���[�`�����J�n����܂���");

            // 1�b�ҋ@
            yield return new WaitForSeconds(TextSpeed);

            Debug.Log(TextSpeed + "�b�o�߂��܂���");

            // �t���[���̏I���܂őҋ@
            yield return null;

            Debug.Log("���̃t���[���̏I���܂őҋ@���܂���");
        }


    }
}
