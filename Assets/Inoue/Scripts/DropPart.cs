using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropPart : MonoBehaviour
{
    //�p�[�c�̃f�[�^
    private BodyPartsData partsData;
    //�v���C���[��manager
    PlayerParameter playerManager;
    void Start()
    {
        //�A�C�e���̉摜�ɂȂ�
        
    }

    // Update is called once per frame
    void Update()
    {
        //P�L�[����������ڐA����
        if (Input.GetKeyDown(KeyCode.P)){
            playerManager.transplant(partsData);
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
}
