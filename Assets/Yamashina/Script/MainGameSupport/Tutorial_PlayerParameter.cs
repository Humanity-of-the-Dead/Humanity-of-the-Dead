using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_PlayerParameter : PlayerParameter
{
    // Start is called before the first frame update

    Tutorial tutorial;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        tutorial = FindAnyObjectByType<Tutorial>();
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void TakeDamage(float damage, int body = 0)
    {
        base.TakeDamage(damage, body);
        GameMgr.ChangeState(GameState.ShowText);    //GameState‚ªShowText‚É•Ï‚í‚é
        tutorial.ShowTextChange();
    }
}
