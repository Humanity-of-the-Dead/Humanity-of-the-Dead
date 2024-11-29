using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SceneInformation", menuName = "ScriptableObjects/StageInformation")]

public class SceneInformation : ScriptableObject
{
    // 全てのシーン
    [System.Serializable]
    public enum SCENE
    {
        Title,      // タイトル
        StageOne,   // ステージ１
        StageTwo,   // ステージ２
        StageThree, // ステージ３
        StageFour,

    }
    // ステージのシーン
    [System.Serializable]
    public enum STAGE
    {
        One,      // ステージ１
        Two,      // ステージ２
        Three,    // ステージ３
        Four,
    }

    [SerializeField] public SceneObject[] sceneObject;
    [SerializeField] public string[] sceneNames;// シーンの名前
    [SerializeField] private SCENE previousScene;
    [SerializeField] public int[] sceneCount;
    public SceneObject GetSceneObject(SCENE scene)
    {
        return sceneObject[(int)scene];
    }

    public string GetSceneName(SCENE scene)
    {
        return sceneNames[(int)scene];
    }
    public int GetSceneInt(SCENE scene)
    {
        return (int)scene;
    }

    public int GetSceneIndex(SCENE scene)
    {
        return sceneCount[(int)scene];
    }
    public SCENE GetPreviousScene() { return previousScene; }

    public void SetPreviousScene(SCENE scene) { previousScene = scene; }
}


