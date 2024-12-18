using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum UpperAttack
{
    NORMAL,
    POLICE,
    NURSE,
    BOSS,
    NONE,
}

public enum LowerAttack
{
    NORMAL,
    POLICE,
    NURSE,
    BOSS,
    NONE,
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
    public AnimationData walk;

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

    LowerAttack downAttack;

    //歩きの配列の番号
    int walkIndex;

    //攻撃の配列の番号
    int attackNumber;

    //体の軸
    int shaft;

    //歩いているときに方向を変更されたか数
    bool isWalk;

    // 値を反転にするフラグ
    bool isActive;

    // 攻撃中かどうか
    bool isAttack;

    // 方向フラグ(右 = false)
    bool isDirection;

    // 静止しているか
    bool isStop;

    // タイマー
    float timeWalk;
    
    // 攻撃のタイマー
    float timeAttack;

    private void Start()
    {
        
        walkIndex = 0;
        attackNumber = 0;
        shaft = 0;

        isAttack = false;
        isActive = false;
        isDirection = false;
        isWalk = false;
        isStop = true;
        timeWalk = 0;
        timeAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeWalk -= Time.deltaTime;
        timeAttack -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.D))
        {
            ////静止状態から左向くとき
            //if (time < 0 && isWalk)
            //{
            //    isStop = true;
            //    time = timeMax * 2;
            //}
            shaft = 0;
            isDirection = false;

            if (isStop)
            {
                // 歩く動作をしている時、呼ばせない
                WalkInstance();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ////静止状態から左向くとき
            //if (time < 0 && !isWalk)
            //{
            //    isStop = true;
            //    time = timeMax * 2;
            //}

            shaft = 180;
            isDirection = true;

            if (isStop)
            {
                // 歩く動作をしている時、呼ばせない
                WalkInstance();
            }
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

        //歩きのモーションの向きを変える
        if (Input.GetKey(KeyCode.D))
        {
            shaft = 0;
            isDirection = false;
            isWalk = true;
            PlayerWalk(walkIndex);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            shaft = 180;
            isDirection = true;
            isWalk = true;
            PlayerWalk(walkIndex);
        }
    }

    /// <summary>
    /// 歩くアニメーション
    /// </summary>
    void PlayerWalk(int walkNumber)
    {
        
        // Quaternion.Euler: 回転軸( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, walk.wholeRotation[walkNumber]);

        // 腕のアニメーション
        if (arm == null || walk.armForwardRotation == null)
        {
            Debug.LogWarning("Armのデータが何かしら抜けてる");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, walk.armForwardRotation[walkNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, -walk.armForwardRotation[walkNumber]);
        }

        // 足のアニメーション
        if (leg == null || walk.legForwardRotation == null || walk.legBackRotation == null)
        {
            Debug.LogWarning("Legのデータが何かしら抜けてる");
            return;
        }
        else if (foot == null || walk.footForwardRotation == null || walk.footBackRotation == null) 
        {
            Debug.LogWarning("Footのデータが何かしら抜けてる");
            return;
        }
        //歩いているときに変更された
        else if(isWalk)
        {
            // 歩き始めの場合
            if (!isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, walk.legForwardRotation[walkNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, walk.legBackRotation[walkNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, walk.footForwardRotation[walkNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, walk.footBackRotation[walkNumber]);
                
            }
            //歩き続けている場合
            if (isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, walk.legBackRotation[walkNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, walk.legForwardRotation[walkNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, walk.footBackRotation[walkNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, walk.footForwardRotation[walkNumber]);
            }
        }
        else
        {
            // 歩き始めの場合
            if (!isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, walk.legBackRotation[walkNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, walk.legForwardRotation[walkNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, walk.footBackRotation[walkNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, walk.footForwardRotation[walkNumber]);
            }
            //歩き続けている場合
            if (isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, walk.legForwardRotation[walkNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, walk.legBackRotation[walkNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, walk.footForwardRotation[walkNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, walk.footBackRotation[walkNumber]);
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
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerUpper.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (playerUpper.armForwardRotation == null || playerUpper.armBackRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.armBackRotation[attackNumber]);
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
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.footForwardRotation[attackNumber]);
                }
                break;
            case UpperAttack.POLICE:

                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, policeUpper.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (policeUpper.armForwardRotation == null || policeUpper.armBackRotation == null)
                {
                    Debug.LogWarning("警察Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.armBackRotation[attackNumber]);
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
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.footForwardRotation[attackNumber]);
                }
                break;
            case UpperAttack.NURSE:

                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (nurseUpper.armForwardRotation == null || nurseUpper.armBackRotation == null)
                {
                    Debug.LogWarning("ナースArmのデータが何かしら抜けてる");    
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.armBackRotation[attackNumber]);
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
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.footForwardRotation[attackNumber]);
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
            case LowerAttack.NORMAL:
                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerLower.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (playerLower.armForwardRotation == null || playerLower.armBackRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, playerLower.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, playerLower.armBackRotation[attackNumber]);
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
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, playerLower.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, playerLower.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, playerLower.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, playerLower.footForwardRotation[attackNumber]);
                }
                break;
            case LowerAttack.POLICE:
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, policeLower.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (policeLower.armForwardRotation == null || policeLower.armBackRotation == null)
                {
                    Debug.LogWarning("警察Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, policeLower.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, policeLower.armBackRotation[attackNumber]);
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
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, policeLower.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, policeLower.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, policeLower.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, policeLower.footForwardRotation[attackNumber]);
                }
                break;
            case LowerAttack.NURSE:

                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, nurseLower.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (nurseLower.armForwardRotation == null || nurseLower.armBackRotation == null)
                {
                    Debug.LogWarning("ナースArmのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.armBackRotation[attackNumber]);
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
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.footForwardRotation[attackNumber]);
                }
                break;
        }
    }

    IEnumerator CallWalkWithDelay()
    {
        for (int i = 0; i < walk.armForwardRotation.Length; i++)
        {
            if (!isAttack)
            {
                PlayerWalk(i);
                // indexNumberの値を増やす(配列番号を上げる)
                walkIndex = i;

                yield return new WaitForSeconds(timeMax);
            }
        }

        isWalk = false;

        isActive = !isActive;

        //歩き続ける処理
        if (Input.GetKey(KeyCode.D))
        {
            shaft = 0;
            isDirection = false;
            ChangeArmAnime();
            KeepWalk();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            shaft = 180;
            isDirection = true;
            ChangeArmAnime();
            KeepWalk();
        }
        else
        {
            isStop = true;
        }
    }

    IEnumerator CallPantieWithDelay()
    {
        for (int i = 0; i < playerUpper.armForwardRotation.Length; i++)
        {
            PlayerPantie();

            // indexNumberの値を増やす(配列番号を上げる)
            attackNumber = (attackNumber + 1) % playerUpper.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }

        timeWalk = 0;
        isAttack = false;
    }

    IEnumerator CallKickWithDelay()
    {
        for (int i = 0; i < playerLower.armForwardRotation.Length; i++)
        {
            PlayerKick();

            // indexNumberの値を増やす(配列番号を上げる)
            attackNumber = (attackNumber + 1) % playerLower.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }

        timeWalk = 0;
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
            walk.armForwardRotation = walk.armForwardRotation.Select(value => value > 0 ? -value : value).ToArray();
        }
        else if (!isActive)
        {
            walk.armForwardRotation = walk.armForwardRotation.Select(value => value < 0 ? -value : value).ToArray();
        }
    }

    /// <summary>
    /// 歩くことを開始の関数
    /// </summary>
    void WalkStart()
    {
        timeWalk = timeMax * playerUpper.armForwardRotation.Length;
        StartCoroutine(CallWalkWithDelay());
        MultiAudio.ins.PlaySEByName("SE_hero_action_run");
    }

    /// <summary>
    /// パンチのアニメーション開始するときの関数
    /// </summary>
    void PantieStart()
    {
        AttackWaite();
        StartCoroutine(CallPantieWithDelay());
    }

    /// <summary>
    /// キックのアニメーション開始するときの関数
    /// </summary>
    void KickStart()
    {
        AttackWaite();
        StartCoroutine(CallKickWithDelay());
    }

    /// <summary>
    /// 歩くことの初期化
    /// </summary>
    void WalkInstance()
    {
        if (timeWalk < 0)
        {
            walkIndex = 0;
            attackNumber = 0;
            isActive = false;
            isStop = false;
            WalkStart();
        }
    }

    /// <summary>
    /// 歩くことを継続したとき
    /// </summary>
    void KeepWalk()
    {
        // 連続入力されているか
        if (timeWalk - 0.01 < 0)
        {
            Debug.Log("Keep");
            walkIndex = 0;
            attackNumber = 0;
            ChangeArmAnime();
            WalkStart();
        }
    }

    /// <summary>
    /// 攻撃の初期化
    /// </summary>
    void AttackWaite()
    {
        timeWalk = timeMax * playerUpper.armForwardRotation.Length;
        timeAttack = timeMax * playerLower.armForwardRotation.Length;
        isAttack = true;
        StopCoroutine(CallWalkWithDelay());
        attackNumber = 1;
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
    public void ChangeLowerMove(LowerAttack isName)
    {
        downAttack = isName;
    }

    /// <summary>
    /// 攻撃中かどうかを渡す
    /// </summary>
    /// <returns></returns>
    public bool SetAttack()
    {
        return isAttack;
    }
}

