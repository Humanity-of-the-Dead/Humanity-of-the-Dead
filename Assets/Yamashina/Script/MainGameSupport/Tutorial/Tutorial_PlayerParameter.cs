using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tutorial;

public class Tutorial_PlayerParameter : PlayerParameter
{
    // Start is called before the first frame update
    private Tutorial tutorial;

    [SerializeField,Header("minHP以下になったら体力の減少を止める")] private float minHP = 10.0f; 

    //private bool isDamage = false;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        tutorial = FindFirstObjectByType<Tutorial>();

        base.Start();
    }
    protected override void Update()
    {

        switch (GameMgr.GetState())
        {
            case GameState.Main:
                DecreasingHP();

                if (iHumanity < 0 || iUpperHP < 0 || iLowerHP < 0)
                {
                    iHumanity = minHP;
                    iUpperHP = minHP;
                    iLowerHP = minHP;
                }
                break;



        }
    }
    public override void TakeDamage(float damage, int body = 0)
    {
        base.TakeDamage(damage, body);

        if (Tutorial.GetState() == Tutorial_State.PlayerDoNotMove)
        {
            GameMgr.ChangeState(GameState.ShowText);    //GameStateがShowTextに変わる
            tutorial.UpdateText();

            Tutorial.ChangeState(Tutorial_State.PlayerAttack);
            //テキスト表示域を表示域
            tutorial.TextArea.SetActive(true);
        }





    }
    protected override void DecreasingHP()
    {
        if (iHumanity > minHP)
            iHumanity -= Time.deltaTime / iDownTime;
        else
            iHumanity = minHP; 


        if (iUpperHP > minHP)
            iUpperHP -= Time.deltaTime / iDownTime;
        else
            iUpperHP = minHP; // ぴったり止める


        if (iLowerHP > minHP)
            iLowerHP -= Time.deltaTime / iDownTime;
        else
            iLowerHP = minHP;
    }
}
