using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class PlayerParameter : MonoBehaviour
{
    //ゲームマネージャー
    GameMgr scGameMgr;

    public static PlayerParameter Instance;

    [Header("1減少するのにかかる時間")]
    [SerializeField] int iDownTime;

    public  int iHumanityMax;     //人間性の最大値
    public  int iUpperHPMax;      //上半身のHPの最大値
    public  int iLowerHPMax;      //下半身のHPの最大値

    private float iHumanity;     //人間性
    private float iUpperHP;      //上半身のHP
    private float iLowerHP;      //下半身のHP
    // Start is called before the first frame update

    [Header("プレイヤーオブジェクト")]
    [SerializeField] GameObject goPlayer;

    //上半身のパーツデータ
    public BodyPartsData UpperDefaultData;
    //下半身のパーツデータ
    public BodyPartsData LowerDefaultData;

    public void Awake()
    {
        CheckInstance();
    }
    private void Start()
    {
        //最大値を設定
        iUpperHPMax = UpperDefaultData.iPartHp;
        iLowerHPMax = LowerDefaultData.iPartHp;
        //パラメータの初期化
        iHumanity = iHumanityMax;
        iUpperHP = iUpperHPMax;
        iLowerHP = iLowerHPMax;

        //シーン遷移で破棄されない
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        //パラメータの値をiDownTime秒で1減少させる
        iHumanity -= Time.deltaTime / iDownTime;
        iUpperHP -= Time.deltaTime / iDownTime;
        iLowerHP -= Time.deltaTime / iDownTime;

        //シーン移動
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("Stage2");
        }
    }

    //慰霊
    //人間性を引数分回復する
    public void comfort(int iRecovery)
    {
        iHumanity += iRecovery;
        //回復した値が最大値を超えていたら最大値にする
        if(iHumanity > iHumanityMax)
        {
            iHumanity = iHumanityMax;
        }
    }
    //移植
    //パーツの画像とパラメータを入れ替える
    //BodyPartsData partsData : 入れ替えるパーツのスクリプタブルオブジェクト
    //テスト段階では引数はnullでいい
    public void transplant(BodyPartsData partsData)
    {
        //partsData = partsData ?? DefaultData;

        //キャラのイメージ取得用
        SpriteRenderer spriteRenderer;

        switch (partsData.enPartsType)
        {
            case PartsType.Upper:
                //パーツデータのHPをMaxに代入
                iUpperHPMax = partsData.iPartHp;
                iUpperHP = iUpperHPMax;
                //SpriteRendererコンポーネント取得
                spriteRenderer = goPlayer.transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
                /*
                //SpriteRendererのSpriteにパーツデータのSpriteを挿入
                spriteRenderer.sprite = partsData.spBody;
                */
                //見た目変更関数待ち
                break;
            case PartsType.Lower:
                //パーツデータのHPをMax代入
                iLowerHPMax = partsData.iPartHp;
                iLowerHP = iLowerHPMax;
                //SpriteRendererコンポーネント取得
                spriteRenderer = goPlayer.transform.GetChild(1).transform.GetComponent<SpriteRenderer>();
                /*
                //SpriteRendererのSpriteにパーツデータのSpriteを挿入
                spriteRenderer.sprite = partsData.spWaist;
                */
                //見た目変更関数待ち
                break;
        }

    }


    //人間性の取得
    public float Humanity
    {
        get { return iHumanity; }
        set { iHumanity = value; }
    }
    //上半身HPの取得
    public float UpperHP
    {
        get { return iUpperHP; }
        set { iUpperHP = value; }
    }
    //下半身HPの取得
    public float LowerHP
    {
        get { return iLowerHP; }
        set { iLowerHP = value; }
    }

    //シングルトンのチェック
    void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
