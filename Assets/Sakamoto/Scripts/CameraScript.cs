using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum STATE { 
    NONE,
    NOMAL,//ノーマルステージ
    BOSS,//ボスステージ
}


public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject goTarget;
    [SerializeField] float fMoveStart;
    [SerializeField] float fMoveLimit;

    ////カメラから見たターゲットの位置
    //Vector2 fTrgPosFromCamera;

    //ゲームステート
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
                //プレイヤーが画面中央に来たら追従する
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

                // カメラのX座標を固定
                bossCamPos.x = fMoveLimit;

                // プレイヤーのジャンプを追従しつつ制約を追加
                float targetY = goTarget.transform.position.y;
                bossCamPos.y = Mathf.Clamp(targetY, 0, 2); 

                this.GetComponent<Transform>().position = bossCamPos;

                //カメラ追従なし
                break;
        }
    }
}

//旧アップデートの中身
////ターゲットの相対位置の取得
//fTrgPosFromCamera = goTarget.transform.position - this.transform.position;
////ターゲットのx位置が3以上ならカメラを右に移動させる
//if(fTrgPosFromCamera.x > 3 && fMoveRight == false)
//{
//    fMoveRight = true;
//}
////ターゲットのx位置が-3以下ならカメラを左手に移動させる
//if (fTrgPosFromCamera.x < -3 && fMoveLeft == false)
//{
//    fMoveLeft = true;
//}

//Debug.Log("右" + fMoveRight);
//Debug.Log("左" + fMoveLeft);
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
