using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMoveAnimation : MonoBehaviour
{
    [SerializeField, Header("頭のImage")] SpriteRenderer headSR;
    [SerializeField, Header("腕のImage、先に右手")] SpriteRenderer armSR;
    [SerializeField, Header("足のImage、先に右足")] SpriteRenderer legSR;

    [Header("全身")] public GameObject playerRc;
    [SerializeField, Header("腕の角度、先に右手")] GameObject[] arm;
    [SerializeField, Header("太腿の角度、先に右足")] GameObject[] leg;
    [SerializeField, Header("すねの角度、先に右足")] GameObject[] foot;

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

    // 方向フラグ(右 = false)
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
            // 歩く動作をしている時、呼ばせない
            if (time < 0)
            {
                // プレイヤーの向きが左から右に変わったとき
                isWalk = false;
                shaft = 0;

                isActive = false;
                ChangeArmAnime();
                WalkStart();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // 歩く動作をしている時、呼ばせない
            if (time < 0)
            {
                // プレイヤーの向きが右から左に変わったとき
                isWalk = true;
                shaft = 180;

                isActive = false;
                ChangeArmAnime();
                WalkStart();
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (!isWalk)
            {
                KeepWalk();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (isWalk)
            {
                KeepWalk();
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
            Debug.LogWarning("armのデータが何かしら抜けてる");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft + 180, armRotation[indexNumber]);
        }

        // 足のアニメーション
        if (leg == null || legBackRotation == null || legForwardRotation == null)
        {
            Debug.LogWarning("Legのデータが何かしら抜けてる");
            return;
        }
        else if (foot == null || footBackRotation == null || footForwardRotation == null)
        {
            Debug.LogWarning("footのデータが何かしら抜けてる");
            return;
        }
        else
        {
            // 歩き始めの場合
            if (!isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, legBackRotation[indexNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, legForwardRotation[indexNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, footBackRotation[indexNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, footForwardRotation[indexNumber]);
            }
            //歩き続けている場合
            if (isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, legForwardRotation[indexNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, legBackRotation[indexNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, footForwardRotation[indexNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, footBackRotation[indexNumber]);
            }
        }
    }

    IEnumerator CallFunctionWithDelay()
    {
        for (int i = 0; i < armRotation.Length; i++)
        {
            PlayerWalk();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % armRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
    }


    /// <summary>
    /// 歩くことを継続した時、腕の配列の中の値を逆にする
    /// </summary>
    void ChangeArmAnime()
    {
        //三項演算子(各要素に対して変換操作を行う)
        if (isActive)
        {
            armRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
        }
        else if (!isActive)
        {
            armRotation = armRotation.Select(value => value < 0 ? -value : value).ToArray();
        }
    }

    /// <summary>
    /// 歩き始めの関数
    /// </summary>
    void WalkStart()
    {
        time = timeMax * armRotation.Length;
        StartCoroutine(CallFunctionWithDelay());
    }

    /// <summary>
    /// 歩くことを継続したとき
    /// </summary>
    void KeepWalk()
    {
        // 連続入力されているか
        if (time - 0.05 < 0)
        {
            isActive = !isActive;
            ChangeArmAnime();
            WalkStart();
        }
    }

    /// <summary>
    /// 頭のイメージ
    /// </summary>
    /// <param name="head">画像データ</param>
    public void ChangeHead(BodyPartsData head)
    {

        headSR.sprite = head.sPartSprite;
    }

    /// <summary>
    /// 腕のイメージ
    /// </summary>
    /// <param name="arm">画像データ</param>
    public void ChangeArm(BodyPartsData arm)
    {
        armSR.sprite = arm.sPartSprite;
    }

    /// <summary>
    /// 足のイメージ
    /// </summary>
    /// <param name="leg">画像データ</param>
    public void ChangeLeg(BodyPartsData leg)
    {
        legSR.sprite = leg.sPartSprite;
    }
}