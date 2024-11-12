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


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            createEnemy();
        }
    }

    //�G�l�~�[�̐���
    void createEnemy()
    {
        //�G�̃C���X�^���X�𐶐�
        Instantiate(goEnemyObject);
        //�v���C���[�p�����[�^�[��n��
        goEnemyObject.GetComponent<EnemyParameters>().PlayerParameter = goPlayerParameter;
        goEnemyObject.GetComponent<newEnemyMovement>().scPlayerParameter = playerParameter;
        //�Q�[���}�l�[�W���[��n��
        goEnemyObject.GetComponent<newEnemyMovement>().gamestate = gameMgr;
        //�|�W�V�������X�|�i�[���W�ɒu��
        goEnemyObject.transform.position = this.transform.position;

        goTarget.GetComponent<PlayerControl>().AddListItem(goEnemyObject);
    }
}
