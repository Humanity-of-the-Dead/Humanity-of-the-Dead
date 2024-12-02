using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField]  public  SceneInformation sceneInformation;
    [SerializeField] private GameObject fadePrefab; // フェード用プレハブ
    private Image fadeInstance; // 実際に使用するフェード用 Image
    [SerializeField] private SceneInformation.SCENE currentScene;  // 今のシーン                  // 今のシーン
    private bool isReloading = false; // リロード中かどうかを判定するフラグ

    public static SceneTransitionManager instance;

    private void Start()
    {

        SetCurrentScene(SceneManager.GetActiveScene().buildIndex);

        StartCoroutine(FadeIn());

        // MultiAudio の初期化を確認
        if (MultiAudio.ins != null && MultiAudio.ins.bgmSource != null)
        {
            PlayBGMForScene();
        }
        else
        {
            Debug.LogWarning("MultiAudio or bgmSource is not ready in Start.");
        }

    }

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Managerオブジェクト全体を保持
            
            SceneManager.sceneLoaded += OnSceneLoaded; // イベント登録
        }
        else
        {
            Destroy(gameObject); // 二重生成防止
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeReferences(); // シーンロード後に再取得
        PlayBGMForScene();

    }
    private void Update()
    {
    }
    public void ReloadCurrentScene()
    {
        if (isReloading) return; // リロード中なら処理をスキップ
        isReloading = true; // リロード中に設定
        StartCoroutine(FadeOut(SceneManager.GetActiveScene().name));
    }
    public void InitializeReferences()
    {

        if (fadeInstance == null)
        {
            if (fadePrefab != null)
            {
                GameObject fadeObject = Instantiate(fadePrefab);
                fadeInstance = fadeObject.GetComponentInChildren<Image>(); // 子オブジェクトから Image を取得

                if (fadeInstance == null)
                {
                    //Debug.LogError("fadePrefab に Image コンポーネントがありません。");
                }
                else
                {
                    //Debug.Log("fadeInstance が正常に設定されました。");
                }

                // Canvas の設定
                Canvas fadeCanvas = fadeObject.GetComponent<Canvas>();
                if (fadeCanvas != null)
                {
                    fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    fadeCanvas.sortingOrder = 100;
                }
                fadeInstance.gameObject.SetActive(false);
                DontDestroyOnLoad(fadeObject);
            }
            else
            {
                //Debug.LogError("フェード用プレハブが設定されていません。");
            }
        }
    }
    public void SetCurrentScene(int sceneIndex) { currentScene = (SceneInformation.SCENE)sceneIndex; }

    private void PlayBGMForScene()
    {
        if (MultiAudio.ins == null || MultiAudio.ins.bgmSource == null)
        {
            Debug.LogError("MultiAudio or its bgmSource is not initialized.");
            return;
        }
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
        else if (sceneName == sceneInformation.GetSceneName(SceneInformation.SCENE.StageFour))
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
        if (isReloading) return; // シーン変更中なら処理をスキップ
        isReloading = true; // シーン変更中に設定
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
        fadeInstance.gameObject.SetActive(true);
        Color fadeColor = fadeInstance.color; // 一時変数を使用
        fadeColor.a = 1; // 最初は完全に不透明
        fadeInstance.color = fadeColor;
        float speed = 1.0f / 0.5f;  // 0.5秒でアルファ値が減少


        // 徐々に透明にする処理
        while (fadeInstance.color.a > 0)
        {
            fadeColor.a -= Time.deltaTime*speed; // アルファ値を減少
            fadeInstance.color = fadeColor; // 更新
            yield return null;
        }

        // 完全に透明になったら非アクティブ化
        fadeInstance.gameObject.SetActive(false);
        isReloading = false; // リロードが完了したのでフラグをリセット

    }

    // <summary>
    // 画面を暗くする
    // </summary>
    // <returns></returns>
    private IEnumerator FadeOut(string stageName)
    {
        fadeInstance.gameObject.SetActive(true);
        Color fadeColor = fadeInstance.color; // 一時変数を使用
        fadeColor.a = 0; // 最初は完全に透明
        fadeInstance.color = fadeColor;
        float speed = 1.0f / 0.5f;  // 0.5秒でアルファ値が減少

        // 徐々に不透明にする処理
        while (fadeInstance.color.a < 1)
        {
            fadeColor.a += Time.deltaTime*speed; // アルファ値を増加
            fadeInstance.color = fadeColor; // 更新
            yield return null;
        }

        sceneInformation.SetPreviousScene((SceneInformation.SCENE)SceneManager.GetActiveScene().buildIndex);

        // シーンの非同期読み込み
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(stageName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        StartCoroutine(FadeIn()); // フェードイン開始
    }
  
    

   
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    
   


  
   

    
   
}

