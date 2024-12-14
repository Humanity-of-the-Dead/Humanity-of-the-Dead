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
    /*[SerializeField]*/
    PlayerParameter scPlayerParameter;
    //プレイヤーコントローラ
    [SerializeField] GameObject goPlayerControl;
    //ゲームマネージャー
    [SerializeField] GameMgr gameMgr;
    //プレイヤーオブジェクト
    [SerializeField] GameObject goTarget;

    [SerializeField] List<GameObject> liEnemyList;
    [Header("敵がスポーンする場所のランダムオフセット範囲")]
    [Tooltip("敵がスポーンする位置のX軸のランダム範囲（負の値も指定可能）")]
    [SerializeField] private float randomOffsetXRange = 2f;

    [Tooltip("敵がスポーンする位置のY軸のランダム範囲（負の値も指定可能）")]
    [SerializeField] private float randomOffsetYRange = 2f;

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
        scPlayerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();
        Debug.Log(scPlayerParameter + "が代入されました");
        createEnemy();
        fTimer = 0;
        //マーカーを消す
        goMarker.SetActive(false);
        Debug.Log(scPlayerParameter + "は健在です");
    }

    void Update()
    {
        if (GameMgr.GetState() == GameState.Main)
        {
            if (Vector2.Distance(this.transform.position, goTarget.transform.position) < 20
    && this.transform.position.x - goTarget.transform.position.x > 0)
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

            for (int i = 0; i < liEnemyList.Count; i++)
            {
                if (liEnemyList[i] == null)
                {
                    liEnemyList.Remove(liEnemyList[i]);
                }
            }

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    createEnemy();
            //}

        }
    }

    //エネミーの生成
    void createEnemy()
    {
        // 敵のインスタンスを生成
        liEnemyList.Add(Instantiate(goEnemyObject));

        // ランダムなオフセットを生成 (スポナーの周囲にスポーンさせる)
        // ランダムなオフセットを専用メソッドで生成
        Vector3 randomOffset = GenerateRandomSpawnOffset();
        // 生成する敵の位置を設定
        liEnemyList[liEnemyList.Count - 1].transform.position = this.transform.position + randomOffset;

        // 他のパラメーターを設定する（元のコードのまま）
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyParameters>().scPlayerParameter = this.scPlayerParameter;
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyParameters>().PlayerControl = goPlayerControl;
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().scPlayerParameter = this.scPlayerParameter;
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().gamestate = gameMgr;

        // プレイヤーのリストに追加
        goTarget.GetComponent<PlayerControl>().AddListItem(liEnemyList[liEnemyList.Count - 1]);

    }
    public Vector3 GenerateRandomSpawnOffset()
    {
        return new Vector3(
            Random.Range(-randomOffsetXRange, randomOffsetXRange),
            Random.Range(-randomOffsetYRange, randomOffsetYRange),
            0
        );
    }
        //ゲーム開始時のエネミー生成
        IEnumerator startCreate()
        {
            //エネミーの最大数の-1体生成する
            if (liEnemyList.Count < fEnemyMax - 1)
            {
                if (fTimer > 1)
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
