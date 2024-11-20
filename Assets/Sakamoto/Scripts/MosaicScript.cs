using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //タイマー
    float fTimer;
    //タイマーの最大値
    [SerializeField] float fTimerMax;
    void Start()
    {
        fTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //タイマーの最大値を超えたら非表示にする
        if(fTimer > fTimerMax)
        {
            this.gameObject.SetActive(false);
            fTimer = 0;
        }
        fTimer += Time.deltaTime;
    }
}
