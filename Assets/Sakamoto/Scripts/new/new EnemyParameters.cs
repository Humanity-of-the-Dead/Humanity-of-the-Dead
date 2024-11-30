using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    public 
    PlayerParameter scPlayerParameter;
    //プレイヤーコントローラ
    public GameObject PlayerControl;

    //ボスフラグ
    [SerializeField]
    bool Boss;

    //クリアテキスト
    [SerializeField]
    GameObject textBox;

    [SerializeField]SceneTransitionManager sceneTransitionManager;
    private void Start()
    {
        scPlayerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();
        sceneTransitionManager =GameObject.FindAnyObjectByType<SceneTransitionManager>();
    }
    void Update()
    {
        //もし耐久値が0になったらドロップする
        if (UpperHP <= 0)
        {
            PlayerControl.GetComponent<PlayerControl>().RemoveListItem(this.gameObject);
            Debug.Log("下半身が落ちたよ");
            Drop(Lowerbodypart,false);
        }
        if (LowerHP <= 0)
        {
            PlayerControl.GetComponent<PlayerControl>().RemoveListItem(this.gameObject);
            Debug.Log("上半身が落ちたよ");
            Drop(Upperbodypart,true);
        }
    }
    //bodyには0か1しか入れてはいけない　BA//GU/RU
    //body : 0->上半身にダメージ
    //body : 1->下半身にダメージ

    public void TakeDamage(int damage, int body = 0)
    {
        //HPが減る仕組み
        //damageはテスト用の関数
    if(body == 0)
        {
            UpperHP -= damage;
        }

    if(body == 1)
        {
            LowerHP -= damage;
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

    //ドロップアイテムを生成する関数　
    //BodyPartsData part->生成した後に与えるパラメータデータ
    //int typ->trueなら上半身が落ちる:falseなら下半身が落ちる
    //デフォルト引数はtrue
    public void Drop(BodyPartsData part , bool typ = true)
    {
        if(typ == true)
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
        this.gameObject.SetActive(false);
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