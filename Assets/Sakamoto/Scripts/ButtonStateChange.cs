using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStateChange : MonoBehaviour
{
    //�{�^�����������Ƃ��Q�[���X�e�[�g��ShowOption�ɂ���
    public void ChangeStateShowOption()
    {
        GameMgr.ChangeState(GameState.ShowOption);
    }
    //�{�^�����������Ƃ��Q�[���X�e�[�g��Main�ɂ���
    public void ChangeStateMain()
    {
        GameMgr.ChangeState(GameState.Main);
    }
}
