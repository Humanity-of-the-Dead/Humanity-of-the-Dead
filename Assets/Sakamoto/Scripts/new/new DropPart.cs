using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newDropPart : MonoBehaviour
{
    //パーツのデータ
    private BodyPartsData partsData;
    ////プレイヤーのmanager
    //PlayerParameter playerManager;

    //プレイヤー情報
    GameObject goPlayerParameter;

    //クリアテキスト
    GameObject goTextBox;

    //ボスフラグ
    bool bBoss;
     SceneTransitionManager sceneTransitionManager;

    //ボタンオブジェクト
    [SerializeField] GameObject[] goButton;


    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        //Jキーを押したら慰霊する
        if(Input.GetKeyUp(KeyCode.J) && goButton[1].activeSelf == true) {
            goPlayerParameter.GetComponent<PlayerParameter>().comfort(10);
            if (bBoss)
            {
                goTextBox.GetComponent<GoalScript>().showText();
                sceneTransitionManager.SceneChange(SceneInformation.SCENE.Title);

            }
            Destroy(this.gameObject);

        }
        //Lキーを押したら移植する
        if (Input.GetKeyDown(KeyCode.L) && goButton[0].activeSelf == true)
        {
            goPlayerParameter.GetComponent<PlayerParameter>().transplant(partsData);
            if (bBoss)
            {
                goTextBox.GetComponent<GoalScript>().showText();
                sceneTransitionManager.SceneChange(SceneInformation.SCENE.Title);
            }
            Destroy(this.gameObject);
        }
    }

    //パーツデータの取得
    public void getPartsData(BodyPartsData partsData)
    {
        this.partsData = partsData;
    }
    //アイテムの画像になる
    public void setImnage()
    {
        Image image = this.GetComponent<Image>();
        image.sprite = partsData.spBody; 
    }

    public void getPlayerManegerObjet(GameObject obj)
    {
        goPlayerParameter = obj;
    }

    //テキストボックスの取得
    public void getTextBox(GameObject obj)
    {
        goTextBox = obj;
    }
    //ボスフラグ
    public void getBossf(bool flag)
    {
        bBoss = flag;
    }
    
    //移植
    public void getTransplant()
    {
        goPlayerParameter.GetComponent<PlayerParameter>().transplant(partsData);
        Destroy(this.gameObject);
    }

    //慰霊
    public void getComfort()
    {
        goPlayerParameter.GetComponent<PlayerParameter>().comfort(10);
        Destroy(this.gameObject);
    }
    public void getSceneTransition(SceneTransitionManager sceneTransitionManager)
    {
       this.sceneTransitionManager = sceneTransitionManager;    

    }


}
