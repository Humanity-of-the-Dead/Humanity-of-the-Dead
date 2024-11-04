using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecibelTransformation : MonoBehaviour
{
    ////�I�[�f�B�I�~�L�T�[
    //[SerializeField] AudioMixer audMix;

    //�{�����[��0~1
    [SerializeField]float vol;
    //�f�V�x��(�{�����[��-80~0)
    float dec;

    // Start is called before the first frame update
    void Start()
    {
        //�{�����[�����f�V�x���ɕϊ�
        dec = Mathf.Clamp(Mathf.Log10(vol) * 20f,-80f,0f);

        Debug.Log(dec);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
