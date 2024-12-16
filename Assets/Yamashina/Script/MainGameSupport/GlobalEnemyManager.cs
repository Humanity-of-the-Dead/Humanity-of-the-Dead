using System.Collections.Generic;
using UnityEngine;

public class GlobalEnemyManager : MonoBehaviour
{
    public static GlobalEnemyManager Instance; // �V���O���g���Ƃ��ė��p
    public int MaxGlobalEnemies = 20; // �S�̂̓G�̍ő吔
    private List<GameObject> allEnemies = new List<GameObject>(); // �S�G���Ǘ�

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �����C���X�^���X��h�~
        }
    }

    // �G��ǉ�����֐�
    public bool AddEnemy(GameObject enemy)
    {
        if (allEnemies.Count < MaxGlobalEnemies)
        {
            allEnemies.Add(enemy);
            return true;
        }
        return false;
    }

    // �G���폜����֐�
    public void RemoveEnemy(GameObject enemy)
    {
        if (allEnemies.Contains(enemy))
        {
            allEnemies.Remove(enemy);
        }
    }

    // �S�̂̓G�����擾
    public int GetEnemyCount()
    {
        return allEnemies.Count;
    }
}
