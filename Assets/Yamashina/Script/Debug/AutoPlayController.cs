using UnityEngine;

public class AutoPlayController : MonoBehaviour
{
    //[SerializeField] PlayerControl playerControl; // PlayerControl�X�N���v�g���Q��
    //[SerializeField] float moveDuration = 5.0f;  // �ړ��̃^�C�~���O��ݒ�
    //private float timeElapsed = 0.0f;            // �o�ߎ���

    void Update()
    {
        //if (playerControl == null) return;  // PlayerControl���ݒ肳��Ă��Ȃ��ꍇ�͉������Ȃ�

        //AutoMove();
        //AutoJump();
        //AutoAttack();
    }

    // �����ړ������i�����_���ɍ��E�ړ��j
    //private void AutoMove()
    //{
    //    //timeElapsed += Time.deltaTime;
    //    //if (timeElapsed >= moveDuration)
    //    //{
    //    //    // �����_���ɍ��E�ړ�
    //    //    if (Random.value > 0.5f)
    //    //    {
    //    //        playerControl.MoveLeft();
    //    //    }
    //    //    else
    //    //    {
    //    //        playerControl.MoveRight();
    //    //    }
    //    //    timeElapsed = 0f;
    //    //}
    //}

    // �����W�����v
    //private void AutoJump()
    //{
    //    if (playerControl.CanJump())
    //    {
    //        playerControl.Jump();
    //    }
    //}

    // �����U���i�㔼�g�Ɖ����g�U���j
    //private void AutoAttack()
    //{
    //    if (playerControl.CanAttack())
    //    {
    //        playerControl.UpperBodyAttack();
    //        playerControl.LowerBodyAttack();
    //    }
    //}
}
