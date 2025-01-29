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
        StageThree_BOSS,
        StageFour,//親友戦闘
        StageFive,//ボス
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
    [SerializeField] private SCENE currentScene;
    [SerializeField] private SCENE nextScene;

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
    public int GetCurrentSceneInt() { return SceneManager.GetActiveScene().buildIndex; }

    public SCENE GetCurrentScene() { return currentScene; }
    public void SetCurrentScene(SCENE scene) { currentScene = scene; }

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

        // 直前のシーンを保存
        if (currentScene != newScene) // 同じシーンで更新しないようにする
        {
            previousScene = currentScene;
        }
        currentScene = newScene; // 新しいシーンを設定
                                 // 次のシーンが存在するか確認し、設定
        int nextIndex = (int)newScene + 1;
        if (nextIndex < sceneNames.Length)
        {
            nextScene = (SCENE)nextIndex;
        }
        else
        {
            nextScene = SCENE.End; // 最後のシーンの後は `End` に設定
        }
    }


}


