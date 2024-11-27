using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum Status
{
    Zombie,
    Boss,
}
public enum DebugMove
{
    None,
    Walk,
    Kick,
    Pantie
}


public class EnemyMoveAnimation : MonoBehaviour
{
    [Header("敵の種類")] public Status status;

    [Header("デバッグ用(通常、None)")] public DebugMove debugMoves;

    [Header("全身")] public GameObject playerRc;
    [SerializeField, Header("腕の角度、先に右手")] GameObject[] arm;
    [SerializeField, Header("手首の角度、先に右手")] GameObject[] hand;
    [SerializeField, Header("太腿の角度、先に右足")] GameObject[] leg;
    [SerializeField, Header("すねの角度、先に右足")] GameObject[] foot;

    [Header("1コマの間隔の時間")] public float timeMax;

    [Header("---歩きのアニメーション---")]
    public AnimationData walk;

    [Header("---上半身のアニメーション---")]
    public AnimationData upper;

    [Header("---下半身のアニメーション---")]
    public AnimationData lower;

    //配列の番号
    int indexNumber;

    //体の軸
    int shaft;

    //歩くアニメーションの角度の数
    int walkLength;

    // 値を反転にするフラグ
    bool isActive;

    // 向いている方向が右を向いているか
    bool isMirror;

    // 攻撃中かどうか
    bool isAttack;

    // 方向フラグ(右 = false)
    bool isWalk;

    // 静止しているか
    bool isStop;

    // タイマー
    float time;

    // 攻撃のタイマー
    float timeAttack;


    private void Start()
    {
        indexNumber = 0;
        shaft = 0;

        isMirror = true;
        isActive = false;
        isAttack = false;
        isWalk = true;
        isStop = false;
        walkLength = walk.armForwardRotation.Length - 1;
        time = 0;
        timeAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeAttack -= Time.deltaTime;

        if (debugMoves != DebugMove.None)
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
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, -walk.armForwardRotation[indexNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, walk.armForwardRotation[indexNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, -walk.armForwardRotation[indexNumber]);
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
        }
    
    }

    /// <summary>
    /// 上半身のモーション
    /// </summary>
    public void PlayerPantie()
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
    public void PlayerKick()
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

    IEnumerator CallWalkWithDelay()
    {
        for (int i = 0; i < walk.wholeRotation.Length; i++)
        {
            PlayerWalk();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % walk.wholeRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
    }

    IEnumerator CallPantieWithDelay()
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

    IEnumerator CallKickWithDelay()
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
    void ChangeArmAnime()
    {
        // ゾンビではない場合　
        if (status !=Status.Zombie)
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
    void WalkStart()
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
            time = timeMax * upper.armForwardRotation.Length * 2;
            timeAttack = timeMax * upper.armForwardRotation.Length;
            StopCoroutine(CallWalkWithDelay());
            Upright();
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
            time = timeMax * lower.armForwardRotation.Length;
            timeAttack = timeMax * lower.armForwardRotation.Length;
            StopCoroutine(CallWalkWithDelay());
            Upright();
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
    /// 直立する
    /// </summary>
    public void Upright()
    {
       
    }

    /// <summary>
    /// 右を向くとき
    /// </summary>
    public void RightMove()
    {
        shaft = 0;
    }

    /// <summary>
    /// 左を向くとき
    /// </summary>
    public void LeftMove()
    {
        shaft = 180;
    }
}
