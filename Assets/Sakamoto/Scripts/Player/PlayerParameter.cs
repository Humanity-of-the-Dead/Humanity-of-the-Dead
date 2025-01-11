using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerParameter : MonoBehaviour
{



    //�ڐA���̃��U�C�N
    private GameObject goMosaic;

    public static PlayerParameter Instance;

    [Header("1��������̂ɂ����鎞��")]
    [SerializeField] private int iDownTime;

    public int iHumanityMax;     //�l�Ԑ��̍ő�l
    public int iUpperHPMax;      //�㔼�g��HP�̍ő�l
    public int iLowerHPMax;      //�����g��HP�̍ő�l

    private float iHumanity;     //�l�Ԑ�
    private float iUpperHP;      //�㔼�g��HP
    private float iLowerHP;      //�����g��HP

    [Header("�v���C���[�R���g���[���X�N���v�g")]
    [SerializeField] private PlayerControl playerControl;

    //�㔼�g�̃p�[�c�f�[�^
    public BodyPartsData UpperData;
    //�����g�̃p�[�c�f�[�^
    public BodyPartsData LowerData;
    //�L�����̃C���[�W�擾�p
    private PlayerMoveAnimation playerMoveAnimation;
    //�㔼�g�̃p�[�c�f�[�^(�ۑ��p)
    private BodyPartsData upperIndex;
    //�����g�̃p�[�c�f�[�^(�ۑ��p)
    private BodyPartsData lowerIndex;
    //�㔼�g�̃p�[�c�f�[�^(�X�e�[�W4�p)
    private BodyPartsData upperPlayer;
    //�����g�̃p�[�c�f�[�^(�X�e�[�W4�p)
    private BodyPartsData lowerPlayer;



    //�Q�[���I�[�o�[�̕W��
    private GameObject goPanel;


    public void Awake()
    {
        CheckInstance();
    }
    private void Start()
    {
        upperIndex = UpperData;
        lowerIndex = LowerData;
        upperPlayer = UpperData;
        lowerPlayer = LowerData;
        InitializeReferences();
        //�R���|�[�l���g�擾

        //�V�[���J�ڂŔj������Ȃ�
        DontDestroyOnLoad(gameObject);

        // �V�[�������[�h���ꂽ��ɎQ�Ƃ��Ď擾
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Update()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        if (!(SceneName == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.Title)))
        {
            switch (GameMgr.GetState())
            {
                case GameState.Main:
                    //�p�����[�^�̒l��iDownTime�b��1����������
                    iHumanity -= Time.deltaTime / iDownTime/* *dgbScale*/;
                    iUpperHP -= Time.deltaTime / iDownTime;
                    iLowerHP -= Time.deltaTime / iDownTime;
                    //Debug.Log(iDownTime);
                    if (iHumanity < 0 || iUpperHP < 0 || iLowerHP < 0)
                    {

                        Debug.Log("�����[�h���J�n���܂�"); // �f�o�b�O���O�Ŋm�F

                        MultiAudio.ins.bgmSource.Stop();



                        //GameOver��BGM�炷�ӏ�
                        MultiAudio.ins.PlayBGM_ByName("BGM_defeated");
                        #region �R�i�ύX
                        ////�p�����[�^�̑S��
                        //iHumanity = iHumanityMax;
                        //iUpperHP = iUpperHPMax;
                        //iLowerHP = iLowerHPMax;

                        #endregion
                        //�v���C���[��������
                        UpperData = upperIndex;
                        LowerData = lowerIndex;

                        //�Q�[���I�[�o�[�̕W��
                        goPanel.SetActive(true);
                        GameMgr.ChangeState(GameState.GameOver);

                    }

                    ////�V�[���ړ�
                    //if (Input.GetKeyDown(KeyCode.M))
                    //{
                    //    SceneManager.LoadScene("Stage2");
                    //}
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


    private void InitializeReferences()
    {
        // �V�[���J�ڌ�ɕK�v�ȃI�u�W�F�N�g���Ď擾
        goMosaic = GameObject.Find("Player Variant");
        goMosaic = goMosaic.transform.Find("Mosaic").gameObject;
        goPanel = GameObject.FindGameObjectWithTag("GameOver");
        playerControl = GameObject.Find("Player Variant").GetComponent<PlayerControl>();
        //�R���|�[�l���g�擾
        playerMoveAnimation = playerControl.GetComponent<PlayerMoveAnimation>();
        //�ő�l��ݒ�
        iUpperHPMax = UpperData.iPartHp;
        iLowerHPMax = LowerData.iPartHp;
        //�p�����[�^�̏�����
        iHumanity = iHumanityMax;
        iUpperHP = iUpperHPMax;
        iLowerHP = iLowerHPMax;



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
    /// �X�e�[�W�N���A���v���C���[�̏�Ԃ�ێ�����
    /// DropPart�ɌĂ�ł��炤
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
    public void DefaultBodyData()
    {
        UpperData = upperPlayer;
        LowerData = lowerPlayer;
        upperIndex = upperPlayer;
        lowerIndex = lowerPlayer;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �V�[���J�ڌ�ɎQ�Ƃ��Ď擾
        InitializeReferences();
        //upperIndex = UpperData;
        //lowerIndex = LowerData;
        Debug.Log($"�V�[�� {scene.name} �����[�h����܂���");
    }
}
