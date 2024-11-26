using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextDisplay_Yamashina : MonoBehaviour
{
    [SerializeField]
    private TextAsset[] textAsset;   // メモ帳のファイル(.txt) 配列

    [SerializeField]
    private Text text;  // 画面上の文字

    [SerializeField]
    private float TypingSpeed = 1.0f;  // 文字の表示速度

    private int LoadText = 0;   // 何枚目のテキストを読み込んでいるのか

    private int n = 0;

    [SerializeField]
    private float[] Position;

    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private GameMgr GameManager;

    [SerializeField]
    private GameObject TextArea; // テキスト表示域

    bool[] Flag;

    [Header("次の文字が表示されるまでの時間")]
    [SerializeField]
    float TextSpeed = 0.1f;

    GameObject TextImage;

    private bool isTextFullyDisplayed = false; // 現在のテキストが完全に表示されたか
    private Coroutine currentCoroutine; // 現在のコルーチンを管理

    // Start is called before the first frame update
    void Start()
    {
        text.text = ""; // 初期化
        Debug.Log(textAsset[0].text);
        TextArea.SetActive(false); // テキスト表示域を非表示
        Flag = new bool[Position.Length];
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.enGameState)
        {
            case GameState.Main:
                for (int i = 0; i < Flag.Length; i++)
                {
                    if (Player.transform.position.x > Position[i] && Flag[i] == false)
                    {
                        Flag[i] = true; // Flag[i]を通った
                        GameManager.ChangeState(GameState.ShowText); // GameStateがShowTextに変わる

                        // テキスト表示域を表示
                        TextArea.SetActive(true);

                        UpdateText();
                    }
                }
                break;

            case GameState.ShowText:
                // エンターキーでテキストをスキップして全表示
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (isTextFullyDisplayed)
                    {
                        // テキストが完全表示されている場合は何もしない
                        return;
                    }

                    // テキストをすべて表示
                    DisplayFullText();
                }

                // マウスクリックで次の処理に進む
                if (isTextFullyDisplayed && Input.GetMouseButtonDown(0))
                {
                    GameManager.ChangeState(GameState.Main);
                    TextArea.SetActive(false); // テキスト表示域を非表示
                }
                break;
        }
    }

    public void UpdateText()
    {
        if (textAsset.Length > LoadText)
        {
            text.text = ""; // 初期化
            isTextFullyDisplayed = false; // テキストが完全に表示されていない状態に

            // コルーチンを開始
            currentCoroutine = StartCoroutine(TextCoroutine());
        }
    }

    IEnumerator TextCoroutine()
    {
        string currentText = textAsset[LoadText].text;

        for (int i = 0; i < currentText.Length; i++)
        {
            yield return new WaitForSeconds(TextSpeed);

            text.text += currentText[i]; // 一文字ずつ追加
           
            yield return null;
        }

        isTextFullyDisplayed = true; // すべての文字が表示された
        LoadText++; // 次のテキストに進む
    }

    private void DisplayFullText()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); // コルーチンを停止
        }

        // 現在のテキストをすべて表示
        text.text = textAsset[LoadText].text;

        isTextFullyDisplayed = true; // 完全表示状態にする
    }
}
