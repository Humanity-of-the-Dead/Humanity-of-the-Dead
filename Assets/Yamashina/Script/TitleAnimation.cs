using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour
{
    [Header("パネルのオブジェクトのセットアクティブ切り替え用")]
    [Tooltip("タイトル画面のオブジェクトを入れる")]

    public GameObject mainPanel;

    [Tooltip("クレジット画面のオブジェクトを入れる")]
    public GameObject CreditPanel;

    [Tooltip("オプション画面のオブジェクトを入れる")]
    public GameObject OptionPanel;

   
    [Header("ボタンのイベントトリガーのアクティブ切り替え用")]
  
    [Tooltip("オプションボタンのイベントトリガーを入れる")]
    [SerializeField] EventTrigger eventTrigger_option;
   
    [Header("ボタンのアクティブ切り替え")]

    [Tooltip("クレジットボタンそのものを入れる")]

    public Button Credit;
    [Tooltip("初めからボタンそのものを入れる")]

    public Button start;
    [Tooltip("オプションボタンそのものを入れる")]

    public Button option;
    [Header("各オブジェクトのアニメーション速度設定")]
    [Tooltip("各オブジェクトの移動速度")]

    [SerializeField] float speed = 1;

    //各パネル・ボタンの開始位置

    [Tooltip("アニメーションの遅延（floatで入力、値が大きいほど遅延が長い）")]

    public float Coroutine;
    [Header("初期BGMの音量、スライダーの値と同じでお願いします！")]
    [Tooltip("Floatの小数点第１まで入力、0.0～１.0まで")]
    public float BGMVolume;
    [Header("初期UIの音量、スライダーの値と同じでお願いします！")]
    [Tooltip("Floatの小数点第１まで入力、0.0～１.0まで")]
    public float UIVolume;


    [Header("クレジット画面のアニメーション開始位置")]
    [Tooltip("クレジット画面が画面外に配置される位置")]

    [SerializeField] Vector3 creditPanelStartPosition;

    [Header("クレジット画面の終了位置")]
    [SerializeField] Vector3 creditPanelEndPosition;

    [Header("オプション画面のアニメーション開始位置")]
    [Tooltip("オプション画面が画面外に配置される位置")]
    [SerializeField] Vector3 OptionPanelStartPosition;

    [Header("オプション画面の終了位置")]
    [SerializeField] Vector3 OptionPaneEndPosition;

    

    enum PanalView
    {
        None,
        Credit,
        Option,

    }
    PanalView panalView = PanalView.None;


    void Start()
    {

        start.onClick.AddListener(() =>
            SceneTransitionManager.instance.NextSceneButton(1));


        MultiAudio.ins.bgmSource.volume = BGMVolume;
        MultiAudio.ins.seSource.volume = UIVolume;

        if (!CreditPanel.activeSelf && !OptionPanel.activeSelf)
        {
            MultiAudio.ins.PlayBGM_ByName("BGM_title");


        }
        mainPanel.SetActive(true);　　//タイトル画面
        CreditPanel.SetActive(false);//クレジット画面
        OptionPanel.SetActive(false);
       
       
    }
    public void MainView()//メイン画面に戻る関数
    {


        //クレジット画面スライドアウト

        if (CreditPanel.activeSelf)
        {
            StartSlideOut();

        }
        if (OptionPanel.activeSelf) { StartSlideOut(); }


        if (panalView == PanalView.Credit)
        {
            MultiAudio.ins.PlayBGM_ByName("BGM_title");
        }
        mainPanel.SetActive(true);
      
        OptionPanel.SetActive(false) ;
        CreditPanel.SetActive(false);

       

        //ボタンのオブジェクトのセットアクティブ切り替え

        //CreditButton.SetActive(true);//クレジットボタン
        //optionButton.SetActive(true);   
    }


   
    public void CreditView() //クレジット画面を表示
    {







        //パネルのオブジェクトのセットアクティブ切り替え
        mainPanel.SetActive(false);
        CreditPanel.SetActive(true);
        OptionPanel.SetActive(false);

        
        //クレジット画面スライドイン開始
        if (CreditPanel.activeSelf)
        {
            CreditPanel.transform.localPosition = creditPanelStartPosition;
            StartSlideIn();
            MultiAudio.ins.PlayBGM_ByName("BGM_credit");

        }
        MultiAudio.ins.bgmSource.loop = false;
    }

    public void OptionView() //オプション画面を表示
    {







        //パネルのオブジェクトのセットアクティブ切り替え
        mainPanel.SetActive(true);
        CreditPanel.SetActive(false);
        OptionPanel.SetActive(true);

      
      
        //クレジット画面スライドイン開始
        if (OptionPanel.activeSelf)
        {
            OptionPanel.transform.localPosition = OptionPanelStartPosition;
            StartSlideIn();
        }
    }
    //スライドインするための関数呼び出し開始
    public void StartSlideIn()
    {
        StartCoroutine(AnimateEachPanelIn());
    }

    //各オブジェクトのスライドイン
    public IEnumerator AnimateEachPanelIn()
    {

        float Credit = 0f;//クレジットのパネルのトランスフォーム値の変化


        //クレジット画面のスライドインのアニメーション

        while (Credit <= 1.0f && CreditPanel.activeSelf)
        {
            CreditPanel.transform.localPosition = Vector3.Lerp(creditPanelStartPosition, creditPanelEndPosition, Credit);

            Credit += speed * Time.deltaTime;

            yield return new WaitForSeconds(Coroutine);
        }
        while (Credit <= 1.0f && OptionPanel.activeSelf)
        {
            OptionPanel.transform.localPosition = Vector3.Lerp(OptionPanelStartPosition, OptionPaneEndPosition, Credit);

            Credit += speed * Time.deltaTime;

            yield return new WaitForSeconds(Coroutine);
        }

    }

    public void StartSlideOut() //画面内から画面外へスライドアウトするための関数呼び出し開始
    {
        StartCoroutine(AnimateEachPanelOut());
    }


    //画面内から画面外へスライドアウトするための関数 
    public IEnumerator AnimateEachPanelOut()
    {
        float Credit = 0f;//クレジットのパネルのトランスフォーム値の変化
                          //var Option = 0f;

        //クレジット画面のスライドアウトのアニメーション
        while (Credit <= 1.0f && CreditPanel.activeSelf)
        {
            CreditPanel.transform.localPosition = Vector3.Lerp(creditPanelEndPosition, creditPanelStartPosition, Credit);
            Credit += speed * Time.deltaTime;
            yield return new WaitForSeconds(Coroutine);
        }
        //クレジット画面のスライドアウトのアニメーション
        while (Credit <= 1.0f && OptionPanel.activeSelf)
        {
            OptionPanel.transform.localPosition = Vector3.Lerp(OptionPaneEndPosition, OptionPanelStartPosition, Credit);
            Credit += speed * Time.deltaTime;
            yield return new WaitForSeconds(Coroutine);
        }
        Debug.Log("通った");

    }

    void Update()
    {
        //Gキーもしくはマウス右クリック


        if (!OptionPanel.activeSelf && !CreditPanel.activeSelf && mainPanel.activeSelf)
        {
            panalView = PanalView.None;
        }
        else if (!OptionPanel.activeSelf && CreditPanel.activeSelf && !mainPanel.activeSelf)
        {
            panalView = PanalView.Credit;
        }
        else if (OptionPanel.activeSelf && !CreditPanel.activeSelf && mainPanel.activeSelf)
        {
            panalView = PanalView.Option;
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.G))
        {
            switch (panalView)
            {

                case PanalView.None:

                    OptionPanel.SetActive(true);
                    OptionPanel.transform.localPosition = OptionPaneEndPosition;
                    CreditPanel.SetActive(false);
                    mainPanel.SetActive(true);

                   
                    break;
                case PanalView.Credit:
                    CreditPanel.SetActive(false);
                    mainPanel.SetActive(true);
                    OptionPanel.SetActive(false);
                    if (mainPanel.activeSelf)
                    {
                        MultiAudio.ins.PlayBGM_ByName("BGM_title");

                    }

                    break;
                case PanalView.Option:
                    CreditPanel.SetActive(false);
                    mainPanel.SetActive(true);
                    OptionPanel.SetActive(false);
                   



                    break;


            }
        }



       
    }
}


