using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sEnemyParameters : MonoBehaviour
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
    public GameObject PlayerParameter;
    //プレイヤーコントローラ
    public GameObject PlayerControl;

    //ボスフラグ
    [SerializeField]
    bool Boss;

    //クリアテキスト
    [SerializeField]
    GameObject textBox;


    //エネミーが銃を撃つかどうかを示すフラグ
    public bool canShoot = false;

    // FirePoint の参照
    public Transform firePoint;

    private void Start()
    {

        // canShoot が true の場合のみ FirePoint を探す
        if (canShoot)
        {
            firePoint = transform.Find("FirePoint");

            if (firePoint == null)
            {
                Debug.LogWarning("FirePoint が見つかりませんでした。");
            }
        }
    }

    void Update()
    {
        // もし耐久値が0になったらドロップする
        if (UpperHP <= 0)
        {
            PlayerControl.GetComponent<PlayerControl>().RemoveListItem(this.gameObject);
            Debug.Log("下半身が落ちたよ");
            Drop(Lowerbodypart);
        }
        if (LowerHP <= 0)
        {
            PlayerControl.GetComponent<PlayerControl>().RemoveListItem(this.gameObject);
            Debug.Log("上半身が落ちたよ");
            Drop(Upperbodypart);
        }
    }

    public void TakeDamage(int damage, int body = 0)
    {
        // HPが減る仕組み
        // damageはテスト用の関数
        if (body == 0)
        {
            UpperHP -= damage;
        }

        if (body == 1)
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

    public void Drop(BodyPartsData part)
    {
        // プレハブをインスタンス化
        drop = Instantiate(prePart);

        // 生成したパーツを自身の場所に持ってくる
        drop.transform.position = this.transform.position;

        //// プレイヤーパラメーターを渡す
        //drop.GetComponent<newDropPart>().getPlayerManegerObjet(PlayerParameter);

        // テキストボックスを渡す
        drop.GetComponent<newDropPart>().getTextBox(textBox);

        // ボスフラグを渡す
        drop.GetComponent<newDropPart>().getBossf(Boss);

        // データとシーン遷移マネージャーを渡す
        drop.GetComponent<newDropPart>().getPartsData(part);

        // 自分のゲームオブジェクトを消す
        this.gameObject.SetActive(false);
    }
}

