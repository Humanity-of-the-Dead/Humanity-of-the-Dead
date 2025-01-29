using System.Collections;
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
    [SerializeField] private AnimationDataSet animationDataSet;
    private UpperAttack upperAttack;

    private LowerAttack downAttack;

    //歩きの配列の番号
    private int walkIndex = 0;
    [SerializeField, Header("エフェクト関連")]
    [Tooltip("エフェクトのプレハブを代入")]
    public GameObject hitGameObject;
    [Tooltip("エフェクトが上半身に出る範囲（X座標),最大と最小")]

    public float upperEffectXMin, upperEffectXMax;
    [Tooltip("エフェクトが上半身に出る範囲（Y座標),最大と最小")]

    public float upperEffectYMin, upperEffectYMax;
    [Tooltip("エフェクトが下半身に出る範囲（X座標),最大と最小")]

    public float lowerEffectXMin, lowerEffectXMax;
    [Tooltip("エフェクトが下半身に出る範囲（Y座標),最大と最小")]

    public float lowerEffectYMin, lowerEffectYMax;


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
    //攻撃のアニメーションが終わったかどうか
    

    // 静止しているか
    private bool isStop = true;


    // タイマー
    public float timeWalk = 0;

    //プレイヤーコントロールで呼ぶためパブリック追記
    // 攻撃のタイマー
    public float timeAttack = 0;
    #endregion

    public const int SHAFT_DIRECTION_RIGHT = 0;
    public const int SHAFT_DIRECTION_LEFT = 180;

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

    // 右を向いているか
    public bool isFacingToRight()
    {
        return shaft == SHAFT_DIRECTION_RIGHT;
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

    public  void ShowHitEffects(int body)
    {
        Debug.Log("ShowHitEffects");
        PlayerControl playerControl=GetComponent<PlayerControl>();
        //このオブジェクトの座標
        for (int i = 0; i < playerControl.enemyObject.Count; i++)
        {
            Vector3 enemyVector3 = new Vector3(playerControl.enemyObject[i].transform.position.x, playerControl.enemyObject[i].transform.position.y);
            //上半身の場合
            if (body == 0)
            {
                //オブジェクトを出すローカル座標
                Vector3 effectVec2Upper = new Vector3(
                    Random.Range(upperEffectXMin, upperEffectXMax),
                    Random.Range(upperEffectYMin, upperEffectYMax));

                //オブジェクトを出す
                Instantiate(hitGameObject, effectVec2Upper + enemyVector3, Quaternion.identity);
                // Debug.Log("effectVec2+thisVec2="+effectVec2+tihsVec2)
                // Debug.Log("hit effect");
            }
            if (body == 1)
            {
                //オブジェクトを出すローカル座標
                Vector3 effectVec3Lower = new Vector2(
                    Random.Range(lowerEffectXMin, lowerEffectXMax),
                    Random.Range(lowerEffectYMin, lowerEffectYMax));

                Instantiate(hitGameObject, effectVec3Lower + enemyVector3, Quaternion.identity);
                Debug.Log($"effectVec3Lower= {effectVec3Lower}");
            }

        }

       

       
    }
    /// <summary>
    /// 歩くアニメーション
    /// </summary>
    private void PlayerWalk()
    {

        // Quaternion.Euler: 回転軸( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.walk.wholeRotation[walkIndex]);

        // 腕のアニメーション
        if (arm == null || animationDataSet.walk.armForwardRotation == null)
        {
            Debug.LogWarning("Armのデータが何かしら抜けてる");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.walk.armForwardRotation[walkIndex]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, -animationDataSet.walk.armForwardRotation[walkIndex]);
        }

        // 足のアニメーション
        if (leg == null || animationDataSet.walk.legForwardRotation == null || animationDataSet.walk.legBackRotation == null)
        {
            Debug.LogWarning("Legのデータが何かしら抜けてる");
            return;
        }
        else if (foot == null || animationDataSet.walk.footForwardRotation == null || animationDataSet.walk.footBackRotation == null)
        {
            Debug.LogWarning("Footのデータが何かしら抜けてる");
            return;
        }
        else
        {
            // 歩き始めの場合
            if (!isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.walk.legBackRotation[walkIndex]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.walk.legForwardRotation[walkIndex]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.walk.footBackRotation[walkIndex]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.walk.footForwardRotation[walkIndex]);
            }
            //歩き続けている場合
            if (isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.walk.legForwardRotation[walkIndex]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.walk.legBackRotation[walkIndex]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.walk.footForwardRotation[walkIndex]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.walk.footBackRotation[walkIndex]);
            }
        }
    }

    /// <summary>
    /// 上半身のモーション
    /// </summary>
    private void PlayerPantie()
    {
        switch (upperAttack)
        {
            case UpperAttack.NORMAL:
                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerUpper.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDataSet.playerUpper.armForwardRotation == null || animationDataSet.playerUpper.armBackRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerUpper.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerUpper.armBackRotation[attackNumber]);
                }

                // 足のアニメーション
                if (animationDataSet.playerUpper.legForwardRotation == null || animationDataSet.playerUpper.legBackRotation == null)
                {
                    Debug.LogWarning("Legのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDataSet.playerUpper.footBackRotation == null || animationDataSet.playerUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerUpper.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerUpper.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerUpper.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerUpper.footForwardRotation[attackNumber]);
                }
                break;
            case UpperAttack.POLICE:

                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeUpper.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDataSet.policeUpper.armForwardRotation == null || animationDataSet.policeUpper.armBackRotation == null)
                {
                    Debug.LogWarning("警察Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeUpper.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeUpper.armBackRotation[attackNumber]);
                }

                // 足のアニメーション
                if (animationDataSet.policeUpper.legForwardRotation == null || animationDataSet.policeUpper.legBackRotation == null)
                {
                    Debug.LogWarning("警察Legのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDataSet.policeUpper.footBackRotation == null || animationDataSet.policeUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("警察Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeUpper.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeUpper.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeUpper.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeUpper.footForwardRotation[attackNumber]);
                }
                break;
            case UpperAttack.NURSE:

                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseUpper.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDataSet.nurseUpper.armForwardRotation == null || animationDataSet.nurseUpper.armBackRotation == null)
                {
                    Debug.LogWarning("ナースArmのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseUpper.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseUpper.armBackRotation[attackNumber]);
                }

                // 足のアニメーション
                if (animationDataSet.nurseUpper.legForwardRotation == null || animationDataSet.nurseUpper.legBackRotation == null)
                {
                    Debug.LogWarning("ナースLegのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDataSet.nurseUpper.footBackRotation == null || animationDataSet.nurseUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("ナースFootのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseUpper.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseUpper.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseUpper.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseUpper.footForwardRotation[attackNumber]);
                }
                break;
        }

    }

    /// <summary>
    /// キックのアニメーション
    /// </summary>
    private void PlayerKick()
    {
        switch (downAttack)
        {
            case LowerAttack.NORMAL:
                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerLower.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDataSet.playerLower.armForwardRotation == null || animationDataSet.playerLower.armBackRotation == null)
                {
                    Debug.LogWarning("Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerLower.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerLower.armBackRotation[attackNumber]);
                }

                // 足のアニメーション
                if (animationDataSet.playerLower.legBackRotation == null || animationDataSet.playerLower.legForwardRotation == null)
                {
                    Debug.LogWarning("Legのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDataSet.playerLower.footBackRotation == null || animationDataSet.playerLower.footForwardRotation == null)
                {
                    Debug.LogWarning("Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerLower.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerLower.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerLower.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.playerLower.footForwardRotation[attackNumber]);
                }
                break;
            case LowerAttack.POLICE:
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeLower.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDataSet.policeLower.armForwardRotation == null || animationDataSet.policeLower.armBackRotation == null)
                {
                    Debug.LogWarning("警察Armのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeLower.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeLower.armBackRotation[attackNumber]);
                }

                // 足のアニメーション
                if (animationDataSet.policeLower.legForwardRotation == null || animationDataSet.policeLower.legBackRotation == null)
                {
                    Debug.LogWarning("警察Legのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDataSet.policeLower.footBackRotation == null || animationDataSet.policeLower.footForwardRotation == null)
                {
                    Debug.LogWarning("警察Footのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeLower.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeLower.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeLower.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.policeLower.footForwardRotation[attackNumber]);
                }
                break;
            case LowerAttack.NURSE:

                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseLower.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDataSet.nurseLower.armForwardRotation == null || animationDataSet.nurseLower.armBackRotation == null)
                {
                    Debug.LogWarning("ナースArmのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseLower.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseLower.armBackRotation[attackNumber]);
                }

                // 足のアニメーション
                if (animationDataSet.nurseLower.legForwardRotation == null || animationDataSet.nurseLower.legBackRotation == null)
                {
                    Debug.LogWarning("ナースLegのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDataSet.nurseLower.footBackRotation == null || animationDataSet.nurseLower.footForwardRotation == null)
                {
                    Debug.LogWarning("ナースFootのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseLower.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseLower.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseLower.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.nurseLower.footForwardRotation[attackNumber]);
                }
                break;
        }
    }

    private IEnumerator CallWalkWithDelay()
    {
        isWalk = true;

        for (int i = 0; i < animationDataSet.walk.armForwardRotation.Length; i++)
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

    private IEnumerator CallPantieWithDelay()
    {
      

        for (int i = 0; i < animationDataSet.playerUpper.armForwardRotation.Length - 1; i++)
        {
            PlayerPantie();
            //ShowHitEffects(0);


            // indexNumberの値を増やす(配列番号を上げる)
            attackNumber = (attackNumber + 1) % animationDataSet.playerUpper.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }

        timeWalk = 0;
        isAttack = false;
        isWalk = false;
        isStop = true;
        // 攻撃アニメーション終了を通知
       

    }

    private IEnumerator CallKickWithDelay()
    {

        for (int i = 0; i < animationDataSet.playerLower.armForwardRotation.Length - 1; i++)
        {
            PlayerKick();
            //ShowHitEffects(1);

            // indexNumberの値を増やす(配列番号を上げる)
            attackNumber = (attackNumber + 1) % animationDataSet.playerLower.armForwardRotation.Length;
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
    private void ChangeArmAnime()
    {
        //三項演算子(各要素に対して変換操作を行う)
        if (isActive)
        {
            animationDataSet.walk.armForwardRotation = animationDataSet.walk.armForwardRotation.Select(value => value > 0 ? -value : value).ToArray();
        }
        else if (!isActive)
        {
            animationDataSet.walk.armForwardRotation = animationDataSet.walk.armForwardRotation.Select(value => value < 0 ? -value : value).ToArray();
        }
    }

    /// <summary>
    /// 歩くことを開始の関数
    /// </summary>
    private void WalkStart()
    {
        timeWalk = timeMax * animationDataSet.playerUpper.armForwardRotation.Length;
        StartCoroutine(CallWalkWithDelay());
        MultiAudio.ins.PlaySEByName("SE_hero_action_run");
    }

    /// <summary>
    /// パンチのアニメーション開始するときの関数
    /// </summary>
    public void PantieStart()
    {
        AttackWait();
        StartCoroutine(CallPantieWithDelay());
    }

    /// <summary>
    /// キックのアニメーション開始するときの関数
    /// </summary>
    public void KickStart()
    {
        AttackWait();
        StartCoroutine(CallKickWithDelay());
    }

    /// <summary>
    /// 歩くことの初期化
    /// </summary>
    private void WalkInstance()
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
    private void KeepWalk()
    {
        // 連続入力されているか
        #region 山品変更

        if (timeWalk < 0.01f + Mathf.Epsilon)
        {
            #endregion
            //Debug.Log("Keep");
            walkIndex = 0;
            attackNumber = 0;
            ChangeArmAnime();
            WalkStart();
        }
    }

    /// <summary>
    /// 攻撃の初期化
    /// </summary>
    private void AttackWait()
    {
        timeWalk = timeMax * (animationDataSet.playerUpper.armForwardRotation.Length - 1);
        timeAttack = timeMax * (animationDataSet.playerLower.armForwardRotation.Length - 1);
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
    private class AnimationDataSet
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
