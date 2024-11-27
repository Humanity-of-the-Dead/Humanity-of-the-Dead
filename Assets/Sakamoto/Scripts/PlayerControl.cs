using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //�Q�[���}�l�[�W���[
    [SerializeField] GameMgr scGameMgr;

    private Rigidbody2D rbody2D;
    [Header("�ړ��X�s�[�h")]
    [SerializeField] float fSpeed;
    [Header("�W�����v��")]
    [SerializeField] float fJmpPower;
    bool bJump = false;


    //�J�����֘A
    [SerializeField] Camera goCamera;
    //����
    float fCameraHeight;
    //��
    float fCameraWidth;

    //�^�[�Q�b�g
    [SerializeField] List<GameObject> liObj;
    //[SerializeField] GameObject[] goObj;

    //�v���C���[�p�����[�^�[�̎擾
    [SerializeField] PlayerParameter playerParameter;

    public GameObject gunObject;    //Gun�R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g
    private Gun gun;                //Gun�X�N���v�g�̃C���X�^���X

    void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();

        // �J�����̍����iorthographicSize�j�̓J�����̒�������㉺�̋�����\��
        fCameraHeight = 2f * goCamera.orthographicSize;

        // �J�����̕��̓A�X�y�N�g��Ɋ�Â��Čv�Z����
        fCameraWidth = fCameraHeight * goCamera.aspect;

        gun = gunObject.GetComponent<Gun>();    //gunObject�ɃA�^�b�`����Ă���Gun�R���|�[�l���g���擾
    }

    // Update is called once per frame
    void Update()
    {
        switch (scGameMgr.enGameState)
        {
            case GameState.Main:
                //���݂̃|�W�V�������擾
                Vector2 vPosition = this.transform.position;

                //�J�����Ƃ̋����̐�Βl�����ȉ��Ȃ�v���C���[�������@��ʊO�ɏo�Ȃ����߂̏��u
                //�ړ�
                Vector3 vPosFromCame = this.transform.position - goCamera.transform.position; //�J������̃v���C���[�̈ʒu
                                                                                              //���ړ�
                if (Input.GetKey(KeyCode.A))
                {
                    if (vPosFromCame.x > -fCameraWidth / 2)
                    {
                        vPosition.x -= Time.deltaTime * fSpeed;
                    }
                }
                //�E�ړ�
                if (Input.GetKey(KeyCode.D))
                {
                    if (fCameraWidth / 2 > vPosFromCame.x)
                    {
                        vPosition.x += Time.deltaTime * fSpeed;
                    }
                }

                //�W�����v

                if (Input.GetKey(KeyCode.W) && bJump == false)
                {
                    this.rbody2D.AddForce(this.transform.up * fJmpPower);
                    bJump = true;
                }

                //�̂���]���Ȃ��悤�ɂ���
                //������transform���擾
                Quaternion quaternion = GetComponent<Transform>().rotation;
                quaternion.z = 0.0f;
                transform.rotation = quaternion;


                //�ړ���̃|�W�V��������
                this.transform.position = vPosition;

                //�U���֘A
                //�㔼�g�U��
                if (Input.GetKeyDown(KeyCode.I))
                {
                    for (int i = 0; i < liObj.Count; i++)
                    {
                        Debug.Log(liObj[i].gameObject.transform.position);
                        Debug.Log(playerParameter.UpperData.AttackArea);
                        //������
                        UpperBodyAttack(i, liObj[i].gameObject.transform.position, playerParameter.UpperData.AttackArea);
                    }
                }
                //�����g�U��
                if (Input.GetKeyDown(KeyCode.K))
                {
                    for (int i = 0; i < liObj.Count; i++)
                    {
                        //������
                        LowerBodyAttack(i, liObj[i].gameObject.transform.position, playerParameter.LowerData.AttackArea);
                    }
                }
                //�������U��
                if(Input.GetKeyDown(KeyCode.U))
                {
                    gun.Shoot();
                }
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Car"))
        {
            bJump = false;
        }
    }

    //�㔼�g�U��
    public void UpperBodyAttack(int EnemyNum,Vector3 vTargetPos, float fReach)
    {
        float fAttackReach = Vector3.Distance(vTargetPos,this.transform.position);
        if(fAttackReach < fReach)
        {
            liObj[EnemyNum].GetComponent<newEnemyParameters>().TakeDamage(1,0);
            Debug.Log("�㔼�g�U������");
        }
        else
        {
            Debug.Log("�㔼�g�U�����s");
        }
    }
    //�����g�U��
    public void LowerBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach)
    {
        float fAttackReach = Vector3.Distance(vTargetPos,this.transform.position);
        if(fAttackReach < fReach)
        {
            liObj[EnemyNum].GetComponent<newEnemyParameters>().TakeDamage(1, 1);
            Debug.Log("�����g�U������");
        }
        else
        {
            Debug.Log("�����g�U�����s");
        }
    }

    public void AddListItem(GameObject obj)
    {
        liObj.Add(obj);
    }
    public void RemoveListItem(GameObject obj)
    {
        liObj.Remove(obj);
    }
}
