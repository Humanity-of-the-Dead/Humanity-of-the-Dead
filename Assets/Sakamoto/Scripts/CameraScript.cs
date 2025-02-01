using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

enum STATE
{
    NONE,
    NOMAL, // ノーマルステージ
    BOSS   // ボスステージ
}

public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject goTarget;
    [SerializeField] private float fMoveStart;
    [SerializeField] private float fMoveLimit;
    [SerializeField] private float smoothSpeed = 5.0f; // カメラの追従速度

    private STATE eState = STATE.NONE;

    void Start()
    {
        eState = STATE.NOMAL;
        goTarget = GameObject.Find("Player Variant");
    }

    void Update()
    {
        // シーン名を取得してボスステージかどうか判定
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

    // プレイヤーをスムーズに追従する処理
    void FollowPlayer()
    {
        Vector3 targetPos = transform.position;

        // X方向の追従
        if (goTarget.transform.position.x > fMoveStart)
        {
            targetPos.x = Mathf.Lerp(transform.position.x, goTarget.transform.position.x, Time.deltaTime * smoothSpeed);
        }

        // Xの上限を適用
        targetPos.x = Mathf.Min(targetPos.x, fMoveLimit);

        // Y方向の追従（最小0に制限）
        targetPos.y = Mathf.Lerp(transform.position.y, goTarget.transform.position.y, Time.deltaTime * smoothSpeed);
        targetPos.y = Mathf.Max(targetPos.y, 0);

        transform.position = targetPos;
    }

    // ボス戦時のカメラ制御
    void FollowBossMode()
    {
        Vector3 targetPos = transform.position;

        // X座標を固定
        targetPos.x = fMoveLimit;

        // Y座標を制限付きで追従
        float targetY = Mathf.Lerp(transform.position.y, goTarget.transform.position.y, Time.deltaTime * smoothSpeed);
        targetPos.y = Mathf.Clamp(targetY, 0, 2);

        transform.position = targetPos;
    }
}
