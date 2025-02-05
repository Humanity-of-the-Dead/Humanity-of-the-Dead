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
    NORMAL,//�m�[�}���X�e�[�W
    STAGE3,
    BOSSLAB,
    BOSS,//�{�X�X�e�[�W
    GAMEOVER,
}


public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject goTarget;
    [SerializeField] private float fMoveStart;
    [SerializeField] private float fMoveLimit;

    ////�J�������猩���^�[�Q�b�g�̈ʒu
    //Vector2 fTrgPosFromCamera;
    private Vector3 cameraPos;
    //�Q�[���X�e�[�g
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

                // �J������X���W���Œ�
                cameraPos.x = fMoveLimit;

                // �v���C���[�̃W�����v��Ǐ]�������ǉ�
                float targetY = goTarget.transform.position.y;
                cameraPos.y = Mathf.Clamp(targetY, 0, 2);

                transform.position = cameraPos;

                //�J�����Ǐ]�Ȃ�
                break;
            case STATE.STAGE3:
                CameraXTracking();
                //Debug.Log($"transform.position.y01: {transform.position.y}");
                // �v���C���[�̃W�����v��Ǐ]�������ǉ�
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
                // �Ǐ]���Ȃ�
                break;
        }


    }
    private void CameraXTracking()
    {
        //�v���C���[����ʒ����ɗ�����Ǐ]����
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

//���A�b�v�f�[�g�̒��g
////�^�[�Q�b�g�̑��Έʒu�̎擾
//fTrgPosFromCamera = goTarget.transform.position - this.transform.position;
////�^�[�Q�b�g��x�ʒu��3�ȏ�Ȃ�J�������E�Ɉړ�������
//if(fTrgPosFromCamera.x > 3 && fMoveRight == false)
//{
//    fMoveRight = true;
//}
////�^�[�Q�b�g��x�ʒu��-3�ȉ��Ȃ�J����������Ɉړ�������
//if (fTrgPosFromCamera.x < -3 && fMoveLeft == false)
//{
//    fMoveLeft = true;
//}

//Debug.Log("�E" + fMoveRight);
//Debug.Log("��" + fMoveLeft);
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
