using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    [Header("太ももの前方の角度")] public float[] legWalkForwardRotation;
    [Header("足の前方の角度")] public float[] footWalkForwardRotation;
    [Header("太ももの後方の角度")] public float[] legWalkBackRotation;
    [Header("足の後方の角度")] public float[] footWalkBackRotation;
    [Header("歩きの継続時間")] public float timeWalk;

    [Header("---パンチのアニメーション---")]
    [Header("全身の角度")] public float[] playerPatRotation;
    [Header("腕の前方角度")] public float[] armPatForwardRotation;
    [Header("腕の後方角度")] public float[] armPatBackRotation;
    [Header("太ももの前方の角度")] public float[] legPatForwardRotation;
    [Header("足の前方の角度")] public float[] footPatForwardRotation;
    [Header("太ももの後方の角度")] public float[] legPatBackRotation;
    [Header("足の後方の角度")] public float[] footPatBackRotation;

    [Header("---キックのアニメーション---")]
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
    bool isAtack;

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

        isAtack = false;
        isActive = false;
        isWalk = false;
        isStop = false;
        walkLength = armWalkRotation.Length - 1;
        time = 0;
        timeAttack = 0;
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
                AttackWaite();
                PantieStart();

            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                AttackWaite();
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
            Debug.LogWarning("armのデータが何かしら抜けてる");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft + 180, armWalkRotation[indexNumber]);
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
    /// パンチのモーション
    /// </summary>
    void PlayerPantie()
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
            arm[1].transform.rotation = Quaternion.Euler(0, shaft + 180, armPatBackRotation[indexNumber]);
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
    /// キックのアニメーション
    /// </summary>
    void PlayerKick()
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
            arm[1].transform.rotation = Quaternion.Euler(0, shaft + 180, armKickBackRotation[indexNumber]);
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
            if (!isAtack)
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
        for (int i = 0; i < armPatForwardRotation.Length; i++)
        {
            PlayerPantie();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % armPatForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }

        time = 0;
        isAtack = false;
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

        time = 0;
        isAtack = false;
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
        time = timeMax * armPatForwardRotation.Length;
        StartCoroutine(CallPantieWithDelay());
    }

    /// <summary>
    /// キックのアニメーション開始するときの関数
    /// </summary>
    void KickStart()
    {
        time = timeMax * armKickForwardRotation.Length;
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
        timeAttack = timeMax * armKickBackRotation.Length;
        isAtack = true;
        StopCoroutine(CallWalkWithDelay());
        Upright();
        indexNumber = 0;
    }

    /// <summary>
    /// 直立する
    /// </summary>
    void Upright()
    {
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerWalkRotation[walkLength]);
        arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[walkLength]);
        arm[1].transform.rotation = Quaternion.Euler(0, shaft + 180, armWalkRotation[walkLength]);
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
    /// <param name="upperBody">画像データ集合体</param>
    public void ChangeUnderBody(BodyPartsData underBody)
    {
        waistSR.sprite = underBody.spWaist;
        footRightSR.sprite = underBody.spRightFoot;
        footLeftSR.sprite = underBody.spLeftFoot;
        legRightSR.sprite = underBody.spRightLeg;
        legLeftSR.sprite = underBody.spLeftHand;
    }
}

