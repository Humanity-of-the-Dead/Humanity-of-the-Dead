using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // ���[�V�����A�j���X�N���v�g
    [SerializeField] PlayerMoveAnimation scPlayerMoveAnimation;
    // �Q�[���}�l�[�W���[
    [SerializeField] GameMgr scGameMgr;
    private Rigidbody2D rbody2D;

    [Header("�ړ��X�s�[�h")]
    [SerializeField] float fSpeed;

    [Header("�W�����v��")]
    [SerializeField] float fJmpPower;

    bool bJump = false;
    [SerializeField] int Jmpconsecutive;

    // �J�����֘A
    [SerializeField] Camera goCamera;
    float fCameraHeight;
    float fCameraWidth;

    // �^�[�Q�b�g
    [SerializeField] List<GameObject> liObj;

    PlayerParameter playerParameter;

    [SerializeField] SceneTransitionManager sceneTransitionManager;

    [SerializeField] Gun Juu;

    private UpperAttack upperAttack;
    private LowerAttack lowerAttack;

    bool bShootFlag;

    // Start is called before the first frame update
    void Start()
    {
        playerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();
        rbody2D = GetComponent<Rigidbody2D>();

        fCameraHeight = 2f * goCamera.orthographicSize;
        fCameraWidth = fCameraHeight * goCamera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        bool debug = true;

#if UNITY_EDITOR

        
            AutoPlayController autoPlayController = gameObject.GetComponent<AutoPlayController>();
            autoPlayController.enabled = true;

#else
            autoPlayController.enabled = false;


#endif

        // �v���C���[��Y���W����
        if (this.transform.position.y > 8.0f)
        {
            rbody2D.velocity = new Vector2(0.0f, -1);
        }

        switch (GameMgr.GetState())
        {
            case GameState.Main:
                bShootFlag = false;
                if (!scPlayerMoveAnimation.SetAttack())
                {
                    bShootFlag = true;
                    GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = true;
                }

                MainExecution();
                break;
        }
    }

    // �Q�[�����C���̃G�N�X�L���[�g
    void MainExecution()
    {
        Vector2 vPosition = this.transform.position;
        Vector3 vPosFromCame = this.transform.position - goCamera.transform.position;

        if (!scPlayerMoveAnimation.SetAttack())
        {
            // ���ړ�
            if (Input.GetKey(KeyCode.A))
            {
                if (vPosFromCame.x > -fCameraWidth / 2)
                {
                    vPosition.x -= Time.deltaTime * fSpeed;
                }
            }
            // �E�ړ�
            if (Input.GetKey(KeyCode.D))
            {
                if (fCameraWidth / 2 > vPosFromCame.x)
                {
                    vPosition.x += Time.deltaTime * fSpeed;
                }
            }

            // �W�����v
            if (Input.GetKey(KeyCode.W) && Jmpconsecutive < 1)
            {
                rbody2D.AddForce(this.transform.up * fJmpPower);
                MultiAudio.ins.PlaySEByName("SE_hero_action_jump");
                bJump = true;
                Jmpconsecutive++;
            }
        }

        // �̂̉�]�h�~
        Quaternion quaternion = GetComponent<Transform>().rotation;
        quaternion.z = 0.0f;
        transform.rotation = quaternion;

        this.transform.position = vPosition;

        // �U������
        if (!scPlayerMoveAnimation.SetAttack() && scPlayerMoveAnimation.timeAttack < 0)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                UpperBodyAttack();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                LowerBodyAttack();
            }
        }
    }

    // �㔼�g�U��
    public void UpperBodyAttack()
    {
        scPlayerMoveAnimation.PantieStart();

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
                Vector2 ShootMoveBector = new Vector2(0, 0);
                if (this.gameObject.transform.GetChild(0).gameObject.transform.eulerAngles.y == 180)
                {
                    ShootMoveBector.x = -1;
                }
                else
                {
                    ShootMoveBector.x = 1;
                }

                if (bShootFlag)
                {
                    Juu.Shoot(ShootMoveBector, this.transform);
                    if (GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay)
                    {
                        MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_upper");
                        GameObject.FindGameObjectWithTag("SE").GetComponent<SoundCoolTime>().canPlay = false;
                    }
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

        for (int i = 0; i < liObj.Count; i++)
        {
            UpperBodyAttack(i, liObj[i].gameObject.transform.position, playerParameter.UpperData.AttackArea, playerParameter.UpperData.iPartAttack);
        }
    }

    // �����g�U��
    public void LowerBodyAttack()
    {
        scPlayerMoveAnimation.KickStart();

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
            LowerBodyAttack(i, liObj[i].gameObject.transform.position, playerParameter.LowerData.AttackArea, playerParameter.LowerData.iPartAttack);
        }
    }

    // �㔼�g�U���̃_���[�W����
    public void UpperBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        float fAttackReach = Vector3.Distance(vTargetPos, this.transform.position);
        if (fAttackReach < fReach)
        {
            liObj[EnemyNum].GetComponent<newEnemyParameters>().TakeDamage(iDamage, 0);
        }
    }

    // �����g�U���̃_���[�W����
    public void LowerBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        float fAttackReach = Vector3.Distance(vTargetPos, this.transform.position);
        if (fAttackReach < fReach)
        {
            liObj[EnemyNum].GetComponent<newEnemyParameters>().TakeDamage(iDamage, 1);
        }
    }

    // �W�����v�̔���
    public bool CanJump()
    {
        return Jmpconsecutive < 1 && !scPlayerMoveAnimation.SetAttack();
    }

    // �W�����v
    public void Jump()
    {
        rbody2D.AddForce(this.transform.up * fJmpPower);
        MultiAudio.ins.PlaySEByName("SE_hero_action_jump");
        bJump = true;
        Jmpconsecutive++;
    }

    // ���ړ�
    public void MoveLeft()
    {
        Vector2 vPosition = this.transform.position;
        vPosition.x -= Time.deltaTime * fSpeed;
        this.transform.position = vPosition;
    }

    // �E�ړ�
    public void MoveRight()
    {
        Vector2 vPosition = this.transform.position;
        vPosition.x += Time.deltaTime * fSpeed;
        this.transform.position = vPosition;
    }

    // �G�e�Ƃ̓����蔻��
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

    // ���X�g�A�C�e���̒ǉ�
    public void AddListItem(GameObject obj)
    {
        liObj.Add(obj);
    }

    // ���X�g�A�C�e���̍폜
    public void RemoveListItem(GameObject obj)
    {
        liObj.Remove(obj);
    }
    public bool CanAttack()
    {
        // �U���A�j���[�V���������s���łȂ��ꍇ
        if (scPlayerMoveAnimation.SetAttack() == false && scPlayerMoveAnimation.timeAttack < 0)
        {
            return true;
        }
        return false;
    }
    // ������
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Car"))
        {
            bJump = false;
            Jmpconsecutive = 0;
        }
    }
}
