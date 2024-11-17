using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpooner : MonoBehaviour
{
    //生成するオブジェクト
    [SerializeField] GameObject goEnemyObject;
    //プレイヤーパラメーター
    [SerializeField] GameObject goPlayerParameter;
    [SerializeField] PlayerParameter playerParameter;
    //プレイヤーコントローラ
    [SerializeField] GameObject goPlayerControl;
    //ゲームマネージャー
    [SerializeField] GameMgr gameMgr;
    //プレイヤーオブジェクト
    [SerializeField] GameObject goTarget;

    [SerializeField] List<GameObject> liEnemyList;

    //タイマー
    float fTimer;
    //タイマーの最大値
    [SerializeField] float fTimerMax;

    [SerializeField] float fEnemyMax;

    private void Start()
    {
        //エネミーを上限-1体生み出す
        for(int i = 0; i < fEnemyMax - 1; i++)
        {
            createEnemy();
        }
    }

    void Update()
    {
        if(Vector2.Distance(this.transform.position,goTarget.transform.position) < 20)
        {
            if (fTimer > fTimerMax && liEnemyList.Count < fEnemyMax) 
            {
                createEnemy();
                fTimer = 0;
            }
            fTimer += Time.deltaTime;
        }
        else
        {
            fTimer = 0;
        }

        for(int i = 0; i < liEnemyList.Count; i++)
        {
            if(liEnemyList[i] == null)
            {
                liEnemyList.Remove(liEnemyList[i]);
            } 
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    createEnemy();
        //}
    }

    //エネミーの生成
    void createEnemy()
    {
        //敵のインスタンスを生成
        liEnemyList.Add(Instantiate(goEnemyObject));
        //プレイヤーパラメーターを渡す
        liEnemyList[liEnemyList.Count　-　1].GetComponent<newEnemyParameters>().PlayerParameter = goPlayerParameter;
        //プレイヤーコントローラを渡す
        liEnemyList[liEnemyList.Count　-　1].GetComponent<newEnemyParameters>().PlayerControl = goPlayerControl;
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().scPlayerParameter = playerParameter;
        //ゲームマネージャーを渡す
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().gamestate = gameMgr;
        //ポジションをスポナー座標に置く
        liEnemyList[liEnemyList.Count - 1].transform.position = this.transform.position;

        goTarget.GetComponent<PlayerControl>().AddListItem(liEnemyList[liEnemyList.Count - 1]);
    }
}
