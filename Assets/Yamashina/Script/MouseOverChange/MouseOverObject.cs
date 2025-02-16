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
        Debug.Log(mouseover.activeSelf+gameObject.name);
    }
    // 元の状態に戻す

    public void OnPointerExit()
    {
        mouseover.SetActive(false);
        Debug.Log(mouseover.activeSelf+gameObject.name);

    }
    public void MyButtonSelect()
    {
        GetComponent<Button>()?.Select();
    }
    }









