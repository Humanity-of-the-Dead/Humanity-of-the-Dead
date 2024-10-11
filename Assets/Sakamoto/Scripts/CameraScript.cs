using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject goTarget;
    [SerializeField] float fMoveLimit;
    //�J�������猩���^�[�Q�b�g�̈ʒu
    Vector2 fTrgPosFromCamera;

    bool fMoveRight;
    bool fMoveLeft;
    // Start is called before the first frame update
    void Start()
    {
        fMoveRight = false;
        fMoveLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        //�^�[�Q�b�g�̑��Έʒu�̎擾
        fTrgPosFromCamera = goTarget.transform.position - this.transform.position;
        //�^�[�Q�b�g��x�ʒu��3�ȏ�Ȃ�J�������E�Ɉړ�������
        if(fTrgPosFromCamera.x > 3 && fMoveRight == false)
        {
            fMoveRight = true;
        }
        //�^�[�Q�b�g��x�ʒu��-3�ȉ��Ȃ�J����������Ɉړ�������
        if (fTrgPosFromCamera.x < -3 && fMoveLeft == false)
        {
            fMoveLeft = true;
        }

        Debug.Log("�E" + fMoveRight);
        Debug.Log("��" + fMoveLeft);
        if(fMoveRight == true)
        {
            if (this.transform.position.x < fMoveLimit)
            {
                Vector3 pos = this.transform.position;
                pos.x += 0.2f;
                this.transform.position = pos;
                if(fTrgPosFromCamera.x < 0)
                {
                    fMoveRight = false;
                }
            }
        }
        if(fMoveLeft == true)
        {
            if (this.transform.position.x > 0)
            {
                Vector3 pos = this.transform.position;
                pos.x -= 0.2f;
                this.transform.position = pos;
                if(fTrgPosFromCamera.x > 0)
                {
                    fMoveLeft = false;
                }
            }
        }
    }
}
