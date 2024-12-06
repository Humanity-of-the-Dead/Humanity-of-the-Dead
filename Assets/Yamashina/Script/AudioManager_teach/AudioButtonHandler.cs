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

       
            MultiAudio.ins.PlayUIByName(hoverSEName);
       
    }
    // SEを再生する
    public void PlaySE()
    {
        MultiAudio.ins.PlayUIByName(clickSEName);
    }

   
}
