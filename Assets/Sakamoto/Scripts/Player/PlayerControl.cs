using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField, Header("���b�ȏ���u���Ă��邩")] private float sleepThreshold = 30.0f; //sleepThreshold�b�ȏ�X���[�v��ԂȂ�^�C�g����

    private float lastInputTime;
    [SerializeField, Header("���u�΍���")] private GameObject SleepPanel;
    private GameObject SleepPanelInstance;
    private Gun Gun;
    //���e�̃V���b�g�t���O
    private bool isShot;
    void Start()
    {
        playerRigidBody2D = GetComponent<Rigidbody2D>();

        SleepPanel = Resources.Load<GameObject>("SleepPanel");
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
    public void InstantiateSkipPanel()
    {
        if (SleepPanelInstance != null) { Destroy(SleepPanelInstance); }
        SleepPanelInstance = Instantiate(SleepPanel);
        ChangeStage1();
        ChangeTutorial();
    }
    private void ChangeStage1()
    {
        Button YesButton = SleepPanelInstance.transform.Find("YesButton").GetComponent<Button>();
        Debug.Log(YesButton);

        if (YesButton != null)
        {
            YesButton.onClick.RemoveAllListeners();
            YesButton.onClick.AddListener(() =>
            {
                SceneTransitionManager.instance.NextSceneButton(0);
                Destroy(SleepPanelInstance);
            });

        }
    }
    private void ChangeTutorial()
    {
        Button NoButton = SleepPanelInstance.transform.Find("NoButton").GetComponent<Button>();
        Debug.Log(NoButton);

        if (NoButton != null)
        {
            NoButton.onClick.RemoveAllListeners();
            NoButton.onClick.AddListener(() =>
            {
               
                Destroy(SleepPanelInstance);
            });

        }
    }
    // Update is called once per frame
    void Update()
    {
        
        //�v���C���[��Y���W�̐���
        //�v���C���[��Y���W��8.0�𒴂����烊�W�b�h�{�f�B�̃t�H�[�X��0�ɂ���

        switch (GameMgr.GetState())
        {
            case GameState.Main:
                //bShootFlag��false�ɂ���
                isShot = false;
                if (8.0f < transform.position.y)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1);
                }
                UpdateTimers();


                //�U���A�j���[�V�������łȂ����bShootFlag��true�ɂ���
                //Debug.Log(playerMoveAnimation.SetAttack());
                if (playerMoveAnimation.SetAttack() == false)
                {
                    isShot = true;
                }

                MainExecution();

                break;
            case GameState.ShowText:
                UpdateTimers();

                break;
            case GameState.ShowOption:

                break;

            case GameState.Tutorial:
                UpdateTimers();
                break;
            case GameState.Hint:
                Time.timeScale = 0.0f;
                //Debug.Log("�v���C���[�������Ă��Ȃ����Ɗm�F");

                break;
            default:
                //Debug.Log("�v���C���[�������Ă��Ȃ����Ɗm�F");
                break;
        }
        if (GameMgr.GetState() != GameState.ShowOption)
        {
            PlayerSleeping();

        }
    }

    private void PlayerSleeping()
    {
        if (Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 ||
        Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.anyKeyDown)
            {
                lastInputTime = Time.time;
            }
            Debug.Log(Time.time-lastInputTime);
            // ��莞�ԑ��삪�Ȃ���΁u���u�v�Ɣ���
            if (Time.time - lastInputTime >= sleepThreshold)
            {
                InstantiateSkipPanel();

                Debug.Log("�v���C���[����莞�ԑ��삵�Ă��܂���");
            }
            
        }
       
    }
    private void UpdateTimers()
    {
        playerMoveAnimation.timeWalk -= Time.deltaTime;
        playerMoveAnimation.timeAttack -= Time.deltaTime;
        Time.timeScale = 1.0f;

    }
    void Move()
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
                    if (isEnemyHit() == false)
                    {
                        Debug.Log("���Ɉړ����܂�");
                        vPosition.x -= Time.deltaTime * playerSpeed;
                    }
                }
                playerMoveAnimation.HandleWalk(PlayerMoveAnimation.SHAFT_DIRECTION_LEFT);
            }
            //�E�ړ�
            if (Input.GetKey(KeyCode.D))
            {
                if (mainCameraWidth / 2 > vPosFromCame.x)
                {
                    if (isEnemyHit() == false)
                    {
                        vPosition.x += Time.deltaTime * playerSpeed;
                    }
                }
                playerMoveAnimation.HandleWalk(PlayerMoveAnimation.SHAFT_DIRECTION_RIGHT);
            }
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
        if (Tutorial.GetState() == Tutorial_State.PlayerDoNotMove)
        {
            return;
        }
        Move();


        #region �R�i�ύX
        //�A�j���[�V�����������ŌĂԂ��߁A�ǋL
        //�U���֘A
        if (!playerMoveAnimation.SetAttack() && playerMoveAnimation.timeAttack < 0)
        {
            //�㔼�g�U��
            if (Input.GetKeyDown(KeyCode.I))
            {
                //Tutorial.NextState();       
                UpperAttack upperattack = PlayerParameter.Instance.UpperData.upperAttack;
                playerMoveAnimation.PantieStart();
                // �x�@�㔼�g�͏e�e�ɓ����蔻�������
                if (upperattack != UpperAttack.POLICE)
                {
                    OnUpperAttackAnimationFinished();
                }

                #endregion
                switch (upperattack)
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
                            Gun.Shoot(ShootMoveBector, transform, PlayerParameter.Instance.UpperData.iPartAttack);

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
                //Tutorial.NextState();

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
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Car"))
        {
            isJump = false;
            jumpCount = 0;
        }


    }
    //private void OnCollisionExit2D(Collision2D other)
    //{
    //    if (other.gameObject.CompareTag("OnTheCar"))
    //    {
    //        GameMgr.ChangeState(GameState.Main); // �{�X�풼�O�ɏ�ԕύX

    //    }
    //}

    //�G�̒e�Ƃ̓����蔻��
    private void OnTriggerEnter2D(Collider2D playerCollision)
    {
        if (playerCollision.gameObject.CompareTag("EnemyShoot"))
        {
            int attack = playerCollision.gameObject.GetComponent<Bullet>().attack;
            if (0 > transform.position.y - playerCollision.transform.position.y)
            {
                PlayerParameter.Instance.UpperHP -= attack;
            }
            else
            {
                PlayerParameter.Instance.LowerHP -= attack;
            }
        }

    }

    public void SetEnabledPlayerRenderer(bool enabled)
    {
        var renderers = transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = enabled;
        }
    }

    //enemy�Ɠ������Ă��邩�ǂ���
    //return :: true->�������Ă���    false->�����Ă��Ȃ�
    bool isEnemyHit()
    {
        for (int i = 0; i < enemyObject.Count; i++)
        {
            //�G�Ƃ�X����
            float distanceX = Mathf.Abs(enemyObject[i].transform.position.x - this.transform.position.x);
            //�G�Ƃ�Y����
            float distanceY = Mathf.Abs(enemyObject[i].transform.position.y - this.transform.position.y);
            //X������player��enemy�̃R���C�_�[��X�T�C�Y�̔����̘a��菬����
            //Y������player��enemy�̃R���C�_�[��Y�T�C�Y�̔����̘a��菬�����Ȃ�if���ɓ���
            //Scale�̔������ƌ����ڂƂ̌덷�ł��܂������Ȃ����ߔ������������傫���l����肽������1.5�Ƃ���
            if ((distanceX < this.GetComponent<BoxCollider2D>().size.x * this.transform.localScale.x / 1.5
                + enemyObject[i].GetComponent<BoxCollider2D>().size.x * enemyObject[i].transform.localScale.x / 1.5) &&
                (distanceY < this.GetComponent<BoxCollider2D>().size.y * this.transform.localScale.y / 2.5
                + enemyObject[i].GetComponent<BoxCollider2D>().size.y * enemyObject[i].transform.localScale.y / 2.5))
            {
                //player���E�������Ă��邩��enemy��player�̉E���ɂ��邩
                //player�����������Ă��邩��enemy��player�̍����ɂ���Ȃ瓖�����Ă���
                if ((playerMoveAnimation.isFacingToRight() == true &&
                    0 < enemyObject[i].transform.position.x - this.transform.position.x) ||
                    (playerMoveAnimation.isFacingToRight() == false &&
                    enemyObject[i].transform.position.x - this.transform.position.x < 0))
                {
                    Debug.Log("�������Ă�");
                    //�������Ă���
                    return true;
                }
            }
        }
        //�����Ă��Ȃ�
        return false;
    }
}
