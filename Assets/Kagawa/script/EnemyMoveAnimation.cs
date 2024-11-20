using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyMoveAnimation : MonoBehaviour
{
    [Header("ゾンビ")] public bool zombie;
    [Header("全身")] public GameObject playerRc;
    [SerializeField, Header("腕の角度、先に右手")] GameObject[] arm;
    [SerializeField, Header("太腿の角度、先に右足")] GameObject[] leg;
    [SerializeField, Header("すねの角度、先に右足")] GameObject[] foot;

    [Header("1コマの間隔の時間")] public float timeMax;

    [Header("---歩きのアニメーション---")]
    [Header("全身の角度")] public float[] playerWalkRotation;
    [Header("腕の角度")] public float[] armWalkRotation;
    [Header("太ももの前方の角度")] public float[] legWalkForwardRotation;
    [Header("足の前方の角度")] public float[] footWalkForwardRotation;
    [Header("太ももの後方の角度")] public float[] legWalkBackRotation;
    [Header("足の後方の角度")] public float[] footWalkBackRotation;
    [Header("歩きの継続時間")] public float timeWalk;

    [Header("---上半身のアニメーション---")]
    [Header("全身の角度")] public float[] playerPatRotation;
    [Header("腕の前方角度")] public float[] armPatForwardRotation;
    [Header("腕の後方角度")] public float[] armPatBackRotation;
    [Header("太ももの前方の角度")] public float[] legPatForwardRotation;
    [Header("足の前方の角度")] public float[] footPatForwardRotation;
    [Header("太ももの後方の角度")] public float[] legPatBackRotation;
    [Header("足の後方の角度")] public float[] footPatBackRotation;

    [Header("---下半身のアニメーション---")]
    [Header("全身の角度")] public float[] playerKickRotation;
    [Header("腕の前方角度")] public float[] armKickForwardRotation;
    [Header("腕の後方角度")] public float[] armKickBackRotation;
    [Header("太ももの前方の角度")] public float[] legKickForwardRotation;
    [Header("足の前方の角度")] public float[] footKickForwardRotation;
    [Header("太ももの後方の角度")] public float[] legKickBackRotation;
    [Header("足の後方の角度")] public float[] footKickBackRotation;

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
        walkLength = armWalkRotation.Length - 1;
        time = 0;
        timeAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeAttack-= Time.deltaTime;
        WalkInstance();
    }

    /// <summary>
    /// 歩くアニメーション
    /// </summary>
    public void PlayerWalk()
    {
        // Quaternion.Euler: 回転軸( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerWalkRotation[indexNumber]);

        // 腕のアニメーション
        if (arm == null || armWalkRotation == null)
        {
            Debug.LogWarning("armのデータが何かしら抜けてる");
            return;
        }
        else if(zombie)
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[indexNumber]);
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[indexNumber]);
        }

        // 足のアニメーション
        if (leg == null || legWalkBackRotation == null || legWalkForwardRotation == null)
        {
            Debug.LogWarning("Legのデータが何かしら抜けてる");
            return;
        }
        else if (foot == null || footWalkBackRotation == null || footWalkForwardRotation == null)
        {
            Debug.LogWarning("footのデータが何かしら抜けてる");
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
    public void PlayerPantie()
    {
        // Quaternion.Euler: 回転軸( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerPatRotation[indexNumber]);

        // 腕のアニメーション
        if (arm == null || armPatForwardRotation == null || armPatBackRotation == null)
        {
            Debug.LogWarning("armのデータが何かしら抜けてる");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armPatForwardRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, armPatBackRotation[indexNumber]);
        }

        // 足のアニメーション
        if (leg == null || legPatBackRotation == null || legPatForwardRotation == null)
        {
            Debug.LogWarning("Legのデータが何かしら抜けてる");
            return;
        }
        else if (foot == null || footPatBackRotation == null || footPatForwardRotation == null)
        {
            Debug.LogWarning("footのデータが何かしら抜けてる");
            return;
        }
        else
        {
            leg[0].transform.rotation = Quaternion.Euler(0, shaft, legPatBackRotation[indexNumber]);
            leg[1].transform.rotation = Quaternion.Euler(0, shaft, legPatForwardRotation[indexNumber]);
            foot[0].transform.rotation = Quaternion.Euler(0, shaft, footPatBackRotation[indexNumber]);
            foot[1].transform.rotation = Quaternion.Euler(0, shaft, footPatForwardRotation[indexNumber]);
        }
    }

    /// <summary>
    /// 下半身のアニメーション
    /// </summary>
    public void PlayerKick()
    {
        // Quaternion.Euler: 回転軸( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerKickRotation[indexNumber]);

        // 腕のアニメーション
        if (arm == null || armKickForwardRotation == null || armKickBackRotation == null)
        {
            Debug.LogWarning("armのデータが何かしら抜けてる");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armKickForwardRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, armKickBackRotation[indexNumber]);
        }

        // 足のアニメーション
        if (leg == null || legKickBackRotation == null || legKickForwardRotation == null)
        {
            Debug.LogWarning("Legのデータが何かしら抜けてる");
            return;
        }
        else if (foot == null || footKickBackRotation == null || footKickForwardRotation == null)
        {
            Debug.LogWarning("footのデータが何かしら抜けてる");
            return;
        }
        else
        {
            leg[0].transform.rotation = Quaternion.Euler(0, shaft, legKickBackRotation[indexNumber]);
            leg[1].transform.rotation = Quaternion.Euler(0, shaft, legKickForwardRotation[indexNumber]);
            foot[0].transform.rotation = Quaternion.Euler(0, shaft, footKickBackRotation[indexNumber]);
            foot[1].transform.rotation = Quaternion.Euler(0, shaft, footKickForwardRotation[indexNumber]);
        }
    }

    IEnumerator CallWalkWithDelay()
    {
        for (int i = 0; i < armWalkRotation.Length; i++)
        {
            PlayerWalk();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % armWalkRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
    }

    IEnumerator CallPantieWithDelay()
    {
        for (int i = 0; i < armPatForwardRotation.Length; i++)
        {
            PlayerPantie();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % armPatForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
        isAttack = false;
    }

    IEnumerator CallKickWithDelay()
    {
        for (int i = 0; i < armKickForwardRotation.Length; i++)
        {
            PlayerKick();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % armKickForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
        isAttack = false;
    }

    /// <summary>
    /// 歩くことを継続した時、腕の配列の中の値を逆にする
    /// </summary>
    void ChangeArmAnime()
    {
        // ゾンビではない場合　
        if (!zombie)
        {
            //三項演算子(各要素に対して変換操作を行う)
            if (isActive)
            {
                armWalkRotation = armWalkRotation.Select(value => value < 0 ? -value : value).ToArray();
            }
            else if (!isActive)
            {
                armWalkRotation = armWalkRotation.Select(value => value > 0 ? -value : value).ToArray();
            }
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
    public void PantieStart()
    {
        if (timeAttack < 0)
        {
            timeAttack = timeMax * armPatBackRotation.Length;
            StopCoroutine(CallWalkWithDelay());
            Upright();
            isAttack = true;
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
            time = timeMax * armKickForwardRotation.Length;
            timeAttack = timeMax * armKickBackRotation.Length;
            StopCoroutine(CallWalkWithDelay());
            Upright();
            isAttack = true;
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
    public void RightMove()
    {
        shaft = 0;
    }

    /// <summary>
    /// 直立する
    /// </summary>
    public void Upright()
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
    /// 左を向くとき
    /// </summary>
    public void LeftMove()
    {
        shaft = 180;
    }
}
