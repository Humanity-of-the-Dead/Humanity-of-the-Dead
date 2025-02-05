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
    [SerializeField, Header("手首の角度、先に右手")] private GameObject[] hand;

    //アニメーションの間隔はアニメーションスクリプト内で完結したいのでプライベート化してシリアライズフィールドを付けることで設定できる
    [SerializeField, Header("1コマの間隔の時間")] private float timeMax;
    [SerializeField, Header("攻撃の1コマの間隔の時間")] private float attackTimeMax;
    [SerializeField, Header("攻撃終了後の硬直時間")] private float afterAttackFreezeTime;
    [SerializeField] private AnimationDataSet animationDataSet;
    private UpperAttack upperAttack;

    private LowerAttack downAttack;

    //歩きの配列の番号
    private int walkIndex = 0;
    [SerializeField, Header("エフェクト関連")]
    [Tooltip("エフェクトのプレハブを代入")]
    private GameObject hitGameObject;
    private GameObject hitGameObjectInstantiated;
    [Tooltip("エフェクトが上半身に出る範囲（X座標),最大と最小")]

    public float upperEffectXMin, upperEffectXMax;
    [Tooltip("エフェクトが上半身に出る範囲（Y座標),最大と最小")]

    public float upperEffectYMin, upperEffectYMax;
    [Tooltip("エフェクトが下半身に出る範囲（X座標),最大と最小")]

    public float lowerEffectXMin, lowerEffectXMax;
    [Tooltip("エフェクトが下半身に出る範囲（Y座標),最大と最小")]

    public float lowerEffectYMin, lowerEffectYMax;
    [SerializeField, Header("ボスエフェクト関連")]

    [Tooltip("ボスエフェクトのプレハブを代入")]
    private GameObject hitGameObject_Boss;
    private GameObject hitGameObjectInstantiated_Boss;

   

    [SerializeField] private int multiEffectNumber;
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
    private List<Vector3> enemyPositions;

    private void Start()
    {
        enemyPositions = new List<Vector3>();
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

    public void SetTimeMax(float time)
    {
        timeMax = time;
    }

    // 右を向いているか
    public bool isFacingToRight()
    {
        return shaft == SHAFT_DIRECTION_RIGHT;
    }

    public void HandleWalk(int direction, bool walksLikeZombie = false)
    {
        shaft = direction;

        if (isStop)
        {
            WalkInit(walksLikeZombie);
        }
        if (!isWalk)
        {
            isWalk = true;
            ChangeArmAnime();
            KeepWalk(walksLikeZombie);
        }
        else if (isWalk && !isAttack)
        {
            PlayerWalk(walksLikeZombie);
        }
    }

    public void ShowHitEffects(int body, Vector3 enemyVector3, bool multipled = false)
    {
        if (hitGameObjectInstantiated != null)
        {
            Destroy(hitGameObjectInstantiated);     
        }
        if(hitGameObjectInstantiated_Boss != null)
        {
            DestroyImmediate(hitGameObjectInstantiated_Boss);       
        }
        //上半身の場合
        if (body == 0)
        {
            if (!multipled)
            {
                //オブジェクトを出すローカル座標
                Vector3 effectVec2Upper = new Vector3(
                    Random.Range(upperEffectXMin, upperEffectXMax),
                    Random.Range(upperEffectYMin, upperEffectYMax));
                    hitGameObjectInstantiated= Instantiate(hitGameObject, effectVec2Upper + enemyVector3, Quaternion.identity);

            }
            else
            {
                enemyPositions = new List<Vector3>
            {
        enemyVector3,
        new Vector3(enemyVector3.x + 1, enemyVector3.y, enemyVector3.z),
        new Vector3(enemyVector3.x - 1, enemyVector3.y, enemyVector3.z)
    };

                for (int i = 0; i < multiEffectNumber; i++)
                {
                        hitGameObjectInstantiated_Boss= Instantiate(hitGameObject_Boss, enemyPositions[i], Quaternion.identity);
                }

            }



            //オブジェクトを出す
            // Debug.Log("effectVec2+thisVec2="+effectVec2+tihsVec2)
            // Debug.Log("hit effect");
        }
        if (body == 1)
        {
            if (!multipled)
            {
                //オブジェクトを出すローカル座標
                Vector3 effectVec3Lower = new Vector3(
                    Random.Range(lowerEffectXMin, lowerEffectXMax),
                    Random.Range(lowerEffectYMin, lowerEffectYMax));
                hitGameObjectInstantiated = Instantiate(hitGameObject, effectVec3Lower + enemyVector3, Quaternion.identity);

            }
            else
            {
                enemyPositions = new List<Vector3>
            {
        enemyVector3,
        new Vector3(enemyVector3.x + 1, enemyVector3.y, enemyVector3.z),
        new Vector3(enemyVector3.x - 1, enemyVector3.y, enemyVector3.z)
    };

                for (int i = 0; i < multiEffectNumber; i++)
                {
                     hitGameObjectInstantiated_Boss = Instantiate(hitGameObject_Boss, enemyPositions[i], Quaternion.identity);
                }
            }

        }






    }
    /// <summary>
    /// 歩くアニメーション
    /// </summary>
    private void PlayerWalk(bool walksLikeZombie = false)
    {
        AnimationData walkAnimation = walksLikeZombie ? animationDataSet.zombieWalk : animationDataSet.walk;
        WalkPoseByIndex(walkAnimation, walkIndex, walksLikeZombie);
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
            case UpperAttack.BOSS:
                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossUpper.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDataSet.bossUpper.armForwardRotation == null || animationDataSet.bossUpper.armBackRotation == null)
                {
                    Debug.LogWarning("ナースArmのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossUpper.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossUpper.armBackRotation[attackNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossUpper.handForwardRotation[attackNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossUpper.handBackRotation[attackNumber]);

                }

                // 足のアニメーション
                if (animationDataSet.bossUpper.legForwardRotation == null || animationDataSet.bossUpper.legBackRotation == null)
                {
                    Debug.LogWarning("ナースLegのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDataSet.bossUpper.footBackRotation == null || animationDataSet.bossUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("ナースFootのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossUpper.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossUpper.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossUpper.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossUpper.footForwardRotation[attackNumber]);
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

            case LowerAttack.BOSS:
                // Quaternion.Euler: 回転軸( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossLower.wholeRotation[attackNumber]);

                // 腕のアニメーション
                if (animationDataSet.bossLower.armForwardRotation == null || animationDataSet.bossLower.armBackRotation == null)
                {
                    Debug.LogWarning("ナースArmのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossLower.armForwardRotation[attackNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossLower.armBackRotation[attackNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossLower.handForwardRotation[attackNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossLower.handBackRotation[attackNumber]);

                }

                // 足のアニメーション
                if (animationDataSet.bossLower.legForwardRotation == null || animationDataSet.bossLower.legBackRotation == null)
                {
                    Debug.LogWarning("ナースLegのデータが何かしら抜けてる");
                    return;
                }
                else if (animationDataSet.bossLower.footBackRotation == null || animationDataSet.bossLower.footForwardRotation == null)
                {
                    Debug.LogWarning("ナースFootのデータが何かしら抜けてる");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossLower.legBackRotation[attackNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossLower.legForwardRotation[attackNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossLower.footBackRotation[attackNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, animationDataSet.bossLower.footForwardRotation[attackNumber]);
                }

                break;
        }
    }
    
    // 待機ポーズ
    private void PoseWaiting()
    {
        // 歩行モーションの最後のポーズを待機ポーズ扱いする
        AnimationData animation = animationDataSet.walk;
        int index = animation.armForwardRotation.Length - 1;
        WalkPoseByIndex(animation, index);
    }

    private void WalkPoseByIndex(AnimationData animation, int index, bool walksLikeZombie = false)
    {
        if (!ValidateAnimationData(animation, index))
        {
            return;
        }

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

    // アニメーションデータのバリデーション(問題が無ければ真)
    public bool ValidateAnimationData(AnimationData animation, int index)
    {
        if (!animation)
        {
            Debug.LogWarning("ValidateAnimationIndex: animation is null");
            return false;
        }

        float[][] rotationsArray = AnimationDataToRotationsArray(animation);

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

    private float[][] AnimationDataToRotationsArray(AnimationData ad)
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

        return rotationsArray;
    }

    private IEnumerator CallWalkWithDelay(bool walksLikeZombie = false)
    {
        isWalk = true;

        AnimationData walkAnimation = walksLikeZombie ? animationDataSet.zombieWalk : animationDataSet.walk;
        for (int i = 0; i < walkAnimation.armForwardRotation.Length; i++)
        {
            if (!isAttack)
            {
                // indexNumberの値を増やす(配列番号を上げる)
                walkIndex = i;
                PlayerWalk(walksLikeZombie);
                yield return new WaitForSeconds(timeMax);
            }
        }

        isWalk = false;
        isActive = !isActive;
    }

    private IEnumerator CallPantieWithDelay()
    {
        int animationLength = animationDataSet.playerUpper.armForwardRotation.Length;
        for (int i = 0; i < animationLength; i++)
        {
            PlayerPantie();

            if (i < animationLength - 1)
            {
                // indexNumberの値を増やす(配列番号を上げる)
                attackNumber++;
                yield return new WaitForSeconds(attackTimeMax);
            }
            else
            {
                yield return new WaitForSeconds(attackTimeMax + afterAttackFreezeTime);
            }
        }

        // 攻撃が終わったら
        PoseWaiting();
        timeWalk = 0;
        isAttack = false;
        isWalk = false;
        isStop = true;
        // 攻撃アニメーション終了を通知


    }

    private IEnumerator CallKickWithDelay()
    {
        int animationLength = animationDataSet.playerUpper.armForwardRotation.Length;
        for (int i = 0; i < animationLength; i++)
        {
            PlayerKick();

            if (i < animationLength - 1)
            {
                // indexNumberの値を増やす(配列番号を上げる)
                attackNumber++;
                yield return new WaitForSeconds(attackTimeMax);
            }
            else
            {
                yield return new WaitForSeconds(attackTimeMax + afterAttackFreezeTime);
            }
        }

        // 攻撃が終わったら
        PoseWaiting();
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
    private void WalkStart(bool walksLikeZombie = false)
    {
        AnimationData walkAnimation = walksLikeZombie ? animationDataSet.zombieWalk : animationDataSet.walk;
        timeWalk = timeMax * walkAnimation.armForwardRotation.Length;
        if (walksLikeZombie)
        {
            StartCoroutine(CallWalkWithDelay(true));
        }
        else
        {
            StartCoroutine(CallWalkWithDelay());
            MultiAudio.ins.PlaySEByName("SE_hero_action_run");
        }
    }

    /// <summary>
    /// パンチのアニメーション開始するときの関数
    /// </summary>
    public void PantieStart()
    {
        AttackInit();
        StartCoroutine(CallPantieWithDelay());
        Debug.Log("PantieStart: test03");
    }

    /// <summary>
    /// キックのアニメーション開始するときの関数
    /// </summary>
    public void KickStart()
    {
        AttackInit();
        StartCoroutine(CallKickWithDelay());
    }

    /// <summary>
    /// 歩くことの初期化
    /// </summary>
    private void WalkInit(bool walksLikeZombie = false)
    {
        if (timeWalk < 0)
        {
            walkIndex = 0;
            attackNumber = 0;
            isActive = false;
            isStop = false;
            WalkStart(walksLikeZombie);
        }
    }

    /// <summary>
    /// 歩くことを継続したとき
    /// </summary>
    private void KeepWalk(bool walksLikeZombie = false)
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
            WalkStart(walksLikeZombie);
        }
    }

    /// <summary>
    /// 攻撃の初期化
    /// </summary>
    private void AttackInit()
    {
        timeWalk = timeMax * (animationDataSet.playerUpper.armForwardRotation.Length - 1);
        timeAttack = attackTimeMax * (animationDataSet.playerLower.armForwardRotation.Length - 1);
        isAttack = true;
        isWalk = false;
        StopCoroutine(CallWalkWithDelay());
        walkIndex = 0;
        attackNumber = 0;
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
        [SerializeField, Header("---ゾンビ歩きのアニメーション---")] public AnimationData zombieWalk;

        [SerializeField, Header("---デフォルトパンチのアニメーション---")] public AnimationData playerUpper;

        [SerializeField, Header("---デフォルトキックのアニメーション---")] public AnimationData playerLower;

        [SerializeField, Header("---警察拳銃のアニメーション---")] public AnimationData policeUpper;

        [SerializeField, Header("---警察下半身のアニメーション---")] public AnimationData policeLower;

        [SerializeField, Header("---ナースパンチのアニメーション---")] public AnimationData nurseUpper;

        [SerializeField, Header("---ナースキックのアニメーション---")] public AnimationData nurseLower;

        [SerializeField, Header("---ボスパンチのアニメーション---")] public AnimationData bossUpper;

        [SerializeField, Header("---ボスキックのアニメーション---")] public AnimationData bossLower;
        //[SerializeField, Header("---攻撃待機アニメーション---")] public AnimationData　attackIdle ;//まだ制作完了していないのでコメントアウト化


    }

}
