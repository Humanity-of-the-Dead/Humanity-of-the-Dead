using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerParameter : MonoBehaviour
{
    //�Q�[���}�l�[�W���[
    [SerializeField]GameObject scGameMgr;

    //�ڐA���̃��U�C�N
    GameObject goMosaic;

    public static PlayerParameter Instance;

    [Header("1��������̂ɂ����鎞��")]
    [SerializeField] int iDownTime;

    public  int iHumanityMax;     //�l�Ԑ��̍ő�l
    public  int iUpperHPMax;      //�㔼�g��HP�̍ő�l
    public  int iLowerHPMax;      //�����g��HP�̍ő�l

    private float iHumanity;     //�l�Ԑ�
    private float iUpperHP;      //�㔼�g��HP
    private float iLowerHP;      //�����g��HP
    // Start is called before the first frame update

    [Header("�v���C���[�I�u�W�F�N�g")]
    [SerializeField] GameObject goPlayer;

    //�㔼�g�̃p�[�c�f�[�^
    public BodyPartsData UpperData;
    //�����g�̃p�[�c�f�[�^
    public BodyPartsData LowerData;
    //�L�����̃C���[�W�擾�p
    PlayerMoveAnimation scPlayerMoveAnimation;
    //�㔼�g�̃p�[�c�f�[�^(�ۑ��p)
    private BodyPartsData upperIndex;
    //�����g�̃p�[�c�f�[�^(�ۑ��p)
    private BodyPartsData lowerIndex;
    //�㔼�g�̃p�[�c�f�[�^(�X�e�[�W4�p)
    private BodyPartsData upperPlayer;
    //�����g�̃p�[�c�f�[�^(�X�e�[�W4�p)
    private BodyPartsData lowerPlayer;



    //�Q�[���I�[�o�[�̕W��
    GameObject goPanel;
    SceneTransitionManager sceneTransitionManager;


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
        scPlayerMoveAnimation = goPlayer.GetComponent<PlayerMoveAnimation>();

        //�V�[���J�ڂŔj������Ȃ�
        DontDestroyOnLoad(gameObject);

        // �V�[�������[�h���ꂽ��ɎQ�Ƃ��Ď擾
        SceneManager.sceneLoaded += OnSceneLoaded;
        sceneTransitionManager = GameObject.FindAnyObjectByType<SceneTransitionManager>();
    }
    private void Update()
    {
       string SceneName = SceneManager.GetActiveScene().name;
        if(!(SceneName == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.Title)))
        {
            switch (GameMgr.GetState())
            {
                case GameState.Main:
                    //�p�����[�^�̒l��iDownTime�b��1����������
                    iHumanity -= Time.deltaTime / iDownTime/* *dgbScale*/;
                    iUpperHP -= Time.deltaTime / iDownTime;
                    iLowerHP -= Time.deltaTime / iDownTime;
                    Debug.Log(iDownTime);
                    if (iHumanity < 0 || iUpperHP < 0 || iLowerHP < 0)
                    {

                        Debug.Log("�����[�h���J�n���܂�"); // �f�o�b�O���O�Ŋm�F

                        GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().Stop();
                        


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
                scPlayerMoveAnimation.ChangeUpperBody(partsData);
                //�U�����[�V�����̕ύX
                scPlayerMoveAnimation.ChangeUpperMove(partsData.upperAttack);
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
                scPlayerMoveAnimation.ChangeUnderBody(partsData);
                //�U�����[�V�����̕ύX
                scPlayerMoveAnimation.ChangeLowerMove(partsData.lowerAttack);
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
        scGameMgr = GameObject.FindGameObjectWithTag("GameManager");
        goMosaic = GameObject.Find("Player Variant").gameObject;
        goMosaic = goMosaic.transform.Find("Mosaic").gameObject;
        goPlayer = GameObject.Find("Player Variant").gameObject;
        goPanel = GameObject.FindGameObjectWithTag("GameOver");

        //�R���|�[�l���g�擾
        scPlayerMoveAnimation = goPlayer.GetComponent<PlayerMoveAnimation>();

        //�ő�l��ݒ�
        iUpperHPMax = UpperData.iPartHp;
        iLowerHPMax = LowerData.iPartHp;
        //�p�����[�^�̏�����
        iHumanity = iHumanityMax;
        iUpperHP = iUpperHPMax;
        iLowerHP = iLowerHPMax;

        scPlayerMoveAnimation.ChangeUpperBody(UpperData);
        scPlayerMoveAnimation.ChangeUpperMove(UpperData.upperAttack);
        scPlayerMoveAnimation.ChangeUnderBody(LowerData);
        scPlayerMoveAnimation.ChangeLowerMove(LowerData.lowerAttack);

        if (scGameMgr == null || goMosaic == null || goPanel == null)
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
