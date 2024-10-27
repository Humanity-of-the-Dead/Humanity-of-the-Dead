using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameter : MonoBehaviour
{
    [Header("1減少するのにかかる時間")]
    [SerializeField] int iDownTime;
    float fTimer; //タイマー

    [SerializeField] int iHumanity;     //人間性
    [SerializeField] int iUpperHP;      //上半身のHP
    [SerializeField] int iLowerHP;      //下半身のHP
    // Start is called before the first frame update

    private void Start()
    {
        //タイマー初期化
        int fTimer = 0;
    }
    private void Update()
    {
        //タイマーの値がiDownTimeを超えたらパラメータを1減らす
        if(fTimer > iDownTime)
        {
            iHumanity--;
            iUpperHP--;
            iLowerHP--;
            //タイマーを0に戻す
            fTimer = 0;
            return;
        }
        fTimer += Time.deltaTime; 
    }

    //人間性の取得
    public int getHumanity()
    {
        return iHumanity;
    }
    //上半身HPの取得
    public int getUpperHP() { 
        return iUpperHP;
    }
    //下半身HPの取得
    public int getLowerHP()
    {
        return iLowerHP;
    }
}
