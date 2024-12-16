using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpooner : MonoBehaviour
{
    [SerializeField] GameObject goEnemyObject;
    PlayerParameter scPlayerParameter;
    [SerializeField] GameObject goPlayerControl;
    [SerializeField] GameMgr gameMgr;
    [SerializeField] GameObject goTarget;

    [SerializeField] List<GameObject> liEnemyList;

    [Header("敵がスポーンする場所のランダムオフセット範囲")]
    [SerializeField] private float randomOffsetXRange = 2f;
    [SerializeField] private float randomOffsetYRange = 2f;

    [SerializeField] GameObject goMarker;
    float fTimer;
    [SerializeField] float fTimerMax;
    [SerializeField] float fEnemyMax;

    private void Start()
    {
        scPlayerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();
        Debug.Log(scPlayerParameter + "が代入されました");
        createEnemy();
        fTimer = 0;
        goMarker.SetActive(false);
    }

    void Update()
    {
        if (GameMgr.GetState() == GameState.Main)
        {
            if (Vector2.Distance(this.transform.position, goTarget.transform.position) < 20
                && this.transform.position.x - goTarget.transform.position.x > 0)
            {
                if (fTimer > fTimerMax && liEnemyList.Count < fEnemyMax && GlobalEnemyManager.Instance.GetEnemyCount() < GlobalEnemyManager.Instance.MaxGlobalEnemies)
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
                    liEnemyList.RemoveAt(i);
                }
            }
        }
    }

    // エネミーの生成
    void createEnemy()
    {
        GameObject newEnemy = Instantiate(goEnemyObject);
        Vector3 randomOffset = GenerateRandomSpawnOffset();
        newEnemy.transform.position = this.transform.position + randomOffset;

        // グローバルに登録成功したらローカルリストにも追加
        if (GlobalEnemyManager.Instance.AddEnemy(newEnemy))
        {
            liEnemyList.Add(newEnemy);

            newEnemy.GetComponent<newEnemyParameters>().scPlayerParameter = this.scPlayerParameter;
            newEnemy.GetComponent<newEnemyParameters>().PlayerControl = goPlayerControl;
            newEnemy.GetComponent<newEnemyMovement>().scPlayerParameter = this.scPlayerParameter;
            newEnemy.GetComponent<newEnemyMovement>().gamestate = gameMgr;
            goTarget.GetComponent<PlayerControl>().AddListItem(newEnemy);
        }
        else
        {
            Destroy(newEnemy); // 上限を超える場合は生成を中止
        }
    }
    private void InitializeReferences()
    {
        if (GlobalEnemyManager.Instance.MaxGlobalEnemies > GlobalEnemyManager.Instance.GetEnemyCount())
        {
            GlobalEnemyManager.Instance.allEnemies.RemoveAll(GlobalEnemyManager.Instance.AddEnemy);
        }
    }
    public Vector3 GenerateRandomSpawnOffset()
    {
        return new Vector3(
            Random.Range(-randomOffsetXRange, randomOffsetXRange),
            Random.Range(-randomOffsetYRange, randomOffsetYRange),
            0
        );
    }
    private void OnEnable()
    {
        // シーンがロードされた後に参照を再取得
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // イベントの解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーン遷移後に参照を再取得
        InitializeReferences();
        
        Debug.Log($"シーン {scene.name} がロードされました");
    }

}
