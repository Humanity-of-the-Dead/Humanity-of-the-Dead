using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum STATE { 
    NONE,
    NOMAL,//ノーマルステージ
    BOSS,//ボスステージ
}


public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject goTarget;
    [SerializeField] private float fMoveStart;
    [SerializeField] private float fMoveLimit;

    ////カメラから見たターゲットの位置
    //Vector2 fTrgPosFromCamera;

    //ゲームステート
   private STATE eState = STATE.NONE;

    //bool fMoveRight;
    //bool fMoveLeft;
    // Start is called before the first frame update
    void Start()
    {
        eState = STATE.NOMAL;
        goTarget = GameObject.Find("Player Variant");
    }

    // Update is called once per frame
    void Update()
    {
        switch (eState)
        {
            case STATE.NOMAL:
                //プレイヤーが画面中央に来たら追従する
                Vector3 vCamPos = transform.position;
                if (goTarget.transform.position.x > fMoveStart)
                {
                    vCamPos.x = goTarget.transform.position.x;
                    transform.position = vCamPos; 
                }
                if(transform.position.x > fMoveLimit)
                {
                    vCamPos.x = fMoveLimit;
                    transform.position = vCamPos;
                }               
                vCamPos.y = goTarget.transform.position.y;
                string sceneName = SceneManager.GetActiveScene().name;
                switch (sceneName)
                {
                    case string name when name == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageOne_BOSS):
                        eState = STATE.BOSS;
                        break;
                    case string name when name == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageTwo_BOSS):
                        eState = STATE.BOSS;

                        break;
                    case string name when name == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageThree_BOSS):
                        eState = STATE.BOSS;
                        break;
                    case string name when name == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageFour):
                        eState = STATE.BOSS;
                        break;
                    case string name when name == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageFive):
                        eState = STATE.BOSS;
                        break;

                }

                if (vCamPos.y < 0)
                {
                    vCamPos.y = 0;
                }
                transform.position = vCamPos;
                break;
            case STATE.BOSS:
                Vector3 bossCamPos = transform.position;

                // カメラのX座標を固定
                bossCamPos.x = fMoveLimit;

                // プレイヤーのジャンプを追従しつつ制約を追加
                float targetY = goTarget.transform.position.y;
                bossCamPos.y = Mathf.Clamp(targetY, 0, 2); 

             transform.position = bossCamPos;

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
