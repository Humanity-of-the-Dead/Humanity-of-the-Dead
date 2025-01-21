using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneInformation", menuName = "ScriptableObjects/StageInformation")]

public class SceneInformation : ScriptableObject
{
    // 全てのシーン
    [System.Serializable]
    public enum SCENE
    {
        Title,      // タイトル
        StageOne,   // ステージ１
        StageOne_BOSS,
        StageTwo,   // ステージ２
        StageTwo_BOSS,
        StageThree, // ステージ３
        StageThree_Five,//ステージ３.5　本当はステージ４がStageThree_Fiveでステージ5がStageFour（ボス）の役割を果たすが、色々と修正箇所が増えるためとりあえずで記載
        StageFour,//ボス戦
        End,
        


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
    [SerializeField] private SCENE currentScene;
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
    public int GetCurrentScene() {  return SceneManager.GetActiveScene().buildIndex; }


    public int GetSceneIndex(SCENE scene)
    {
        return sceneCount[(int)scene];
    }
    public SCENE GetPreviousScene() { return previousScene; }

    public void SetPreviousScene(SCENE scene) { previousScene = scene; }
    public void SetCurrentScene(SCENE scene) { currentScene = scene; }


}


