using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UpperAttack
{
    NORMAL,
    POLICE,
    NURSE,
}

public enum DownAttack
{
    NORMAL,
    POLICE,
    NURSE,
}

public class PlayerMoveAnimation : MonoBehaviour
{
    [SerializeField, Header("頭のImage")] SpriteRenderer headSR;
    [SerializeField, Header("体ののImage")] SpriteRenderer bodySR;
    [SerializeField, Header("右腕のImage")] SpriteRenderer armRightSR;
    [SerializeField, Header("左腕のImage")] SpriteRenderer armLeftSR;
    [SerializeField, Header("右手首のImage")] SpriteRenderer handRightSR;
    [SerializeField, Header("左手首のImage")] SpriteRenderer handLeftSR;
    [SerializeField, Header("腰のImage")] SpriteRenderer waistSR;
    [SerializeField, Header("右太腿のImage")] SpriteRenderer legRightSR;
    [SerializeField, Header("左太腿のImage")] SpriteRenderer legLeftSR;
    [SerializeField, Header("右足のImage")] SpriteRenderer footRightSR;
    [SerializeField, Header("左足のImage")] SpriteRenderer footLeftSR;

    [Header("全身")] public GameObject playerRc;
    [SerializeField, Header("腕の角度、先に右手")]  GameObject[] arm;
    [SerializeField, Header("太腿の角度、先に右足")]  GameObject[] leg;
    [SerializeField, Header("すねの角度、先に右足")]  GameObject[] foot;

    [Header("1コマの間隔の時間")] public float timeMax;

    [Header("---歩きのアニメーション---")]
    [Header("全身の角度")] public float[] playerWalkRotation;
    [Header("腕の角度")] public float[] armWalkRotation;
    [Header("太ももの前方角度")] public float[] legWalkForwardRotation;
    [Header("足の前方角度")] public float[] footWalkForwardRotation;
    [Header("太ももの後方角度")] public float[] legWalkBackRotation;
    [Header("足の後方角度")] public float[] footWalkBackRotation;
    [Header("歩きの継続時間")] public float timeWalk;

    [Header("---デフォルトパンチのアニメーション---")]
    public AnimationData playerUpper;

    [Header("---デフォルトキックのアニメーション---")]
    public AnimationData playerLower;

    [Header("---警察拳銃のアニメーション---")]
    public AnimationData policeUpper;

    [Header("---警察下半身のアニメーション---")]
    public AnimationData policeLower;

    [Header("---ナースパンチのアニメーション---")]
    public AnimationData nurseUpper;

    [Header("---ナースキックのアニメーション---")]
    public AnimationData nurseLower;

    UpperAttack upperAttack;

    DownAttack downAttack;

    //配列の番号
    int indexNumber;

    //体の軸
    int shaft;

    //歩くアニメーションの角度の数
    int walkLength;

    // 値を反転にするフラグ
    bool isActive;

    // 攻撃中かどうか
    bool isAttack;

    // 方向フラグ(右 = false)
    bool isWalk;

    // 静止しているか
    bool isStop;

    // タイマー
    float time;
    
    // タイマー
    float timeAttack;

    private void Start()
    {
        indexNumber = 0;
        shaft = 0;

        isAttack = false;
        isActive = false;
        isWalk = false;
        isStop = false;
        walkLength = armWalkRotation.Length - 1;
        time = 0;
        timeAttack = 0;

        upperAttack = UpperAttack.NORMAL;
        downAttack = DownAttack.NORMAL; 
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeAttack -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.D))
        {
            shaft = 0;

            //静止状態から左向くとき
            if (time < 0 && isWalk)
            {
                isStop = true;
                time = timeMax * 2;
                Upright();
            }

            // プレイヤーの向きが左から右に変わったとき
            isWalk = false;
           

            // 歩く動作をしている時、呼ばせない
            WalkInstance();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            shaft = 180;

            //静止状態から左向くとき
            if (time < 0 && !isWalk)
            {
                isStop = true;
                time = timeMax * 2;
                Upright();
            }

            // プレイヤーの向きが右から左に変わったとき
            isWalk = true;
            
            // 歩く動作をしている時、呼ばせない
            WalkInstance();
        }

        if (timeAttack < 0)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                PantieStart();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                KickStart();
            }
        }
        

        if (Input.GetKey(KeyCode.D))
        {
            if (!isWalk)
            {
                if(isStop)
                {
                    isStop = false;
                    WalkInstance();
                }
                else
                {
                    KeepWalk();
                }
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (isWalk)
            {
                if (isStop)
                {
                    isStop = false;
                    WalkInstance();
                }
                else
                {
                    KeepWalk();
                }
            }
        }
    }

    /// <summary>
    /// 歩くアニメーション
    /// </summary>
    void PlayerWalk()
    {
        // Quaternion.Euler: 回転軸( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerWalkRotation[indexNumber]);

        // 腕のアニメーション
        if (arm == null || armWalkRotation == null)
        {
            Debug.LogWarning("Armのデータが何かしら抜けてる");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, -armWalkRotation[indexNumber]);
        }

        // 足のアニメーション
        if (leg == null || legWalkBackRotation == null || legWalkForwardRotation == null)
        {
            Debug.LogWarning("Legのデータが何かしら抜けてる");
            return;
        }
        else if (foot == null || footWalkBackRotation == null || footWalkForwardRotation == null) 
        {
            Debug.LogWarning("Footのデータが何かしら抜けてる");
            return;
        }
        else
        {
            // 歩き始めの場合
            if (!isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, legWalkBackRotation[indexNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, legWalkForwardRotation[indexNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, footWalkBackRotation[indexNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, footWalkForwardRotation[indexNumber]);
            }
            //歩き続けている場合
            if (isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, legWalkForwardRotation[indexNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, legWalkBackRotation[indexNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, footWalkForwardRotation[indexNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, footWalkBackRotation[indexNumber]);
            }
        }
    }

    /// <summary>
    /// 上半身のモーション
    /// </summary>
    void PlayerPantie()
    {
        switch(upperAttack)
        {
            case UpperAttack.NORMAL:
                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerUpper.wholeRotation[indexNumber]);

                // 腕のアニメーション
                if (playerUpper.armForwardRotation == null || playerUpper.armBackRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.armBackRotation[indexNumber]);
                }

                // 足のアニメーション
                if (playerUpper.legForwardRotation == null || playerUpper.legBackRotation == null)
                {
                    Debug.LogWarning("Legのデータが何かしら抜けてる");
                    return;
                }
                else if (playerUpper.footBackRotation == null || playerUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.footForwardRotation[indexNumber]);
                }
                break;
            case UpperAttack.POLICE:

                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, policeUpper.wholeRotation[indexNumber]);

                // 腕のアニメーション
                if (policeUpper.armForwardRotation == null || policeUpper.armBackRotation == null)
                {
                    Debug.LogWarning("警察Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.armBackRotation[indexNumber]);
                }

                // 足のアニメーション
                if (policeUpper.legForwardRotation == null || policeUpper.legBackRotation == null)
                {
                    Debug.LogWarning("警察Legのデータが何かしら抜けてる");
                    return;
                }
                else if (policeUpper.footBackRotation == null || policeUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("警察Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.footForwardRotation[indexNumber]);
                }
                break;
            case UpperAttack.NURSE:

                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.wholeRotation[indexNumber]);

                // 腕のアニメーション
                if (nurseUpper.armForwardRotation == null || nurseUpper.armBackRotation == null)
                {
                    Debug.LogWarning("ナースArmのデータが何かしら抜けてる");    
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.armBackRotation[indexNumber]);
                }

                // 足のアニメーション
                if (nurseUpper.legForwardRotation == null || nurseUpper.legBackRotation == null)
                {
                    Debug.LogWarning("ナースLegのデータが何かしら抜けてる");
                    return;
                }
                else if (nurseUpper.footBackRotation == null || nurseUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("ナースFootのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.footForwardRotation[indexNumber]);
                }
                break;
        }
    }

    /// <summary>
    /// キックのアニメーション
    /// </summary>
    void PlayerKick()
    {
        switch(downAttack)
        {
            case DownAttack.NORMAL:
                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerLower.wholeRotation[indexNumber]);

                // 腕のアニメーション
                if (playerLower.armForwardRotation == null || playerLower.armBackRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, playerLower.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, playerLower.armBackRotation[indexNumber]);
                }

                // 足のアニメーション
                if (playerLower.legBackRotation == null || playerLower.legForwardRotation == null)
                {
                    Debug.LogWarning("Legのデータが何かしら抜けてる");
                    return;
                }
                else if (playerLower.footBackRotation == null || playerLower.footForwardRotation == null)
                {
                    Debug.LogWarning("Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, playerLower.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, playerLower.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, playerLower.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, playerLower.footForwardRotation[indexNumber]);
                }
                break;
            case DownAttack.POLICE:
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, policeLower.wholeRotation[indexNumber]);

                // 腕のアニメーション
                if (policeLower.armForwardRotation == null || policeLower.armBackRotation == null)
                {
                    Debug.LogWarning("警察Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, policeLower.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, policeLower.armBackRotation[indexNumber]);
                }

                // 足のアニメーション
                if (policeLower.legForwardRotation == null || policeLower.legBackRotation == null)
                {
                    Debug.LogWarning("警察Legのデータが何かしら抜けてる");
                    return;
                }
                else if (policeLower.footBackRotation == null || policeLower.footForwardRotation == null)
                {
                    Debug.LogWarning("警察Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {   
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, policeLower.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, policeLower.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, policeLower.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, policeLower.footForwardRotation[indexNumber]);
                }
                break;
            case DownAttack.NURSE:

                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, nurseLower.wholeRotation[indexNumber]);

                // 腕のアニメーション
                if (nurseLower.armForwardRotation == null || nurseLower.armBackRotation == null)
                {
                    Debug.LogWarning("ナースArmのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.armBackRotation[indexNumber]);
                }

                // 足のアニメーション
                if (nurseLower.legForwardRotation == null || nurseLower.legBackRotation == null)
                {
                    Debug.LogWarning("ナースLegのデータが何かしら抜けてる");
                    return;
                }
                else if (nurseLower.footBackRotation == null || nurseLower.footForwardRotation == null)
                {
                    Debug.LogWarning("ナースFootのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.footForwardRotation[indexNumber]);
                }
                break;
        }
    }

    IEnumerator CallWalkWithDelay()
    {
        for (int i = 0; i < armWalkRotation.Length; i++)
        {
            if (!isAttack)
            {
                PlayerWalk();

                // indexNumberの値を増やす(配列番号を上げる)
                indexNumber = (indexNumber + 1) % armWalkRotation.Length;
                yield return new WaitForSeconds(timeMax);
            }
        }
    }

    IEnumerator CallPantieWithDelay()
    {
        for (int i = 0; i < playerUpper.armForwardRotation.Length; i++)
        {
            PlayerPantie();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % playerUpper.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }

        time = 0;
        isAttack = false;
    }

    IEnumerator CallKickWithDelay()
    {
        for (int i = 0; i < playerLower.armForwardRotation.Length; i++)
        {
            PlayerKick();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % playerLower.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }

        time = 0;
        isAttack = false;
    }

    /// <summary>
    /// 歩くことを継続した時、腕の配列の中の値を逆にする
    /// </summary>
    void ChangeArmAnime()
    {
        //三項演算子(各要素に対して変換操作を行う)
        if (isActive)
        {
            armWalkRotation = armWalkRotation.Select(value => value > 0 ? -value : value).ToArray();
        }
        else if (!isActive)
        {
            armWalkRotation = armWalkRotation.Select(value => value < 0 ? -value : value).ToArray();
        }
    }

    /// <summary>
    /// 歩くことを開始の関数
    /// </summary>
    void WalkStart()
    {
        time = timeMax * armWalkRotation.Length;
        StartCoroutine(CallWalkWithDelay());
    }

    /// <summary>
    /// パンチのアニメーション開始するときの関数
    /// </summary>
    void PantieStart()
    {
        AttackWaite();
        time = timeMax * playerUpper.armForwardRotation.Length;
        StartCoroutine(CallPantieWithDelay());
    }

    /// <summary>
    /// キックのアニメーション開始するときの関数
    /// </summary>
    void KickStart()
    {
        AttackWaite();
        time = timeMax * playerLower.armForwardRotation.Length;
        StartCoroutine(CallKickWithDelay());
    }

    /// <summary>
    /// 歩くことの初期化
    /// </summary>
    void WalkInstance()
    {
        if (time < 0)
        {
            indexNumber = 0;
            isActive = false;
            ChangeArmAnime();
            WalkStart();
        }
    }

    /// <summary>
    /// 歩くことを継続したとき
    /// </summary>
    void KeepWalk()
    {
        // 連続入力されているか
        if (time - 0.05 < 0)
        {
            indexNumber = 0;
            isActive = !isActive;
            ChangeArmAnime();
            WalkStart();
        }
    }

    /// <summary>
    /// 攻撃の初期化
    /// </summary>
    void AttackWaite()
    {
        timeAttack = timeMax * playerLower.armForwardRotation.Length;
        isAttack = true;
        StopCoroutine(CallWalkWithDelay());
        Upright();
        indexNumber = 1;
    }

    /// <summary>
    /// 直立する
    /// </summary>
    void Upright()
    {
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerWalkRotation[walkLength]);
        arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[walkLength]);
        arm[1].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[walkLength]);
        leg[0].transform.rotation = Quaternion.Euler(0, shaft, legWalkBackRotation[walkLength]);
        leg[1].transform.rotation = Quaternion.Euler(0, shaft, legWalkForwardRotation[walkLength]);
        foot[0].transform.rotation = Quaternion.Euler(0, shaft, footWalkBackRotation[walkLength]);
        foot[1].transform.rotation = Quaternion.Euler(0, shaft, footWalkForwardRotation[walkLength]);
    }

    /// <summary>
    /// 上半身のイメージ
    /// </summary>
    /// <param name="upperBody">画像データ集合体</param>
    public void ChangeUpperBody(BodyPartsData upperBody)
    {
        bodySR.sprite = upperBody.spBody;
        armRightSR.sprite = upperBody.spRightArm;
        armLeftSR.sprite = upperBody.spLeftArm;
        handRightSR.sprite = upperBody.spRightHand;
        handLeftSR.sprite = upperBody.spLeftHand;
    }

    /// <summary>
    /// 下半身のイメージ
    /// </summary>
    /// <param name="underBody">画像データ集合体</param>
    public void ChangeUnderBody(BodyPartsData underBody)
    {
        waistSR.sprite = underBody.spWaist;
        footRightSR.sprite = underBody.spRightFoot;
        footLeftSR.sprite = underBody.spLeftFoot;
        legRightSR.sprite = underBody.spRightLeg;
        legLeftSR.sprite = underBody.spLeftLeg;
    }

    /// <summary>
    /// 上半身の攻撃の変化
    /// </summary>
    /// <param name="isName">移植する物体</param>
    public void ChangeUpperMove(UpperAttack isName)
    {
        upperAttack = isName;
    }

    /// <summary>
    /// 下半身の攻撃の変化
    /// </summary>
    /// <param name="isName">移植する物体</param>
    public void ChangeDownMove(DownAttack isName)
    {
        downAttack = isName;
    }
}

