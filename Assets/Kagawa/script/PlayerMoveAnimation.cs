using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMoveAnimation : MonoBehaviour
{
    [Header("全身")]public GameObject playerRc;
    [SerializeField, Header("腕、先に右手")] public GameObject[] arm;     
    [SerializeField, Header("太腿、先に右足")] public GameObject[] leg;   
    [SerializeField, Header("すね、先に右足")] public GameObject[] foot;

    [Header("全身の角度")] public float[] playerRotation;
    [Header("腕の角度")] public float[] armRotation;
    [Header("太ももの前方の角度")] public float[] legForwardRotation;
    [Header("足の前方の角度")] public float[] footForwardRotation;
    [Header("太ももの後方の角度")] public float[] legBackRotation;
    [Header("足の後方の角度")] public float[] footBackRotation;
    [Header("歩きの継続時間")] public float timeWalk;


    int i = 0;
    int j = 0;

    bool isActive;
    bool isWalk;
    float time = 0;

    [Header("時間")] public float timeMax;

    private void Start()
    {
        isActive = false;
        isWalk = false;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;        
        if (Input.GetKey(KeyCode.D))
        {
            if (time < 0)
            {
                if (!isWalk)
                {
                    time = timeMax * armRotation.Length;
                    StartCoroutine(CallFunctionWithDelay());
                }
            }       
        }
    }

    void interval()
    {

        playerRc.transform.rotation = Quaternion.Euler(0, 0, playerRotation[j]);
        if (arm == null || armRotation == null)
        {
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, 0, armRotation[j]);
            arm[1].transform.rotation = Quaternion.Euler(0, 180, armRotation[j]);
        }


        if (leg == null ||foot == null)
        {
            Debug.Log("asa");
            return;
        }
        else
        {
            if (!isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, 0, legBackRotation[j]);
                leg[1].transform.rotation = Quaternion.Euler(0, 0, legForwardRotation[j]);
                foot[0].transform.rotation = Quaternion.Euler(0, 0,  footBackRotation[j]);
                foot[1].transform.rotation = Quaternion.Euler(0, 0,  footForwardRotation[j]);
            }
            if (isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, 0, legForwardRotation[j]);
                leg[1].transform.rotation = Quaternion.Euler(0, 0, legBackRotation[j]);
                foot[0].transform.rotation = Quaternion.Euler(0, 0, footForwardRotation[j]);
                foot[1].transform.rotation = Quaternion.Euler(0, 0, footBackRotation[j]);
            }
        }    
    }

    private IEnumerator CallFunctionWithDelay()
    {
        for (int i = 0; i < armRotation.Length; i++)
        {
            interval();
            //// jの値を増やす
            j = (j + 1) % armRotation.Length;
            // 配列番号が0に戻ったとき配列の値をマイナスに変える
            if (j == 0 && !isActive)
            {
                armRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
                isActive = true;
            }
            else if (j == 0 && isActive)
            {
                armRotation = armRotation.Select(value => value < 0 ? -value : value).ToArray();
                isActive = false;
            }
            yield return new WaitForSeconds(timeMax); 
        }
    }
}

