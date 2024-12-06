using UnityEngine;
using UnityEngine.UI; // Text コンポーネント用
using System.Collections;

public class TextDisplay_Yamashina : MonoBehaviour
{
    [SerializeField]
    private TextAsset[] textAsset;   // テキストファイル配列

    [SerializeField]
    private Text text;  // UnityのTextコンポーネント

    [SerializeField]
    private float TypingSpeed = 1.0f;  // 文字表示速度

    private int LoadText = 0;   // 現在表示中のテキストのインデックス

    [SerializeField]
    private float[] Position; // 位置配列

    [SerializeField]
    private GameObject Player; // プレイヤーの参照

    [SerializeField]
    private GameMgr GameManager; // ゲーム管理スクリプト

    [SerializeField]
    private GameObject TextArea; // テキスト表示エリア

    [SerializeField]
    private string customNewline = "[BR]"; // カスタム改行文字列

    private bool[] Flag; // フラグでテキストの表示を制御

    [Header("1文字あたりの表示間隔")]
    [SerializeField]
    private float TextSpeed = 0.1f;

    private Coroutine TypingCoroutine; // 現在のコルーチンを保持
    private bool isTextFullyDisplayed = false; // 現在のテキストが完全に表示されているか

    // 開始時に実行される
    void Start()
    {
        text.text = "";  // 初期化
        TextArea.SetActive(false);  // テキスト表示領域を非表示
        Flag = new bool[Position.Length];
    }

    // 毎フレーム実行される
    void Update()
    {
        switch (GameMgr.GetState())
        {
            case GameState.Main:
                for (int i = 0; i < Flag.Length; i++)
                {
                    if (Player.transform.position.x > Position[i] && Flag[i] == false)
                    {
                        Flag[i] = true; // 一度だけ通る
                        GameMgr.ChangeState(GameState.ShowText);

                        TextArea.SetActive(true);
                        UpdateText(); // 最初のテキストを更新
                    }
                }
                break;

            case GameState.ShowText:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (!isTextFullyDisplayed)
                    {
                        DisplayFullText(); // 現在のテキストを一気に表示
                    }
                    else if (LoadText < textAsset.Length - 1)
                    {
                        LoadNextText(); // 次のテキストを表示
                    }
                    else
                    {
                        CloseTextArea(); // 全てのテキストを読み終えたら閉じる
                    }
                }
                break;
        }
    }

    // テキストを更新
    public void UpdateText()
    {
        if (TypingCoroutine != null)
        {
            StopCoroutine(TypingCoroutine); // コルーチンの競合を防ぐ
        }

        if (textAsset.Length > LoadText)
        {
            text.text = ""; // テキストをクリア
            isTextFullyDisplayed = false; // テキストが完全に表示されていない
            TypingCoroutine = StartCoroutine(TextCoroutine()); // コルーチン開始
        }
        else
        {
            Debug.Log("すべてのテキストが表示されました");
        }
    }

    // コルーチンで1文字ずつ表示
    IEnumerator TextCoroutine()
    {
        string currentText = textAsset[LoadText].text;

        // 改行処理: [BR] を \n に変換
        if (!string.IsNullOrEmpty(customNewline))
        {
            currentText = currentText.Replace(customNewline, "\n");
        }

        // テキストを一文字ずつ表示
        for (int i = 0; i < currentText.Length; i++)
        {
            text.text += currentText[i];
            yield return new WaitForSeconds(TextSpeed);
        }

        // テキストが完全に表示された
        isTextFullyDisplayed = true;
    }

    // テキストを一気に表示
    private void DisplayFullText()
    {
        if (TypingCoroutine != null)
        {
            StopCoroutine(TypingCoroutine); // コルーチン停止
        }

        string fullText = textAsset[LoadText].text;

        // 改行処理: [BR] を \n に変換
        if (!string.IsNullOrEmpty(customNewline))
        {
            fullText = fullText.Replace(customNewline, "\n");
        }

        text.text = fullText; // テキストを完全表示
        isTextFullyDisplayed = true; // フラグを更新
    }

    // 次のテキストを読み込む
    private void LoadNextText()
    {
        if (LoadText < textAsset.Length - 1)
        {
            LoadText++;
            UpdateText(); // 新しいテキストを表示
        }
        else
        {
            Debug.Log("最後のテキストです");
        }
    }

    // テキストエリアを閉じる
    private void CloseTextArea()
    {
        GameMgr.ChangeState(GameState.Main);
        TextArea.SetActive(false); // テキストエリアを非表示
    }
}
