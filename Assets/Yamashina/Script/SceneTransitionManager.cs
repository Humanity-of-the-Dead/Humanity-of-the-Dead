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

        PlayBGMForScene();

    }

    public void SetCurrentScene(int sceneIndex) { currentScene = (SceneInformation.SCENE)sceneIndex; }

    private void PlayBGMForScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string bgmName = "";

        if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.Title))
        {
            bgmName = "BGM_title"; // タイトル画面のBGM名
        }
        else if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.StageOne))
        {
            bgmName = "BGM_stage_01"; // ステージ1のBGM名
        }
        else if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.StageTwo))
        {
            bgmName = "BGM_stage_02"; // ステージ2のBGM名
        }
        else if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.StageThree))
        {
            bgmName = "BGM_stage_03"; // ステージ2のBGM名

        }
        else if(sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.StageFour))
        {
            bgmName = "BGM_stage_04"; // ステージ2のBGM名

        }


        if (!string.IsNullOrEmpty(bgmName))
        {
            MultiAudio.ins.PlayBGM_ByName(bgmName); // BGMを再生
            Debug.Log(bgmName);
        }
        else
        {
            Debug.LogWarning($"No BGM assigned for the scene '{sceneName}'.");
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
