using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //���[�V�����A�j���X�N���v�g
    [SerializeField] PlayerMoveAnimation scPlayerMoveAnimation;

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
    PlayerParameter playerParameter;


    [SerializeField] SceneTransitionManager sceneTransitionManager;

    [SerializeField] Gun Juu;

    private UpperAttack upperAttack;
    LowerAttack lowerAttack;

    //���e�̃V���b�g�t���O
    bool bShootFlag;
    void Start()
    {


        //����_���ȓz
        //playerParameter = GameObject.FindAnyObjectByType<PlayerParameter>();
        //���ꂢ�����
        playerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();

        rbody2D = GetComponent<Rigidbody2D>();

        // �J�����̍����iorthographicSize�j�̓J�����̒�������㉺�̋�����\��
        fCameraHeight = 2f * goCamera.orthographicSize;

        // �J�����̕��̓A�X�y�N�g��Ɋ�Â��Čv�Z����
        fCameraWidth = fCameraHeight * goCamera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[��Y���W�̐���
        //�v���C���[��Y���W��8.0�𒴂����烊�W�b�h�{�f�B�̃t�H�[�X��0�ɂ���
        if (8.0f < this.transform.position.y)
        {
            this.rbody2D.velocity = new Vector2(0.0f, -1);
        }

        switch (scGameMgr.enGameState)
        {
            case GameState.Main:
                //bShootFlag��false�ɂ���
                bShootFlag = false;
                //�U���A�j���[�V�������łȂ����bShootFlag��true�ɂ���
                Debug.Log(scPlayerMoveAnimation.SetAttack());
                if (scPlayerMoveAnimation.SetAttack() == false)
                {
                    bShootFlag = true;
                    GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = true;

                }

                MainExecution();
                break;
        }
    }

    //�Q�[�����C���̃G�N�X�L���[�g
    void MainExecution()
    {
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
            MultiAudio.ins.PlaySEByName("SE_hero_action_jump");
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

            switch (playerParameter.UpperData.upperAttack)
            {
                case UpperAttack.NORMAL:
                    if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                    {
                        MultiAudio.ins.PlaySEByName("SE_hero_attack_upper");
                        GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = false;
                    }
                    break;

                case UpperAttack.POLICE:
                    Debug.Log("�����ɏe�e�̔��˂̃v���O�����������ł�");
                    //���̉�
                    Vector2 ShootMoveBector = new Vector2(0, 0);
                    //�q��playerRC�̃��[�e�[�V����Y�������Ă���
                    // y = 0�̂Ƃ��͉E�����A0 y = 180�̂Ƃ��͍�����
                    Debug.Log(this.gameObject.transform.GetChild(0).gameObject.transform.eulerAngles.y);
                    if (this.gameObject.transform.GetChild(0).gameObject.transform.eulerAngles.y == 180)
                    {
                        ShootMoveBector.x = -1;
                    }
                    else
                    {
                        ShootMoveBector.x = 1;
                    }

                    Debug.Log(ShootMoveBector);
                    Debug.Log("shootFlag��" + bShootFlag);

                    //bShootFlag��true�Ȃ�e�𔭎˂���
                    if (bShootFlag == true)
                    {
                        Debug.Log("�e����");
                        Juu.Shoot(ShootMoveBector, this.transform);
                        if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                        {
                            MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_upper");
                            GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = false;

                        }
                        //bShootFlag = false;
                    }
                    break;

                case UpperAttack.NURSE:
                    if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                    {
                        MultiAudio.ins.PlaySEByName("SE_nurse_attack_upper");
                        GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = false;
                    }
                    break;
            }

            if (playerParameter.UpperData.sPartsName == "�{�X�̏㔼�g")
            {
                MultiAudio.ins.PlaySEByName("SE_lastboss_attack_upper");
            }

            for (int i = 0; i < liObj.Count; i++)
            {
                Debug.Log(liObj[i].gameObject.transform.position);
                Debug.Log(playerParameter.UpperData.AttackArea);
                //������
                UpperBodyAttack(i, liObj[i].gameObject.transform.position, playerParameter.UpperData.AttackArea, playerParameter.UpperData.iPartAttack);
            }
        }
        //�����g�U��
        if (Input.GetKeyDown(KeyCode.K))
        {
            switch (playerParameter.LowerData.lowerAttack)
            {
                case LowerAttack.NORMAL:
                    if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                    {
                        MultiAudio.ins.PlaySEByName("SE_hero_attack_lower");
                        GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = false;
                    }

                    break;

                case LowerAttack.POLICE:
                    if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                    {
                        MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_lower");
                        GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = false;
                    }
                    break;

                case LowerAttack.NURSE:
                    if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                    {
                        MultiAudio.ins.PlaySEByName("SE_nurse_attack_lower");
                    }
                    break;
            }
            for (int i = 0; i < liObj.Count; i++)
            {
                //������
                LowerBodyAttack(i, liObj[i].gameObject.transform.position, playerParameter.LowerData.AttackArea, playerParameter.LowerData.iPartAttack);
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
        }

    }

    //������
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Car"))
        {
            bJump = false;
        }
    }

    //�㔼�g�U��
    public void UpperBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        float fAttackReach = Vector3.Distance(vTargetPos, this.transform.position);
        if (fAttackReach < fReach)
        {

            liObj[EnemyNum].GetComponent<newEnemyParameters>().TakeDamage(iDamage, 0);
            Debug.Log("�㔼�g�U������");

        }
        else
        {
            Debug.Log("�㔼�g�U�����s");
        }
    }
    //�����g�U��
    public void LowerBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        float fAttackReach = Vector3.Distance(vTargetPos, this.transform.position);
        if (fAttackReach < fReach)
        {
            liObj[EnemyNum].GetComponent<newEnemyParameters>().TakeDamage(iDamage, 1);
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


    //�G�̒e�s�̓����蔻��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyShoot"))
        {
            if (0 < this.transform.position.y - collision.gameObject.transform.position.y)
            {
                playerParameter.UpperHP -= 3;
            }
            else
            {
                playerParameter.LowerHP -= 3;
            }
        }

    }
}
