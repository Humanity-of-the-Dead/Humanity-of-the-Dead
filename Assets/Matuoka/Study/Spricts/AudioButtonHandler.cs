using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioButtonHandler: MonoBehaviour
{
    //–Â‚ç‚·‰¹‚Ì—v‘f”Ô†
    [Header("‰¹‚Ì—v‘f”Ô†")][SerializeField] int ind=-1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //BGMÄ¶
    public void PlayBGM()
    {
        if (ind >= 0) MultiAudio_Matsuoka.ins.ChooseSongsBGM(ind);
    }

    //SEÄ¶
    public void PlayOneShot()
    {
            //if (ind >= 0) mulAud_Mat.ChooseSongsSE(ind);
    }
}
