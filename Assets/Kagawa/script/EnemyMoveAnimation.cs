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
    public AnimationData Upper;

    [Header("---下半身のアニメーション---")]
    public AnimationData Lower;

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
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, Upper.wholeRotation[indexNumber]);

        // 腕のアニメーション
        if (Upper.armForwardRotation == null || Upper.armBackRotation == null)
        {
            Debug.LogWarning("Armのデータが何かしら抜けてる");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, Upper.armForwardRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, Upper.armBackRotation[indexNumber]);
        }

        // 足のアニメーション
        if (Upper.legForwardRotation == null || Upper.legBackRotation == null)
        {
            Debug.LogWarning("Legのデータが何かしら抜けてる");
            return;
        }
        else if (Upper.footBackRotation == null || Upper.footForwardRotation == null)
        {
            Debug.LogWarning("Footのデータが何かしら抜けてる");
            return;
        }
        else
        {
            leg[0].transform.rotation = Quaternion.Euler(0, shaft, Upper.legBackRotation[indexNumber]);
            leg[1].transform.rotation = Quaternion.Euler(0, shaft, Upper.legForwardRotation[indexNumber]);
            foot[0].transform.rotation = Quaternion.Euler(0, shaft, Upper.footBackRotation[indexNumber]);
            foot[1].transform.rotation = Quaternion.Euler(0, shaft, Upper.footForwardRotation[indexNumber]);
        }
    }

    /// <summary>
    /// 下半身のアニメーション
    /// </summary>
    public void PlayerKick()
    {
        // Quaternion.Euler: 回転軸( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, Lower.wholeRotation[indexNumber]);

        // 腕のアニメーション
        if (Lower.armForwardRotation == null || Lower.armBackRotation == null)
        {
            Debug.LogWarning("Armのデータが何かしら抜けてる");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, Lower.armForwardRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, Lower.armBackRotation[indexNumber]);
        }

        // 足のアニメーション
        if (Lower.legForwardRotation == null || Lower.legBackRotation == null)
        {
            Debug.LogWarning("Legのデータが何かしら抜けてる");
            return;
        }
        else if (Lower.footBackRotation == null || Lower.footForwardRotation == null)
        {
            Debug.LogWarning("Footのデータが何かしら抜けてる");
            return;
        }
        else
        {
            leg[0].transform.rotation = Quaternion.Euler(0, shaft, Lower.legBackRotation[indexNumber]);
            leg[1].transform.rotation = Quaternion.Euler(0, shaft, Lower.legForwardRotation[indexNumber]);
            foot[0].transform.rotation = Quaternion.Euler(0, shaft, Lower.footBackRotation[indexNumber]);
            foot[1].transform.rotation = Quaternion.Euler(0, shaft, Lower.footForwardRotation[indexNumber]);
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
        for (int i = 0; i < Upper.armForwardRotation.Length; i++)
        {
            PlayerPantie();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % Upper.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
        time = timeMax;
        isAttack = false;
    }

    IEnumerator CallKickWithDelay()
    {
        for (int i = 0; i < Lower.armForwardRotation.Length; i++)
        {
            PlayerKick();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % Lower.armForwardRotation.Length;
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
            Debug.Log("パンチスタート");
            isAttack = true;
            time = timeMax * Upper.armForwardRotation.Length * 2;
            timeAttack = timeMax * Upper.armForwardRotation.Length;
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
            time = timeMax * Lower.armForwardRotation.Length;
            timeAttack = timeMax * Lower.armForwardRotation.Length;
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
        Debug.Log("歩き始め");
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
        Debug.Log("アプライト");
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerWalkRotation[walkLength]);
        arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[walkLength]);
        arm[1].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[walkLength]);
        leg[0].transform.rotation = Quaternion.Euler(0, shaft, legWalkBackRotation[walkLength]);
        leg[1].transform.rotation = Quaternion.Euler(0, shaft, legWalkForwardRotation[walkLength]);
        foot[0].transform.rotation = Quaternion.Euler(0, shaft, footWalkBackRotation[walkLength]);
        foot[1].transform.rotation = Quaternion.Euler(0, shaft, footWalkForwardRotation[walkLength]);
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
