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

    [SerializeField] List<GameObject> liEnemyList;

    //タイマー
    float fTimer;

    void Update()
    {
        if(Vector2.Distance(this.transform.position,goTarget.transform.position) < 10)
        {

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            createEnemy();
        }
    }

    //エネミーの生成
    void createEnemy()
    {
        //敵のインスタンスを生成
        liEnemyList.Add(Instantiate(goEnemyObject));
        //プレイヤーパラメーターを渡す
        liEnemyList[liEnemyList.Count-1].GetComponent<EnemyParameters>().PlayerParameter = goPlayerParameter;
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().scPlayerParameter = playerParameter;
        //ゲームマネージャーを渡す
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().gamestate = gameMgr;
        //ポジションをスポナー座標に置く
        liEnemyList[liEnemyList.Count - 1].transform.position = this.transform.position;

        goTarget.GetComponent<PlayerControl>().AddListItem(liEnemyList[liEnemyList.Count - 1]);
    }
}
