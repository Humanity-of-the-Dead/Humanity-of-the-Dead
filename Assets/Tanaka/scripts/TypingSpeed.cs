using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour
{
    [SerializeField]
    float TextSpeed = 1.0f;
    void Start()
    {
        // コルーチンの開始
        StartCoroutine(TextCoroutine());
    }

    IEnumerator TextCoroutine()
    {
        int Text = 9;

        for (int i = 0; i < Text; i++)
        {
            Debug.Log("コルーチンが開始されました");

            // 1秒待機
            yield return new WaitForSeconds(TextSpeed);

            Debug.Log(TextSpeed + "秒経過しました");

            // フレームの終わりまで待機
            yield return null;

            Debug.Log("次のフレームの終わりまで待機しました");
        }


    }
}
