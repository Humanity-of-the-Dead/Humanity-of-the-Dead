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
    #region 山品変更　
    /// <summary>
    /// 変更理由：プレイヤーの最初のイラストのスプライトレンダラーはプレイヤーコントロールが持つべき
    /// </summary>    

    [SerializeField, Header("全身")] private GameObject playerRc;
    //アニメーションの配列はアニメーションスクリプト内で完結したいのでプライベート化してシリアライズフィールドを付けることで各種設定できる
    [SerializeField, Header("腕の角度、先に右手")] private GameObject[] arm;
    [SerializeField, Header("太腿の角度、先に右足")] private GameObject[] leg;
    [SerializeField, Header("すねの角度、先に右足")] private GameObject[] foot;
    //アニメーションの間隔はアニメーションスクリプト内で完結したいのでプライベート化してシリアライズフィールドを付けることで設定できる
    [SerializeField, Header("1コマの間隔の時間")] private float timeMax;
    [SerializeField] private AnimationDatas animationDatas;
    private UpperAttack upperAttack;

    private LowerAttack downAttack;

    //歩きの配列の番号
    private int walkIndex = 0;


    //攻撃の配列の番号
    private int attackNumber = 0;


    //体の軸
    private int shaft = 0;

    //歩いているときに方向を変更されたか数
    private bool isWalk = false;


    // 値を反転にするフラグ
    private bool isActive = false;


    // 攻撃中かどうか
    private bool isAttack = false;


    // 静止しているか
    private bool isStop = true;


    // タイマー
    public float timeWalk = 0;

    //プレイヤーコントロールで呼ぶためパブリック追記
    // 攻撃のタイマー
    public float timeAttack = 0;
    #endregion

    private void Start()
    {

        walkIndex = 0;
        attackNumber = 0;
        shaft = 0;

        isAttack = false;
        isActive = false;
        isWalk = false;
        isStop = true;
        timeWalk = 0;
        timeAttack = 0;
    }



    public void HandleWalk(int direction)
    {
        shaft = direction;

        if (isStop)
        {
            WalkInstance();
        }
        if (!isWalk)
        {
            isWalk = true;
            ChangeArmAnime();
            KeepWalk();
        }
        else if (isWalk && !isAttack)
        {
            PlayerWalk();
        }
    }


    /// <summary>
    /// 歩くアニメーション
    /// </summary>
    void PlayerWalk()
    {

        // Quaternion.Euler: 回転軸( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, animationDatas.walk.wholeRotation[walkIndex]);

        // 腕のアニメーション
        if (arm == null || animationDatas.walk.armForwardRotation == null)
        {
            Debug.LogWarning("Armのデータが何かしら抜けてる");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.walk.armForwardRotation[walkIndex]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, -animationDatas.walk.armForwardRotation[walkIndex]);
        }

        // 足のアニメーション
        if (leg == null || animationDatas.walk.legForwardRotation == null || animationDatas.walk.legBackRotation == null)
        {
            Debug.LogWarning("Legのデータが何かしら抜けてる");
            return;
        }
        else if (foot == null || animationDatas.walk.footForwardRotation == null || animationDatas.walk.footBackRotation == null)
        {
            Debug.LogWarning("Footのデータが何かしら抜けてる");
            return;
        }
        else
        {
            // 歩き始めの場合
            if (!isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.walk.legBackRotation[walkIndex]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.walk.legForwardRotation[walkIndex]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.walk.footBackRotation[walkIndex]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.walk.footForwardRotation[walkIndex]);
            }
            //歩き続けている場合
            if (isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.walk.legForwardRotation[walkIndex]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.walk.legBackRotation[walkIndex]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.walk.footForwardRotation[walkIndex]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.walk.footBackRotation[walkIndex]);
            }
        }
    }

    /// <summary>
    /// 上半身のモーション
    /// </summary>
    void PlayerPantie()
    {
        switch (upperAttack)
        {
            case UpperAttack.NORMAL:
                // Quaternion.Euler: 回転軸( x, y, z)
                transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerUpper.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDatas.playerUpper.armForwardRotation == null || animationDatas.playerUpper.armBackRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerUpper.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerUpper.armBackRotation[attackNumber]);
                }

                // 足のアニメーション
                if (animationDatas.playerUpper.legForwardRotation == null || animationDatas.playerUpper.legBackRotation == null)
                {
                    Debug.LogWarning("Legのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDatas.playerUpper.footBackRotation == null || animationDatas.playerUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerUpper.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerUpper.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerUpper.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerUpper.footForwardRotation[attackNumber]);
                }
                break;
            case UpperAttack.POLICE:

                // Quaternion.Euler: 回転軸( x, y, z)
                transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeUpper.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDatas.policeUpper.armForwardRotation == null || animationDatas.policeUpper.armBackRotation == null)
                {
                    Debug.LogWarning("警察Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeUpper.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeUpper.armBackRotation[attackNumber]);
                }

                // 足のアニメーション
                if (animationDatas.policeUpper.legForwardRotation == null || animationDatas.policeUpper.legBackRotation == null)
                {
                    Debug.LogWarning("警察Legのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDatas.policeUpper.footBackRotation == null || animationDatas.policeUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("警察Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeUpper.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeUpper.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeUpper.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeUpper.footForwardRotation[attackNumber]);
                }
                break;
            case UpperAttack.NURSE:

                // Quaternion.Euler: 回転軸( x, y, z)
                transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseUpper.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDatas.nurseUpper.armForwardRotation == null || animationDatas.nurseUpper.armBackRotation == null)
                {
                    Debug.LogWarning("ナースArmのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseUpper.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseUpper.armBackRotation[attackNumber]);
                }

                // 足のアニメーション
                if (animationDatas.nurseUpper.legForwardRotation == null || animationDatas.nurseUpper.legBackRotation == null)
                {
                    Debug.LogWarning("ナースLegのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDatas.nurseUpper.footBackRotation == null || animationDatas.nurseUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("ナースFootのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseUpper.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseUpper.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseUpper.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseUpper.footForwardRotation[attackNumber]);
                }
                break;
        }
    }

    /// <summary>
    /// キックのアニメーション
    /// </summary>
    void PlayerKick()
    {
        switch (downAttack)
        {
            case LowerAttack.NORMAL:
                // Quaternion.Euler: 回転軸( x, y, z)
                transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerLower.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDatas.playerLower.armForwardRotation == null || animationDatas.playerLower.armBackRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerLower.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerLower.armBackRotation[attackNumber]);
                }

                // 足のアニメーション
                if (animationDatas.playerLower.legBackRotation == null || animationDatas.playerLower.legForwardRotation == null)
                {
                    Debug.LogWarning("Legのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDatas.playerLower.footBackRotation == null || animationDatas.playerLower.footForwardRotation == null)
                {
                    Debug.LogWarning("Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerLower.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerLower.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerLower.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.playerLower.footForwardRotation[attackNumber]);
                }
                break;
            case LowerAttack.POLICE:
                transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeLower.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDatas.policeLower.armForwardRotation == null || animationDatas.policeLower.armBackRotation == null)
                {
                    Debug.LogWarning("警察Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeLower.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeLower.armBackRotation[attackNumber]);
                }

                // 足のアニメーション
                if (animationDatas.policeLower.legForwardRotation == null || animationDatas.policeLower.legBackRotation == null)
                {
                    Debug.LogWarning("警察Legのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDatas.policeLower.footBackRotation == null || animationDatas.policeLower.footForwardRotation == null)
                {
                    Debug.LogWarning("警察Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeLower.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeLower.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeLower.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.policeLower.footForwardRotation[attackNumber]);
                }
                break;
            case LowerAttack.NURSE:

                // Quaternion.Euler: 回転軸( x, y, z)
                transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseLower.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDatas.nurseLower.armForwardRotation == null || animationDatas.nurseLower.armBackRotation == null)
                {
                    Debug.LogWarning("ナースArmのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseLower.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseLower.armBackRotation[attackNumber]);
                }

                // 足のアニメーション
                if (animationDatas.nurseLower.legForwardRotation == null || animationDatas.nurseLower.legBackRotation == null)
                {
                    Debug.LogWarning("ナースLegのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDatas.nurseLower.footBackRotation == null || animationDatas.nurseLower.footForwardRotation == null)
                {
                    Debug.LogWarning("ナースFootのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseLower.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseLower.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseLower.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDatas.nurseLower.footForwardRotation[attackNumber]);
                }
                break;
        }
    }

    IEnumerator CallWalkWithDelay()
    {
        for (int i = 0; i < animationDatas.walk.armForwardRotation.Length; i++)
        {
            if (!isAttack)
            {
                // indexNumberの値を増やす(配列番号を上げる)
                walkIndex = i;
                PlayerWalk();

                yield return new WaitForSeconds(timeMax);
            }
        }

        isWalk = false;
        isActive = !isActive;
    }

    IEnumerator CallPantieWithDelay()
    {
        for (int i = 0; i < animationDatas.playerUpper.armForwardRotation.Length - 1; i++)
        {
            PlayerPantie();

            // indexNumberの値を増やす(配列番号を上げる)
            attackNumber = (attackNumber + 1) % animationDatas.playerUpper.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }

        timeWalk = 0;
        isAttack = false;
        isWalk = false;
        isStop = true;
    }

    IEnumerator CallKickWithDelay()
    {
        for (int i = 0; i < animationDatas.playerLower.armForwardRotation.Length - 1; i++)
        {
            PlayerKick();

            // indexNumberの値を増やす(配列番号を上げる)
            attackNumber = (attackNumber + 1) % animationDatas.playerLower.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }

        timeWalk = 0;
        isWalk = false;
        isStop = true;
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
            animationDatas.walk.armForwardRotation = animationDatas.walk.armForwardRotation.Select(value => value > 0 ? -value : value).ToArray();
        }
        else if (!isActive)
        {
            animationDatas.walk.armForwardRotation = animationDatas.walk.armForwardRotation.Select(value => value < 0 ? -value : value).ToArray();
        }
    }

    /// <summary>
    /// 歩くことを開始の関数
    /// </summary>
    void WalkStart()
    {
        timeWalk = timeMax * animationDatas.playerUpper.armForwardRotation.Length;
        StartCoroutine(CallWalkWithDelay());
        MultiAudio.ins.PlaySEByName("SE_hero_action_run");
    }

    /// <summary>
    /// パンチのアニメーション開始するときの関数
    /// </summary>
    public void PantieStart()
    {
        AttackWaite();
        StartCoroutine(CallPantieWithDelay());
    }

    /// <summary>
    /// キックのアニメーション開始するときの関数
    /// </summary>
    public void KickStart()
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
        #region 山品変更

        if (timeWalk < 0.01f + Mathf.Epsilon)
        {
            #endregion
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
        timeWalk = timeMax * (animationDatas.playerUpper.armForwardRotation.Length - 1);
        timeAttack = timeMax * (animationDatas.playerLower.armForwardRotation.Length - 1);
        isAttack = true;
        isWalk = false;
        StopCoroutine(CallWalkWithDelay());
        walkIndex = 0;
        attackNumber = 1;
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
    [System.Serializable]//クラスを
    private class AnimationDatas
    {
        [SerializeField, Header("---歩きのアニメーション---")] public AnimationData walk;

        [SerializeField, Header("---デフォルトパンチのアニメーション---")] public AnimationData playerUpper;

        [SerializeField, Header("---デフォルトキックのアニメーション---")] public AnimationData playerLower;

        [SerializeField, Header("---警察拳銃のアニメーション---")] public AnimationData policeUpper;

        [SerializeField, Header("---警察下半身のアニメーション---")] public AnimationData policeLower;

        [SerializeField, Header("---ナースパンチのアニメーション---")] public AnimationData nurseUpper;

        [SerializeField, Header("---ナースキックのアニメーション---")] public AnimationData nurseLower;

    }

}
