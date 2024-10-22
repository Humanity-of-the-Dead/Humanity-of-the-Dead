using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyChangeManager : MonoBehaviour
{
    
    [SerializeField] SpriteRenderer headSP;    //“ª
    [SerializeField] SpriteRenderer armSP;     //˜r
    [SerializeField] SpriteRenderer legSP;     //‘«

    [SerializeField] List<Sprite> head;        //“ª
    [SerializeField] List<Sprite> arm;         //˜r
    [SerializeField] List<Sprite> leg;         //‘«

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeHead(int headNumber)
    {
        if (headNumber > head.Count)
        {
            Debug.LogWarning("‚»‚ñ‚È’l‚Í‚È‚¢‚æ");
            return;
        }

        headSP.sprite = head[headNumber];
    }


    void ChangeArm(int armNumber)
    {
        if (armNumber > arm.Count)
        {
            Debug.LogWarning("‚»‚ñ‚È’l‚Í‚È‚¢‚æ");
            return;
        }

        armSP.sprite = arm[armNumber];
    }


    void ChangeLeg(int legNumber)
    {
        if (legNumber > leg.Count)
        {
            Debug.LogWarning("‚»‚ñ‚È’l‚Í‚È‚¢‚æ");
            return;
        }

        legSP.sprite = leg[legNumber];
    }
}
