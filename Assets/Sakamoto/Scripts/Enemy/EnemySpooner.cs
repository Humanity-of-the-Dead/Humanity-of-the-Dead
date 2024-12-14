using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpooner : MonoBehaviour
{
    //��������I�u�W�F�N�g
    [SerializeField] GameObject goEnemyObject;
    ////�v���C���[�p�����[�^�[
    //[SerializeField] GameObject goPlayerParameter;
    /*[SerializeField]*/
    PlayerParameter scPlayerParameter;
    //�v���C���[�R���g���[��
    [SerializeField] GameObject goPlayerControl;
    //�Q�[���}�l�[�W���[
    [SerializeField] GameMgr gameMgr;
    //�v���C���[�I�u�W�F�N�g
    [SerializeField] GameObject goTarget;

    [SerializeField] List<GameObject> liEnemyList;
    [Header("�G���X�|�[������ꏊ�̃����_���I�t�Z�b�g�͈�")]
    [Tooltip("�G���X�|�[������ʒu��X���̃����_���͈́i���̒l���w��\�j")]
    [SerializeField] private float randomOffsetXRange = 2f;

    [Tooltip("�G���X�|�[������ʒu��Y���̃����_���͈́i���̒l���w��\�j")]
    [SerializeField] private float randomOffsetYRange = 2f;

    //�}�[�J�[
    [SerializeField] GameObject goMarker;
    //�^�C�}�[
    float fTimer;
    //�^�C�}�[�̍ő�l
    [SerializeField] float fTimerMax;

    [SerializeField] float fEnemyMax;

    private void Start()
    {
        //PlayerParameter�X�N���v�g���擾
        scPlayerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();
        Debug.Log(scPlayerParameter + "���������܂���");
        createEnemy();
        fTimer = 0;
        //�}�[�J�[������
        goMarker.SetActive(false);
        Debug.Log(scPlayerParameter + "�͌��݂ł�");
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

    //�G�l�~�[�̐���
    void createEnemy()
    {
        // �G�̃C���X�^���X�𐶐�
        liEnemyList.Add(Instantiate(goEnemyObject));

        // �����_���ȃI�t�Z�b�g�𐶐� (�X�|�i�[�̎��͂ɃX�|�[��������)
        // �����_���ȃI�t�Z�b�g���p���\�b�h�Ő���
        Vector3 randomOffset = GenerateRandomSpawnOffset();
        // ��������G�̈ʒu��ݒ�
        liEnemyList[liEnemyList.Count - 1].transform.position = this.transform.position + randomOffset;

        // ���̃p�����[�^�[��ݒ肷��i���̃R�[�h�̂܂܁j
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyParameters>().scPlayerParameter = this.scPlayerParameter;
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyParameters>().PlayerControl = goPlayerControl;
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().scPlayerParameter = this.scPlayerParameter;
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().gamestate = gameMgr;

        // �v���C���[�̃��X�g�ɒǉ�
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
        //�Q�[���J�n���̃G�l�~�[����
        IEnumerator startCreate()
        {
            //�G�l�~�[�̍ő吔��-1�̐�������
            if (liEnemyList.Count < fEnemyMax - 1)
            {
                if (fTimer > 1)
                {
                    //�G�l�~�[����
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
