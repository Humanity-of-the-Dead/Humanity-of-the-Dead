using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MouseOverObject: MonoBehaviour
{

    public GameObject mouseover;
    //void Start()
    //{
    //    mouseover.SetActive(false);
    //}

    //オブジェクトのセットアクティブを有効にするだけ
    public void OnPointerEnter()
    {
        mouseover.SetActive(true);
    }
    // 元の状態に戻す

    public void OnPointerExit()
    {
        mouseover.SetActive(false);
    }
    public void MyButtonSelect()
    {
        GetComponent<Button>()?.Select();
    }
    }









