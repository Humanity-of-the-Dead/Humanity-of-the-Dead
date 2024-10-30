using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyParameters : MonoBehaviour
{
    //部位の耐久値を設定できる
    [SerializeField]
    private int HP;

    //テスト用　敵に与えるダメージを設定できる
    [SerializeField]
    private int damage;

    //ドロップする画像を設定できる
    [SerializeField]
    private Image deathImage;

    void Update()
    {
        //もし耐久値が0になったらドロップする
        if (HP <= 0)
        {
            ShowDeathImage();
        }
    }
    public void TakeDamage(int damage)
    {
        //HPが減る仕組み
        //damageはテスト用の関数
        HP -= damage;
    }
    void ShowDeathImage()
    {
        //多分ドロップ画像設定するとこ
        if (deathImage != null)
        {
            deathImage.enabled = true;
        }
    }

    //ドロップの挙動作ってないから画面に出るだけなので調節する
    //倒されたら体が消失するプログラムが必要
    //今の時点だと両方ドロップしてしまうので修正する
    //今はImageを入れることになってるけど、ここをSprite入れれるようにしたい

    //このプログラムの動きをテスト用に可視化する
}