using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Study : MonoBehaviour
{
    //�I�[�f�B�I�\�[�X
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //�I�[�f�B�I�\�[�X
        audioSource = GetComponent<AudioSource>();

        ////�I�[�f�B�I�\�[�X�𗬂�
        //audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //�N���b�N���ꂽ�Ƃ�
        if (Input.GetMouseButton(0))
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
