using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Study : MonoBehaviour
{
    //オーディオソース
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //オーディオソース
        audioSource = GetComponent<AudioSource>();

        ////オーディオソースを流す
        //audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //クリックされたとき
        if (Input.GetMouseButton(0))
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
