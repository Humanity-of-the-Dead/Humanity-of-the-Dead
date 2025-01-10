using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //���[�V�����A�j���X�N���v�g
    [SerializeField]  private PlayerMoveAnimation playerMoveAnimation;

    //�Q�[���}�l�[�W���[
    [SerializeField] private GameMgr scGameMgr;

    private Rigidbody2D rbody2D;
    [Header("�ړ��X�s�[�h")]
    [SerializeField] private float fSpeed;
    [Header("�W�����v��")]
    [SerializeField] private float fJmpPower;
    private bool bJump = false;
    //�A���W�����v
    [SerializeField] private int Jmpconsecutive;


    //�J�����֘A
    [SerializeField] private Camera goCamera;
    //����
    private float fCameraHeight;
    //��
    private float fCameraWidth;

    //�^�[�Q�b�g
    [SerializeField] private List<GameObject> liObj;
    //[SerializeField] GameObject[] goObj;

    //�v���C���[�p�����[�^�[�̎擾
    private PlayerParameter playerParameter;



    [SerializeField] private Gun Gun;

    private UpperAttack upperAttack;
   private LowerAttack lowerAttack;

    //���e�̃V���b�g�t���O
    private bool bShootFlag;
    void Start()
    {


        //����_���ȓz
        //playerParameter = GameObject.FindAnyObjectByType<PlayerParameter>();
        //���ꂢ�����
        playerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();
        playerMoveAnimation = GetComponent<PlayerMoveAnimation>();  
        Gun = GetComponent<Gun>();  
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

        switch (GameMgr.GetState())
        {
            case GameState.Main:
                //bShootFlag��false�ɂ���
                bShootFlag = false;
                //�U���A�j���[�V�������łȂ����bShootFlag��true�ɂ���
                //Debug.Log(playerMoveAnimation.SetAttack());
                if (playerMoveAnimation.SetAttack() == false)
                {
                    bShootFlag = true;
                    MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = true;

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

        if (!playerMoveAnimation.SetAttack())
        {
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

            if (Input.GetKey(KeyCode.W) && Jmpconsecutive < 1)
            {
                this.rbody2D.AddForce(this.transform.up * fJmpPower);
                MultiAudio.ins.PlaySEByName("SE_hero_action_jump");
                bJump = true;
                Jmpconsecutive++;
            }

            //�y�Ɏ��̃V�[���s�������Ȃ炱�̉��̃R�[�h���R�����g�A�E�g�����@�m�F��R�����g�A�E�g���Ă�����

            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    SceneTransitionManager.instance.NextSceneButton(SceneTransitionManager.instance.sceneInformation.GetCurrentScene() + 1); 
            //}
            //�����܂�
            //�y�Ƀ{�X��s�������Ȃ�ȉ��̃R�[�h���R�����g����
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    vPosition = new Vector2(190.0f, -1.536416f);
            //}
            //�����܂�

        }

        //�̂���]���Ȃ��悤�ɂ���
        //������transform���擾
        Quaternion quaternion = GetComponent<Transform>().rotation;
        quaternion.z = 0.0f;
        transform.rotation = quaternion;


        //�ړ���̃|�W�V��������
        this.transform.position = vPosition;

        #region �R�i�ύX
        //�A�j���[�V�����������ŌĂԂ��߁A�ǋL
        //�U���֘A
            if (!playerMoveAnimation.SetAttack() && playerMoveAnimation.timeAttack <0)
            {
            //�㔼�g�U��
            if (Input.GetKeyDown(KeyCode.I))
            {
                playerMoveAnimation.PantieStart();
                #endregion
                switch (playerParameter.UpperData.upperAttack)
                {
                    case UpperAttack.NORMAL:
                        if (MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay)
                        {
                            MultiAudio.ins.PlaySEByName("SE_hero_attack_upper");
                            MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = false;
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
                            Gun.Shoot(ShootMoveBector, this.transform);
                            if (MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay)
                            {
                                MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_upper");
                                MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = false;

                            }
                            //bShootFlag = false;
                        }
                        break;

                    case UpperAttack.NURSE:
                        if (MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay)
                        {
                            MultiAudio.ins.PlaySEByName("SE_nurse_attack_upper");
                            MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = false;
                        }
                        break;
                }

                if (playerParameter.UpperData.sPartsName == "�{�X�̏㔼�g")
                {
                    MultiAudio.ins.PlaySEByName("SE_lastboss_attack_upper");
                }

                for (int i = 0; i < liObj.Count; i++)
                {
                    //Debug.Log(liObj[i].gameObject.transform.position);
                    //Debug.Log(playerParameter.UpperData.AttackArea);
                    //������
                    UpperBodyAttack(i, liObj[i].gameObject.transform.position, playerParameter.UpperData.AttackArea, playerParameter.UpperData.iPartAttack);
                }
            }
            //�����g�U��
            if (Input.GetKeyDown(KeyCode.K))
            {
                #region �R�i�ύX
                playerMoveAnimation.KickStart();
                #endregion
                switch (playerParameter.LowerData.lowerAttack)
                {
                    case LowerAttack.NORMAL:
                        if (MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay)
                        {
                            MultiAudio.ins.PlaySEByName("SE_hero_attack_lower");
                            MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = false;
                        }

                        break;

                    case LowerAttack.POLICE:
                        if (MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay)
                        {
                            MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_lower");
                            MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay = false;
                        }
                        break;

                    case LowerAttack.NURSE:
                        if (MultiAudio.ins.seSource.GetComponent<SoundCoolTime>().canPlay)
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
        }
      

    }

    //������
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Car"))
        {
            bJump = false;
            Jmpconsecutive = 0;
        }
    }

    //�㔼�g�U��
    public void UpperBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        float fAttackReach = Vector3.Distance(vTargetPos, this.transform.position);
        if (fAttackReach < fReach)
        {

            liObj[EnemyNum].GetComponent<newEnemyParameters>().TakeDamage(iDamage, 0);
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");

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
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");
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
            if (0 > this.transform.position.y - collision.gameObject.transform.position.y)
            {
                playerParameter.UpperHP -= 1;
            }
            else
            {
                playerParameter.LowerHP -= 1;
            }
        }

    }
}
