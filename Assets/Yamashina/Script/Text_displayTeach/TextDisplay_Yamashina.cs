using UnityEngine;
using UnityEngine.UI; // Text コンポーネント用
using System.Collections;

public class TextDisplay_Yamashina : MonoBehaviour
{

    [System.Serializable]　//2次元配列をインスペクター上で表示するため
    struct TexeDataSet　//一個のアセットの配列のまとまり
    {
        public TextAsset[] textAsset; //メモ帳のファイル(.txt) 配列
    }

    [SerializeField]
    private TexeDataSet[] textData;  //構造体の配列

    [SerializeField]
    private Text text;  //画面上の文字

    [SerializeField]
    private float TypingSpeed = 1.0f;  //文字の表示速度

    private int LoadDataIndex = 0;　//今何個目の構造体を読み込んでいるか
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

    float timer = 0;

    //ゲームクリアパネル
    [SerializeField] GameObject GameClear;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";// 初期化
        //Debug.Log(textAsset[0].text);
        //StartCoroutine("TextCoroutine");
        //テキスト表示域を非表示
        TextArea.SetActive(false);
        Flag = new bool[Position.Length];
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameMgr.GetState())
        {
            case GameState.Main:
                for (int i = 0; i < Flag.Length; i++)
                {
                    if (Player.transform.position.x > Position[i] && Flag[i] == false)
                    {
                        //this.gameObject.SetActive(true);    //オブジェクトを表示
                        Flag[i] = true;     //Flag[i]を通った
                        GameMgr.ChangeState(GameState.ShowText);    //GameStateがShowTextに変わる
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
                    else
                    {
                        if (LoadText < textData[LoadDataIndex].textAsset.Length - 1)
                        {
                            LoadNextText(); // 次のテキストを表示
                            UpdateText();//ここでコルーチンを一度再開
                            return;//一度処理を停止
                        }
                        //Debug.Log(textAsset.Length);
                        LoadDataIndex++;　//構造体の配列番号を勧める
                        CloseTextArea(); // 全てのテキストを読み終えたら閉じる
                    }
                }
                break;
            case GameState.Clear:
                if (timer > 1)
                {
                    int iNextIndex = SceneTransitionManager.instance.sceneInformation.GetCurrentScene() + 1;
                    if (iNextIndex > 5)
                    {
                        iNextIndex = 5;
                    }
                    AudioSource BGM = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();

                    if (BGM.isPlaying == false)
                    {
                        SceneTransitionManager.instance.NextSceneButton(iNextIndex);

                    }
                    timer = 0;
                }
                timer += Time.deltaTime;

                break;
            case GameState.AfterBOss:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (!isTextFullyDisplayed)
                    {
                        DisplayFullText(); //テキスト全表示
                    }
                    else
                    {
                        //if (LoadText < textAsset.Length - 1)
                        //{
                        //    LoadNextText(); // 次のテキストを表示
                        //}
                        //Debug.Log(textAsset.Length);

                        CloseTextArea(); // 全てのテキストを読み終えたら閉じる
                        AudioSource BGM = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
                        BGM.Stop();
                        MultiAudio.ins.PlayBGM_ByName("BGM_clear");

                        BGM.loop = false;

                        if (BGM.isPlaying)
                        {

                            GameClear.SetActive(true); // ゲームクリア表示を表示する
                        }

                        GameMgr.ChangeState(GameState.Clear);    //GameStateがClearに変わる


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
        if (textData[LoadDataIndex].textAsset.Length > LoadText)//構造体の読み込みたい配列番号のテキストアセットの長さとLoadTextを比較
        {
            text.text = "";
            isTextFullyDisplayed = false;
            Debug.Log($"表示テキスト: {textData[LoadDataIndex].textAsset[LoadText].text}");
            TypingCroutine = StartCoroutine(TextCoroutine());
        }
        else
        {
            Debug.Log("全テキストが表示されました");
        }
    }
    IEnumerator TextCoroutine()
    {
        string currentText = textData[LoadDataIndex].textAsset[LoadText].text;

        if (!string.IsNullOrEmpty(customNewline))
        {
            currentText = currentText.Replace(customNewline, "\n");
        }

        for (int i = 0; i < currentText.Length; i++)   //テキストの中の文字を取得して、文字数を増やしていく
        {
            string currentChra = currentText.Substring(0, i); //現在の文字を所得する
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
        string fullText = textData[LoadDataIndex].textAsset[LoadText].text;

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
        if (LoadText < textData[LoadDataIndex].textAsset.Length - 1)
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
        GameMgr.ChangeState(GameState.Main);
        TextArea.SetActive(false); // テキストエリアを非表示
    }
}


