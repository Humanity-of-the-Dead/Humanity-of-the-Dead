using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//スライダを使うのに必要
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    //スライダ
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        //スライダを取得
        slider = GetComponent<Slider>();

        //スライダの最大値、現在値
        float maxVal,nowVal;

        maxVal = 100f;
        nowVal = 40;

        //スライダの最大値設定
        slider.maxValue = maxVal;

        //スライダの現在値設定
        slider.value= nowVal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //スライダの値が変更されるたびに実行
    public void Method()
    {
        //スライダの値を出力
        Debug.Log("現在地:"+slider.value);

        if(slider.value >= 50)
        {
            Debug.Log("50以上です");
        }
    }
}
