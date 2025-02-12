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
    Pantie,
    attackIdle
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

    [SerializeField, Header("攻撃終了後の硬直時間")] private float afterAttackFreezeTime;

    [SerializeField, Header("---歩きのアニメーション---")] private AnimationData walk;

    [SerializeField, Header("---上半身のアニメーション---")] private AnimationData upper;

    [SerializeField, Header("---下半身のアニメーション---")] private AnimationData lower;
    [SerializeField, Header("---攻撃待機アニメーション")] private AnimationData attackIdleData;

    [SerializeField, Header("攻撃待機アニメーションが始まってから\n攻撃するまでの秒数")]

    private float attackIdle = 0;
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
    [SerializeField, Header("エフェクト関連")]
    [Tooltip("エフェクトのプレハブを代入")]
    public GameObject hitGameObject;
    private GameObject hitGameObjectInstantiated;
    [Tooltip("エフェクトが上半身に出る範囲（X座標),最大と最小")]

    public float upperEffectXMin, upperEffectXMax;
    [Tooltip("エフェクトが上半身に出る範囲（Y座標),最大と最小")]

    public float upperEffectYMin, upperEffectYMax;
    [Tooltip("エフェクトが下半身に出る範囲（X座標),最大と最小")]

    public float lowerEffectXMin, lowerEffectXMax;
    [Tooltip("エフェクトが下半身に出る範囲（Y座標),最大と最小")]

    public float lowerEffectYMin, lowerEffectYMax;
    private newEnemyMovement newEnemyMovement;

    private void Start()
    {
        indexNumber = 0;
        shaft = SHAFT_DIRECTION_LEFT;
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, walk.wholeRotation[indexNumber]);

        newEnemyMovement=GetComponent<newEnemyMovement>();  
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
    public void ShowHitEffects(int body, Vector3 playerVector3 )
    {
    
        if (hitGameObjectInstantiated !=null)
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

                case DebugMove.attackIdle:
                    AttackIdle();
                    break;
            }
            Destroy(hitGameObjectInstantiated);      
        }

       
        //上半身の場合
        if (body == 0)
        {
            //オブジェクトを出すローカル座標
            Vector3 effectVec2Upper = new Vector3(
                Random.Range(upperEffectXMin, upperEffectXMax),
                Random.Range(upperEffectYMin, upperEffectYMax));

            //オブジェクトを出す
            hitGameObjectInstantiated= Instantiate(hitGameObject, effectVec2Upper + playerVector3, Quaternion.identity);
            // Debug.Log("effectVec2+thisVec2="+effectVec2+tihsVec2)
            // Debug.Log("hit effect");
        }

        if (body == 1)
        {
            //オブジェクトを出すローカル座標
            Vector3 effectVec3Lower = new Vector2(
                Random.Range(lowerEffectXMin, lowerEffectXMax),
                Random.Range(lowerEffectYMin, lowerEffectYMax));

            hitGameObjectInstantiated= Instantiate(hitGameObject, effectVec3Lower + playerVector3, Quaternion.identity);

        }
    }
    private IEnumerator CallAttackIdleWithDelay()
    {
        for (int i = 0; i < attackIdleData.armForwardRotation.Length; i++)
        {
            AttackIdle();
            indexNumber = (indexNumber + 1) % attackIdleData.armForwardRotation.Length;
            yield return new WaitForSeconds(attackIdle);
        }

        isAttack = false;
    }
    /// <summary>
    /// 攻撃待機アニメーション
    /// </summary>
    /// <summary>
    /// 攻撃待機アニメーション
    /// </summary>
    /// <summary>
    /// 攻撃待機アニメーション
    /// </summary>
    private void AttackIdle()
    {
        // Rotate the whole body
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, attackIdleData.wholeRotation[indexNumber]);

        // Arm animation
        if (attackIdleData.armForwardRotation == null || attackIdleData.armBackRotation == null)
        {
            Debug.LogWarning("AttackIdle Armデータが何かしら抜けてる");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, attackIdleData.armForwardRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, attackIdleData.armBackRotation[indexNumber]);
            hand[0].transform.rotation = Quaternion.Euler(0, shaft, attackIdleData.handForwardRotation[indexNumber]);
            hand[1].transform.rotation = Quaternion.Euler(0, shaft, attackIdleData.handBackRotation[indexNumber]);

        }

        // Leg animation
        if (attackIdleData.legForwardRotation == null || attackIdleData.legBackRotation == null)
        {
            Debug.LogWarning("AttackIdle Legデータが何かしら抜けてる");
            return;
        }
        else if (attackIdleData.footBackRotation == null || attackIdleData.footForwardRotation == null)
        {
            Debug.LogWarning("AttackIdle Footデータが何かしら抜けてる");
            return;
        }
        else
        {
            leg[0].transform.rotation = Quaternion.Euler(0, shaft, attackIdleData.legForwardRotation[indexNumber]);
            leg[1].transform.rotation = Quaternion.Euler(0, shaft, attackIdleData.legBackRotation[indexNumber]);
            foot[0].transform.rotation = Quaternion.Euler(0, shaft, attackIdleData.footForwardRotation[indexNumber]);
            foot[1].transform.rotation = Quaternion.Euler(0, shaft, attackIdleData.footBackRotation[indexNumber]);
        }
    }         /// 歩くアニメーション
              /// </summary>
    public void PlayerWalk()
    {
        switch (status)
        {
            case Status.Zombie:
                WalkPoseByIndex(walk, indexNumber, true);
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
        if (indexNumber >= upper.wholeRotation.Length)
        {
            Debug.LogWarning("IndexNumber is out of range for wholeRotation array.");
            return;
        }
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
        if (indexNumber >= lower.wholeRotation.Length)
        {
            Debug.LogWarning("IndexNumber is out of range for wholeRotation array.");
            return;
        }

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
        // 攻撃待機アニメーションを呼び出す
        yield return StartCoroutine(CallAttackIdleWithDelay());

        int animationLength = upper.armForwardRotation.Length;
        for (int i = 0; i < animationLength; i++)
        {
            PlayerPantie();

            // indexNumberの値を増やす(配列番号を上げる)
            indexNumber = (indexNumber + 1) % upper.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);

            if (i < animationLength - 1)
            {
                // indexNumberの値を増やす(配列番号を上げる)
                indexNumber++;
                yield return new WaitForSeconds(timeAttackMax);
            }
            else
            {
                yield return new WaitForSeconds(timeAttackMax + afterAttackFreezeTime);
            }
        }

        // 攻撃が終わったら
        PoseWaiting(status == Status.Zombie);
        time = timeMax;
        isAttack = false;

    }

    private IEnumerator CallKickWithDelay()
    {
        // 攻撃待機アニメーションを呼び出す
        yield return StartCoroutine(CallAttackIdleWithDelay());

        int animationLength = upper.armForwardRotation.Length;
        for (int i = 0; i < animationLength; i++)
        {
            PlayerKick();

            if (i < animationLength - 1)
            {
                // indexNumberの値を増やす(配列番号を上げる)
                indexNumber++;
                yield return new WaitForSeconds(timeAttackMax);
            }
            else
            {
                yield return new WaitForSeconds(timeAttackMax + afterAttackFreezeTime);
            }
        }

        // 攻撃が終わったら
        PoseWaiting(status == Status.Zombie);
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
        //Debug.Log("右向き");
    }

    /// <summary>
    /// 左を向くとき
    /// </summary>
    public void TurnToLeft()
    {
        shaft = SHAFT_DIRECTION_LEFT;
        //Debug.Log("左向き");

    }
    #region 山品変更

    public bool SetAttack()
    {
        return isAttack;
    }
    #endregion

    // 待機ポーズ
    private void PoseWaiting(bool walksLikeZombie = false)
    {
        // 歩行モーションの最後のポーズを待機ポーズ扱いする
        int index = walk.armForwardRotation.Length - 1;
        WalkPoseByIndex(walk, index, walksLikeZombie);
    }

    // TODO: PlayerMoveAnimationのものとほぼ同一コード。できれば共通化したい
    private void WalkPoseByIndex(AnimationData animation, int index, bool walksLikeZombie = false)
    {
        PlayerMoveAnimation playerMoveAnimation = GameObject.Find("Player Variant").GetComponent<PlayerMoveAnimation>(); ;
        playerMoveAnimation.ValidateAnimationData(animation, index);

        // 胴体
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, animation.wholeRotation[index]);

        // 腕
        // ChangeArmAnime()にてisActiveを使用して左右の動きをスイッチしているので注意
        if (walksLikeZombie)
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, animation.armForwardRotation[index]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, animation.armForwardRotation[index]);
            hand[0].transform.rotation = Quaternion.Euler(0, shaft, animation.armForwardRotation[index]);
            hand[1].transform.rotation = Quaternion.Euler(0, shaft, animation.armForwardRotation[index]);
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, animation.armForwardRotation[index]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, -animation.armForwardRotation[index]);
            hand[0].transform.rotation = Quaternion.Euler(0, shaft, animation.armForwardRotation[index]);
            hand[1].transform.rotation = Quaternion.Euler(0, shaft, -animation.armForwardRotation[index]);
        }

        // 脚
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
