using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsManager : MonoBehaviour
{
    private GameState enGameState;
    
    //各種パラメータ
    [SerializeField] string sPartsName;   //部位の名前
    [SerializeField] int iHP;     //ヒットポイント   
    [SerializeField] int iAttack;     //攻撃力

    [SerializeField] GameObject goTextBox1;
    [SerializeField] GameObject goTextBox2;
    [SerializeField] GameObject goTextBox3;

    float time;
    // Start is called before the first frame update
    void Start()
    {
        //ゲームステートの初期化
        enGameState = GameState.Main;

        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        goTextBox1.gameObject.GetComponent<Text>().text = sPartsName;
        goTextBox2.gameObject.GetComponent<Text>().text = ($"{iHP}");
        goTextBox3.gameObject.GetComponent<Text>().text = ($"{iAttack}");

        if(time > 1 && iHP > 0)
        {
            iHP -= 1;
            time = 0;
        }
    }

    public void setParameter(BodyPartsData data)
    {
        sPartsName = data.sPartsName;
        iHP = data.iPartHp;
        iAttack = data.iPartAttack;
    }

}
