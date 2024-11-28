using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        //J�L�[����������ԗ삷��
        if(Input.GetKeyUp(KeyCode.J) && goButton[1].activeSelf == true) {
            scPlayerParameter.comfort(10);
            if (bBoss)
            {
                goTextBox.GetComponent<GoalScript>().showText();
                sceneTransitionManager.SceneChange(SceneInformation.SCENE.Title);

            }
            Debug.Log(this.transform.position);
            GameObject obj = Instantiate(goGrave);
            obj.transform.position = new Vector3(this.gameObject.transform.position.x,
                                                        0.5f, this.gameObject.transform.position.z);
            Destroy(this.gameObject);

        }
        //L�L�[����������ڐA����
        if (Input.GetKeyDown(KeyCode.L) && goButton[0].activeSelf == true)
        {
            scPlayerParameter.transplant(partsData);
            if (bBoss)
            {
                //goTextBox.GetComponent<GoalScript>().showText();
                sceneTransitionManager.SceneChange(SceneInformation.SCENE.Title);
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


}
