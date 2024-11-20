using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_MultiAudio : MonoBehaviour
{
    [SerializeField] int num;
    [SerializeField] bool isBGM;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {
        if (isBGM) {
            MultiAudio_Matsuoka.ins.ChooseSongsBGM(num);
        }
        else
        {
            MultiAudio_Matsuoka.ins.ChooseSongsSE(num);
        }
    }
}
