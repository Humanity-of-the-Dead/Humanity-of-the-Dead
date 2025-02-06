using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEditor.UIElements.ToolbarMenu;

public class EnemySpooner : MonoBehaviour
{
    [SerializeField] private GameObject goEnemyObject;
    [SerializeField] private PlayerControl playerControl;

    [SerializeField] private List<GameObject> liEnemyList;

    [Header("敵がスポーンする場所のランダムオフセット範囲")]
    [SerializeField] private float randomOffsetXRange = 2f;

    //[SerializeField] GameObject goMarker;
    private float fTimer;
    [Header("何秒後に敵が出るか")]

    [SerializeField] private float fTimerMax;
    [Header("各スポナーから出る敵の最大数")]

    [SerializeField] private float fEnemyMax;

    private void Start()
    {
        createEnemy();
        fTimer = 0;
    }

    void Update()
    {
        if (GameMgr.GetState() == GameState.Main)
        {
            if (IsPlayerInRange() && CanSpawnEnemy())
            {
                //Debug.Log(IsPlayerInRange());
                //Debug.Log(CanSpawnEnemy()); 
                fTimer += Time.deltaTime;

                if (fTimer > fTimerMax)
                {

                    createEnemy();
                    fTimer = 0;
                }
            }
            else
            {
                fTimer = 0;
            }
            //Debug.Log("fTimerは" + fTimer);

            // 敵リストから削除された敵をクリーンアップ
            CleanupEnemyList();
        }
    }

    // プレイヤーが範囲内かつ右側にいるかを確認
    private bool IsPlayerInRange()
    {
        float distance = Vector2.Distance(transform.position, playerControl.transform.position);
        float positionDifference = this.transform.position.x - playerControl.transform.position.x;
        return distance < 20 && positionDifference > 0;
    }

    // 敵がスポーン可能かを確認
    private bool CanSpawnEnemy()
    {
        return liEnemyList.Count < fEnemyMax &&
               GlobalEnemyManager.Instance.GetEnemyCount() < GlobalEnemyManager.Instance.MaxGlobalEnemies;
    }

    // 敵リストから null を削除
    private void CleanupEnemyList()
    {
        liEnemyList.RemoveAll(enemy => enemy == null);
    }

    // エネミーの生成
    private void createEnemy()
    {
        GameObject newEnemy = Instantiate(goEnemyObject);
        Vector3 randomOffset = GenerateRandomSpawnOffset();
        newEnemy.transform.position = transform.position + randomOffset;

        // グローバルに登録成功したらローカルリストにも追加
        if (GlobalEnemyManager.Instance.AddEnemy(newEnemy))
        {
            liEnemyList.Add(newEnemy);

            newEnemy.GetComponent<newEnemyParameters>().playerControl = playerControl;//???
            playerControl.AddListItem(newEnemy);
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
    private Vector3 GenerateRandomSpawnOffset()
    {
        return new Vector3(
            Random.Range(-randomOffsetXRange, randomOffsetXRange),
           this.transform.position.y, 0
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
