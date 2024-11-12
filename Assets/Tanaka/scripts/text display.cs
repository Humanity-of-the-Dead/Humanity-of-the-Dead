using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class textdisplay : MonoBehaviour
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
    bool[] Flag;

    [SerializeField]
    float TextSpeed = 1.0f;
    
    GameObject TextImage;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";// 初期化
        Debug.Log(textAsset[0].text);
        //StartCoroutine("TextCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.enGameState)
        {
            case GameState.Main:
                if (Player.transform.position.x > Position[1] && Flag[1] == false)
                {
                    //this.gameObject.SetActive(true);    //オブジェクトを表示
                    Flag[1] = true;     //Flag[1]を通った
                    GameManager.ChangeState(GameState.ShowText);    //GameStateがShowTextに変わる

                    UpdateText();
                }
                if (Player.transform.position.x > Position[2] && Flag[2] == false)
                {
                    //this.gameObject.SetActive(true);    //オブジェクトを表示
                    Flag[2] = true;     //Flag[1]を通った
                    GameManager.ChangeState(GameState.ShowText);    //GameStateがShowTextに変わる

                    UpdateText();
                }
                break;
            case GameState.ShowText:
                if (Input.GetMouseButtonDown(0))
                {
                    //this.gameObject.SetActive(false);   //オブジェクトを非表示
                    GameManager.ChangeState(GameState.Main);
                }
                break;
        }
   
    }
    public void UpdateText()
    {
        int TextMax = 0;

        if (textAsset.Length > LoadText)
        {//テキストをLoadTextの分表示
            //text.text = //textAsset[LoadText].text;
            text.text = ""; //からのテキストをおいて初期化しているように見せる

            Debug.Log(textAsset[LoadText].text);
            Debug.Log(textAsset[LoadText].text.Length); //テキスト上に何文字あるかデバック

            //Debug.Log(textAsset[LoadText]);
            // Debug.Log(textAsset.Length);    //全体のテキスト数
            //Debug.Log(LoadText);            //現在表示されているテキスト番号

            StartCoroutine("TextCoroutine");        //テキストを呼び出されるたびにコルーチンを走らせて文字を加算していく
            //}
        }
    }
    IEnumerator TextCoroutine()
    {
        for (int i = 0; i < textAsset[LoadText].text.Length; i++)   //テキストの中の文字を取得して、文字数を増やしていく
        {                                                           //テキストが進むたびにコルーチンが呼び出される
            //textAsset[LoadText].text.Lengthのよって中のテキストデータの文字数の所得
            yield return new WaitForSeconds(TextSpeed);

            text.text += textAsset[LoadText].text[i];  //iが増えるたびに文字を一文字ずつ表示していく

            //i++;

            yield return null;
            
        }
        LoadText++;  //ボタンを押すたびにテキストを進める
    }
}
