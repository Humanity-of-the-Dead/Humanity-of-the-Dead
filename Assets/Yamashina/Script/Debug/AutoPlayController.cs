using UnityEngine;

public class AutoPlayController : MonoBehaviour
{
    //[SerializeField] PlayerControl playerControl; // PlayerControlスクリプトを参照
    //[SerializeField] float moveDuration = 5.0f;  // 移動のタイミングを設定
    //private float timeElapsed = 0.0f;            // 経過時間

    void Update()
    {
        //if (playerControl == null) return;  // PlayerControlが設定されていない場合は何もしない

        //AutoMove();
        //AutoJump();
        //AutoAttack();
    }

    // 自動移動処理（ランダムに左右移動）
    //private void AutoMove()
    //{
    //    //timeElapsed += Time.deltaTime;
    //    //if (timeElapsed >= moveDuration)
    //    //{
    //    //    // ランダムに左右移動
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

    // 自動ジャンプ
    //private void AutoJump()
    //{
    //    if (playerControl.CanJump())
    //    {
    //        playerControl.Jump();
    //    }
    //}

    // 自動攻撃（上半身と下半身攻撃）
    //private void AutoAttack()
    //{
    //    if (playerControl.CanAttack())
    //    {
    //        playerControl.UpperBodyAttack();
    //        playerControl.LowerBodyAttack();
    //    }
    //}
}
