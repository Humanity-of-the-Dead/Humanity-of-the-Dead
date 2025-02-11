using UnityEngine;

public class newEnemyMovement : MonoBehaviour
{
    // 移動を始める場所、終わりの場所、普段の移動速度、追跡中の移動速度、敵の索敵可能な範囲を設定

    [Header("エネミーの移動関連の設定")]
    [Tooltip("移動の絶対値")]
    [SerializeField] private float moveDistance = 5f;
    [Tooltip("普段の移動速度")]
    [SerializeField] private float normalSpeed = 2f;
    [Tooltip("追跡中の移動速度")]
    [SerializeField] private float chaseSpeed = 4f;
    [Tooltip("敵の索敵可能な範囲")]
    [SerializeField] private float chaseRange = 5f;
    [Tooltip("敵の攻撃可能な範囲")]
    [SerializeField] private float attackRange = 2f;
    [Tooltip("敵の攻撃待機時間")]
    [SerializeField] private float waitTime = 1f;

    [Header("エネミーの基本設定")]
    [Tooltip("上半身パーツの攻撃力と攻撃範囲を反映")]

    [SerializeField] private BodyPartsData upperPart;
    [Tooltip("下半身パーツの攻撃力と攻撃範囲を反映")]

    [SerializeField] private BodyPartsData lowerPart;
    private Gun gun;

    private newEnemyParameters newEnemyParameters;
    private EnemyMoveAnimation enemyMoveAnimation;

    private enum EnemyState { Search, Walk, Attack, Wait, IsDead }

    private EnemyState enemyState = EnemyState.Search;

    private float pointA, pointB;   // 開始位置と終了位置(A: 移動可能範囲の右端/ B: 左端)
    private bool isMovingToPointB = true; // 進行方向
    public PlayerControl player; // プレイヤーの位置
    private float timer;//攻撃後の時間
    [Tooltip("ボスの移動可能な最小X座標")]
    [SerializeField] private float bossMinX = -8f;
    [Tooltip("ボスの移動可能な最大X座標")]
    [SerializeField] private float bossMaxX = 8f;
    void Start()
    {

        // プレイヤーを探すやつ
        player = FindFirstObjectByType<PlayerControl>();




        enemyMoveAnimation = GetComponent<EnemyMoveAnimation>();

        pointA = transform.position.x + moveDistance;
        pointB = transform.position.x - moveDistance;
        newEnemyParameters = GetComponent<newEnemyParameters>();
        if (upperPart.sPartsName == "警察の上半身")
        {
            gun = GetComponent<Gun>();
        }

    }

    void Update()
    {
        // プレイヤーとの距離を計算
        //Debug.Log(distanceToPlayer.ToString());
        switch (GameMgr.GetState())
        {
            case GameState.Main:

                EnemyAction();


                break;
            
            case GameState.ShowText:
            case GameState.Hint:
                break;


        }
    }
    private void MoveTowards(Vector3 target, float moveSpeed)
    {
        // ボスの移動範囲を制限
        if (newEnemyParameters.Boss)
        {
            target.x = Mathf.Clamp(target.x, bossMinX, bossMaxX);
        }
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log($"Collided with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

            //Debug.Log("敵同士が衝突し、回れ右");
            if (isMovingToPointB)
            {
                enemyMoveAnimation?.TurnToRight();
            }
            else
            {
                enemyMoveAnimation?.TurnToLeft();
            }
            isMovingToPointB = !isMovingToPointB;
        }
    }

    // 
    bool IsLeftFromPlayer()
    {
        float Direction = player.transform.position.x - gameObject.transform.position.x;
        if (Direction < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnUpperAttackAnimationFinished()
    {
        // 上半身攻撃判定
        UpperEnemyAttack((float)upperPart.iPartAttack);

    }

    private void OnLowerAttackAnimationFinished()
    {
        LowerEnemyAttack((float)lowerPart.iPartAttack);
    }
    void UpperEnemyAttack(float upperDamage)
    {

        IDamageable damageable = PlayerParameter.Instance.GetComponent<IDamageable>();
        damageable?.TakeDamage(upperDamage, 0);
        Debug.Log("上半身攻撃ダメージ判断");


    }
    void LowerEnemyAttack(float lowerDamage)
    {
        IDamageable damageable = PlayerParameter.Instance.GetComponent<IDamageable>();
        damageable?.TakeDamage(lowerDamage, 1);
        //Debug.Log("下半身攻撃ダメージ判断");

    }

    // 上半身での攻撃とSE判定
    private void AttackWithSeUpper()
    {
        Debug.Log("AttackWithSeUpper");
        //上半身攻撃
        enemyMoveAnimation.PantieStart();

        if (upperPart.sPartsName != "警察の上半身")
        {
            OnUpperAttackAnimationFinished();
        }

        //攻撃者の上半身を確認
        switch (upperPart.sPartsName)
        {
            case "警察の上半身":
                Vector2 ShootMoveVector = (player.transform.position - enemyMoveAnimation.playerRc.transform.position).normalized;
                float enemyRotationY = enemyMoveAnimation.playerRc.transform.eulerAngles.y;
                Debug.Log(enemyRotationY);
                if (enemyRotationY == 180)
                {
                    // 左向きの場合、方向ベクトルのxを反転
                    ShootMoveVector.x = -Mathf.Abs(ShootMoveVector.x);
                }
                else
                {
                    // 右向きの場合、方向ベクトルのxはそのまま
                    ShootMoveVector.x = Mathf.Abs(ShootMoveVector.x);
                }

                // 銃を使って弾を発射
                gun.Shoot(ShootMoveVector, transform, upperPart.iPartAttack);
                //警察官の上半身で攻撃するSEを鳴らす
                MultiAudio.ins.PlaySEByName(
                    "SE_policeofficer_attack_upper");

                Debug.Log("警官が上半身で攻撃");

                break;

            case "ボスの上半身":
                //ラスボス上半身の攻撃音のSEを鳴らす
                MultiAudio.ins.PlaySEByName(
                    "SE_lastboss_attack_upper");

                Debug.Log("ボスが上半身で攻撃");

                break;

            case "キノコの上半身":
                //主人公上半身の攻撃音のSEを鳴らす
                MultiAudio.ins.PlaySEByName(
                    "SE_hero_attack_upper");

                Debug.Log("キノコ雑魚が上半身で攻撃");

                break;

            case "看護師の上半身":
                //ナース上半身の攻撃音のSEを鳴らす
                MultiAudio.ins.PlaySEByName(
                    "SE_nurse_attack_upper");

                Debug.Log("看護師が上半身で攻撃");

                break;
        }

        //MultiAudio.ins.PlaySEByName("SE_common_hit_attack");
    }

    // 下半身での攻撃とSE判定
    private void AttackWithSeLower()
    {
        Debug.Log("AttackWithSeLower");
        //下半身攻撃
        enemyMoveAnimation.KickStart();
        OnLowerAttackAnimationFinished();
        //攻撃者の下半身を確認
        switch (lowerPart.sPartsName)
        {
            case "警察の下半身":
                //警察官下半身の攻撃音のSEをならす
                MultiAudio.ins.PlaySEByName(
                    "SE_policeofficer_attack_lower");

                Debug.Log("警官が下半身で攻撃");
                break;

            case "ボスの下半身":
                //ラスボス下半身の攻撃音のSEを鳴らす
                MultiAudio.ins.PlaySEByName(
                    "SE_lastboss_attack_lower");

                Debug.Log("ボスが下半身で攻撃");

                break;

            case "キノコの下半身":
                //主人公下半身の攻撃音のSEを鳴らす
                MultiAudio.ins.PlaySEByName(
                    "SE_hero_attack_lower");

                Debug.Log("キノコ雑魚が下半身で攻撃");
                break;

            case "看護師の下半身":
                //ナース下半身の攻撃音のSEを鳴らす
                MultiAudio.ins.PlaySEByName(
                    "SE_nurse_attack_lower");

                Debug.Log("看護師が下半身で攻撃");

                break;
        }
    }
    void EnemyAction()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (enemyState)
        {
            case EnemyState.Search:
                enemyMoveAnimation.WalkInstance();
                // プレイヤーが追跡範囲内に入っているかどうか判断
                if (distanceToPlayer < chaseRange)
                {
                    enemyState = EnemyState.Walk;
                }
                else
                {
                    // いつもの挙動
                    float Target = isMovingToPointB ? pointB : pointA;
                    Vector3 target = new Vector3(Target, transform.position.y, transform.position.z);
                    // ボスの場合、移動範囲を制限
                    if (newEnemyParameters.Boss)
                    {
                        target.x = Mathf.Clamp(target.x, bossMinX, bossMaxX);
                    }
                    MoveTowards(target, normalSpeed);

                    // 敵が折り返し地点に到達したかどうか判断
                    if (transform.position == target)
                    {
                        // 到達したら回れ右
                        if (isMovingToPointB == true) enemyMoveAnimation.TurnToRight();
                        else enemyMoveAnimation.TurnToLeft();
                        isMovingToPointB = !isMovingToPointB;
                    }
                }
                break;
            case EnemyState.Walk:
                // プレイヤーを追跡
                enemyMoveAnimation.WalkInstance();
                MoveTowards(player.transform.position, chaseSpeed);
                if (IsLeftFromPlayer() != isMovingToPointB)
                {
                    if (isMovingToPointB == true) enemyMoveAnimation.TurnToRight();
                    else enemyMoveAnimation.TurnToLeft();
                    isMovingToPointB = !isMovingToPointB;

                }
                // プレイヤーが攻撃範囲内に入っているかどうか判断
                if ((distanceToPlayer < upperPart.AttackArea || distanceToPlayer < lowerPart.AttackArea)
                    && IsLeftFromPlayer() == isMovingToPointB)
                {
                    enemyState = EnemyState.Wait;
                }
                break;
            case EnemyState.Wait:
                //moveAnimation.Upright();
                if (timer > waitTime)
                {
                    timer = 0;
                    if ((distanceToPlayer < upperPart.AttackArea || distanceToPlayer < lowerPart.AttackArea) && IsLeftFromPlayer() == isMovingToPointB)
                    {
                        enemyState = EnemyState.Attack;
                    }
                    else
                    {
                        enemyState = EnemyState.Search;
                    }
                    break;
                }
                timer += Time.deltaTime;
                break;
            case EnemyState.Attack:
                if (IsLeftFromPlayer() != isMovingToPointB)
                {
                    // エネミーがプレイヤーを向いていなければ攻撃中止(何もしない)
                }
                else if (distanceToPlayer > upperPart.AttackArea && distanceToPlayer > lowerPart.AttackArea)
                {
                    // 両半身とも攻撃範囲外なら攻撃中止
                }
                else if (distanceToPlayer <= upperPart.AttackArea && distanceToPlayer > lowerPart.AttackArea)
                {
                    AttackWithSeUpper();

                }
                else if (distanceToPlayer > upperPart.AttackArea && distanceToPlayer <= lowerPart.AttackArea)
                {
                    AttackWithSeLower();

                }
                else
                {
                    // 両半身とも攻撃範囲内の場合
                    // 乱数を取得する
                    int num = Random.Range(0, 2);
                    if (num == 0)
                    {
                        AttackWithSeUpper();
                    }
                    else
                    {
                        AttackWithSeLower();
                    }
                }

                enemyState = EnemyState.Search;
                //moveAnimation.PlayerPantie();
                break;
            case EnemyState.IsDead:
                break;
        }
    }
}
// プレイヤーに向かって移動
//ゾンビ(仮)の画像を使っています。本来のオブジェクトにアタッチする。
//プレイヤー(仮)の画像を使っています。TagがPlayerになっていないと動かん
//テスト用にプレイヤーを見つけると移動速度変わるようにしてるけどインスペクターからカスタムできる

