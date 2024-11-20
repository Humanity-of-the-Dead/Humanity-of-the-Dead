using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioButtonHandler_Matsuoka : MonoBehaviour, IPointerEnterHandler
{
    [Header("番号か名前か")]
    [Tooltip("クリック用")][SerializeField] bool isNum_Click;
    [Tooltip("マウスが上に来た用")][SerializeField] bool isNum_OnMou;

    [Header("音の配列番号")]
    [Tooltip("クリック用")][SerializeField] int ind_Click = -1;
    [Tooltip("マウスが上に来た用")][SerializeField] int ind_OnMou = -1;

    [Header("音の名前")]
    [Tooltip("クリック用")][SerializeField] string name_Click;
    [Tooltip("マウスが上に来た用")][SerializeField] string name_OnMou;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ボタンが押されたとき、SEを鳴らす
    public void OnClickButtonPlayOnShotSE()
    {
        if (isNum_Click)
        {
            if (ind_Click >= 0) MultiAudio_Matsuoka.ins.ChooseSongsSE(ind_Click);
        }
        //else ここに名前を使って鳴らす処理
    }

    //ボタンの上にマウスが来た時、SEをならす
    public void OnPointerEnter(PointerEventData eveData)
    {
        if (isNum_OnMou)
        {
            if(ind_OnMou >= 0)MultiAudio_Matsuoka.ins.ChooseSongsSE(ind_OnMou);
        }
        //else ここに名前を使って鳴らす処理
    }
}
