using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerParameter : CharacterStats
{
    //�ڐA���̃��U�C�N
    private GameObject goMosaic;

    public static PlayerParameter Instance;

    [SerializeField, Header("1��������̂ɂ����鎞��")]
    protected float iDownTime;
    [SerializeField, Header("�X�e�[�W�P�̌������x�̒����p")]

    protected float slowFactor;
    [SerializeField, Header("�l�Ԑ��̍ő�l")]
    public int iHumanityMax;     //�l�Ԑ��̍ő�l
    [SerializeField, Header("�㔼�g��HP�̍ő�l,�p�[�c���ς��x��\n���l���ς�邽�ߐݒ�͂��̂܂܂�OK")]

    public int iUpperHPMax;      //�㔼�g��HP�̍ő�l
    [SerializeField, Header("�����g��HP�̍ő�l,�p�[�c���ς��x��\n���l���ς�邽�ߐݒ�͂��̂܂܂�OK")]

    public int iLowerHPMax;      //�����g��HP�̍ő�l

    protected float iHumanity;     //�l�Ԑ�
    protected float iUpperHP;      //�㔼�g��HP
    protected float iLowerHP;      //�����g��HP

    [Header("�v���C���[�R���g���[���X�N���v�g")]
    [SerializeField] private PlayerControl playerControl;

    //�㔼�g�̃p�[�c�f�[�^
    public BodyPartsData UpperData;
    //�����g�̃p�[�c�f�[�^
    public BodyPartsData LowerData;
    //�㔼�g�̃p�[�c�f�[�^
    public BodyPartsData upperDataDefault;
    //�����g�̃p�[�c�f�[�^
    public BodyPartsData lowerDataDefault;
    //�㔼�g�̃p�[�c�f�[�^(�X�e�[�W4�p)
    public BodyPartsData UpperDataForStageFour;
    //�����g�̃p�[�c�f�[�^(�X�e�[�W4�p)
    public BodyPartsData LowerDataForStageFour;
    //�L�����̃C���[�W�擾�p
    private PlayerMoveAnimation playerMoveAnimation;
    //�㔼�g�̃p�[�c�f�[�^(�ۑ��p)
    private BodyPartsData upperIndex;
    //�����g�̃p�[�c�f�[�^(�ۑ��p)
    private BodyPartsData lowerIndex;
    ////�㔼�g�̃p�[�c�f�[�^(�X�e�[�W4�p)
    //private BodyPartsData upperPlayer;
    ////�����g�̃p�[�c�f�[�^(�X�e�[�W4�p)
    //private BodyPartsData lowerPlayer;

    private EnemyMoveAnimation enemyMoveAnimation;


    //�Q�[���I�[�o�[�̕W��
    private GameObject goPanel;

    private bool hasDroped = false;

    private const float GAMEOVER_ZOMBIEWALK_TIMEMAX = 0.3f;
    private const float GAMEOVER_ZOMBIEWALK_SPEED = 0.2f;


    protected virtual void Awake()
    {
        CheckInstance();
        InitBodyIndex();
        //�R���|�[�l���g�擾
        InitializeReferences();
    }
    protected virtual void Start()
    {
        enemyMoveAnimation = FindObjectOfType<EnemyMoveAnimation>();


        //�V�[���J�ڂŔj������Ȃ�
        DontDestroyOnLoad(gameObject);

    }

    protected virtual void Update()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        if (!(SceneName == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.Title)))
        {
            switch (GameMgr.GetState())
            {
                case GameState.Main:
                    //�p�����[�^�̒l��iDownTime�b��1����������
                    DecreasingHP();
                    //Debug.Log(iDownTime);
                    if (iHumanity < 0 || iUpperHP < 0 || iLowerHP < 0)
                    {

                        Debug.Log("�����[�h���J�n���܂�"); // �f�o�b�O���O�Ŋm�F

                        AudioSource BGM = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
                        MultiAudio.ins.PlayBGM_ByName("BGM_defeated");

                        BGM.loop = false;


                        #region �R�i�ύX
                        ////�p�����[�^�̑S��
                        //iHumanity = iHumanityMax;
                        //iUpperHP = iUpperHPMax;
                        //iLowerHP = iLowerHPMax;

                        #endregion
                        //�v���C���[��������
                        //�Q�[���I�[�o�[�̕W��
                        goPanel.SetActive(true);
                        GameMgr.ChangeState(GameState.GameOver);

                    }
                    break;



                ////�V�[���ړ�
                //if (Input.GetKeyDown(KeyCode.M))
                //{
                //    SceneManager.LoadScene("Stage2");
                //}

                case GameState.GameOver:
                    if (iHumanity < 0)
                    {
                        // �������ֈړ�
                        // �]���r�����A�j���[�V����
                        playerMoveAnimation.SetTimeMax(GAMEOVER_ZOMBIEWALK_TIMEMAX);
                        playerMoveAnimation.HandleWalk(PlayerMoveAnimation.SHAFT_DIRECTION_LEFT, true);
                        // �ړ�
                        Vector3 vPosition = playerControl.transform.position;
                        vPosition.x -= Time.deltaTime * GAMEOVER_ZOMBIEWALK_SPEED;
                        playerControl.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        playerControl.transform.position = vPosition;
                    }
                    else if (iUpperHP < 0)
                    {
                        DropAndRemovePlayerOnce(false);
                    }
                    else if (iLowerHP < 0)
                    {
                        DropAndRemovePlayerOnce(true);
                    }
                    break;
            }
        }

    }

    //�ԗ�
    //�l�Ԑ����������񕜂���
    public void comfort(int iRecovery)
    {
        iHumanity += iRecovery;
        //�񕜂����l���ő�l�𒴂��Ă�����ő�l�ɂ���
        if (iHumanity > iHumanityMax)
        {
            iHumanity = iHumanityMax;
        }

    }

    //�ڐA
    //�p�[�c�̉摜�ƃp�����[�^�����ւ���
    //BodyPartsData partsData : ����ւ���p�[�c�̃X�N���v�^�u���I�u�W�F�N�g
    //�e�X�g�i�K�ł͈�����null�ł���
    public void transplant(BodyPartsData partsData)
    {
        //�ڐA���Ƀ��U�C�N��\��������
        //���U�C�N���̂����ԍ��ŏ����邩��\�������ł���
        goMosaic.SetActive(true);

        //partsData = partsData ?? DefaultData;



        switch (partsData.enPartsType)
        {
            case PartsType.Upper:
                //�p�[�c�f�[�^��HP��Max�ɑ��
                iUpperHPMax = partsData.iPartHp;
                iUpperHP = iUpperHPMax;
                //partData�̏㏑��
                UpperData = partsData;
                /*
                //SpriteRenderer��Sprite�Ƀp�[�c�f�[�^��Sprite��}��
                spriteRenderer.sprite = partsData.spBody;
                */
                //�����ڕύX�֐��҂�
                playerControl.ChangeUpperBody(partsData);
                //�U�����[�V�����̕ύX
                playerMoveAnimation.ChangeUpperMove(partsData.upperAttack);
                break;
            case PartsType.Lower:
                //�p�[�c�f�[�^��HP��Max���
                iLowerHPMax = partsData.iPartHp;
                iLowerHP = iLowerHPMax;
                //partData�̏㏑��
                LowerData = partsData;
                /*
                //SpriteRenderer��Sprite�Ƀp�[�c�f�[�^��Sprite��}��
                spriteRenderer.sprite = partsData.spWaist;
                */
                //�����ڕύX�֐��҂�
                playerControl.ChangeUnderBody(partsData);
                //�U�����[�V�����̕ύX
                playerMoveAnimation.ChangeLowerMove(partsData.lowerAttack);
                break;
        }

    }

    /// <summary>
    /// �㉺�������ꊇ�ڐA
    /// </summary>
    public void transplantBoth(BodyPartsData upperPart, BodyPartsData lowerPart)
    {
        // ���U�C�N��\��������
        goMosaic.SetActive(true);

        // �p�[�c�f�[�^��HP��Max�ɑ��
        iUpperHPMax = upperPart.iPartHp;
        iUpperHP = iUpperHPMax;
        iLowerHPMax = lowerPart.iPartHp;
        iLowerHP = iLowerHPMax;
        // ���ʃf�[�^�̏㏑��
        UpperData = upperPart;
        LowerData = lowerPart;
        // �����ڕύX�֐��҂�
        playerControl.ChangeUpperBody(upperPart);
        playerControl.ChangeUnderBody(lowerPart);
        // �U�����[�V�����̕ύX
        playerMoveAnimation.ChangeUpperMove(upperPart.upperAttack);
        playerMoveAnimation.ChangeLowerMove(lowerPart.lowerAttack);
    }

    /// <summary>
    /// �e�F�̐g�̂��㉺�Ƃ��ڐA
    /// </summary>
    public void transplantFriendBoth()
    {
        transplantBoth(UpperDataForStageFour, LowerDataForStageFour);
    }

    //�l�Ԑ��̎擾
    public float Humanity
    {
        get { return iHumanity; }
        set { iHumanity = value; }
    }
    //�㔼�gHP�̎擾
    public float UpperHP
    {
        get { return iUpperHP; }
        set { iUpperHP = value; }
    }
    //�����gHP�̎擾
    public float LowerHP
    {
        get { return iLowerHP; }
        set { iLowerHP = value; }
    }

    //�V���O���g���̃`�F�b�N
    void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// upperIndex��lowerIndex��������
    /// </summary>
    public void InitBodyIndex()
    {
        upperIndex = upperDataDefault;
        lowerIndex = lowerDataDefault;
    }

    /// <summary>
    /// �V�[���ǂݍ��ݎ��̏���������
    /// upperIndex/upperIndex�͏���������Ȃ�
    /// </summary>
    protected virtual void InitializeReferences()
    {
        // �V�[���J�ڌ�ɕK�v�ȃI�u�W�F�N�g���Ď擾
        goMosaic = GameObject.Find("Player Variant");
        goMosaic = goMosaic.transform.Find("Mosaic").gameObject;
        goPanel = GameObject.Find("GameResult");
        goPanel = goPanel.transform.Find("GameOver").gameObject;
        playerControl = GameObject.Find("Player Variant").GetComponent<PlayerControl>();
        //�R���|�[�l���g�擾
        playerMoveAnimation = playerControl.GetComponent<PlayerMoveAnimation>();

        UpperData = upperIndex;
        LowerData = lowerIndex;
        Debug.Log($"upperIndex��{upperIndex}");
        Debug.Log($"lowerIndex��{lowerIndex}");

        //�ő�l��ݒ�
        iUpperHPMax = UpperData.iPartHp;
        iLowerHPMax = LowerData.iPartHp;
        //�p�����[�^�̏�����
        iHumanity = iHumanityMax;
        iUpperHP = iUpperHPMax;
        iLowerHP = iLowerHPMax;
        //Debug.Log(hitGameObject);


        hasDroped = false;

        playerControl.ChangeUpperBody(UpperData);
        playerMoveAnimation.ChangeUpperMove(UpperData.upperAttack);
        playerControl.ChangeUnderBody(LowerData);
        playerMoveAnimation.ChangeLowerMove(LowerData.lowerAttack);

        if (goMosaic == null || goPanel == null)
        {
            Debug.LogWarning("�K�v�ȃI�u�W�F�N�g��������܂���");
        }

    }

    /// <summary>
    /// �X�e�[�W�J�ڎ��v���C���[�̏�Ԃ�ێ�����
    /// �G���X�e�[�W����TextDisplay�ɌĂ�ł��炤
    /// �{�X�X�e�[�W����DropPart�ɌĂ�ł��炤
    /// </summary>
    public void KeepBodyData()
    {
        upperIndex = UpperData;
        lowerIndex = LowerData;
    }

    /// <summary>
    /// �X�e�[�W�N���A4�̎��f�t�H���g�̏�Ԃɂ���
    /// DropPart�ɌĂ�ł��炤
    /// </summary>
    public void SetBadyForStageFour()
    {
        UpperData = UpperDataForStageFour;
        LowerData = LowerDataForStageFour;
        upperIndex = UpperDataForStageFour;
        lowerIndex = LowerDataForStageFour;
    }

    private void OnEnable()
    {
        // �V�[�������[�h���ꂽ��ɎQ�Ƃ��Ď擾
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // �C�x���g�̉���
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public override void TakeDamage(float damage, int body = 0)
    {
        //HP������d�g��
        //damage�̓e�X�g�p�̊֐�
        if (body == 0)
        {
            //�㔼�g��HP�����炷
            UpperHP -= damage;
            enemyMoveAnimation.ShowHitEffects(body, playerControl.transform.position);
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");

            //Debug.Log(UpperHP);

        }

        if (body == 1)
        {
            //�����g��HP�����炷
            LowerHP -= damage;
            enemyMoveAnimation.ShowHitEffects(body, playerControl.transform.position);
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");

            //Debug.Log(LowerHP);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = SceneTransitionManager.instance.sceneInformation.GetCurrentSceneName();

        // �V�[���� Title �܂��� End �łȂ��ꍇ�� InitializeReferences �����s
        if (sceneName != SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.Title) &&
            sceneName != SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.End))
        {
            InitializeReferences();
        }

        Debug.Log($"�V�[�� {scene.name} �����[�h����܂���");
    }

    private void DropAndRemovePlayerOnce(bool dropsUpper)
    {
        if (!hasDroped)
        {
            hasDroped = true;

            GameObject bodyPart = dropsUpper ? UpperData.DropPartUpper : LowerData.DropPartLower;
            GameObject drop = Instantiate(bodyPart);
            drop.transform.position = playerControl.transform.position;
            // �C�V���N�C���C�{�^����\��
            drop.GetComponentInChildren<DropButton>().ShowsButton = false;

            // �v���C���[���\��
            playerControl.SetEnabledPlayerRenderer(false);

            MultiAudio.ins.PlaySEByName("SE_common_breakbody");
        }
    }

    protected virtual void DecreasingHP()
    {
        string sceneName = SceneTransitionManager.instance.sceneInformation.GetCurrentSceneName();

        if (sceneName != SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageOne))
        {
            iHumanity -= Time.deltaTime / iDownTime/* *dgbScale*/;
            iUpperHP -= Time.deltaTime / iDownTime;
            iLowerHP -= Time.deltaTime / iDownTime;


        }
        else
        {
            iHumanity -= (Time.deltaTime / iDownTime) * slowFactor;
            iUpperHP -= Time.deltaTime / iDownTime * slowFactor;
            iLowerHP -= Time.deltaTime / iDownTime * slowFactor;
        }

    }
}
