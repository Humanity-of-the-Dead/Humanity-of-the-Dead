using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    //�I�v�V�������
    [SerializeField]GameObject optScr;

    //�I�v�V������ʂ�\�����Ă��邩
    bool isShoOptScr = false;

    // Start is called before the first frame update
    void Start()
    {
        //�I�v�V������ʂ��Ƃ���
        optScr.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�I�v�V�����{�^���������ꂽ�Ƃ�
    //�I�v�V������ʂ��Ƃ��Ă�����I�v�V������ʂ�\��
    //�I�v�V������ʂ��J���Ă��������
    public void OnCrickOptionButton()
    {
        //�I�v�V������ʂ����Ă���Ƃ�
        if (!isShoOptScr)
        {
            isShoOptScr=true;

            //�I�v�V������ʂ�\��
            optScr.SetActive(true);
        }
        else
        {
            isShoOptScr=false;

            //�I�v�V������ʂ��\��
            optScr.SetActive(false);
        }
    }
}
