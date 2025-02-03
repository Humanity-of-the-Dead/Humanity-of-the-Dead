using System;
using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// 敵の種類
/// </summary>
public enum Status
{
    Zombie,
    Boss,
}

/// <summary>
/// デバック用
/// </summary>
public enum DebugMove
{
    None,
    Walk,
    Kick,
    Pantie
}

public class EnemyMoveAnimation : MonoBehaviour
{
    [SerializeField, Header("敵の種類")]
    private Status status;

    [SerializeField, Header("デバッグ用(通常、None)")] private DebugMove debugMoves;
    [SerializeField, Header("デバッグ用: index指定でアニメーションの1コマ分だけ表示\n(アニメーション閲覧時は-1)")] private int indexNumberForDebug = -1;

    [SerializeField, Header("全身")] public GameObject playerRc;
    [SerializeField, Header("腕の角度、先に右手")] private GameObject[] arm;
    [SerializeField, Header("手首の角度、先に右手")] private GameObject[] hand;
    [SerializeField, Header("太腿の角度、先に右足")] private GameObject[] leg;
    [SerializeField, Header("すねの角度、先に右足")] private GameObject[] foot;

    [SerializeField, Header("1コマの間隔の時間(歩き)")] private float timeMax;

    [SerializeField, Header("1コマの間隔の時間(攻撃)")] private float timeAttackMax;

    [SerializeField, Header("---歩きのアニメーション---")] private AnimationData walk;

    [SerializeField, Header("---上半身のアニメーション---")] private AnimationData upper;

    [SerializeField, Header("---下半身のアニメーション---")] private AnimationData lower;

    //配列の番号
    private int indexNumber;

    //体の軸
    private int shaft;

    //歩くアニメーションの角度の数
    private int walkLength;
    // 値を反転にするフラグ
    private bool isActive;

    // 向いている方向が右を向いているか
    private bool isMirror;

    // 攻撃中かどうか
    private bool isAttack;

    // 方向フラグ(右 = false)
    //private bool isDirection;

    //// 静止しているか
    //private bool isStop;

    // タイマー
    private float time;

    // 攻撃のタイマー
    private float timeAttack;
    public const int SHAFT_DIRECTION_RIGHT = 0;
    public const int SHAFT_DIRECTION_LEFT = 180;


    private void Start()
    {
        indexNumber = 0;
        shaft = SHAFT_DIRECTION_LEFT;
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, walk.wholeRotation[indexNumber]);


        isMirror = true;
        isActive = false;
        isAttack = false;

        walkLength = walk.armForwardRotation.Length - 1;
        time = 0;
        timeAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeAttack -= Time.deltaTime;

        SetDebugmodeIfDebugMove();
    }
    /// <summary>
    /// 歩くアニメーション
    /// </summary>
    public void PlayerWalk()
    {
        switch (status)
        {
            case Status.Zombie:
                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, walk.wholeRotation[indexNumber]);

                // 腕のアニメーション
                if (walk.armForwardRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, walk.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, walk.armForwardRotation[indexNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, walk.armForwardRotation[indexNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, walk.armForwardRotation[indexNumber]);
                }

                // 足のアニメーション
                if (walk.legForwardRotation == null || walk.legBackRotation == null)
                {
                    Debug.LogWarning("Legのデータが何かしら抜けてる");
                    return;
                }
                else if (walk.footBackRotation == null || walk.footForwardRotation == null)
                {
                    Debug.LogWarning("Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    if (isActive)
                    {
                        leg[0].transform.rotation = Quaternion.Euler(0, shaft, walk.legBackRotation[indexNumber]);
                        leg[1].transform.rotation = Quaternion.Euler(0, shaft, walk.legForwardRotation[indexNumber]);
                        foot[0].transform.rotation = Quaternion.Euler(0, shaft, walk.footBackRotation[indexNumber]);
                        foot[1].transform.rotation = Quaternion.Euler(0, shaft, walk.footForwardRotation[indexNumber]);
                    }
                    else
                    {
                        leg[0].transform.rotation = Quaternion.Euler(0, shaft, walk.legForwardRotation[indexNumber]);
                        leg[1].transform.rotation = Quaternion.Euler(0, shaft, walk.legBackRotation[indexNumber]);
                        foot[0].transform.rotation = Quaternion.Euler(0, shaft, walk.footForwardRotation[indexNumber]);
                        foot[1].transform.rotation = Quaternion.Euler(0, shaft, walk.footBackRotation[indexNumber]);
                    }
                }
                break;

            case Status.Boss:
                WalkPoseByIndex(walk, indexNumber);
                break;
        }

    }

    /// <summary>
    /// 上半身のモーション
    /// </summary>
    private void PlayerPantie()
    {
        switch (status)
        {
            case Status.Zombie:
                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, upper.wholeRotation[indexNumber]);

                // 腕のアニメーション
                if (upper.armForwardRotation == null || upper.armBackRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, upper.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, upper.armBackRotation[indexNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, upper.armForwardRotation[indexNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, upper.armBackRotation[indexNumber]);
                }

                // 足のアニメーション
                if (upper.legForwardRotation == null || upper.legBackRotation == null)
                {
                    Debug.LogWarning("Legのデータが何かしら抜けてる");
                    return;
                }
                else if (upper.footBackRotation == null || upper.footForwardRotation == null)
                {
                    Debug.LogWarning("Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, upper.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, upper.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, upper.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, upper.footForwardRotation[indexNumber]);
                }
                break;
            case Status.Boss:

                playerRc.transform.rotation = Quaternion.Euler(0, shaft, upper.wholeRotation[indexNumber]);

                // 腕のアニメーション
                if (upper.armForwardRotation == null || upper.armBackRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else if (lower.handForwardRotation == null || lower.handBackRotation == null)
                {
                    Debug.LogWarning("Handのデータが何かしら抜けている");
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, upper.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, upper.armBackRotation[indexNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, upper.handForwardRotation[indexNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, upper.handBackRotation[indexNumber]);
                }

                // 足のアニメーション
                if (upper.legForwardRotation == null || upper.legBackRotation == null)
                {
                    Debug.LogWarning("Legのデータが何かしら抜けてる");
                    return;
                }
                else if (upper.footBackRotation == null || upper.footForwardRotation == null)
                {
                    Debug.LogWarning("Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, upper.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, upper.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, upper.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, upper.footForwardRotation[indexNumber]);
                }
                break;
        }
    }

    /// <summary>
    /// 下半身のアニメーション
    /// </summary>
    private void PlayerKick()
    {
        switch (status)
        {
            case Status.Zombie:
                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, lower.wholeRotation[indexNumber]);

                // 腕のアニメーション
                if (lower.armForwardRotation == null || lower.armBackRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, lower.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, lower.armBackRotation[indexNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, lower.armForwardRotation[indexNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, lower.armBackRotation[indexNumber]);
                }

                // 足のアニメーション
                if (lower.legForwardRotation == null || lower.legBackRotation == null)
                {
                    Debug.LogWarning("Legのデータが何かしら抜けてる");
                    return;
                }
                else if (lower.footBackRotation == null || lower.footForwardRotation == null)
                {
                    Debug.LogWarning("Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, lower.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, lower.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, lower.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, lower.footForwardRotation[indexNumber]);
                }
                break;
            case Status.Boss:
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, lower.wholeRotation[indexNumber]);

                // 腕のアニメーション
                if (lower.armForwardRotation == null || lower.armBackRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else if (lower.handForwardRotation == null || lower.handBackRotation == null)
                {
                    Debug.LogWarning("Handのデータが何かしら抜けている");
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, lower.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, lower.armBackRotation[indexNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, lower.handForwardRotation[indexNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, lower.handBackRotation[indexNumber]);
                }

                // 足のアニメーション
                if (lower.legForwardRotation == null || lower.legBackRotation == null)
                {
                    Debug.LogWarning("Legのデータが何かしら抜けてる");
                    return;
                }
                else if (lower.footBackRotation == null || lower.footForwardRotation == null)
                {
                    Debug.LogWarning("Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, lower.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, lower.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, lower.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, lower.footForwardRotation[indexNumber]);
                }
                break;
        }
    }

    private IEnumerator CallWalkWithDelay()
    {

        for (int i = 0; i < walk.wholeRotation.Length; i++)
        {
            PlayerWalk();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % walk.wholeRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
    }

    private IEnumerator CallPantieWithDelay()
    {

        for (int i = 0; i < upper.armForwardRotation.Length; i++)
        {
            PlayerPantie();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % upper.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
        time = timeMax;
        isAttack = false;
       
    }

    private IEnumerator CallKickWithDelay()
    {

        for (int i = 0; i < lower.armForwardRotation.Length; i++)
        {
            PlayerKick();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % lower.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
        time = timeMax;
        isAttack = false;
       
    }

    /// <summary>
    /// 歩くことを継続した時、腕の配列の中の値を逆にする
    /// </summary>
    private void ChangeArmAnime()
    {
        // ゾンビではない場合　
        if (status != Status.Zombie)
        {
            //三項演算子(各要素に対して変換操作を行う)
            if (isActive)
            {
                walk.armForwardRotation = walk.armForwardRotation.Select(value => value < 0 ? -value : value).ToArray();
            }
            else if (!isActive)
            {
                walk.armForwardRotation = walk.armForwardRotation.Select(value => value > 0 ? -value : value).ToArray();
            }
        }
    }

    /// <summary>
    /// 歩くことを開始の関数
    /// </summary>
    private void WalkStart()
    {
        time = timeMax * walk.armForwardRotation.Length;
        StartCoroutine(CallWalkWithDelay());
    }

    /// <summary>
    /// パンチのアニメーション開始するときの関数
    /// </summary>
    public void PantieStart()
    {
        if (timeAttack < 0)
        {
            isAttack = true;
            time = timeAttackMax * upper.armForwardRotation.Length * 2;
            timeAttack = timeAttackMax * upper.armForwardRotation.Length;
            StopCoroutine(CallWalkWithDelay());
            indexNumber = 0;
            StartCoroutine(CallPantieWithDelay());
        }
    }

    /// <summary>
    /// キックのアニメーション開始するときの関数
    /// </summary>
    public void KickStart()
    {
        if (timeAttack < 0)
        {
            isAttack = true;
            time = timeAttackMax * lower.armForwardRotation.Length;
            timeAttack = timeAttackMax * lower.armForwardRotation.Length;
            StopCoroutine(CallWalkWithDelay());
            indexNumber = 0;
            StartCoroutine(CallKickWithDelay());
        }
    }

    /// <summary>
    /// 歩くことの初期化
    /// </summary>
    public void WalkInstance()
    {
        if (time < 0 && !isAttack)
        {
            isActive = !isActive;
            ChangeArmAnime();
            WalkStart();
        }
    }

    /// <summary>
    /// 歩くことを継続したとき
    /// </summary>
    public void KeepWalk()
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
    /// 右を向くとき
    /// </summary>
    public void TurnToRight()
    {
        shaft = SHAFT_DIRECTION_RIGHT;
        Debug.Log("右向き");
    }

    /// <summary>
    /// 左を向くとき
    /// </summary>
    public void TurnToLeft()
    {
        shaft = SHAFT_DIRECTION_LEFT;
        Debug.Log("左向き");

    }
    #region 山品変更

    public bool SetAttack()
    {
        return isAttack;
    }
    #endregion

    
    private void WalkPoseByIndex(AnimationData animation, int index)
    {
        ValidateAnimationData(animation, index);
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, walk.wholeRotation[index]);

        arm[0].transform.rotation = Quaternion.Euler(0, shaft, animation.armForwardRotation[index]);
        arm[1].transform.rotation = Quaternion.Euler(0, shaft, -animation.armForwardRotation[index]);
        hand[0].transform.rotation = Quaternion.Euler(0, shaft, animation.armForwardRotation[index]);
        hand[1].transform.rotation = Quaternion.Euler(0, shaft, -animation.armForwardRotation[index]);

        if (isActive)
        {
            leg[0].transform.rotation = Quaternion.Euler(0, shaft, animation.legBackRotation[index]);
            leg[1].transform.rotation = Quaternion.Euler(0, shaft, animation.legForwardRotation[index]);
            foot[0].transform.rotation = Quaternion.Euler(0, shaft, animation.footBackRotation[index]);
            foot[1].transform.rotation = Quaternion.Euler(0, shaft, animation.footForwardRotation[index]);
        }
        else
        {
            leg[0].transform.rotation = Quaternion.Euler(0, shaft, animation.legForwardRotation[index]);
            leg[1].transform.rotation = Quaternion.Euler(0, shaft, animation.legBackRotation[index]);
            foot[0].transform.rotation = Quaternion.Euler(0, shaft, animation.footForwardRotation[index]);
            foot[1].transform.rotation = Quaternion.Euler(0, shaft, animation.footBackRotation[index]);
        }
    }

    // アニメーションデータのバリデーション(問題が無ければ真)
    private bool ValidateAnimationData(AnimationData ad, int index)
    {
        float[][] rotationsArray = {
            ad.wholeRotation,
            ad.armForwardRotation,
            ad.armBackRotation,
            ad.handForwardRotation,
            ad.handBackRotation,
            ad.legForwardRotation,
            ad.legBackRotation,
            ad.footForwardRotation,
            ad.footBackRotation
        };

        if (rotationsArray.Contains(null))
        {
            Debug.LogWarning("ValidateAnimationIndex: Rotation data is null");
            return false;
        }

        int[] lengthArray = rotationsArray.Select(x => x.Length).ToArray();
        if (lengthArray.Min() <= index)
        {
            Debug.LogWarning("ValidateAnimationIndex: Index was outside the bounds of the array");
            return false;
        }

        return true;
    }

    private void SetDebugmodeIfDebugMove()
    {
        if (debugMoves != DebugMove.None)
        {
            if (indexNumberForDebug > -1)
            {
                indexNumber = indexNumberForDebug;
                switch (debugMoves)
                {
                    case DebugMove.Walk:
                        PlayerWalk();
                        break;
                    case DebugMove.Pantie:
                        PlayerPantie();
                        break;
                    case DebugMove.Kick:
                        PlayerKick();
                        break;
                }

            }
            else
            {
                switch (debugMoves)
                {
                    case DebugMove.Walk:
                        WalkInstance();
                        break;
                    case DebugMove.Pantie:
                        PantieStart();
                        break;
                    case DebugMove.Kick:
                        KickStart();
                        break;
                }
            }
        }
    }
}
