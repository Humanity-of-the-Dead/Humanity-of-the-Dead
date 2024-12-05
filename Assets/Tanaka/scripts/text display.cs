using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class textdisplay: MonoBehaviour
{
    [SerializeField]
    private TextAsset[] textAsset;   //メモ帳のファイル(.txt)　配列

    [SerializeField]
    private Text text;  //画面上の文字

    [SerializeField]
    private float TypingSpeed = 1.0f;  //文字の表示速度

    private int LoadText = 0;   //何枚目のテキストを読み込んでいるのか

    private int n = 0;

    [SerializeField]
    private float[] Position;

    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private GameMgr GameManager;

    [SerializeField]
    private GameObject TextArea; //テキスト表示域

    [SerializeField]
    private string customNewline = "[BR]"; // 改行として扱う文字列を指定

    bool[] Flag;

    [Header("次の文字が表示されるまでの時間")]
    [SerializeField]
    float TextSpeed = 0.1f;
    
    GameObject TextImage;

    private bool isTextFullyDisplayed = false; // 現在のテキストが完全に表示されたか

    private Coroutine TypingCroutine;  //コルーチンの管理

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";// 初期化
        Debug.Log(textAsset[0].text);
        //StartCoroutine("TextCoroutine");
        //テキスト表示域を非表示
        TextArea.SetActive(false);
        Flag = new bool[Position.Length];
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.enGameState)
        {
            case GameState.Main:
                for(int i = 0; i < Flag.Length; i++)
                {
                    if (Player.transform.position.x > Position[i] && Flag[i] == false)
                    {
                        //this.gameObject.SetActive(true);    //オブジェクトを表示
                        Flag[i] = true;     //Flag[i]を通った
                        GameManager.ChangeState(GameState.ShowText);    //GameStateがShowTextに変わる
                        UpdateText();
                        //テキスト表示域を表示域
                        TextArea.SetActive(true);

                    }
                }
                break;
            case GameState.ShowText:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (!isTextFullyDisplayed)
                    {
                        DisplayFullText(); //テキスト全表示
                    }
                    else if (LoadText < textAsset.Length - 1)
                    {
                        LoadNextText(); // 次のテキストを表示
                        Debug.Log(textAsset.Length);
                        GameManager.ChangeState(GameState.AfterBOss);    //GameStateがShowTextに変わる

                    }
                    else
                    {
                        CloseTextArea(); // 全てのテキストを読み終えたら閉じる
                    }


                }
                break;
            case GameState.AfterBOss:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (!isTextFullyDisplayed)
                    {
                        DisplayFullText(); //テキスト全表示
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
    public void UpdateText()
    {
        if (TypingCroutine != null)
        {
            StopCoroutine(TypingCroutine);
        }

        Debug.Log($"UpdateText: LoadText = {LoadText}");
        if (textAsset.Length > LoadText)
        {
            text.text = "";
            isTextFullyDisplayed = false;
            Debug.Log($"表示テキスト: {textAsset[LoadText].text}");
            TypingCroutine = StartCoroutine(TextCoroutine());
        }
        else
        {
            Debug.Log("全テキストが表示されました");
        }
    }
    IEnumerator TextCoroutine()
    {
        string currentText = textAsset[LoadText].text;

        if (!string.IsNullOrEmpty(customNewline))
        {
           currentText = currentText.Replace(customNewline, "\n");
        }

        for (int i = 0; i < currentText.Length; i++)   //テキストの中の文字を取得して、文字数を増やしていく
        {
            string currentChra = currentText.Substring(0,i); //現在の文字を所得する
            if (string.IsNullOrWhiteSpace(currentChra))
            {
                text.text = currentChra; //空白部分をそのまま設定する
                yield return new WaitForSeconds(TextSpeed);
                continue;  //次のループへ

            }
                //テキストが進むたびにコルーチンが呼び出される
            //textAsset[LoadText].text.Lengthによって中のテキストデータの文字数の所得
            yield return new WaitForSeconds(TextSpeed); //指定された時間待機する

            text.text = currentChra;  //iが増えるたびに文字を一文字ずつ表示していく
           
        }

            isTextFullyDisplayed = true; //全ての文字が表示されたかを示すフラグ
    }
    private void DisplayFullText()
    {
        if (TypingCroutine != null)
        {
            StopCoroutine(TypingCroutine); // コルーチンを停止
        }
        string fullText = textAsset[LoadText].text;

        if (!string.IsNullOrEmpty(customNewline))
        {

            fullText = fullText.Replace(customNewline, "\n");
        }
        // 現在のテキストをすべて表示
        text.text = fullText;

        isTextFullyDisplayed = true; // 完全表示状態にする
    }
    // 次のテキストを読み込む
    private void LoadNextText()
    {
        if (LoadText < textAsset.Length - 1)
        {
            LoadText++;
            //UpdateText(); // 新しいテキストを表示
        }
        else
        {
            Debug.Log("最後のテキストです");
        }
    }

    // テキストエリアを閉じる
    private void CloseTextArea()
    {
        GameManager.ChangeState(GameState.Main);
        TextArea.SetActive(false); // テキストエリアを非表示
    }
}
