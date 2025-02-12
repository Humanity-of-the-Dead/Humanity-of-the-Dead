using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_PlayerParameter : PlayerParameter
{
    // Start is called before the first frame update
    private Tutorial tutorial;
    private bool isDamage = false;
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
        if(!isDamage) {
            GameMgr.ChangeState(GameState.ShowText);    //GameState��ShowText�ɕς��
            tutorial.UpdateText();
            Tutorial.NextState();   
            //�e�L�X�g�\�����\����
            tutorial.TextArea.SetActive(true);
        }
        
        isDamage = true;    


    }
}
