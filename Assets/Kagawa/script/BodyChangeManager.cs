using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyChangeManager : MonoBehaviour
{
    public GameObject test;
    public GameObject[] test2;
    
    [SerializeField] SpriteRenderer headSP;    //頭
    [SerializeField] SpriteRenderer armSP;     //腕
    [SerializeField] SpriteRenderer legSP;     //足

    [SerializeField] List<Sprite> head;        //頭
    [SerializeField] List<Sprite> arm;         //腕
    [SerializeField] List<Sprite> leg;         //足

    // 色のリスト（任意の数の色を設定可能）
    public List<Color> colors = new List<Color>();

    public float[] index;
    public float[] index2;
    public float[] index3;


    int i = 0;
    int j = 0;
    // 現在の色のインデックス
    private int currentColorIndex = 0;

    float time = 0;

    public float timeMax;

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
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

        if(time<0)
        {
            interval();
            time = timeMax;
        }
       
    }

    void ChangeHead(int headNumber)
    {
        if (headNumber > head.Count)
        {
            Debug.LogWarning("そんな値はないよ");
            return;
        }

        headSP.sprite = head[headNumber];
    }


    void ChangeArm(int armNumber)
    {
        if (armNumber > arm.Count)
        {
            Debug.LogWarning("そんな値はないよ");
            return;
        }

        armSP.sprite = arm[armNumber];
    }


    void ChangeLeg(int legNumber)
    {
        if (legNumber > leg.Count)
        {
            Debug.LogWarning("そんな値はないよ");
            return;
        }

        legSP.sprite = leg[legNumber];
    }

    void interval()
    {
        if(!test)
        {
            return;
        }
        else
        {
            Debug.Log("asa");
            SpriteRenderer renderer = test.GetComponent<SpriteRenderer>();

            if (renderer != null)
            {
                // リストの現在のインデックスの色に変更
                renderer.color = colors[currentColorIndex];

                // インデックスを更新（リストの最後まで行ったら最初に戻る）
                currentColorIndex = (currentColorIndex + 1) % colors.Count;
            }
        }

        test2[0].transform.position = new Vector2(index[j], -2);
        test2[1].transform.position = new Vector2(index2[j], -4);
        test2[2].transform.position = new Vector2(-index[j], -2);
        test2[3].transform.position = new Vector2(-index2[j], -4);
        test2[4].transform.rotation = Quaternion.Euler(0, 0, index3[j]);
        test2[5].transform.rotation = Quaternion.Euler(0, 0, -index3[j]);
        j=(j+1)%index.Length;
    }
}
