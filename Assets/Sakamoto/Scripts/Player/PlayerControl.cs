using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    [System.Serializable]
    //�X�v���C�g�֘A����̃N���X�ɂ܂Ƃ߂Ă��܂��āA���̃f�[�^��[System.Serializable]�ŃC���X�y�N�^�ł��ݒ�ł���
    private class CharacterSprites
    {
        public SpriteRenderer head;
        public SpriteRenderer body;
        public SpriteRenderer armRight;
        public SpriteRenderer armLeft;
        public SpriteRenderer handRight;
        public SpriteRenderer handLeft;
        public SpriteRenderer waist;
        public SpriteRenderer legRight;
        public SpriteRenderer legLeft;
        public SpriteRenderer footRight;
        public SpriteRenderer footLeft;
    }
    // �L�����N�^�[�p�[�c (SpriteRenderer)
    [SerializeField, Header("�L�����N�^�[�p�[�c")]
    private CharacterSprites characterSprites;

    //���[�V�����A�j���X�N���v�g
    private PlayerMoveAnimation playerMoveAnimation;



    //private Rigidbody2D rigidbody2D;
    [SerializeField, Header("�ړ��X�s�[�h")]
    private float playerSpeed;
    [SerializeField, Header("�W�����v��")]
    private float playerJumpPower;
    //�W�����v�ł��邩�ǂ���
    private bool isJump = false;
    //�A���W�����v
    private int jumpCount;


    //�J�����֘A
    [SerializeField, Header("�J��������")] private Camera mainCamera;
    //����
    private float mainCameraHeight;
    //��
    private float mainCameraWidth;

    //�^�[�Q�b�g
    [SerializeField, Header("�{�X�̃I�u�W�F�N�g������\n�{�X�ȊO�̓X�|�i�[������ɐ���")]
    public List<GameObject> enemyObject;
    //private float originalGravityScale;
    Rigidbody2D playerRigidBody2D;

    //[SerializeField] GameObject[] goObj;




    private Gun Gun;
    //���e�̃V���b�g�t���O
    private bool isShot;
    void Start()
    {
        playerRigidBody2D = GetComponent<Rigidbody2D>();


        //����_���ȓz
        //playerParameter = GameObject.FindAnyObjectByType<PlayerParameter>();
        //���ꂢ�����
        playerMoveAnimation = GetComponent<PlayerMoveAnimation>();
        Gun = GetComponent<Gun>();
        mainCamera = FindAnyObjectByType<Camera>();
        // �J�����̍����iorthographicSize�j�̓J�����̒�������㉺�̋�����\��
        mainCameraHeight = 2f * mainCamera.orthographicSize;

        // �J�����̕��̓A�X�y�N�g��Ɋ�Â��Čv�Z����
        mainCameraWidth = mainCameraHeight * mainCamera.aspect;
        // �U���A�j���[�V�����I�����̃R�[���o�b�N��ݒ�
        // �㔼�g�U���A�j���[�V�����I�����̃R�[���o�b�N��ݒ�
         }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[��Y���W�̐���
        //�v���C���[��Y���W��8.0�𒴂����烊�W�b�h�{�f�B�̃t�H�[�X��0�ɂ���
        if (8.0f < transform.position.y)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1);
        }
        UpdateTimers();
        switch (GameMgr.GetState())
        {
            case GameState.Main:
                //bShootFlag��false�ɂ���
                isShot = false;
                //�U���A�j���[�V�������łȂ����bShootFlag��true�ɂ���
                //Debug.Log(playerMoveAnimation.SetAttack());
                if (playerMoveAnimation.SetAttack() == false)
                {
                    isShot = true;

                }

                MainExecution();

                break;
            case GameState.ShowOption:
                //Debug.Log("�v���C���[�������Ă��Ȃ����Ɗm�F");

                break;
            default:
                //Debug.Log("�v���C���[�������Ă��Ȃ����Ɗm�F");
                break;
        }
    }

    private void UpdateTimers()
    {
        playerMoveAnimation.timeWalk -= Time.deltaTime;
        playerMoveAnimation.timeAttack -= Time.deltaTime;
    }
    void MoveAndJump()
    {
        //���݂̃|�W�V�������擾
        Vector3 vPosition = transform.position;

        //�J�����Ƃ̋����̐�Βl�����ȉ��Ȃ�v���C���[�������@��ʊO�ɏo�Ȃ����߂̏��u
        //�ړ�
        Vector3 vPosFromCame = vPosition - mainCamera.transform.position; //�J������̃v���C���[�̈ʒu

        if (!playerMoveAnimation.SetAttack())
        {
            //���ړ�
            if (Input.GetKey(KeyCode.A))
            {
                if (vPosFromCame.x > -mainCameraWidth / 2)
                {
                    vPosition.x -= Time.deltaTime * playerSpeed;

                }
                playerMoveAnimation.HandleWalk(PlayerMoveAnimation.SHAFT_DIRECTION_LEFT);
            }
            //�E�ړ�
            if (Input.GetKey(KeyCode.D))
            {
                if (mainCameraWidth / 2 > vPosFromCame.x)
                {
                    vPosition.x += Time.deltaTime * playerSpeed;
                }
                playerMoveAnimation.HandleWalk(PlayerMoveAnimation.SHAFT_DIRECTION_RIGHT);

            }

            //�W�����v

            if (Input.GetKey(KeyCode.W) && jumpCount < 1)
            {
                Vector2 upVector = Vector2.up;
                playerRigidBody2D.velocity = upVector;
                playerRigidBody2D.AddForce(transform.up * playerJumpPower, ForceMode2D.Force);
                Debug.Log(transform.position);
                MultiAudio.ins.PlaySEByName("SE_hero_action_jump");
                isJump = true;
                jumpCount++;

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

        //�̂���]���Ȃ��悤�ɂ���̃I�C���[���O�Őݒ肷��΂ł���
        //������transform���擾

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.position = vPosition;
    }
    //�Q�[�����C���̃G�N�X�L���[�g
    void MainExecution()
    {

        MoveAndJump();



        #region �R�i�ύX
        //�A�j���[�V�����������ŌĂԂ��߁A�ǋL
        //�U���֘A
        if (!playerMoveAnimation.SetAttack() && playerMoveAnimation.timeAttack < 0)
        {
            //�㔼�g�U��
            if (Input.GetKeyDown(KeyCode.I))
            {

                playerMoveAnimation.PantieStart();
                OnUpperAttackAnimationFinished();
                #endregion
                switch (PlayerParameter.Instance.UpperData.upperAttack)
                {
                    case UpperAttack.NORMAL:

                        MultiAudio.ins.PlaySEByName("SE_hero_attack_upper");

                        break;

                    case UpperAttack.POLICE:
                        Vector2 ShootMoveBector = new Vector2(0, 0);
                        //�q��playerRC�̃��[�e�[�V����Y�������Ă���
                        // y = 0�̂Ƃ��͉E�����A0 y = 180�̂Ƃ��͍�����
                        Debug.Log(transform.GetChild(0).transform.eulerAngles.y);
                        if (transform.GetChild(0).transform.eulerAngles.y == 180)
                        {
                            ShootMoveBector.x = -1;
                        }
                        else
                        {
                            ShootMoveBector.x = 1;
                        }

                        Debug.Log(ShootMoveBector);
                        Debug.Log("shootFlag��" + isShot);

                        //isShot��true�Ȃ�e�𔭎˂���
                        if (isShot == true)
                        {
                            Debug.Log("�e����");
                            Gun.Shoot(ShootMoveBector, transform);

                            MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_upper");

                            //bShootFlag = false;
                        }
                        break;

                    case UpperAttack.NURSE:
                        MultiAudio.ins.PlaySEByName("SE_nurse_attack_upper");
                        break;
                    case UpperAttack.BOSS:
                        MultiAudio.ins.PlaySEByName("SE_lastboss_attack_upper");

                        break;
                }

               

               

            }
            //�����g�U��
            if (Input.GetKeyDown(KeyCode.K))
            {
                #region �R�i�ύX
                playerMoveAnimation.KickStart();
                OnLowerAttackAnimationFinished();   

                #endregion
                switch (PlayerParameter.Instance.LowerData.lowerAttack)
                {
                    case LowerAttack.NORMAL:
                        MultiAudio.ins.PlaySEByName("SE_hero_attack_lower");

                        break;

                    case LowerAttack.POLICE:
                        MultiAudio.ins.PlaySEByName("SE_policeofficer_attack_lower");
                        break;

                    case LowerAttack.NURSE:
                        MultiAudio.ins.PlaySEByName("SE_nurse_attack_lower");
                        break;

                        case LowerAttack.BOSS:
                        MultiAudio.ins.PlaySEByName("SE_lastboss_attack_lower");
                        break;
                }

                

            }
        }


    }

    //�A�j���[�V�����������Ă����֐����ړ��i�A�j���[�V�������̂��̂ɂ͊֌W�Ȃ����߁j
    /// <summary>
    /// �㔼�g�̃C���[�W
    /// </summary>
    /// <param name="upperBody">�摜�f�[�^�W����</param>
    public void ChangeUpperBody(BodyPartsData upperBody)
    {
        characterSprites.body.sprite = upperBody.spBody;
        characterSprites.armRight.sprite = upperBody.spRightArm;
        characterSprites.armLeft.sprite = upperBody.spLeftArm;
        characterSprites.handRight.sprite = upperBody.spRightHand;
        characterSprites.handLeft.sprite = upperBody.spLeftHand;
    }
    //�A�j���[�V�����������Ă����֐����ړ��i�A�j���[�V�������̂��̂ɂ͊֌W�Ȃ����߁j

    /// <summary>
    /// �����g�̃C���[�W
    /// </summary>
    /// <param name="underBody">�摜�f�[�^�W����</param>
    public void ChangeUnderBody(BodyPartsData underBody)
    {
        characterSprites.waist.sprite = underBody.spWaist;
        characterSprites.footRight.sprite = underBody.spRightFoot;
        characterSprites.footLeft.sprite = underBody.spLeftFoot;
        characterSprites.legRight.sprite = underBody.spRightLeg;
        characterSprites.legLeft.sprite = underBody.spLeftLeg;
    }

    //private void OnDestroy()
    //{
    //    // �R�[���o�b�N�̉���
    //    if (playerMoveAnimation != null)
    //    {
    //        playerMoveAnimation.OnUpperAttackAnimationFinished -= OnUpperAttackAnimationFinished;
    //        playerMoveAnimation.OnLowerAttackAnimationFinished -= OnLowerAttackAnimationFinished;
    //    }
    //}

    private void OnUpperAttackAnimationFinished()
    {
        // �㔼�g�U������
        for (int i = 0; i < enemyObject.Count; i++)
        {
            UpperBodyAttack(i, enemyObject[i].transform.position, PlayerParameter.Instance.UpperData.AttackArea, PlayerParameter.Instance.UpperData.iPartAttack);
        }
    }

    private void OnLowerAttackAnimationFinished()
    {
        // �����g�U������
        for (int i = 0; i < enemyObject.Count; i++)
        {
            LowerBodyAttack(i, enemyObject[i].transform.position, PlayerParameter.Instance.LowerData.AttackArea, PlayerParameter.Instance.LowerData.iPartAttack);
        }
    }

    //�㔼�g�U��
    public void UpperBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        bool isAttackingToEnemy = IsFacingToTarget(transform.position.x, vTargetPos.x, playerMoveAnimation.isFacingToRight());

        float fAttackReach = Vector3.Distance(vTargetPos, transform.position);
        if (!isAttackingToEnemy || fAttackReach >= fReach)
        {
            return;
        }
        IDamageable damageable = enemyObject[EnemyNum].GetComponent<IDamageable>();
        damageable?.TakeDamage(iDamage, 0);
        Debug.Log("�㔼�g�U���_���[�W���f");

    }
    //�����g�U��
    public void LowerBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach, int iDamage)
    {
        bool isAttackingToEnemy = IsFacingToTarget(transform.position.x, vTargetPos.x, playerMoveAnimation.isFacingToRight());

        float fAttackReach = Vector3.Distance(vTargetPos, transform.position);
        if (!isAttackingToEnemy || fAttackReach >= fReach)
        {
            return;
        }
        IDamageable damageable = enemyObject[EnemyNum].GetComponent<IDamageable>();
        damageable?.TakeDamage(iDamage, 1);
        Debug.Log("�����g�U���_���[�W���f");

    }

    private bool IsFacingToTarget(float playerPosX, float targetPosX, bool isFacingToRight)
    {
        bool isFacingToRightTarget = playerPosX <= targetPosX && isFacingToRight;
        bool isFacingToLeftTarget = playerPosX >= targetPosX && !isFacingToRight;
        return isFacingToRightTarget || isFacingToLeftTarget;
    }


    public void AddListItem(GameObject obj) => enemyObject.Add(obj);
    public void RemoveListItem(GameObject obj) => enemyObject.Remove(obj);

    //������
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Car"))
        {
            isJump = false;
            jumpCount = 0;
        }
    }


    //�G�̒e�s�̓����蔻��
    private void OnTriggerEnter2D(Collider2D playerCollision)
    {
        if (playerCollision.gameObject.CompareTag("EnemyShoot"))
        {
            if (0 > transform.position.y - playerCollision.transform.position.y)
            {
                PlayerParameter.Instance.UpperHP -= 1;
            }
            else
            {
                PlayerParameter.Instance.LowerHP -= 1;
            }
        }

    }

}
