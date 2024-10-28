using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMoveAnimetion : MonoBehaviour
{
    [SerializeField, Header("腕、先に右手")] GameObject[] arm;     
    [SerializeField, Header("太腿、先に右足")] GameObject[] leg;   
    [SerializeField, Header("すね、先に右足")] GameObject[] foot;
    public GameObject[] test3;
    public GameObject[] test4;

    public float[] armRotation;
    public float[] legRotation;
    public float[] index6;
    public float[] index7;
    public float[] index8;


    int i = 0;
    int j = 0;

    bool isActive;
    float time = 0;

    public float timeMax;

    private void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 0)
        {
            interval();
            time = timeMax;
        }

    }

    void interval()
    {
        if (test3 == null /*|| index4 == null*/)
        {
            return;
        }
        else
        {
            //test3[0].transform.rotation = Quaternion.Euler(0, 0, index4[j]);
            //test3[1].transform.rotation = Quaternion.Euler(0, 180, index4[j]);
        }
        if (test4 == null /*|| index5 == null*/)
        {
            return;
        }
        else
        {
            if (!isActive)
            {
                //test4[0].transform.rotation = Quaternion.Euler(0, 0, index5[j]);
                test4[1].transform.rotation = Quaternion.Euler(0, 0, index6[j]);
                test4[2].transform.rotation = Quaternion.Euler(0, 0, index7[j]);
                test4[3].transform.rotation = Quaternion.Euler(0, 0, index8[j]);
            }
            if (isActive)
            {
                test4[0].transform.rotation = Quaternion.Euler(0, 0, index7[j]);
                test4[1].transform.rotation = Quaternion.Euler(0, 0, index8[j]);
                //test4[2].transform.rotation = Quaternion.Euler(0, 0, index5[j]);
                test4[3].transform.rotation = Quaternion.Euler(0, 0, index6[j]);
            }
        }

        

        // jの値を増やす
        //j = (j + 1) % index4.Length;

        // 配列番号が0に戻ったとき配列の値をマイナスに変える
        if (j == 0 && !isActive)
        {
            //index4 = index4.Select(value => value > 0 ? -value : value).ToArray();
            isActive = true;
            return;
        }
        else if (j == 0 && isActive)
        {
            Debug.Log("asa");
            //index4 = index4.Select(value => value < 0 ? -value : value).ToArray();
            isActive = false;
        }
    }
}

