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

    [Header("1コマの間隔の時間")] public float timeMax;

    //配列の番号
    int indexNumber;

    //体の軸
    int shaft;

    // 値を反転にするフラグ
    bool isActive;

    // 向いている方向が右を向いているか
    bool isMirror;

    // 継続して歩くフラグ
    bool isWalk;

    // タイマー
    float time = 0;


    private void Start()
    {
        indexNumber = 0;
        shaft = 0;

        isMirror = true;
        isActive = false;
        isWalk = false;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.D))
        {
            // プレイヤーの向きが左から右に変わったとき
            if (!isMirror)
            {
                shaft = 0;
                MoveMirror();
            }
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            // プレイヤーの向きが右から左に変わったとき
            if (isMirror)
            {
                shaft = 180;
                MoveMirror();
            }
        }


        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
           
            // 連続入力されているか
            if (time - 0.05 < 0)
            {
                isWalk = true;
                //isActive = true;
            }

            // 歩く動作をしている時、呼ばせない
            if (time < 0)
            {
                if (isWalk)
                {
                    // 配列の中の値をマイナスにする
                    KeepWalk();
                    isWalk = false;
                }
                time = timeMax * armRotation.Length;
                StartCoroutine(CallFunctionWithDelay());
            }
        }
    }

    /// <summary>
    /// 歩くアニメーション
    /// </summary>
    void PlayerWalk()
    {
        // Quaternion.Euler: 回転軸( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerRotation[indexNumber]);

        // 腕のアニメーション
        if (arm == null || armRotation == null)
        {
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, 0, armRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, 180, armRotation[indexNumber]);
        }

        // 足のアニメーション
        if (leg == null ||foot == null)
        {
            return;
        }
        else
        {
            // 歩き始めの場合
            if (!isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, 0, legBackRotation[indexNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, 0, legForwardRotation[indexNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, 0,  footBackRotation[indexNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, 0,  footForwardRotation[indexNumber]);
            }
            //歩き続けている場合
            if (isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, 0, legForwardRotation[indexNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, 0, legBackRotation[indexNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, 0, footForwardRotation[indexNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, 0, footBackRotation[indexNumber]);
            }
        }    
    }

    private IEnumerator CallFunctionWithDelay()
    {
        for (int i = 0; i < armRotation.Length; i++)
        {
            PlayerWalk();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % armRotation.Length;

            // 配列の中の値を元に戻す
            if(isActive)
            {
                KeepWalk();
                isActive = false;
            }
            yield return new WaitForSeconds(timeMax); 
        }
    }

    
    /// <summary>
    /// 歩くことを継続した時
    /// 配列の中の値を逆にする
    /// </summary>
    void KeepWalk()
    {

        armRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
    }

    /// <summary>
    /// 向く方向が変わったとき配列の中の値を逆にする
    /// </summary>
    void MoveMirror()
    {
        armRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
        legForwardRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
        legBackRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
        footForwardRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
        footBackRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
    }
}

