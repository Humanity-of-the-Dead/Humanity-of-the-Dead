using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick : MonoBehaviour
{
    //サウンドエフェクト
    AudioSource audSou;

    // Start is called before the first frame update
    void Start()
    {
        //SE取得
        audSou = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ボタンが押されたときSEを流す
    public void OnClickPlayOneShot()
    {
        audSou.PlayOneShot(audSou.clip);
    }
}
