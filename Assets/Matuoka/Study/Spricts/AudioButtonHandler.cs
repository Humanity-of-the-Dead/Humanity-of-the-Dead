using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioButtonHandler: MonoBehaviour
{
    //�炷���̗v�f�ԍ�
    [Header("���̗v�f�ԍ�")][SerializeField] int ind=-1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //BGM�Đ�
    public void PlayBGM()
    {
        if (ind >= 0) MultiAudio_Matsuoka.ins.ChooseSongsBGM(ind);
    }

    //SE�Đ�
    public void PlayOneShot()
    {
            //if (ind >= 0) mulAud_Mat.ChooseSongsSE(ind);
    }
}
