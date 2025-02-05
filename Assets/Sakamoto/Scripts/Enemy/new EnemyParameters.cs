using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class newEnemyParameters : CharacterStats
{
    //部位の耐久値を設定できる
    [SerializeField, Header("敵の上半身HP")]
    private int UpperHP;
    [SerializeField, Header("敵の下半身HP")]
    private int LowerHP;



    //ボディパーツ
    [SerializeField, Header("敵の上半身データ")]
    private BodyPartsData Upperbodypart;

    [SerializeField, Header("敵の下半身データ")]
    private BodyPartsData Lowerbodypart;

    //上半身のドロップパーツ
    [SerializeField, Header("敵の上半身ドロップパーツ")]
    private GameObject preUpperPart;

    //下半身のドロップパーツ
    [SerializeField, Header("敵の下半身ドロップパーツ")]
    private GameObject preLowerPart;


    [SerializeField, Header("プレイヤーコントロールのスクリプト代入\n自動で入るため何も入れない")]

    //プレイヤーコントローラ
    public PlayerControl playerControl;

    private PlayerMoveAnimation playerMoveAnimation;

    //ボスフラグ
    [SerializeField, Header("ボスかどうか、チェックが入っているならボス")]
    public bool Boss;

    [SerializeField, Header("両部位への攻撃が必要か？チェックが入っているなら必要")]
    public bool needsAttackingBothParts;

    [SerializeField, Header("敵が打ってくる一個の弾のダメージ量")] float bulletDamage;    //  //クリアテキスト
                                                                         //  [SerializeField]
                                                                         //private  GameObject textBox;



    //敵のHPゲージ関連
    [SerializeField, Header("敵の上半身ゲージのイメージコンポーネントを設定\n必ずUpperHP_Barが入っていることを確認")]
    private Image UpperHPBar;

    [SerializeField, Header("敵の下半身ゲージのイメージコンポーネントを設定\n必ずLowerHP_Barが入っていることを確認")]
    private Image LowerHPBar;
    // HPバー全体のコンテナ 
    [SerializeField, Header("敵のゲージのオブジェクトを設定\n必ずHPBar_Objectが入っていることを確認")]
    private GameObject HPBarContainer;
    //各敵キャラの最大HP
    private int MaxUpperHP;
    private int MaxLowerHP;

    [SerializeField, Header("HPバーを表示する距離")]
    [Tooltip("プレイヤーと敵キャラクターの距離がこの値以下の場合にHPバーを表示します。\n" +
             "値を小さくするとプレイヤーが近づかないとHPバーが表示されなくなり、\n" +
             "値を大きくすると遠くからでもHPバーが表示されます。")]
    private float displayRange = 0.3f;
    [SerializeField, Header("敵を倒してからHPバーが消えるまでの秒数")]
    [Tooltip("HPが0の状態のHPが表示されてからのカウントです。")]
    private float hpBarDestroy = 0.3f;
    //private Transform player; // プレイヤーの位置
    //点滅エフェクト
    private Renderer[] enemyRenderer;


    [SerializeField, Header("点滅の継続時間")] private float flashDuration = 1.0f;
    [SerializeField, Header("点滅の間隔 (秒)")] private float flashInterval = 0.1f;

    private bool isFlashing = false;

    private bool hasDroped = false;

    //public bool isDropInstantiated = false;
    private void Start()
    {
        MaxLowerHP = LowerHP;
        MaxUpperHP = UpperHP;
        // HPバーを非表示に設定
        if (HPBarContainer != null)
        {
            HPBarContainer.SetActive(false);
        }
        else
        {
            Debug.LogWarning("HPBarContainerがnull");
        }
        enemyRenderer = transform.GetComponentsInChildren<Renderer>();
        playerControl = GameObject.Find("Player Variant").GetComponent<PlayerControl>();
        playerMoveAnimation = playerControl.GetComponent<PlayerMoveAnimation>();
        //Debug.Log(playerControl);

    }


    void Update()
    {
        float DistanceToPlayer = Vector3.Distance(transform.position, playerControl.transform.position);
        // プレイヤーが一定距離以内にいる場合にHPバーを表示playerControl
        if (DistanceToPlayer < displayRange)
        {
            if (HPBarContainer != null)
            {
                HPBarContainer.SetActive(true);
            }
        }
        else
        {
            if (HPBarContainer != null)
            {
                HPBarContainer.SetActive(false);
            }
        }

        AdjustHpIfNeededAttackingBothParts();

        // 部位が破壊された際にHPバーを一瞬表示
        if (UpperHP <= 0)
        {
            playerControl.RemoveListItem(this.gameObject);
            StartCoroutine(FlashObject());

            if (Boss==true)
            {
                Debug.Log("ボスエフェクト開始");
                playerMoveAnimation.ShowHitEffectsBoss(0, transform.position);
                Debug.Log("ボスエフェクトコルーチン関数呼び出し終了");

            }

            //Debug.Log("上半身が破壊された");
            //Drop(Upperbodypart, false);
            StartCoroutine(ShowHPBarAndDestroy(UpperHPBar, Lowerbodypart, false));



        }
        if (LowerHP <= 0)
        {
            playerControl.RemoveListItem(this.gameObject);
            StartCoroutine(FlashObject());

            if (Boss)
            {
                playerMoveAnimation.ShowHitEffectsBoss(1, transform.position);

            }

            //Debug.Log("下半身が破壊された");
            //Drop(Lowerbodypart, true);

            StartCoroutine(ShowHPBarAndDestroy(LowerHPBar, Upperbodypart, true));




        }
        //if (GameMgr.GetState() == GameState.ShowText&&!Boss)
        //{
        //    Destroy(this.gameObject);   
        //}

    }


    //bodyには0か1しか入れてはいけない　BA//GU/RU
    //body : 0->上半身にダメージ
    //body : 1->下半身にダメージ

    public override void TakeDamage(float damage, int body = 0)
    {
        //HPが減る仕組み
        //damageはテスト用の関数
        if (body == 0)
        {
            //上半身のHPを減らす
            UpperHP -= (int)damage;
            //ShowHitEffects(body);
            playerMoveAnimation.ShowHitEffects(body, transform.position);

            UpdateHPBar(UpperHPBar, UpperHP, MaxUpperHP);
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");

            //Debug.Log(UpperHP);
            //Debug.Log(MaxUpperHP);

        }

        if (body == 1)
        {
            //下半身のHPを減らす
            LowerHP -= (int)damage;
            //ShowHitEffects(body);
            playerMoveAnimation.ShowHitEffects(body, transform.position);
            UpdateHPBar(LowerHPBar, LowerHP, MaxLowerHP);
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");

            //Debug.Log(LowerHP);
            //Debug.Log(MaxLowerHP);
        }
    }
    //敵のHPバーを変更
    private void UpdateHPBar(Image hpBarMask, float currentHP, float maxHP)
    {
        if (hpBarMask != null)
        {
            // Fill Amountを現在のHP比率に更新
            hpBarMask.fillAmount = currentHP / maxHP;
        }
    }

    private IEnumerator FlashObject()
    {
        isFlashing = true;
        float elapsedTime = 0;
        while (elapsedTime < flashDuration)
        {
            foreach (Renderer r in enemyRenderer)
            {
                r.enabled = !r.enabled;
            }
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval;
        }

        foreach (Renderer r in enemyRenderer)
        {
            r.enabled = true;
        }
        isFlashing = false;
    }

    private IEnumerator ShowHPBarAndDestroy(Image hpBar, BodyPartsData part, bool typ)
    {
        if (hpBar != null)
        {

            hpBar.gameObject.SetActive(true);
            yield return new WaitForSeconds(hpBarDestroy); // 継続時間は調整可能
            hpBar.gameObject.SetActive(false);
        }
        if (!hasDroped)
        {
            hasDroped = true;
            Drop(part, typ);
            MultiAudio.ins.PlaySEByName("SE_common_breakbody");
        }
    }
    //ドロップアイテムを生成する関数　
    //BodyPartsData part->生成した後に与えるパラメータデータ
    //int typ->trueなら上半身が落ちる:falseなら下半身が落ちる
    //デフォルト引数はtrue
    public void Drop(BodyPartsData part, bool typ = true)
    {
        GameObject drop = null;
        if (typ == true)
        {
            //プレハブをインスタンス化
            drop = Instantiate(preUpperPart);
            //isDropInstantiated = true;

        }
        else
        {
            //プレハブをインスタンス化
            drop = Instantiate(preLowerPart);
            //isDropInstantiated = true;

        }

        //生成したパーツを自身の場所に持ってくる
        drop.transform.position = transform.position;




        //ボスフラグを渡す
        drop.GetComponent<newDropPart>().getBossf(Boss);



        //
        drop.GetComponent<newDropPart>().getPartsData(part);
        //自分のゲームオブジェクトを消す
        GlobalEnemyManager.Instance.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShoot"))
        {
            bulletDamage = (float)collision.gameObject.GetComponent<Bullet>().attack;
            TakeDamage(bulletDamage, 0);
        }
    }

    // 両部位への攻撃が必要な場合はHPの調整を行なう
    private void AdjustHpIfNeededAttackingBothParts()
    {
        if (needsAttackingBothParts)
        {
            // 部位HPが1の時、その部位のHPを削り切ったものとする
            // 上半身HPが0になった時、かつ下半身HPを削り切っていないとき
            if (UpperHP <= 0 && LowerHP > 1)
            {
                // HPを破壊手前で止める
                UpperHP = 1;
            }
            // 下半身
            else if (LowerHP <= 0 && UpperHP > 1)
            {
                // HPを破壊手前で止める
                LowerHP = 1;
            }
        }
    }

    //ドロップの挙動作ってないから画面に出るだけなので調節する
    //倒されたら体が消失するプログラムが必要
    //今の時点だと両方ドロップしてしまうので修正する
    //今はImageを入れることになってるけど、ここをSprite入れれるようにしたい

    //このプログラムの動きをテスト用に可視化する

    //ダメージのgetとset

}