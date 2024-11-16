using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_MultiAudio : MonoBehaviour
{
    [SerializeField] int num;
    [SerializeField] bool isBGM;

    [SerializeField]MultiAudio_Matsuoka MulAud_Mat;

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
            MulAud_Mat.ChooseSongsBGM(num);
        }
        else
        {
            MulAud_Mat.ChooseSongsSE(num);
        }
    }
}
