using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCoolTime : MonoBehaviour
{
    [SerializeField, Header("�����̃N�[���^�C��")] private float coolTime;
    [SerializeField, Header("�����𗬂��邩�ǂ���")] public bool canPlay = true;
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
