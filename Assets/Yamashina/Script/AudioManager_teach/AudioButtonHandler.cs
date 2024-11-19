using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioButtonHandler : MonoBehaviour
{
    // 再生するBGMのインデックス
    public int bgmIndex = -1;
    // 再生するSEのインデックス
    public int seIndex = -1;

    // BGMを再生する
    public void PlayBGM()
    {
        if (bgmIndex >= 0)
        {
            MultiAudio.ins.ChooseSongs_BGM(bgmIndex);
        }
    }

    // SEを再生する
    public void PlaySE()
    {
        if (seIndex >= 0)
        {
            MultiAudio.ins.ChooseSongs_SE(seIndex);
        }
    }

   
}
