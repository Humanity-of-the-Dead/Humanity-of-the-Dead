using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropPart : MonoBehaviour
{
    //パーツのデータ
    private BodyPartsData partsData;
    //プレイヤーのmanager
    PlayerParameter playerManager;
    void Start()
    {
        //アイテムの画像になる
        
    }

    // Update is called once per frame
    void Update()
    {
        //Pキーを押したら移植する
        if (Input.GetKeyDown(KeyCode.P)){
            playerManager.transplant(partsData);
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
        image.sprite = partsData.sPartSprite; 
    }
}
