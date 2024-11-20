using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private SceneInformation sceneInformation;
    [SerializeField] private Image fade;                            // フェード
    [SerializeField] private SceneInformation.SCENE currentScene;  // 今のシーン                  // 今のシーン
    [SerializeField] AudioSource bgmAudioSource;
    //private SoundTable soundTable;  // BGM テーブル

    private void Start()
    {
        bgmAudioSource = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();

        SetCurrentScene(SceneManager.GetActiveScene().buildIndex);

        StartCoroutine(FadeIn());

        //PlayBGMForScene();

    }

    public void SetCurrentScene(int sceneIndex) { currentScene = (SceneInformation.SCENE)sceneIndex; }

    private void PlayBGMForScene()
    {
        // 現在のシーンに応じたBGMクリップを設定する
        int bgmIndex = -1;
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(bgmIndex);
        // 現在のシーン名をデバッグ出力
        Debug.Log("Current Scene Name: '" + sceneName + "'");

        // sceneInformation から取得するシーン名をデバッグ出力（前後の空白を確認）
        string titleSceneName = sceneInformation.GetSceneName(SceneInformation.SCENE.Title);
        string stageOneSceneName = sceneInformation.GetSceneName(SceneInformation.SCENE.StageOne);
        string stageTwoSceneName = sceneInformation.GetSceneName(SceneInformation.SCENE.StageTwo);

        Debug.Log("Title Scene Name: '" + titleSceneName + "'");
        Debug.Log("Stage One Scene Name: '" + stageOneSceneName + "'");
        Debug.Log("Stage Two Scene Name: '" + stageTwoSceneName + "'");

        Debug.Log(bgmIndex);
        //string bgmName = "";

        //if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.Title))
        //{
        //    bgmName = "TitleBGM"; // タイトル画面のBGM名
        //}
        //else if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.StageOne))
        //{
        //    bgmName = "StageOneBGM"; // ステージ1のBGM名
        //}
        //else if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.StageTwo))
        //{
        //    bgmName = "StageTwoBGM"; // ステージ2のBGM名
        //}
        // シーンごとのBGM名をswitch-caseで設定
        //switch (currentScene)
        //{
        //    case SceneInformation.SCENE.Title:
        //        bgmName = "TitleBGM"; // タイトル画面のBGM
        //        break;

        //    case SceneInformation.SCENE.StageOne:
        //        bgmName = "StageOneBGM"; // ステージ1のBGM
        //        break;

        //    case SceneInformation.SCENE.StageTwo:
        //        bgmName = "StageTwoBGM"; // ステージ2のBGM
        //        break;

        //    default:
        //        Debug.LogWarning($"No BGM assigned for the scene '{currentScene}'.");
        //        return; // 未設定の場合は終了
        //}
        if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.Title))
        {
            bgmIndex = 0;
        }
        else if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.StageOne))
        {
            bgmIndex = 1;
        }
        else if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.StageTwo))
        {
            bgmIndex = 2;
        }

        if (bgmIndex >= 0)
        {

            Debug.Log("Selected BGM Index: " + bgmIndex);  // デバッグ用
            //MultiAudio.ins.PlaySEByName(bgmName);
            MultiAudio_Matsuoka.ins.ChooseSongsBGM(bgmIndex);    
        }
        else
        {
            Debug.LogWarning("No matching scene found for BGM selection.");
        }

        if (sceneName.Trim() == titleSceneName.Trim())
        {
            bgmIndex = 0;
        }
    }



    ///// <summary>
    ///// シーンを遷移させる
    ///// </summary>
    ///// <param name="scene"></param>

    public void SceneChange(SceneInformation.SCENE scene)
    {
        StartCoroutine(FadeOut(sceneInformation.GetSceneObject(scene)));
    }

    //ボタンでシーン遷移する場合
    public void NextSceneButton(int index)
    {
        SceneChange((SceneInformation.SCENE)index);
    }


    // <summary>
    // 画面を明るくする
    // <summary>    / <returns
    private IEnumerator FadeIn()
    {
        fade.gameObject.SetActive(true);
        fade.color = Color.black;
        while (fade.color.a > 0)
        {
            fade.color += new Color(0, 0, 0, -Time.deltaTime);
            yield return null;
        }
        fade.gameObject.SetActive(false);
    }

    // <summary>
    // 画面を暗くする
    // </summary>
    // <returns></returns>
    private IEnumerator FadeOut(string stageName)
    {
        fade.gameObject.SetActive(true);
        while (fade.color.a < 1)
        {
            fade.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        sceneInformation.SetPreviousScene((SceneInformation.SCENE)SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(stageName);
    }
}
