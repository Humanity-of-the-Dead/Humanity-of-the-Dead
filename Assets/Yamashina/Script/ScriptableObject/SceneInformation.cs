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
        Tutorial,
        StageOne,   // ステージ１
        StageOne_BOSS,
        StageTwo,   // ステージ２
        StageTwo_BOSS,
        StageThree, // ステージ３
        StageThree_BOSS,
        StageThreeDotFive,//親友戦闘
        StageFour,//ボス
        End,



    }
    // ステージのシーン
    //[System.Serializable]
    //public enum STAGE
    //{
    //    One,      // ステージ１
    //    Two,      // ステージ２
    //    Three,    // ステージ３
    //    Four,
    //}

    [SerializeField] public SceneObject[] sceneObject;
    [SerializeField] public string[] sceneNames;// シーンの名前

    [SerializeField] private SCENE previousScene;
    [SerializeField] public SCENE currentScene = SCENE.Title;
    [SerializeField] private SCENE nextScene;

    [SerializeField] public int[] sceneCount;
    public SceneObject GetSceneObject(SCENE scene)
    {
        return sceneObject[(int)scene];
    }


    public string GetSceneName(SCENE scene)
    {
        int index = (int)scene;
        if (index >= 0 && index < sceneNames.Length)
        {
            return sceneNames[index];
        }
        return "UnknownScene"; // 安全なデフォルト値
    }
    public int GetSceneInt(SCENE scene)
    {
        return (int)scene;
    }
    public int GetCurrentSceneInt() { return SceneManager.GetActiveScene().buildIndex; }

    public SCENE GetCurrentScene() { return currentScene; }
    public void SetCurrentScene(SCENE scene) { currentScene = scene; }
    public string GetCurrentSceneName()
    {
        return sceneNames[(int)currentScene]; // previousScene の名前を取得
    }
    public int GetSceneIndex(SCENE scene)
    {
        return sceneCount[(int)scene];
    }
    public SCENE GetPreviousScene() { return previousScene; }
    public string GetPreviousSceneName()
    {
        return sceneNames[(int)previousScene]; // previousScene の名前を取得
    }
    public void SetPreviousScene(SCENE scene) { previousScene = scene; }
    public SCENE GetNextScene() { return nextScene; }

    public string GetNextSceneName()
    {
        return sceneNames[(int)nextScene]; // previousScene の名前を取得

    }

    public void UpdateScene(SCENE newScene)
    {
        Debug.Log($"Before UpdateScene: previousScene = {previousScene}, currentScene = {currentScene}, nextScene = {nextScene}");

        // `previousScene` を `currentScene` に更新
        previousScene = currentScene;

        // `currentScene` を `newScene` に更新
        currentScene = newScene;

        // `nextScene` を手動で設定（シーンの流れを明示的に決める）
        switch (newScene)
        {
            case SCENE.Title:
                nextScene = SCENE.StageOne;
                break;
            case SCENE.StageOne:
                nextScene = SCENE.StageOne_BOSS;
                break;
            case SCENE.StageOne_BOSS:
                nextScene = SCENE.StageTwo;
                break;
            case SCENE.StageTwo:
                nextScene = SCENE.StageTwo_BOSS;
                break;
            case SCENE.StageTwo_BOSS:
                nextScene = SCENE.StageThree;
                break;
            case SCENE.StageThree:
                nextScene = SCENE.StageThree_BOSS;
                break;
            case SCENE.StageThree_BOSS:
                nextScene = SCENE.StageThreeDotFive;
                break;
            case SCENE.StageThreeDotFive:
                nextScene = SCENE.StageFour;
                break;
            case SCENE.StageFour:
                nextScene = SCENE.End;
                break;
            default:
                nextScene = SCENE.End;
                break;
        }

        Debug.Log($"After UpdateScene: previousScene = {previousScene}, currentScene = {currentScene}, nextScene = {nextScene}");
    }

    
}


