using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColorChange : MonoBehaviour

{
    [SerializeField] Text Text;
    public Color hoverColor = new Color(0, 0, 0, 0.6f);  //マウスオーバー時の色アルファ値設定
    private Color originalColor;              // 元のテキストの色

    Button button;  // Start is called before the first frame update
    void Start()
    {
        originalColor = Text.color;


    }

    //テキスト色変更
    public void Text_ColorChange()
    {

        Text.color = hoverColor;
    }
    //テキスト色変更を元に戻す
    public void Text_ColorRestart()
    {
        
        Text.color = originalColor;
       
    }


   
    
}
