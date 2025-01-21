using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class newDropPart : MonoBehaviour//
{
    //�p�[�c�̃f�[�^
    private BodyPartsData partsData;
    ////�v���C���[��manager
    //PlayerParameter playerManager;

    

    //�N���A�e�L�X�g
    //GameObject goTextBox;

    //�{�X�t���O
    bool bBoss;

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
        // J�L�[����������ԗ삷��
        if (Input.GetKeyUp(KeyCode.J) && goButton.Length > 0 && goButton[0] != null && goButton[0].activeSelf)
        {
            PlayerParameter.Instance.comfort(10);
            MultiAudio.ins.PlaySEByName("SE_hero_action_irei");
            Debug.Log(this.transform.position);
            GameObject obj = Instantiate(goGrave);
            obj.transform.position = new Vector3(this.gameObject.transform.position.x, 0.5f, this.gameObject.transform.position.z);
            if (bBoss)
            {
                GameClear();
            }
            Destroy(this.gameObject);
        }

        // L�L�[����������ڐA����
        if (Input.GetKeyDown(KeyCode.L) && goButton.Length > 1 && goButton[1] != null && goButton[1].activeSelf)
        {
            PlayerParameter.Instance.transplant(partsData);
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
   
   
    //�e�L�X�g�{�b�N�X�̎擾
    //public void getTextBox(GameObject obj)
    //{
    //    goTextBox = obj;
    //}
    //�{�X�t���O
    public void getBossf(bool flag)
    {
        bBoss = flag;
    }

    //�ڐA
    public void getTransplant()
    {
        PlayerParameter.Instance.transplant(partsData);
        Destroy(gameObject);
    }

    //�ԗ�
    public void getComfort()
    {
        PlayerParameter.Instance.comfort(10);
        Destroy(gameObject);
    }
  

    //�Q�[���N���A����
    private void GameClear()
    {
        ////�Q�[���N���A��\��
        //goPanel.SetActive(true);
        //goTextBox.GetComponent<GoalScript>().showText();
        //�e�L�X�g�{�b�N�X�̕\��
        //goTextBox.SetActive(true);
        //GameState��AfterBOss�ɐ؂�ւ���
        GameMgr.ChangeState(GameState.AfterBOss);
        //SceneTransitionManager.instance.NextSceneButton(iNextIndex);
        
        //�v���C���[�̏�Ԃ�ێ�����
        PlayerParameter.Instance.KeepBodyData();

        //���݂̃V�[���̈��̃V�[���̃C���f�b�N�X���擾
        int iNextIndex = SceneTransitionManager.instance.sceneInformation.GetCurrentScene() + 1;
        //�X�e�[�W��4�̎�
        if (iNextIndex == 4)
        {
            PlayerParameter.Instance.DefaultBodyData();
        }
        //�C���f�b�N�X������ɍs������^�C�g���̃C���f�b�N�X����
        if (iNextIndex > 4)
        {
            //DontDestroyOnLoad�ɂȂ��Ă���PlayerParameter�I�u�W�F�N�g���폜
            SceneManager.MoveGameObjectToScene(PlayerParameter.Instance.gameObject, SceneManager.GetActiveScene());
            iNextIndex = 0;
        }
        

    }

    /// <summary>
    /// ���Ɛڂ�����
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Car"))
        {
            Rigidbody2D dropRigidbody =GetComponent<Rigidbody2D>();
            //dropRigidbody.bodyType = RigidbodyType2D.Kinematic; 
           
        }
    }
}
