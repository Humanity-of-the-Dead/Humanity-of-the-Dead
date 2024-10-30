using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropPart : MonoBehaviour
{
    //�p�[�c�̃f�[�^
    private BodyPartsData partsData;
    ////�v���C���[��manager
    //PlayerParameter playerManager;

    //�v���C���[���
    GameObject goPlayerParameter;

    //�N���A�e�L�X�g
    GameObject goTextBox;

    //�{�X�t���O
    bool bBoss;

    void Start()
    {
        //�A�C�e���̉摜�ɂȂ�
        
    }

    // Update is called once per frame
    void Update()
    {
        //O�L�[����������ԗ삷��
        if(Input.GetKeyUp(KeyCode.O)) {
            goPlayerParameter.GetComponent<PlayerParameter>().comfort(10);
            if (bBoss)
            {
                goTextBox.GetComponent<GoalScript>().showText();
            }
            Destroy(this.gameObject);

        }
        //P�L�[����������ڐA����
        if (Input.GetKeyDown(KeyCode.P)){
            goPlayerParameter.GetComponent<PlayerParameter>().transplant(partsData);
            if (bBoss)
            {
                goTextBox.GetComponent<GoalScript>().showText();
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
        image.sprite = partsData.sPartSprite; 
    }

    public void getPlayerManegerObjet(GameObject obj)
    {
        goPlayerParameter = obj;
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
}
