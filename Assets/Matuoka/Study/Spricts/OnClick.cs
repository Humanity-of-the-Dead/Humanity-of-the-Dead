using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick : MonoBehaviour
{
    //�T�E���h�G�t�F�N�g
    AudioSource audSou;

    // Start is called before the first frame update
    void Start()
    {
        //SE�擾
        audSou = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�{�^���������ꂽ�Ƃ�SE�𗬂�
    public void OnClickPlayOneShot()
    {
        audSou.PlayOneShot(audSou.clip);
    }
}
