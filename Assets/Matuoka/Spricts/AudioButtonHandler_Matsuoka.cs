using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioButtonHandler_Matsuoka : MonoBehaviour
{
    [Header("SE�̗v�f�ԍ�")]
    [SerializeField] int ind;
    [Header("SE�̖��O")]
    [SerializeField] string name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�v�f�ԍ��Ŏw�肵��SE��炷
    public void SoundAnSE_Num()
    {
        MultiAudio_Matsuoka.ins.ChooseSongsSE_Num(ind);
    }

    //���O�Ŏw�肵��SE��炷
}
