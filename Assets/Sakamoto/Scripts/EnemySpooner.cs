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
    [SerializeField] PlayerParameter scPlayerParameter;
    //�v���C���[�R���g���[��
    [SerializeField] GameObject goPlayerControl;
    //�Q�[���}�l�[�W���[
    [SerializeField] GameMgr gameMgr;
    //�v���C���[�I�u�W�F�N�g
    [SerializeField] GameObject goTarget;

    [SerializeField] List<GameObject> liEnemyList;

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
        scPlayerParameter = GameObject.FindAnyObjectByType<PlayerParameter>().GetComponent<PlayerParameter>();
        Debug.Log(scPlayerParameter + "���������܂���");
        createEnemy();
        fTimer = 0;
        //�}�[�J�[������
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

    //�G�l�~�[�̐���
    void createEnemy()
    {
        //�G�̃C���X�^���X�𐶐�
        liEnemyList.Add(Instantiate(goEnemyObject));
        //�v���C���[�p�����[�^�[��n��
        liEnemyList[liEnemyList.Count�@-�@1].GetComponent<newEnemyParameters>().scPlayerParameter = this.scPlayerParameter;
        //�v���C���[�R���g���[����n��
        liEnemyList[liEnemyList.Count�@-�@1].GetComponent<newEnemyParameters>().PlayerControl = goPlayerControl;
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().scPlayerParameter = this.scPlayerParameter;
        //�Q�[���}�l�[�W���[��n��
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().gamestate = gameMgr;
        //�|�W�V�������X�|�i�[���W�ɒu��
        liEnemyList[liEnemyList.Count - 1].transform.position = this.transform.position;

        goTarget.GetComponent<PlayerControl>().AddListItem(liEnemyList[liEnemyList.Count - 1]);
    }

    //�Q�[���J�n���̃G�l�~�[����
    IEnumerator startCreate()
    {
        //�G�l�~�[�̍ő吔��-1�̐�������
        if(liEnemyList.Count < fEnemyMax - 1)
        {
            if(fTimer > 1)
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
