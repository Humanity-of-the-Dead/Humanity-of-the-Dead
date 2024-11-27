using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpooner : MonoBehaviour
{
    //生成するオブジェクト
    [SerializeField] GameObject goEnemyObject;
    ////プレイヤーパラメーター
    //[SerializeField] GameObject goPlayerParameter;
    [SerializeField] PlayerParameter scPlayerParameter;
    //プレイヤーコントローラ
    [SerializeField] GameObject goPlayerControl;
    //ゲームマネージャー
    [SerializeField] GameMgr gameMgr;
    //プレイヤーオブジェクト
    [SerializeField] GameObject goTarget;

    [SerializeField] List<GameObject> liEnemyList;

    //マーカー
    [SerializeField] GameObject goMarker;
    //タイマー
    float fTimer;
    //タイマーの最大値
    [SerializeField] float fTimerMax;

    [SerializeField] float fEnemyMax;

    private void Start()
    {
        //PlayerParameterスクリプトを取得
        scPlayerParameter = GameObject.FindAnyObjectByType<PlayerParameter>().GetComponent<PlayerParameter>();
        Debug.Log(scPlayerParameter + "が代入されました");
        createEnemy();
        fTimer = 0;
        //マーカーを消す
        goMarker.SetActive(false);

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
        liEnemyList[liEnemyList.Count　-　1].GetComponent<newEnemyParameters>().scPlayerParameter = this.scPlayerParameter;
        //プレイヤーコントローラを渡す
        liEnemyList[liEnemyList.Count　-　1].GetComponent<newEnemyParameters>().PlayerControl = goPlayerControl;
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().scPlayerParameter = this.scPlayerParameter;
        //ゲームマネージャーを渡す
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().gamestate = gameMgr;
        //ポジションをスポナー座標に置く
        liEnemyList[liEnemyList.Count - 1].transform.position = this.transform.position;

        goTarget.GetComponent<PlayerControl>().AddListItem(liEnemyList[liEnemyList.Count - 1]);
    }

    //ゲーム開始時のエネミー生成
    IEnumerator startCreate()
    {
        //エネミーの最大数の-1体生成する
        if(liEnemyList.Count < fEnemyMax - 1)
        {
            if(fTimer > 1)
            {
                //エネミー生成
                createEnemy();
                fTimer = 0;
            }
            fTimer += Time.deltaTime; 
            yield return null;
        }
        else
        {
            yield break;
        }
    }
}
