using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject goTarget;
    [SerializeField] float fMoveLimit;
    //カメラから見たターゲットの位置
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
        //ターゲットの相対位置の取得
        fTrgPosFromCamera = goTarget.transform.position - this.transform.position;
        //ターゲットのx位置が3以上ならカメラを右に移動させる
        if(fTrgPosFromCamera.x > 3 && fMoveRight == false)
        {
            fMoveRight = true;
        }
        //ターゲットのx位置が-3以下ならカメラを左手に移動させる
        if (fTrgPosFromCamera.x < -3 && fMoveLeft == false)
        {
            fMoveLeft = true;
        }

        Debug.Log("右" + fMoveRight);
        Debug.Log("左" + fMoveLeft);
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
