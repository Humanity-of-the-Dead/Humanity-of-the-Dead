using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class newDropPart : MonoBehaviour
{
    //�p�[�c�̃f�[�^
    private BodyPartsData partsData;
    ////�v���C���[��manager
    //PlayerParameter playerManager;

    //�v���C���[���
    PlayerParameter scPlayerParameter;

    //�N���A�e�L�X�g
    GameObject goTextBox;

    //�{�X�t���O
    bool bBoss;
     SceneTransitionManager sceneTransitionManager;

    //�{�^���I�u�W�F�N�g
    [SerializeField] GameObject[] goButton;

    //����
    [SerializeField] GameObject goGrave;

    //�Q�[���N���A�̕W��
    GameObject goPanel;


    void Start()
    {
        //GameClear�^�O�����Q�[���I�u�W�F�N�g���擾
        goPanel = GameObject.Find("GameResult").gameObject;
        goPanel = goPanel.transform.Find("GameClear").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //J�L�[����������ԗ삷��
        if(Input.GetKeyUp(KeyCode.J) && goButton[1].activeSelf == true) {
            scPlayerParameter.comfort(10);
            MultiAudio.ins.PlaySEByName("SE_hero_action_irei");
            Debug.Log(this.transform.position);
            GameObject obj = Instantiate(goGrave);
            obj.transform.position = new Vector3(this.gameObject.transform.position.x,
                                                        0.5f, this.gameObject.transform.position.z);
            if (bBoss)
            {
                GameClear();
            }
            Destroy(this.gameObject);

        }
        //L�L�[����������ڐA����
        if (Input.GetKeyDown(KeyCode.L) && goButton[0].activeSelf == true)
        {
            scPlayerParameter.transplant(partsData);
            MultiAudio.ins.PlaySEByName("SE_hero_action_ishoku");
            if (bBoss)
            {
                GameClear();
            }
            Destroy(this.gameObject);
        }
    }

    //�p�[�c�f�[�^�̎擾
    public void getPartsData(BodyPartsData partsData)
    {
        this.partsData = partsData;
    }
    //�A�C�e���̉摜�ɂȂ�
    public void setImnage()
    {
        Image image = this.GetComponent<Image>();
        image.sprite = partsData.spBody; 
    }

    public void getPlayerManegerObjet(PlayerParameter scr)
    {
        scPlayerParameter = scr;
    }

    //�e�L�X�g�{�b�N�X�̎擾
    public void getTextBox(GameObject obj)
    {
        goTextBox = obj;
    }
    //�{�X�t���O
    public void getBossf(bool flag)
    {
        bBoss = flag;
    }
    
    //�ڐA
    public void getTransplant()
    {
        scPlayerParameter.transplant(partsData);
        Destroy(this.gameObject);
    }

    //�ԗ�
    public void getComfort()
    {
        scPlayerParameter.comfort(10);
        Destroy(this.gameObject);
    }
    public void getSceneTransition(SceneTransitionManager sceneTransitionManager)
    {
       this.sceneTransitionManager = sceneTransitionManager;    

    }

    //�Q�[���N���A����
    private void GameClear()
    {
        //�Q�[���N���A��\��
        goPanel.SetActive(true);
        //goTextBox.GetComponent<GoalScript>().showText();

        //�v���C���[�̏�Ԃ�ێ�����
        scPlayerParameter.KeepBodyData();
        
        //���݂̃V�[���̈��̃V�[���̃C���f�b�N�X���擾
        int iNextIndex = SceneTransitionManager.instance.sceneInformation.GetCurrentScene() + 1;
        //�X�e�[�W��4�̎�
        if (iNextIndex == 4)
        {
            scPlayerParameter.DefaultBodyData();
        }
        //�C���f�b�N�X������ɍs������^�C�g���̃C���f�b�N�X����
        if (iNextIndex > 4)
        {
            //DontDestroyOnLoad�ɂȂ��Ă���PlayerParameter�I�u�W�F�N�g���폜
            SceneManager.MoveGameObjectToScene(scPlayerParameter.gameObject, SceneManager.GetActiveScene());
            iNextIndex = 0;
        }
        GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().Stop();

        MultiAudio.ins.PlayBGM_ByName("BGM_clear");
        //�e�L�X�g�{�b�N�X�̕\��
        goTextBox.SetActive(true);
        //GameState��AfterBOss�ɐ؂�ւ���
        GameMgr.ChangeState(GameState.AfterBOss);
        //SceneTransitionManager.instance.NextSceneButton(iNextIndex);


    }
}
