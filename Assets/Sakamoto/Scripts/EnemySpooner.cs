using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpooner : MonoBehaviour
{
    //生成するオブジェクト
    [SerializeField] GameObject goEnemyObject;
    //プレイヤーパラメーター
    [SerializeField] GameObject goPlayerParameter;
    [SerializeField] PlayerParameter playerParameter;
    //ゲームマネージャー
    [SerializeField] GameMgr gameMgr;
    //プレイヤーオブジェクト
    [SerializeField] GameObject goTarget;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            createEnemy();
        }
    }

    //エネミーの生成
    void createEnemy()
    {
        //敵のインスタンスを生成
        Instantiate(goEnemyObject);
        //プレイヤーパラメーターを渡す
        goEnemyObject.GetComponent<EnemyParameters>().PlayerParameter = goPlayerParameter;
        goEnemyObject.GetComponent<newEnemyMovement>().scPlayerParameter = playerParameter;
        //ゲームマネージャーを渡す
        goEnemyObject.GetComponent<newEnemyMovement>().gamestate = gameMgr;
        //ポジションをスポナー座標に置く
        goEnemyObject.transform.position = this.transform.position;

        goTarget.GetComponent<PlayerControl>().AddListItem(goEnemyObject);
    }
}
