using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioButtonHandler_Matsuoka : MonoBehaviour
{
    [Header("SEの要素番号")]
    [SerializeField] int ind;
    [Header("SEの名前")]
    [SerializeField] string name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //要素番号で指定してSEを鳴らす
    public void SoundAnSE_Num()
    {
        MultiAudio_Matsuoka.ins.ChooseSongsSE_Num(ind);
    }

    //名前で指定してSEを鳴らす
}
