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

    //[SerializeField] AudioSource bgmAudioSource;
    //private SoundTable soundTable;  // BGM テーブル

    private void Start()
    {
        //bgmAudioSource = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        StartCoroutine(FadeIn());
        SetCurrentScene(SceneManager.GetActiveScene().buildIndex);



    }
    private void Update()
    {
        
    }
    public void SetCurrentScene(int sceneIndex) { currentScene = (SceneInformation.SCENE)sceneIndex; }

    //private void PlayBGMForScene()
    //{
    //    AudioClip clip = null;
    //    SceneInformation.SCENE currentScene;

    //    // 現在のシーン名から T_SceneInformation.SCENE の enum 値を取得
    //    string sceneName = SceneManager.GetActiveScene().name;

    //    // シーン名に応じた BGM を取得
    //    if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.Title))
    //    {

    //        currentScene = SceneInformation.SCENE.Title;
    //    }
    //    else if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.StageOne))
    //    {

    //        currentScene = SceneInformation.SCENE.StageOne;
    //    }
    //    else if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.StageTwo))
    //    {

    //        currentScene = T_SceneInformation.SCENE.Battle_Enemy2;
    //    }
    //    else if (sceneName == sceneInformation.GetSceneName(T_SceneInformation.SCENE.Battle_Enemy3))
    //    {

    //        currentScene = T_SceneInformation.SCENE.Battle_Enemy3;
    //    }
    //    else if (sceneName == sceneInformation.GetSceneName(T_SceneInformation.SCENE.Battle_Boss1))
    //    {

    //        currentScene = T_SceneInformation.SCENE.Battle_Boss1;
    //    }
    //    else if (sceneName == sceneInformation.GetSceneName(T_SceneInformation.SCENE.Battle_Boss2))
    //    {
    //        currentScene = T_SceneInformation.SCENE.Battle_Boss2;
    //        Debug.Log(currentScene);

    //    }
    //    else if (sceneName == sceneInformation.GetSceneName(T_SceneInformation.SCENE.Battle_Boss3))
    //    {

    //        currentScene = T_SceneInformation.SCENE.Battle_Boss3;
    //    }
    //    else if (sceneName == sceneInformation.GetSceneName(T_SceneInformation.SCENE.StageOne))
    //        currentScene = T_SceneInformation.SCENE.StageOne;
    //    else if (sceneName == sceneInformation.GetSceneName(T_SceneInformation.SCENE.StageTwo))
    //        currentScene = T_SceneInformation.SCENE.StageTwo;
    //    else if (sceneName == sceneInformation.GetSceneName(T_SceneInformation.SCENE.StageThree))
    //        currentScene = T_SceneInformation.SCENE.StageThree;
    //    else
    //        return;

    //    // シーン名に応じた BGM を取得 (ここで変更)
    //    switch (currentScene)
    //    {
    //        case T_SceneInformation.SCENE.Title:
    //            clip = soundTable.GetAudioClip("BGM_Outgame_All");
    //            Debug.Log(clip);
    //            break;

    //        case T_SceneInformation.SCENE.Battle_Enemy1:
    //            clip = soundTable.GetAudioClip("BGM_Battle_Enemy");
    //            break;
    //        case T_SceneInformation.SCENE.Battle_Enemy2:
    //            clip = soundTable.GetAudioClip("BGM_Battle_Enemy");
    //            break;
    //        case T_SceneInformation.SCENE.Battle_Enemy3:
    //            clip = soundTable.GetAudioClip("BGM_Battle_Enemy");
    //            break;
    //        case T_SceneInformation.SCENE.Battle_Boss1:
    //            clip = soundTable.GetAudioClip("BGM_Battle_Boss1");
    //            break;
    //        case T_SceneInformation.SCENE.Battle_Boss2:

    //            clip = soundTable.GetAudioClip("BGM_Battle_Boss2");
    //            Debug.Log(clip);

    //            break;
    //        case T_SceneInformation.SCENE.Battle_Boss3:
    //            clip = soundTable.GetAudioClip("BGM_Battle_Boss3_01");
    //            break;

    //        case T_SceneInformation.SCENE.StageOne:
    //            clip = soundTable.GetAudioClip("BGM_Novel_Normal");
    //            break;
    //        case T_SceneInformation.SCENE.StageTwo:
    //            clip = soundTable.GetAudioClip("BGM_Novel_Normal");
    //            break;
    //        case T_SceneInformation.SCENE.StageThree:
    //            clip = soundTable.GetAudioClip("BGM_Novel_Normal");
    //            break;
    //        default:
    //            clip = soundTable.GetAudioClip("BGM_Novel_Normal");
    //            break;

    //    }

    //    // BGM を設定して再生
    //    if (clip != null && bgmAudioSource.clip != clip)
    //    {
    //        bgmAudioSource.clip = clip;
    //        Debug.Log(clip);
    //        bgmAudioSource.Play();
    //        Debug.Log("Playing BGM: " + clip.name);
    //    }
    //    else
    //    {
    //        Debug.Log(clip);

    //        Debug.LogWarning("Clip not found for scene: " + sceneName);
    //    }
    //}



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
