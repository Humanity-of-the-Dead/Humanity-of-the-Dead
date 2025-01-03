using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum STATE { 
    NONE,
    NOMAL,//�m�[�}���X�e�[�W
    BOSS,//�{�X�X�e�[�W
}


public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject goTarget;
    [SerializeField] float fMoveStart;
    [SerializeField] float fMoveLimit;

    ////�J�������猩���^�[�Q�b�g�̈ʒu
    //Vector2 fTrgPosFromCamera;

    //�Q�[���X�e�[�g
    STATE eState = STATE.NONE;

    //bool fMoveRight;
    //bool fMoveLeft;
    // Start is called before the first frame update
    void Start()
    {
        eState = STATE.NOMAL;
        //fMoveRight = false;
        //fMoveLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (eState)
        {
            case STATE.NOMAL:
                //�v���C���[����ʒ����ɗ�����Ǐ]����
                Vector3 vCamPos = this.GetComponent<Transform>().position;
                if (goTarget.transform.position.x > fMoveStart)
                {
                    vCamPos.x = goTarget.transform.position.x;
                    this.GetComponent<Transform>().position = vCamPos; 
                }
                if(this.transform.position.x > fMoveLimit)
                {
                    vCamPos.x = fMoveLimit;
                    this.GetComponent<Transform>().position = vCamPos;
                    eState = STATE.BOSS;
                }               
                vCamPos.y = goTarget.transform.position.y;
                if(vCamPos.y < 0)
                {
                    vCamPos.y = 0;
                }
                this.GetComponent<Transform>().position = vCamPos;
                break;
            case STATE.BOSS:
                Vector3 bossCamPos = this.transform.position;

                // �J������X���W���Œ�
                bossCamPos.x = fMoveLimit;

                // �v���C���[�̃W�����v��Ǐ]�������ǉ�
                float targetY = goTarget.transform.position.y;
                bossCamPos.y = Mathf.Clamp(targetY, 0, 2); 

                this.GetComponent<Transform>().position = bossCamPos;

                //�J�����Ǐ]�Ȃ�
                break;
        }
    }
}

//���A�b�v�f�[�g�̒��g
////�^�[�Q�b�g�̑��Έʒu�̎擾
//fTrgPosFromCamera = goTarget.transform.position - this.transform.position;
////�^�[�Q�b�g��x�ʒu��3�ȏ�Ȃ�J�������E�Ɉړ�������
//if(fTrgPosFromCamera.x > 3 && fMoveRight == false)
//{
//    fMoveRight = true;
//}
////�^�[�Q�b�g��x�ʒu��-3�ȉ��Ȃ�J����������Ɉړ�������
//if (fTrgPosFromCamera.x < -3 && fMoveLeft == false)
//{
//    fMoveLeft = true;
//}

//Debug.Log("�E" + fMoveRight);
//Debug.Log("��" + fMoveLeft);
//if(fMoveRight == true)
//{
//    if (this.transform.position.x < fMoveLimit)
//    {
//        Vector3 pos = this.transform.position;
//        pos.x += 0.2f;
//        this.transform.position = pos;
//        if(fTrgPosFromCamera.x < 0)
//        {
//            fMoveRight = false;
//        }
//    }
//}
//if(fMoveLeft == true)
//{
//    if (this.transform.position.x > 0)
//    {
//        Vector3 pos = this.transform.position;
//        pos.x -= 0.2f;
//        this.transform.position = pos;
//        if(fTrgPosFromCamera.x > 0)
//        {
//            fMoveLeft = false;
//        }
//    }
//}
