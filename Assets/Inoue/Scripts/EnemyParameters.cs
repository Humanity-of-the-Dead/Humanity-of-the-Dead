using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyParameters : MonoBehaviour
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

    //プレハブのパーツ
    [SerializeField]
    private GameObject prePart;

    GameObject drop;

    //プレイヤーパラメータ-
    public 
    GameObject PlayerParameter;

    //ボスフラグ
    [SerializeField]
    bool Boss;

    //クリアテキスト
    [SerializeField]
    GameObject textBox;

    [SerializeField]SceneTransitionManager sceneTransitionManager;
    private void Start()
    {
        sceneTransitionManager=GameObject.FindAnyObjectByType<SceneTransitionManager>();
    }
    void Update()
    {
        //もし耐久値が0になったらドロップする
        if (UpperHP <= 0)
        {
           
            Drop(Lowerbodypart);
            Debug.Log("下半身が落ちたよ");
        }
        if (LowerHP <= 0)
        {
            Drop(Upperbodypart);
            Debug.Log("上半身が落ちたよ");
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
    public  void Drop(BodyPartsData part)
    {
        //プレハブをインスタンス化
        drop = Instantiate(prePart);

        //生成したパーツを自身の場所に持ってくる
        drop.transform.position = this.transform.position;

        //プレイヤーパラメーターを渡す
        drop.GetComponent<newDropPart>().getPlayerManegerObjet(PlayerParameter);

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


    //ドロップの挙動作ってないから画面に出るだけなので調節する
    //倒されたら体が消失するプログラムが必要
    //今の時点だと両方ドロップしてしまうので修正する
    //今はImageを入れることになってるけど、ここをSprite入れれるようにしたい

    //このプログラムの動きをテスト用に可視化する

    //ダメージのgetとset

}