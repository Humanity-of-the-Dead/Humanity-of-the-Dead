using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tutorial;

public class Tutorial_PlayerParameter : PlayerParameter
{
    // Start is called before the first frame update
    private Tutorial tutorial;
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
        base.Update();
    }
    public override void TakeDamage(float damage, int body = 0)
    {
        base.TakeDamage(damage, body);

        if (Tutorial.GetState() == Tutorial_State.PlayerDoNotMove)
        {
            GameMgr.ChangeState(GameState.ShowText);    //GameState��ShowText�ɕς��
            tutorial.UpdateText();

            Tutorial.ChangeState(Tutorial_State.PlayerAttack);
            //�e�L�X�g�\�����\����
            tutorial.TextArea.SetActive(true);
        }
         




    }
    protected override void DecreasingHP()
    {
        base.DecreasingHP();
    }
}
