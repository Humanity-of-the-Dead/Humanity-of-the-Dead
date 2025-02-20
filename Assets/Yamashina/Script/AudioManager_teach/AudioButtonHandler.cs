using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioButtonHandler : MonoBehaviour, IPointerClickHandler

{
    public string clickSEName = "";
    public string hoverSEName = "";

    // BGMを再生する
   

    public void OnPointerClick(PointerEventData eventData)
    {
        MultiAudio.ins.PlaySEByName(hoverSEName);
        MultiAudio.ins.PlayUIByName(clickSEName);

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
