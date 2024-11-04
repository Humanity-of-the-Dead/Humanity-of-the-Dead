using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BodyChangeManager : MonoBehaviour
{   
    [SerializeField, Header("頭")] SpriteRenderer headSR;    //頭
    [SerializeField, Header("腕、先に右手")] SpriteRenderer armSR;     //腕
    [SerializeField, Header("足、先に右足")] SpriteRenderer legSR;     //足

    [SerializeField] List<Sprite> head;        //頭
    [SerializeField] List<Sprite> arm;         //腕
    [SerializeField] List<Sprite> leg;         //足

    // 色のリスト（任意の数の色を設定可能）
    public List<Color> colors = new List<Color>();

    // 現在の色のインデックス
    private int currentColorIndex = 0;

    int i = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            ChangeHead(i);
            i++;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeArm(i);
            i++;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeLeg(i);
            i++;
        }
    }

    /// <summary>
    /// 頭の画像変化
    /// プレイヤーか、エネミーに呼んでもらう
    /// </summary>
    /// <param name="headNumber">エネミーの識別番号</param>
    public void ChangeHead(int headNumber)
    {
        if (headNumber > head.Count)
        {
            Debug.LogWarning("そんな値はないよ");
            return;
        }

        headSR.sprite = head[headNumber];
    }


    /// <summary>
    /// 腕の画像変化
    /// プレイヤーか、エネミーに呼んでもらう
    /// </summary>
    /// <param name="armNumber">エネミーの識別番号</param>
    public void ChangeArm(int armNumber)
    {
        if (armNumber > arm.Count)
        {
            Debug.LogWarning("そんな値はないよ");
            return;
        }

        armSR.sprite = arm[armNumber];
    }

    /// <summary>
    /// 足の画像変化
    /// プレイヤーか、エネミーに呼んでもらう
    /// </summary>
    /// <param name="legNumber">エネミーの識別番号</param>
    public void ChangeLeg(int legNumber)
    {
        if (legNumber > leg.Count)
        {
            Debug.LogWarning("そんな値はないよ");
            return;
        }

        legSR.sprite = leg[legNumber];
    }

}
