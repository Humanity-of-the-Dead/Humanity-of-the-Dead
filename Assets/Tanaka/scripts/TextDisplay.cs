using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TextDisplay : MonoBehaviour
{
    [System.Serializable]　//2次元配列をインスペクター上で表示するため

    struct TextDataSet
    {
        public TextAsset[] textAsset;   //メモ帳のファイル(.txt)　配列
    }

    [SerializeField]
    private TextDataSet[] textDataSet; //構造体の配列

    [SerializeField]
    private Text text;  //画面上の文字

    [SerializeField]
    private float TypingSpeed = 1.0f;  //文字の表示速度

    private int LoadDataIndex = 0; //今何個目の構造体を読み込んでいるか

    private int LoadText = 0;   //何枚目のテキストを読み込んでいるのか

    private int n = 0;

    [SerializeField]
    private float[] Position;

    [SerializeField]
    private PlayerControl Player;


    [SerializeField]
    public GameObject TextArea; //テキスト表示域

    [SerializeField]
    private string customNewline = "[BR]"; // 改行として扱う文字列を指定

    bool[] Flag;

    [Header("次の文字が表示されるまでの時間")]
    [SerializeField]
    float TextSpeed = 0.1f;

    [SerializeField, Header("クリアのイメージが出てからシーン遷移するまでの時間")]

    private float clearToTransitionTime = 0.1f;

    private bool isTextFullyDisplayed = false; // 現在のテキストが完全に表示されたか

    private Coroutine TypingCroutine;  //コルーチンの管理

    float timer = 0;
    public bool IsTextFullyDisplayed()
    {
        return isTextFullyDisplayed; // メソッドを通じて状態を取得
    }
    //ゲームクリアパネル
    [SerializeField] GameObject GameClear;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";// 初期化
        Player = GameObject.Find("Player Variant").GetComponent<PlayerControl>();
        //Debug.Log(textAsset[0].text);
        //StartCoroutine("TextCoroutine");
        //テキスト表示域を非表示
        Flag = new bool[Position.Length];
        GameClear.SetActive(false);

        //TextArea.SetActive(true);

        //UpdateText();
    }

    // Update is called once per frame
    void Update()
    {



        switch (GameMgr.GetState())
        {
            case GameState.Main:
                if (Player.transform.position.x > Position[Position.Length - 1])
                {
                    //this.gameObject.SetActive(true);    //オブジェクトを表示
                    GameMgr.ChangeState(GameState.Clear);    //GameStateがShowTextに変わる
                    Flag[Position.Length - 1] = true;

                }
                for (int i = 0; i < Position.Length; i++)
                {
                    if (Player.transform.position.x > Position[i] && Flag[i] == false)
                    {
                        Flag[i] = true;

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
                        if (LoadText < textDataSet[LoadDataIndex].textAsset.Length - 1)
                        {
                            LoadNextText(); // 次のテキストを表示
                            UpdateText();
                            return;
                        }
                        //Debug.Log(textAsset.Length);
                        //GameMgr.ChangeState(GameState.Main);    //GameStateがMainに変わる
                        LoadDataIndex++; //構造体の配列番号を進める
                        CloseTextArea(); // 全てのテキストを読み終えたら閉じる
                        LoadText = 0;



                    }
                }


                break;



            case GameState.Clear:

                if (timer > 1)
                {
                    int iNextIndex = SceneTransitionManager.instance.sceneInformation.GetCurrentScene() + 1;
                    if (iNextIndex > SceneTransitionManager.instance.sceneInformation.sceneCount.Length)
                    {
                        iNextIndex = SceneTransitionManager.instance.sceneInformation.sceneCount.Length;
                    }


                    SceneTransitionManager.instance.NextSceneButton(iNextIndex);


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
                        if (LoadText < textDataSet[LoadDataIndex].textAsset.Length - 1)
                        {
                            LoadNextText(); // 次のテキストを表示
                            UpdateText();

                            return;
                        }
                        else

                        {
                            TextArea.SetActive(false);

                        }

                    }

                }
                if (!TextArea.activeSelf)
                {
                    GameClear.SetActive(true);

                }
                //Debug.Log(textDataSet[LoadDataIndex].textAsset.Length > LoadText);

                AudioSource BGM = MultiAudio.ins.bgmSource;
                if (GameClear.activeSelf) // テキストエリアが非表示の間
                {
                    // BGMが再生されていない場合、新しいBGMを再生
                    if (BGM.isPlaying && BGM.clip.name != "BGM_clear")
                    {
                        BGM.Stop();
                        MultiAudio.ins.PlayBGM_ByName("BGM_clear");
                        BGM.loop = false; // BGMをループしない
                    }


                    // BGMが最後まで流れたことを確認
                    if (BGM.time >= BGM.clip.length - clearToTransitionTime) // 0.1秒のマージンを持たせる
                    {
                        // クリア状態に遷移
                        GameMgr.ChangeState(GameState.Clear);
                    }


                }




                //    }

                //}





                break;
        }
    }
    public void UpdateText()
    {
        if (TypingCroutine != null)
        {

            Debug.Log("Stopping previous TypingCoroutine");

            StopCoroutine(TypingCroutine);
        }

        Debug.Log($"TypingCroutineは{TypingCroutine}");

        //Debug.Log($"UpdateText: LoadText = {LoadText}");
        if (textDataSet[LoadDataIndex].textAsset.Length > LoadText)
        {
            text.text = "";
            isTextFullyDisplayed = false;
            Debug.Log($"Displaying text: {textDataSet[LoadDataIndex].textAsset[LoadText].text}");
            Debug.Log("Starting new TypingCoroutine");

            TypingCroutine = StartCoroutine(TextCoroutine());
        }
        else
        {
            //Debug.Log("全テキストが表示されました");
        }
    }
    IEnumerator TextCoroutine()
    {
        Debug.Log("TextCoroutine started");

        string currentText = textDataSet[LoadDataIndex].textAsset[LoadText].text;

        if (!string.IsNullOrEmpty(customNewline))
        {
            currentText = currentText.Replace(customNewline, "\n");
        }

        for (int i = 0; i < currentText.Length; i++)   //テキストの中の文字を取得して、文字数を増やしていく
        {
            string currentChra = currentText.Substring(0, i); //現在の文字を所得する
            //Debug.Log($"Setting Text.text: {currentChra}");

            if (string.IsNullOrWhiteSpace(currentChra))
            {
                text.text = currentChra; //空白部分をそのまま設定する
                //Debug.Log($"Text.text is now: {text.text}");

                yield return new WaitForSeconds(TextSpeed);
                continue;  //次のループへ

            }
            //テキストが進むたびにコルーチンが呼び出される
            //textAsset[LoadText].text.Lengthによって中のテキストデータの文字数の所得
            yield return new WaitForSeconds(TextSpeed); //指定された時間待機する

            text.text = currentChra;  //iが増えるたびに文字を一文字ずつ表示していく

        }

        isTextFullyDisplayed = true; //全ての文字が表示されたかを示すフラグ
        //Debug.Log("TextCoroutine completed");

    }
    private void DisplayFullText()
    {
        if (TypingCroutine != null)
        {
            StopCoroutine(TypingCroutine); // コルーチンを停止
        }
        string fullText = textDataSet[LoadDataIndex].textAsset[LoadText].text;

        if (!string.IsNullOrEmpty(customNewline))
        {

            fullText = fullText.Replace(customNewline, "\n");
        }
        Debug.Log($"Setting full text: {fullText}");

        // 現在のテキストをすべて表示
        text.text = fullText;
        Debug.Log($"Text.text after full display: {text.text}");

        isTextFullyDisplayed = true; // 完全表示状態にする
    }
    // 次のテキストを読み込む
    private void LoadNextText()
    {
        if (LoadText < textDataSet[LoadDataIndex].textAsset.Length - 1)
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
    //private void OnGUI()
    //{
    //    GUI.skin.label.fontSize = 30;  // 例えば30に設定

    //    GUI.Label(new Rect(1000.0f, 500.0f, Screen.width, Screen.height), isTextFullyDisplayed.ToString());
    //    //GUI.Label(new Rect(1000.0f, 1000.0f, Screen.width, Screen.height), TypingCroutine.ToString());
    //    if (TypingCroutine == null)
    //    {

    //    }

    //}
}