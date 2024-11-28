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

                        //テキスト表示域を表示域
                        TextArea.SetActive(true);

                        UpdateText();
                    }
                }
                break;
            case GameState.ShowText:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (isTextFullyDisplayed)
                    {
                        return;
                    }
                    // テキストをすべて表示
                    DisplayFullText();
                }

                    if (isTextFullyDisplayed && Input.GetMouseButtonDown(0))
                    {
                        //this.gameObject.SetActive(false);   //オブジェクトを非表示
                        GameManager.ChangeState(GameState.Main);

                        //テキスト表示域を非表示
                        TextArea.SetActive(false);
                    }
                
                break;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isTextFullyDisplayed)
            {
                Debug.Log("クリック無効");
                return;
            }
            if (TypingCroutine != null)
            {
                StopCoroutine(TypingCroutine); //コルーチン実行中の場合文章を表示する
                text.text = textAsset[LoadText].text; //テキストを全て表示
                TypingCroutine = null;

                //配列の範囲内かどうか確認
                if (LoadText < textAsset.Length - 1)
                {
                    LoadText++;
                }
                else
                {
                    Debug.Log("最後の文章");
                    isTextFullyDisplayed = true;
                }
            }
            else
            {
                if (LoadText < textAsset.Length)
                {
                    UpdateText();
                }
                else
                {
                    Debug.Log("全テキストが既に表示されています。");
                    isTextFullyDisplayed = true; // 再確認して終了フラグを設定
                }
            }
        }

    }
    public void UpdateText()
    {
        if (textAsset.Length > LoadText)
        {//テキストをLoadTextの分表示
            //text.text = //textAsset[LoadText].text;
            text.text = ""; //からのテキストをおいて初期化しているように見せる

            //Debug.Log(textAsset[LoadText].text);
            //Debug.Log(textAsset[LoadText].text.Length); //テキスト上に何文字あるかデバック
            Debug.Log($"テキスト{LoadText}を表示開始: {textAsset[LoadText].text}");

            //Debug.Log(textAsset[LoadText]);
            // Debug.Log(textAsset.Length);    //全体のテキスト数
            //Debug.Log(LoadText);            //現在表示されているテキスト番号

            TypingCroutine = StartCoroutine(TextCoroutine()); //コルーチンを再スタート       //テキストを呼び出されるたびにコルーチンを走らせて文字を加算していく
        }
        else
        {
            Debug.Log("全テキストが表示された");
        }
        if (textAsset == null || textAsset.Length == 0)
        {
            Debug.LogError("textAssetがない");
            return;
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
            string currentChra = currentText[i].ToString();
            if (currentChra == "") continue;  //改行処理
                
                //テキストが進むたびにコルーチンが呼び出される
            //textAsset[LoadText].text.Lengthのよって中のテキストデータの文字数の所得
            yield return new WaitForSeconds(TextSpeed);

            text.text += textAsset[LoadText].text[i];  //iが増えるたびに文字を一文字ずつ表示していく
           
        }
        if (LoadText < textAsset.Length - 1)
        {
            LoadText++;
        }
        else
        {
            Debug.Log("最後の文章");
            isTextFullyDisplayed = true;
        }
        TypingCroutine = null;
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
}
