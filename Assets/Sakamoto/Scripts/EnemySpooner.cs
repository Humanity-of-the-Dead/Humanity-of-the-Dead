using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpooner : MonoBehaviour
{
    //��������I�u�W�F�N�g
    [SerializeField] GameObject goEnemyObject;
    //�v���C���[�p�����[�^�[
    [SerializeField] GameObject goPlayerParameter;
    [SerializeField] PlayerParameter playerParameter;
    //�Q�[���}�l�[�W���[
    [SerializeField] GameMgr gameMgr;
    //�v���C���[�I�u�W�F�N�g
    [SerializeField] GameObject goTarget;

    [SerializeField] List<GameObject> liEnemyList;

    //�^�C�}�[
    float fTimer;

    void Update()
    {
        if(Vector2.Distance(this.transform.position,goTarget.transform.position) < 10)
        {

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            createEnemy();
        }
    }

    //�G�l�~�[�̐���
    void createEnemy()
    {
        //�G�̃C���X�^���X�𐶐�
        liEnemyList.Add(Instantiate(goEnemyObject));
        //�v���C���[�p�����[�^�[��n��
        liEnemyList[liEnemyList.Count-1].GetComponent<EnemyParameters>().PlayerParameter = goPlayerParameter;
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().scPlayerParameter = playerParameter;
        //�Q�[���}�l�[�W���[��n��
        liEnemyList[liEnemyList.Count - 1].GetComponent<newEnemyMovement>().gamestate = gameMgr;
        //�|�W�V�������X�|�i�[���W�ɒu��
        liEnemyList[liEnemyList.Count - 1].transform.position = this.transform.position;

        goTarget.GetComponent<PlayerControl>().AddListItem(liEnemyList[liEnemyList.Count - 1]);
    }
}
