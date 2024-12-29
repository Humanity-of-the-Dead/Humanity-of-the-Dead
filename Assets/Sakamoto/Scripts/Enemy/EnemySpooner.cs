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

    [Header("�G���X�|�[������ꏊ�̃����_���I�t�Z�b�g�͈�")]
    [SerializeField] private float randomOffsetXRange = 2f;

    [SerializeField] GameObject goMarker;
    float fTimer;
    [Header("���b��ɓG���o�邩")]

    [SerializeField] float fTimerMax;
    [Header("�e�X�|�i�[����o��G�̍ő吔")]

    [SerializeField] float fEnemyMax;

    private void Start()
    {
        scPlayerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();
        Debug.Log(scPlayerParameter + "���������܂���");
        createEnemy();
        fTimer = 0;
        goMarker.SetActive(false);
    }

    void Update()
    {
        if (GameMgr.GetState() == GameState.Main)
        {
            if (IsPlayerInRange() && CanSpawnEnemy())
            {
                Debug.Log(IsPlayerInRange());
                Debug.Log(CanSpawnEnemy()); 
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
            Debug.Log("fTimer��" + fTimer);

            // �G���X�g����폜���ꂽ�G���N���[���A�b�v
            CleanupEnemyList();
        }
    }

    // �v���C���[���͈͓����E���ɂ��邩���m�F
    private bool IsPlayerInRange()
    {
        float distance = Vector2.Distance(this.transform.position, goTarget.transform.position);
        float positionDifference = this.transform.position.x - goTarget.transform.position.x;
        return distance < 20 && positionDifference > 0;
    }

    // �G���X�|�[���\�����m�F
    private bool CanSpawnEnemy()
    {
        return liEnemyList.Count < fEnemyMax &&
               GlobalEnemyManager.Instance.GetEnemyCount() < GlobalEnemyManager.Instance.MaxGlobalEnemies;
    }

    // �G���X�g���� null ���폜
    private void CleanupEnemyList()
    {
        liEnemyList.RemoveAll(enemy => enemy == null);
    }

    // �G�l�~�[�̐���
    void createEnemy()
    {
        GameObject newEnemy = Instantiate(goEnemyObject);
        Vector3 randomOffset = GenerateRandomSpawnOffset();
        newEnemy.transform.position = this.transform.position + randomOffset;

        // �O���[�o���ɓo�^���������烍�[�J�����X�g�ɂ��ǉ�
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
            Destroy(newEnemy); // ����𒴂���ꍇ�͐����𒆎~
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
           this.transform.position.y, 0
        );
    }
    private void OnEnable()
    {
        // �V�[�������[�h���ꂽ��ɎQ�Ƃ��Ď擾
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // �C�x���g�̉���
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �V�[���J�ڌ�ɎQ�Ƃ��Ď擾
        InitializeReferences();

        Debug.Log($"�V�[�� {scene.name} �����[�h����܂���");
    }

}
