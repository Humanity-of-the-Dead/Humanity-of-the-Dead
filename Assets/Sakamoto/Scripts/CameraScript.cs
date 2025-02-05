using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

enum STATE
{
    NONE,
    NORMAL,//ノーマルステージ
    STAGE3,
    BOSSLAB,
    BOSS,//ボスステージ
    GAMEOVER,
}


public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject goTarget;
    [SerializeField] private float fMoveStart;
    [SerializeField] private float fMoveLimit;

    ////カメラから見たターゲットの位置
    //Vector2 fTrgPosFromCamera;
    private Vector3 cameraPos;
    //ゲームステート
    private STATE eState = STATE.NONE;

    //bool fMoveRight;
    //bool fMoveLeft;
    // Start is called before the first frame update
    void Start()
    {
        eState = STATE.NORMAL;
        goTarget = GameObject.Find("Player Variant");
        cameraPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (eState)
        {
            case STATE.NORMAL:

                CameraXTracking();
                cameraPos.y = goTarget.transform.position.y;
                string sceneName = SceneManager.GetActiveScene().name;
                var bossScenes = new[]
                {
                  SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageOne_BOSS),
                  SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageTwo_BOSS),

                };

                if (bossScenes.Contains(sceneName))
                {
                    eState = STATE.BOSS;
                }

                if (sceneName == SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageThree))
                {
                    eState = STATE.STAGE3;
                }

                var bossLabScenes = new[]
                {
                SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageThree_BOSS),
                  SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageThreeDotFive),
                  SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageFour)
                };
                if (bossLabScenes.Contains(sceneName))
                {
                    eState = STATE.BOSSLAB;

                }
                if (GameMgr.GetState() == GameState.GameOver)
                {
                    eState = STATE.GAMEOVER;
                }

                if (cameraPos.y < 0)
                {
                    cameraPos.y = 0;
                }
                transform.position = cameraPos;
                break;
            case STATE.BOSS:

                // カメラのX座標を固定
                cameraPos.x = fMoveLimit;

                // プレイヤーのジャンプを追従しつつ制約を追加
                float targetY = goTarget.transform.position.y;
                cameraPos.y = Mathf.Clamp(targetY, 0, 2);

                transform.position = cameraPos;

                //カメラ追従なし
                break;
            case STATE.STAGE3:
                CameraXTracking();
                //Debug.Log($"transform.position.y01: {transform.position.y}");
                // プレイヤーのジャンプを追従しつつ制約を追加
                cameraPos.y = Mathf.Clamp(goTarget.transform.position.y, 0, 1.5f);
                transform.position = cameraPos;

                //Debug.Log($"transform.position.y02: {transform.position.y}");

                break;
            case STATE.BOSSLAB:
                cameraPos.x = fMoveLimit;
                cameraPos.y = Mathf.Clamp(goTarget.transform.position.y, 0, 1.5f);
                transform.position = cameraPos;


                break;
            case STATE.GAMEOVER:
                // 追従しない
                break;
        }


    }
    private void CameraXTracking()
    {
        //プレイヤーが画面中央に来たら追従する
        if (goTarget.transform.position.x > fMoveStart)
        {
            cameraPos.x = goTarget.transform.position.x;
            //Debug.Log($"transform.position.y03: {transform.position.y}");

            transform.position = cameraPos;
            //Debug.Log($"transform.position.y04: {transform.position.y}");

        }
        if (transform.position.x > fMoveLimit)
        {
            cameraPos.x = fMoveLimit;
            //Debug.Log($"transform.position.y05: {transform.position.y}");

            transform.position = cameraPos;
            //Debug.Log($"transform.position.y06: {transform.position.y}");

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
