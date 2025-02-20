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

    [Tooltip("はじめからボタンのイベントトリガーを入れる")]
    [SerializeField] EventTrigger eventTrigger_Start;
    [Tooltip("クレジットボタンのイベントトリガーを入れる")]
    [SerializeField] EventTrigger eventTrigger_Credit;

 
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
    //[Tooltip("Floatの小数点第１まで入力、0.0～１.0まで")]
    //public float BGMVolume;
    //[Header("初期UIの音量、スライダーの値と同じでお願いします！")]
    //[Tooltip("Floatの小数点第１まで入力、0.0～１.0まで")]
    //public float UIVolume;


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

    [SerializeField, Header("オプション画面のタイトルボタン")]
    private Button TitleButton;
    [SerializeField, Header("クレジット画面の戻るボタン")]
    private Button creditReturn;
    [SerializeField, Header("オプション画面の戻るボタン")]
    private Button optionReturn;

    [SerializeField, Header("チュートリアルスキップするかのウィンドウ")] private GameObject skipPanel;
    private GameObject skipPanelInstance;


    enum PanelView
    {
        None,
        Tutorial,
        Credit,
        Option,

    }
    PanelView panelView = PanelView.None;


    void Start()
    {
        eventTrigger_Start=start.GetComponent<EventTrigger>();  
        eventTrigger_Credit=Credit.GetComponent<EventTrigger>();    
        start.onClick.AddListener(() =>
           InstantiateSkipPanel());


        TitleButton.interactable = false;
        //MultiAudio.ins.bgmSource.volume = BGMVolume;
        //MultiAudio.ins.seSource.volume = UIVolume;

        if (!CreditPanel.activeSelf && !OptionPanel.activeSelf)
        {
            MultiAudio.ins.PlayBGM_ByName("BGM_title");


        }
        option.onClick.AddListener(() =>  OptionView());
        mainPanel.SetActive(true);　　//タイトル画面
        CreditPanel.SetActive(false);//クレジット画面
        OptionPanel.SetActive(false);


    }


    public void InstantiateSkipPanel()
    {
        if (skipPanelInstance != null) { Destroy(skipPanelInstance); }
        skipPanelInstance = Instantiate(skipPanel);
        PlayerControl.lastInputTime = Time.time;        
        ChangeStage1();
        ChangeTutorial();
        start.interactable = false;
        eventTrigger_Start.enabled = false;
        eventTrigger_option.enabled = false;    
        option.interactable = false;    
        option.GetComponent<AudioButtonHandler>().enabled = false;  
        start.GetComponent<AudioButtonHandler>().enabled = false;    
        Credit.interactable = false;    
        Credit.GetComponent<AudioButtonHandler>().enabled = false;
        eventTrigger_Credit.enabled = false;
    }

    private void ChangeStage1()
    {
        Button YesButton = skipPanelInstance.transform.Find("YesButton").GetComponent<Button>();
        Debug.Log(YesButton);

        if (YesButton != null)
        {
            YesButton.onClick.RemoveAllListeners();
            YesButton.onClick.AddListener(() =>
            {
                SceneTransitionManager.instance.NextSceneButton(2);
                Destroy(skipPanelInstance);
            });

        }
    }
    private void ChangeTutorial()
    {
        Button NoButton = skipPanelInstance.transform.Find("NoButton").GetComponent<Button>();
        Debug.Log(NoButton);

        if (NoButton != null)
        {
            NoButton.onClick.RemoveAllListeners();
            NoButton.onClick.AddListener(() => 
            { 
                SceneTransitionManager.instance.NextSceneButton(1);
                Destroy(skipPanelInstance);
            });

        }
    }

    public void MainView()//メイン画面に戻る関数
    {


      

        if (panelView == PanelView.Credit)
        {
            MultiAudio.ins.PlayBGM_ByName("BGM_title");
        }
        mainPanel.SetActive(true);

        OptionPanel.SetActive(false);
        CreditPanel.SetActive(false);

        start.interactable = true;
        eventTrigger_Start.enabled = true;
        start.GetComponent<AudioButtonHandler>().enabled = true;
        eventTrigger_option.enabled = true;
        option.interactable = true;
        option.GetComponent<AudioButtonHandler>().enabled = true;
        Credit.interactable = true;
        Credit.GetComponent<AudioButtonHandler>().enabled = true;
        eventTrigger_Credit.enabled = true;

    }



    public void CreditView() //クレジット画面を表示
    {







        //パネルのオブジェクトのセットアクティブ切り替え
        mainPanel.SetActive(false);
        CreditPanel.SetActive(true);
        OptionPanel.SetActive(false);


     
        MultiAudio.ins.PlayBGM_ByName("BGM_credit");

        MultiAudio.ins.bgmSource.loop = false;
    }

    public void OptionView() //オプション画面を表示
    {







        //パネルのオブジェクトのセットアクティブ切り替え
        mainPanel.SetActive(true);
        CreditPanel.SetActive(false);
        OptionPanel.SetActive(true);


        option.interactable = false;
        option.GetComponent<AudioButtonHandler>().enabled = false;
        eventTrigger_option.enabled = false;

    }

    //}




    void Update()
    {
        //Gキーもしくはマウス右クリック
      

        if (!OptionPanel.activeSelf && !CreditPanel.activeSelf && mainPanel.activeSelf&&(skipPanelInstance==null||!skipPanelInstance.activeSelf))
        {
            panelView = PanelView.None;
        }
        else if(!OptionPanel.activeSelf && !CreditPanel.activeSelf && mainPanel.activeSelf && (skipPanelInstance == null || skipPanelInstance.activeSelf))
        {
            panelView = PanelView.Tutorial;
        }
        else if (!OptionPanel.activeSelf && CreditPanel.activeSelf && !mainPanel.activeSelf&& (skipPanelInstance == null || !skipPanelInstance.activeSelf))
        {
            panelView = PanelView.Credit;
        }
        else if (OptionPanel.activeSelf && !CreditPanel.activeSelf && mainPanel.activeSelf&& (skipPanelInstance == null || !skipPanelInstance.activeSelf))
        {
            panelView = PanelView.Option;
        }
        MouseOverObject mouseOver = FindAnyObjectByType<MouseOverObject>();
        if (Input.GetKeyDown(KeyCode.G))
        {
            switch (panelView)
            {

                case PanelView.None:

                    option.onClick.Invoke();


                    mouseOver.mouseover.SetActive(false);

                    break;
                case PanelView.Credit:
                    //CreditPanel.SetActive(false);
                    //mainPanel.SetActive(true);
                    //OptionPanel.SetActive(false);
                    mouseOver.mouseover.SetActive(false);
                    creditReturn.onClick.Invoke();
                    if (mainPanel.activeSelf)
                    {
                        MultiAudio.ins.PlayBGM_ByName("BGM_title");

                    }

                    break;
                case PanelView.Option:

                    optionReturn.onClick.Invoke();
                    mouseOver.mouseover.SetActive(false);




                    break;


            }
        }

        if (Input.GetKeyDown(KeyCode.X))

        {
            switch (panelView)
            {

                case PanelView.None:

                    start.onClick.Invoke();
                    break;

                case PanelView.Tutorial:
                    Button NoButton = skipPanelInstance.transform.Find("NoButton").GetComponent<Button>();

                    NoButton.onClick.Invoke();
                    break;
            }

        }
        if (Input.GetKeyDown(KeyCode.C))

        {
            switch (panelView)
            {

                case PanelView.None:
                    CreditView();
                    Credit.onClick.Invoke();

                    panelView = PanelView.Credit;
                  
                    break;
            }

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (panelView)
            {
                case PanelView.Tutorial:
                    Button YesButton = skipPanelInstance.transform.Find("YesButton").GetComponent<Button>();
                    YesButton.onClick.Invoke();
                    break;
            }
        }
            //デバッグ用仮
            if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }



    }
}


