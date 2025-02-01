using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

enum STATE
{
    NONE,
    NOMAL, // �m�[�}���X�e�[�W
    BOSS   // �{�X�X�e�[�W
}

public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject goTarget;
    [SerializeField] private float fMoveStart;
    [SerializeField] private float fMoveLimit;
    [SerializeField] private float smoothSpeed = 5.0f; // �J�����̒Ǐ]���x

    private STATE eState = STATE.NONE;

    void Start()
    {
        eState = STATE.NOMAL;
        goTarget = GameObject.Find("Player Variant");
    }

    void Update()
    {
        // �V�[�������擾���ă{�X�X�e�[�W���ǂ�������
        string sceneName = SceneManager.GetActiveScene().name;
        var bossScenes = new[]
        {
            SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageOne_BOSS),
            SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageTwo_BOSS),
            SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageThree_BOSS),
            SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageFour),
            SceneTransitionManager.instance.sceneInformation.GetSceneName(SceneInformation.SCENE.StageFive)
        };

        if (bossScenes.Contains(sceneName))
        {
            eState = STATE.BOSS;
        }
    }

    void LateUpdate()
    {
        switch (eState)
        {
            case STATE.NOMAL:
                FollowPlayer();
                break;

            case STATE.BOSS:
                FollowBossMode();
                break;
        }
    }

    // �v���C���[���X���[�Y�ɒǏ]���鏈��
    void FollowPlayer()
    {
        Vector3 targetPos = transform.position;

        // X�����̒Ǐ]
        if (goTarget.transform.position.x > fMoveStart)
        {
            targetPos.x = Mathf.Lerp(transform.position.x, goTarget.transform.position.x, Time.deltaTime * smoothSpeed);
        }

        // X�̏����K�p
        targetPos.x = Mathf.Min(targetPos.x, fMoveLimit);

        // Y�����̒Ǐ]�i�ŏ�0�ɐ����j
        targetPos.y = Mathf.Lerp(transform.position.y, goTarget.transform.position.y, Time.deltaTime * smoothSpeed);
        targetPos.y = Mathf.Max(targetPos.y, 0);

        transform.position = targetPos;
    }

    // �{�X�펞�̃J��������
    void FollowBossMode()
    {
        Vector3 targetPos = transform.position;

        // X���W���Œ�
        targetPos.x = fMoveLimit;

        // Y���W�𐧌��t���ŒǏ]
        float targetY = Mathf.Lerp(transform.position.y, goTarget.transform.position.y, Time.deltaTime * smoothSpeed);
        targetPos.y = Mathf.Clamp(targetY, 0, 2);

        transform.position = targetPos;
    }
}
