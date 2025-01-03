using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCoolTime : MonoBehaviour
{
    [SerializeField] float coolTime;
    public bool canPlay = true;
    [SerializeField] float realTime;

    void Update()
    {
        if (canPlay == false)
        {
            realTime += Time.deltaTime;
            if (realTime > coolTime)
            {
                canPlay = true;
                realTime = 0;
            }
        }
    }
}
