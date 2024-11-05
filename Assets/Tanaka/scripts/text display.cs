using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textdisplay : MonoBehaviour
{
    [SerializeField]
    private TextAsset[] textAsset;   //メモ帳のファイル(.txt)　配列

    [SerializeField]
    private Text text;  //画面上の文字

    private int LoadText = 0;   //何枚目のテキストを読み込んでいるのか

    private int n = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        text.text = "";// textAsset.text;
        Debug.Log(textAsset[0].text);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            //for (int i = 0; i < textAsset.Length; i++)
            //{
             
            if (textAsset.Length > LoadText )
            {//テキストをLoadTextの分表示
                text.text = textAsset[LoadText].text;


                Debug.Log(textAsset[LoadText].text);
                Debug.Log(textAsset[LoadText].text.Length); //テキスト上に何文字あるかデバック
                
                //Debug.Log(textAsset[LoadText]);
                Debug.Log(textAsset.Length);    //全体のテキスト数
                Debug.Log(LoadText);            //現在表示されているテキスト番号

                LoadText++;

                //}
            }

        }
        

    }
}
