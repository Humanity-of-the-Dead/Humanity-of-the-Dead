using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class text_test : MonoBehaviour
{
    [SerializeField]
    private TextAsset[] textAsset;   //メモ帳のファイル(.txt)　配列

    [SerializeField]
    private Text text;  //画面上の文字

    [SerializeField]
    private float TypingSpeed = 1.0f;  //文字の表示速度

    private int LoadText = 0;   //何枚目のテキストを読み込んでいるのか

    private int n = 0;

    [Header("次の文字が表示されるまでの時間")][SerializeField]
    float TextSpeed = 0.1f;
    
    GameObject TextImage;

    private bool isTextFullyDisplayed = false; // 現在のテキストが完全に表示されたか

    private Coroutine TypingCroutine;  //コルーチンの管理

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";// 初期化
        if (textAsset.Length > 0)
        {
            Debug.Log(textAsset[0].text);
        }
        //StartCoroutine("TextCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        //switch (GameManager.enGameState)
        //{
        //    case GameState.Main:
        //        if (Player.transform.position.x > Position[1] && Flag[1] == false)
        //        {
        //            //this.gameObject.SetActive(true);    //オブジェクトを表示
        //            Flag[1] = true;     //Flag[1]を通った
        //            GameManager.ChangeState(GameState.ShowText);    //GameStateがShowTextに変わる

        //            UpdateText();
        //        }
        //        if (Player.transform.position.x > Position[2] && Flag[2] == false)
        //        {
        //            //this.gameObject.SetActive(true);    //オブジェクトを表示
        //            Flag[2] = true;     //Flag[1]を通った
        //            GameManager.ChangeState(GameState.ShowText);    //GameStateがShowTextに変わる

        //            UpdateText();
        //        }
        //        break;
        //    case GameState.ShowText:
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            //this.gameObject.SetActive(false);   //オブジェクトを非表示
        //            GameManager.ChangeState(GameState.Main);
        //        }
        //        break;
        //}
   
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
            //Debug.Log(textAsset.Length);    //全体のテキスト数
           // Debug.Log(LoadText);            //現在表示されているテキスト番号

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
        for (int i = 0; i < textAsset[LoadText].text.Length; i++)   //テキストの中の文字を取得して、文字数を増やしていく
        {                                                           //テキストが進むたびにコルーチンが呼び出される
            //textAsset[LoadText].text.Lengthによって中のテキストデータの文字数の所得
            yield return new WaitForSeconds(TextSpeed); //TextSpeed文待機して文字を増やす

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
}
