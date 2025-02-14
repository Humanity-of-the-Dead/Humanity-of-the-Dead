using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextDisplay : MonoBehaviour
{
    [System.Serializable]　//2次元配列をインスペクター上で表示するため

    struct TextDataSet
    {
        public TextAsset[] textAsset;   //メモ帳のファイル(.txt)　配列
    }

    [SerializeField]
    private TextDataSet[] textDataSet; //構造体の配列

    [SerializeField, Header("ヒント(.txt) 配列")]
    private TextAsset[] hintTextAssetArray;

    [SerializeField]
    private Text text;  //画面上の文字

    [SerializeField]
    private float TypingSpeed = 1.0f;  //文字の表示速度

    public int LoadDataIndex = 0; //今何個目の構造体を読み込んでいるか

    public int LoadText = 0;   //何枚目のテキストを読み込んでいるのか

    private int n = 0;

    [SerializeField]
    protected float[] Position;

    [SerializeField]
    protected PlayerControl Player;


    [SerializeField]
    public GameObject TextArea; //テキスト表示域

    [SerializeField]
    private string customNewline = "[BR]"; // 改行として扱う文字列を指定
    private string newline = "\n";

    protected bool[] Flag;

    [Header("次の文字が表示されるまでの時間")]
    [SerializeField]
    float TextSpeed = 0.1f;

    [SerializeField, Header("クリアのイメージが出てからシーン遷移するまでの時間")]

    private float clearToTransitionTime = 0.1f;

    private bool isTextFullyDisplayed = false; // 現在のテキストが完全に表示されたか

    private Coroutine TypingCroutine;  //コルーチンの管理

    private bool displaysEnterKey = false;  // enterKeyを表示しているかどうか
    private string enterKeyAlternativeChar = "・";   // enterKeyの位置を決めるための代替文字

    float timer = 0;
    [SerializeField, Header("EnterKeyのプレハブ")]
    private GameObject enterKeyPrefab;

    private GameObject enterKeyInstance; // 既存の enterKeyInstance を保持する変数

    public bool IsTextFullyDisplayed()
    {
        return isTextFullyDisplayed; // メソッドを通じて状態を取得
    }

    //ゲームクリアパネル
    [SerializeField] GameObject GameClear;

    // Start is called before the first frame update
    protected virtual void Start()
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
    protected virtual void Update()
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
                ShowTextChange();

                break;

            case GameState.ShowText:

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (!isTextFullyDisplayed)
                    {
                        DisplayFullText(textDataSet[LoadDataIndex].textAsset[LoadText].text); //テキスト全表示
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
                        FinishTextShowText(); // 全てのテキストを読み終えたら閉じる
                    }
                }
                else
                {
                    // 正常に末尾の文字の位置を取得するため、テキスト描画系処理と同時実行を避ける必要がある
                    DisplayEnterKeyOnLastCharIfNeeded();
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    FinishTextShowText();
                }

                break;

            case GameState.Hint:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    UpdateHintText();
                }
                else
                {
                    DisplayEnterKeyOnLastCharIfNeeded();
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    FinishTextHint();
                }

                break;

            case GameState.Clear:

                if (timer > 1)
                {
                    int iNextIndex = SceneTransitionManager.instance.sceneInformation.GetCurrentSceneInt() + 1;
                    if (iNextIndex > SceneTransitionManager.instance.sceneInformation.sceneCount.Length)
                    {
                        iNextIndex = SceneTransitionManager.instance.sceneInformation.sceneCount.Length;
                    }

                    PlayerParameter.Instance.KeepBodyData();
                    SceneTransitionManager.instance.GoToNextScene(iNextIndex);


                    timer = 0;
                }
                timer += Time.deltaTime;


                break;
            case GameState.AfterBoss:


                if (Input.GetKeyDown(KeyCode.Return))
                {

                    if (!isTextFullyDisplayed)
                    {
                        DisplayFullText(textDataSet[LoadDataIndex].textAsset[LoadText].text); //テキスト全表示
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
                            FinishTextAfterBoss();
                        }

                    }

                }
                else
                {
                    DisplayEnterKeyOnLastCharIfNeeded();
                }
                ShowGameClearUI();  

                break;

        }
    }

    protected virtual void ShowGameClearUI()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FinishTextAfterBoss();
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
            RemoveEnterKey();
            Debug.Log($"Displaying text: {textDataSet[LoadDataIndex].textAsset[LoadText].text}");
            Debug.Log("Starting new TypingCoroutine");

            TypingCroutine = StartCoroutine(TextCoroutine(textDataSet[LoadDataIndex].textAsset[LoadText].text));
        }
        else
        {
            //Debug.Log("全テキストが表示されました");
        }
    }

    public virtual void ShowHintText()
    {
        GameMgr.ChangeState(GameState.Hint);
        TextArea.SetActive(true);
        LoadText = 0;
        initCurrentTextDisplay();
        TypingCroutine = StartCoroutine(TextCoroutine(hintTextAssetArray[LoadText].text));
    }

    protected virtual void UpdateHintText()
    {
        if (isTextFullyDisplayed)
        {
            initCurrentTextDisplay();
            if (LoadText < hintTextAssetArray.Length - 1)
            {
                LoadText++;
                TypingCroutine = StartCoroutine(TextCoroutine(hintTextAssetArray[LoadText].text));
            }
            else
            {
                FinishTextHint();
            }
        }
        else
        {
            DisplayFullText(hintTextAssetArray[LoadText].text);
        }
    }

    protected virtual void initCurrentTextDisplay()
    {
        text.text = "";
        isTextFullyDisplayed = false;
        RemoveEnterKey();

        if (TypingCroutine != null)
        {
            StopCoroutine(TypingCroutine);
        }
    }

    public virtual void FinishTextShowText()
    {
        LoadDataIndex++;
        CloseTextArea();
        LoadText = 0;
    }
    private void FinishTextAfterBoss()
    {
        TextArea.SetActive(false);
    }
    protected virtual void FinishTextHint()
    {
        CloseTextArea();
        LoadText = 0;
    }
    IEnumerator TextCoroutine(string textStr)
    {
        Debug.Log("TextCoroutine started");

        string currentText = GetTextStrFormatted(textStr);

        // テキストの中の文字を取得して、文字数を増やしていく
        // Substringで1文字目から取得していくため、i=1でスタート
        for (int i = 1; i <= currentText.Length; i++)
        {
            string currentChra = currentText.Substring(0, i); //現在の文字を所得する
            //Debug.Log($"Setting Text.text: {currentChra}");

            if (string.IsNullOrWhiteSpace(currentChra))
            {
                text.text = currentChra; //空白部分をそのまま設定する
                //Debug.Log($"Text.text is now: {text.text}");

                yield return new WaitForSecondsRealtime(TextSpeed);
                continue;  //次のループへ

            }
            //テキストが進むたびにコルーチンが呼び出される
            //textAsset[LoadText].text.Lengthによって中のテキストデータの文字数の所得
            yield return new WaitForSecondsRealtime(TextSpeed); //指定された時間待機する

            text.text = currentChra;  //iが増えるたびに文字を一文字ずつ表示していく

        }

        isTextFullyDisplayed = true; //全ての文字が表示されたかを示すフラグ
        Debug.Log("TextCoroutine completed");

    }
    private void DisplayFullText(string textStr)
    {
        if (TypingCroutine != null)
        {
            StopCoroutine(TypingCroutine); // コルーチンを停止
        }
        string fullText = GetTextStrFormatted(textStr);

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

    // 受け取ったテキストの内容を整形して渡す
    private string GetTextStrFormatted(string str)
    {
        // 改行コード統一
        if (!string.IsNullOrEmpty(customNewline))
        {
            str = str.Replace(customNewline, newline).Replace("\r\n", newline).Replace("\r", newline);
        }

        // enterKey用代替文字を末尾に追加
        str += enterKeyAlternativeChar;

        return str;
    }

    private void DisplayEnterKeyOnLastCharIfNeeded()
    {
        if (isTextFullyDisplayed && !displaysEnterKey && TextArea.activeSelf)
        {
            DisplayEnterKeyOnLastChar();
        }
    }

    // 表示テキストの末尾の文字をenterKeyに置き換える。事前にenterKeyAlternativeCharをテキスト末尾に付与して使用
    private void DisplayEnterKeyOnLastChar()
    {
        string textStr = text.text;
        int lastCharIndex = textStr.Length - 1;

        // 表示されない文字の数
        string[] invisibleStrs = { "\n", "\x20", "□" };
        int invisibleCharCount = textStr.Length - RemoveByChars(textStr, invisibleStrs).Length;

        // 表示中の各文字(を囲む四角形)の4頂点の座標を取得
        IList<UIVertex> vertexs = text.cachedTextGenerator.verts;

        // 末尾文字の左上頂点のインデックス
        // 表示されていない文字には座標が無いので、その分差し引く
        int vertexCountPerChar = 4; // 1文字につき4頂点
        int vIdx = (lastCharIndex - invisibleCharCount) * vertexCountPerChar;

        Vector3 enterKeyPosition = Vector3.zero;
        // vIdxが末尾文字のものとして正しいかチェック
        if (vIdx == vertexs.Count - vertexCountPerChar)
        {
            UIVertex topLeft = vertexs[vIdx];
            UIVertex bottomRight = vertexs[vIdx + 2];

            // 各頂点座標をピクセル単位からユニット単位に変換
            topLeft.position /= text.pixelsPerUnit;
            bottomRight.position /= text.pixelsPerUnit;

            // 2頂点の中央 = 文字の中央座標
            Vector3 lastCharPosistion = (topLeft.position + bottomRight.position) / 2f;

            // テキスト表示域の座標分調整
            enterKeyPosition = lastCharPosistion + text.transform.localPosition;
            //Debug.Log($"末尾文字の中心座標lastCharPosistion: {lastCharPosistion}");
        }
        else
        {
            // テキストボックス右下辺り
            Vector3 defaultPosition = new Vector3(545f, 117f, 0f);
            enterKeyPosition = defaultPosition;

            Debug.Log("failed to get last character position of text.");
            Debug.Log($"vIdx= {vIdx}, vertexs.Count= {vertexs.Count}");
        }

        // 末尾の文字の位置に新しい enterKeyInstance を生成して置換
        enterKeyInstance = Instantiate(enterKeyPrefab);
        if (textStr.Length > 0)
        {
            text.text = textStr.Remove(textStr.Length - 1);
        }
        enterKeyInstance.transform.localPosition = enterKeyPosition;
        enterKeyInstance.transform.SetParent(TextArea.transform.GetChild(0).transform.GetChild(0).transform, false);
        //Debug.Log($"新しい enterKeyInstance を生成: {enterKeyInstance.transform.localPosition}");

        displaysEnterKey = true;
    }

    private void RemoveEnterKey()
    {
        if (enterKeyInstance != null)
        {
            Destroy(enterKeyInstance);
        }
        displaysEnterKey = false;
    }

    // targetStrからremoveStrsの文字を取り除いて返す
    private string RemoveByChars(string targetStr, string[] removeStrs)
    {
        //削除する文字にマッチするパターンを作成する
        string removePattern = "[" + string.Join("", removeStrs) + "]";
        //削除する文字を""に置換する
        return Regex.Replace(targetStr, removePattern, "");
    }

    public virtual void ShowTextChange()
    {
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