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

    [SerializeField]
    private string customNewline = "[BR]"; // 改行として扱う文字列を指定

    bool[] Flag;

    [Header("次の文字が表示されるまでの時間")]
    [SerializeField]
    float TextSpeed = 0.1f;

    GameObject TextImage;

    private bool isTextFullyDisplayed = false; // 現在のテキストが完全に表示されたか

    private Coroutine TypingCroutine;  // コルーチンの管理

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";  // 初期化
        Debug.Log(textAsset[0].text);
        TextArea.SetActive(false);  // テキスト表示域を非表示
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

                        TextArea.SetActive(true); // テキスト表示域を表示

                        UpdateText(); // 最初のテキストを更新
                    }
                }
                break;
            case GameState.ShowText:
                // エンターキーでアニメーションを再開
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (isTextFullyDisplayed)
                    {
                        // 次のテキストを表示
                        LoadNextText();
                    }
                    else
                    {
                        // テキストが完全に表示されていない場合、アニメーションを再開
                        DisplayFullText();
                    }
                }

                // テキストが完全に表示された状態で、クリックで閉じる
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
            text.text = ""; // テキストを空にして初期化

            TypingCroutine = StartCoroutine(TextCoroutine()); // コルーチンを開始
        }
        else
        {
            Debug.Log("全てのテキストが表示されました");
        }
    }

    IEnumerator TextCoroutine()
    {
        string currentText = textAsset[LoadText].text;

        // 改行処理: [BR]を\nに変換
        if (!string.IsNullOrEmpty(customNewline))
        {
            currentText = currentText.Replace(customNewline, "\n");
        }

        for (int i = 0; i < currentText.Length; i++)
        {
            // 改行があった場合はそのまま表示し、次の行に進む
            if (currentText[i] == '\n')
            {
                text.text += "\n";  // 改行を追加
                continue;
            }

            yield return new WaitForSeconds(TextSpeed);  // 文字が表示されるまで待機
            text.text += currentText[i];  // 一文字ずつ追加
        }

        // テキストが完全に表示された後、次のテキストへ進む処理
        isTextFullyDisplayed = true; // 現在のテキストが完全に表示された
    }

    private void DisplayFullText()
    {
        // コルーチンがまだ走っている場合は止めて全て表示する
        if (TypingCroutine != null)
        {
            StopCoroutine(TypingCroutine);
        }

        string fullText = textAsset[LoadText].text;

        // 改行処理: [BR]を\nに変換
        if (!string.IsNullOrEmpty(customNewline))
        {
            fullText = fullText.Replace(customNewline, "\n");
        }

        text.text = fullText; // 完全に表示
        isTextFullyDisplayed = true; // 完全表示状態にする
    }

    private void LoadNextText()
    {
        // 次のテキストを読み込む
        if (LoadText < textAsset.Length - 1)
        {
            LoadText++;
            isTextFullyDisplayed = false; // 次のテキストはまだ完全に表示されていない

            UpdateText(); // 新しいテキストを表示する
        }
        else
        {
            Debug.Log("最後のテキストです");
        }
    }
}
