using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    //オプション画面
    [SerializeField]GameObject optScr;

    //オプション画面を表示しているか
    bool isShoOptScr = false;

    // Start is called before the first frame update
    void Start()
    {
        //オプション画面をとじる
        optScr.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //オプションボタンが押されたとき
    //オプション画面がとじていたらオプション画面を表示
    //オプション画面が開いていたら閉じる
    public void OnCrickOptionButton()
    {
        //オプション画面が閉じているとき
        if (!isShoOptScr)
        {
            isShoOptScr=true;

            //オプション画面を表示
            optScr.SetActive(true);
        }
        else
        {
            isShoOptScr=false;

            //オプション画面を非表示
            optScr.SetActive(false);
        }
    }
}
