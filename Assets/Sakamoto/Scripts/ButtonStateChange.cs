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
    public void ChangeStateShowHint()
    {
        GameMgr.ChangeState(GameState.Hint);
    }
    //�{�^�����������Ƃ��Q�[���X�e�[�g��Main�ɂ���
    public void ChangeStateMain()
    {
        GameMgr.ChangeState(GameState.Main);
    }
}
