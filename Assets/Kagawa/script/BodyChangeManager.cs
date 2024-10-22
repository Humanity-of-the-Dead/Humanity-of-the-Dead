using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyChangeManager : MonoBehaviour
{
    
    [SerializeField] SpriteRenderer headSP;    //��
    [SerializeField] SpriteRenderer armSP;     //�r
    [SerializeField] SpriteRenderer legSP;     //��

    [SerializeField] List<Sprite> head;        //��
    [SerializeField] List<Sprite> arm;         //�r
    [SerializeField] List<Sprite> leg;         //��

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeHead(int headNumber)
    {
        if (headNumber > head.Count)
        {
            Debug.LogWarning("����Ȓl�͂Ȃ���");
            return;
        }

        headSP.sprite = head[headNumber];
    }


    void ChangeArm(int armNumber)
    {
        if (armNumber > arm.Count)
        {
            Debug.LogWarning("����Ȓl�͂Ȃ���");
            return;
        }

        armSP.sprite = arm[armNumber];
    }


    void ChangeLeg(int legNumber)
    {
        if (legNumber > leg.Count)
        {
            Debug.LogWarning("����Ȓl�͂Ȃ���");
            return;
        }

        legSP.sprite = leg[legNumber];
    }
}
