using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class newEnemyParameters : MonoBehaviour
{
    //部位の耐久値を設定できる
    [SerializeField]
    private int UpperHP;

    [SerializeField]
    private int LowerHP;

    //テスト用　敵に与えるダメージを設定できる
    [SerializeField]
    private int damage;

    ////ドロップする画像を設定できる
    //[SerializeField]
    //private Image deathImage;

    //ボディパーツ
    [SerializeField]
    private BodyPartsData Upperbodypart;

    [SerializeField]
    private BodyPartsData Lowerbodypart;

    //上半身のドロップパーツ
    [SerializeField]
    private GameObject preUpperPart;

    //下半身のドロップパーツ
    [SerializeField]
    private GameObject preLowerPart;

    GameObject drop;

    //プレイヤーパラメータ-
    public PlayerParameter scPlayerParameter;
    //プレイヤーコントローラ
    public GameObject PlayerControl;

    //ボスフラグ
    [SerializeField]
    bool Boss;

    //クリアテキスト
    [SerializeField]
    GameObject textBox;

    [SerializeField] SceneTransitionManager sceneTransitionManager;

    //敵のHPゲージ関連
    [SerializeField]
    private Image UpperHPBar;

    [SerializeField]
    private Image LowerHPBar;
    // HPバー全体のコンテナ 
    [SerializeField]
    private GameObject HPBarContainer;
    //各敵キャラの最大HP
    private int MaxUpperHP;
    private int MaxLowerHP;

    // HPバーを表示する距離
    // この値は、敵キャラクターとプレイヤーの距離がこの範囲内に入ったときにHPバーを表示するためのものです。
    // 具体的には、プレイヤーと敵キャラクターの位置間の距離が displayRange に設定された数値より小さい場合、HPバーが表示されます。
    // 逆に、この距離を超えるとHPバーは非表示になります。
    // ※この値を小さくすると、プレイヤーに近づかないとHPバーが表示されなくなり、
    // 　大きくすると遠くからでもHPバーが表示されるようになります。
    [Header("HPバーを表示する距離")]
    [Tooltip("プレイヤーと敵キャラクターの距離がこの値以下の場合にHPバーを表示します。\n" +
             "値を小さくするとプレイヤーが近づかないとHPバーが表示されなくなり、\n" +
             "値を大きくすると遠くからでもHPバーが表示されます。")]
    [SerializeField]
    private float displayRange = 5f;
    [Header("HPバーを表示してから、")]
    [Tooltip("プレイヤーと敵キャラクターの距離がこの値以下の場合にHPバーを表示します。\n" +
            "値を小さくするとプレイヤーが近づかないとHPバーが表示されなくなり、\n" +
            "値を大きくすると遠くからでもHPバーが表示されます。")]
    [SerializeField]
    private float hpBarDestory = 0.3f;
    private Transform player; // プレイヤーの位置

    //ヒットエフェクト
    [SerializeField] GameObject hitGameObject;
    //エフェクトの出現範囲
    //上半身
    [SerializeField] float upperEffectXMin, upperEffectXMax, upperEffectYMin, upperEffectYMax;

    //下半身
    [SerializeField] float lowerEffectXMin, lowerEffectXMax, lowerEffectYMin, lowerEffectYMax;
    //\エフェクト・・・

    private void Start()
    {
        player = GameObject.Find("Player Variant").gameObject.transform;
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
        scPlayerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();
        sceneTransitionManager = GameObject.FindAnyObjectByType<SceneTransitionManager>();
    }


    void Update()
    {
        float DistanceToPlayer = Vector3.Distance(transform.position, player.position);
        // プレイヤーが一定距離以内にいる場合にHPバーを表示
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
            PlayerControl.GetComponent<PlayerControl>().RemoveListItem(this.gameObject);
            Debug.Log("上半身が破壊された");
            //Drop(Upperbodypart, false);
            MultiAudio.ins.PlaySEByName("SE_common_breakbody");
            StartCoroutine(ShowHPBarAndDestroy(UpperHPBar, Upperbodypart, true));
        }
        if (LowerHP <= 0)
        {
            PlayerControl.GetComponent<PlayerControl>().RemoveListItem(this.gameObject);
            Debug.Log("下半身が破壊された");
            //Drop(Lowerbodypart, true);
            MultiAudio.ins.PlaySEByName("SE_common_breakbody");
            StartCoroutine(ShowHPBarAndDestroy(LowerHPBar, Lowerbodypart, false))
                ;
        }
    }


    //bodyには0か1しか入れてはいけない　BA//GU/RU
    //body : 0->上半身にダメージ
    //body : 1->下半身にダメージ

    public void TakeDamage(int damage, int body = 0)
    {
        //HPが減る仕組み
        //damageはテスト用の関数
        if (body == 0)
        {
            //上半身のHPを減らす
            UpperHP -= damage;
            ShowHitEffects(body);
            UpdateHPBar(UpperHPBar, UpperHP, MaxUpperHP);
            Debug.Log(UpperHP);
            Debug.Log(MaxUpperHP);

        }

        if (body == 1)
        {
            //下半身のHPを減らす
            LowerHP -= damage;
            ShowHitEffects(body);
            UpdateHPBar(LowerHPBar, LowerHP, MaxLowerHP);
            Debug.Log(LowerHP);
            Debug.Log(MaxLowerHP);
        }
    }
    //敵のHPバーを変更
    private void UpdateHPBar(Image hpBarMask, int currentHP, int maxHP)
    {
        if (hpBarMask != null)
        {
            // Fill Amountを現在のHP比率に更新
            hpBarMask.fillAmount = (float)currentHP / maxHP;
        }
    }
    //攻撃がヒットしたエフェクト(オブジェクト)を出す
    void ShowHitEffects(int body)
    {
        //このオブジェクトの座標
        Vector2 tihsVec2 = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

        //エフェクト
        GameObject obj = null;

        //上半身の場合
        if (body == 0)
        {
            //オブジェクトを出すローカル座標
            Vector2 effectVec2 = new Vector2(
                Random.Range(upperEffectXMin, upperEffectXMax),
                Random.Range(upperEffectYMin, upperEffectYMax));

            //オブジェクトを出す
            obj = Instantiate(hitGameObject, effectVec2 + tihsVec2, Quaternion.identity);
            // Debug.Log("effectVec2+thisVec2="+effectVec2+tihsVec2)
            // Debug.Log("hit effect");
        }

        if (body == 1)
        {
            //オブジェクトを出すローカル座標
            Vector2 effectVec2 = new Vector2(
                Random.Range(lowerEffectXMin, lowerEffectXMax),
                Random.Range(lowerEffectYMin, lowerEffectYMax));

            obj = Instantiate(hitGameObject, effectVec2 + tihsVec2, Quaternion.identity);
        }
    }

    void ShowDeathImage()
    {
        ////多分ドロップ画像設定するとこ
        //if (deathImage != null)
        //{
        //    deathImage.enabled = true;
        //}
    }
    private IEnumerator ShowHPBarAndDestroy(Image hpBar, BodyPartsData part, bool typ)
    {
        if (hpBar != null)
        {

            hpBar.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f); // 継続時間は調整可能
            hpBar.gameObject.SetActive(false);
        }
        Drop(part, typ);
    }
    //ドロップアイテムを生成する関数　
    //BodyPartsData part->生成した後に与えるパラメータデータ
    //int typ->trueなら上半身が落ちる:falseなら下半身が落ちる
    //デフォルト引数はtrue
    public void Drop(BodyPartsData part, bool typ = true)
    {
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
        drop.transform.position = this.transform.position;

        //プレイヤーパラメーターを渡す
        drop.GetComponent<newDropPart>().getPlayerManegerObjet(scPlayerParameter);

        //テキストボックスを渡す
        drop.GetComponent<newDropPart>().getTextBox(textBox);

        //ボスフラグを渡す
        drop.GetComponent<newDropPart>().getBossf(Boss);



        //
        drop.GetComponent<newDropPart>().getPartsData(part);
        drop.GetComponent<newDropPart>().getSceneTransition(sceneTransitionManager);
        //自分のゲームオブジェクトを消す
        GlobalEnemyManager.Instance.RemoveEnemy(this.gameObject);
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShoot"))
        {
            TakeDamage(1, 0);
        }
    }

    //ドロップの挙動作ってないから画面に出るだけなので調節する
    //倒されたら体が消失するプログラムが必要
    //今の時点だと両方ドロップしてしまうので修正する
    //今はImageを入れることになってるけど、ここをSprite入れれるようにしたい

    //このプログラムの動きをテスト用に可視化する

    //ダメージのgetとset

}