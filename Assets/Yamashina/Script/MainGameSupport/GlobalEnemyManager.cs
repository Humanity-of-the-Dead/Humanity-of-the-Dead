using System.Collections.Generic;
using UnityEngine;

public class GlobalEnemyManager : MonoBehaviour
{
    public static GlobalEnemyManager Instance; // シングルトンとして利用
    [Header("1ステージごとの敵の最大数")]
    public int MaxGlobalEnemies = 20; // 全体の敵の最大数
    [Header("敵のオブジェクトのリスト、自動で格納")]

    public List<GameObject> allEnemies = new List<GameObject>(); // 全敵を管理

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        //DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject); // 複数インスタンスを防止
        }
    }

    // 敵を追加する関数
    public bool AddEnemy(GameObject enemy)
    {
        if (allEnemies.Count < MaxGlobalEnemies)
        {
            allEnemies.Add(enemy);
            return true;
        }
        return false;
    }

    // 敵を削除する関数
    public void RemoveEnemy(GameObject enemy)
    {
        if (allEnemies.Contains(enemy))
        {
            allEnemies.Remove(enemy);
        }
        else
        {
            return;
        }
    }

    // 全体の敵数を取得
    public int GetEnemyCount()
    {
        return allEnemies.Count;
    }

   

}
