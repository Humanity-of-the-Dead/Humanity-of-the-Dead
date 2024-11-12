using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageColorChange : MonoBehaviour
{
    Image image;
    [SerializeField] Sprite sprite_Unover;//マウスが触れていない時の画像
    [SerializeField] Sprite sprite_over;//マウスオーバー時の画像




    //マウスオーバー時に画像変更するメソッド
    public void Changed()
    {
        image = GetComponent<Image>();
        image.sprite = sprite_over;
    }
    //画像変更を元に戻す

    public void Restart()
    {
        image = GetComponent<Image>();

        Debug.Log(image.sprite);
        Debug.Log(sprite_Unover);
        image.sprite = sprite_Unover;
       

    }

}
