using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCoolTime : MonoBehaviour
{
    [SerializeField, Header("音源のクールタイム")] private float coolTime;
    [SerializeField, Header("音源を流せるかどうか")] public bool canPlay = true;
    private float realTime;

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
