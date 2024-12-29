using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioButtonHandler : MonoBehaviour, IPointerEnterHandler

{
    public string clickSEName = "";
    public string hoverSEName = "";

    // BGMを再生する
   

    public void OnPointerEnter(PointerEventData eventData)
    {
        //UIの場合再生
        MultiAudio.ins.PlayUIByName(hoverSEName);
        //SEの場合再生
        MultiAudio.ins.PlaySEByName(hoverSEName);
    }
    // SEを再生する
    public void PlaySE()
    {
        //UIの場合再生
        MultiAudio.ins.PlayUIByName(clickSEName);
        //SEの場合再生
        MultiAudio.ins.PlaySEByName(clickSEName);
    }

   
}
