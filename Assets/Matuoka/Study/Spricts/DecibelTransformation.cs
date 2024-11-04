using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecibelTransformation : MonoBehaviour
{
    ////オーディオミキサー
    //[SerializeField] AudioMixer audMix;

    //ボリューム0~1
    [SerializeField]float vol;
    //デシベル(ボリューム-80~0)
    float dec;

    // Start is called before the first frame update
    void Start()
    {
        //ボリュームをデシベルに変換
        dec = Mathf.Clamp(Mathf.Log10(vol) * 20f,-80f,0f);

        Debug.Log(dec);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
