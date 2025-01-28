using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    //ボスフラグ
    [SerializeField, Header("ボスかどうか、チェックが入っているならボス")]
    public bool Boss;

    [SerializeField,Header("敵が打ってくる一個の弾のダメージ量")] float bulletDamage ;    //  //クリアテキスト
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

        playerControl = GameObject.Find("Player Variant").GetComponent<PlayerControl>();
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

        // 部位が破壊された際にHPバーを一瞬表示
        if (UpperHP <= 0)
        {
            playerControl.RemoveListItem(this.gameObject);
            //Debug.Log("上半身が破壊された");
            //Drop(Upperbodypart, false);
            StartCoroutine(ShowHPBarAndDestroy(UpperHPBar, Lowerbodypart, false));

        }
        if (LowerHP <= 0)
        {
            playerControl.RemoveListItem(this.gameObject);
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
            ShowHitEffects(body);
            Debug.Log(hitGameObject);

            UpdateHPBar(UpperHPBar, UpperHP, MaxUpperHP);
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");

            Debug.Log(UpperHP);
            Debug.Log(MaxUpperHP);

        }

        if (body == 1)
        {
            //下半身のHPを減らす
            LowerHP -= (int)damage;
            ShowHitEffects(body);
            Debug.Log(hitGameObject);
            UpdateHPBar(LowerHPBar, LowerHP, MaxLowerHP);
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");

            Debug.Log(LowerHP);
            Debug.Log(MaxLowerHP);
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
    //攻撃がヒットしたエフェクト(オブジェクト)を出す
    public override void ShowHitEffects(int body)
    {
        //このオブジェクトの座標
        Vector3 enemyVector3 = new Vector3(transform.position.x,transform.position.y);

        

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
        }
    }


    private IEnumerator ShowHPBarAndDestroy(Image hpBar, BodyPartsData part, bool typ)
    {
        if (hpBar != null)
        {

            hpBar.gameObject.SetActive(true);
            yield return new WaitForSeconds(hpBarDestroy); // 継続時間は調整可能
            hpBar.gameObject.SetActive(false);
        }
        Drop(part, typ);
        MultiAudio.ins.PlaySEByName("SE_common_breakbody");

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
        }
        else
        {
            //プレハブをインスタンス化
            drop = Instantiate(preLowerPart);
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
            TakeDamage(bulletDamage, 0);
        }
    }

    //ドロップの挙動作ってないから画面に出るだけなので調節する
    //倒されたら体が消失するプログラムが必要
    //今の時点だと両方ドロップしてしまうので修正する
    //今はImageを入れることになってるけど、ここをSprite入れれるようにしたい

    //このプログラムの動きをテスト用に可視化する

    //ダメージのgetとset

}