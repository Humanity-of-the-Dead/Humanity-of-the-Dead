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

    //[Tooltip("オプション画面のオブジェクトを入れる")]
    //public GameObject OptionPanel;

    [Header("ボタンのオブジェクトのセットアクティブ切り替え用")]
    [Tooltip("クレジットボタンのオブジェクトを入れる")]

    [SerializeField] GameObject CreditButton; //クレジット

    [Tooltip("初めからボタンのオブジェクトを入れる")]
    [SerializeField] GameObject StartButton; //初めから

    //[Tooltip("オプションボタンのオブジェクトを入れる")]
    //[SerializeField] GameObject optionButton;

    [Header("ボタンのイベントトリガーのアクティブ切り替え用")]
    [Tooltip("クレジットボタンのイベントトリガーを入れる")]
    [SerializeField] EventTrigger eventTrigger_Credit;
    //[Tooltip("オプションボタンのイベントトリガーを入れる")]
    //[SerializeField] EventTrigger eventTrigger_option;
    [Tooltip("はじめからボタンのイベントトリガーを入れる")]

    [SerializeField] EventTrigger eventTrigger_Start;
    [Header("ボタンのアクティブ切り替え")]

    [Tooltip("クレジットボタンそのものを入れる")]

    public Button Credit;
    [Tooltip("初めからボタンそのものを入れる")]

    public Button start;
    //[Tooltip("オプションボタンそのものを入れる")]

    //public Button option;
    [Header("各オブジェクトのアニメーション速度設定")]
    [Tooltip("各オブジェクトの移動速度")]

    [SerializeField] float speed = 1;

    //各パネル・ボタンの開始位置

    [Tooltip("アニメーションの遅延（floatで入力、値が大きいほど遅延が長い）")]

    public float Coroutine;
    [Header("メイン画面のアニメーション開始位置")]
    [Tooltip("メイン画面が画面外に配置される位置")]
    [SerializeField] Vector3 startPanelPosition;

    [Header("メイン画面の終了位置")]

    [SerializeField] Vector3 startPanelEndPosition;


    [Header("スタートボタンのアニメーション開始位置")]
    [SerializeField] Vector3 startButtonPosition;

    [Header("スタートボタンのアニメーション終了位置")]
    [SerializeField] Vector3 startButtonEndPosition;


    //[Header("オプションボタンのアニメーション開始位置"")]
    // [Tooltip("オプション画面が画面外に配置される位置")]
    //[SerializeField] Vector3 OptionPanelPosition;

    //[Header("オプション画面の終了位置")]
    //[SerializeField] Vector3 OptionPanelEndPosition;

    //[Header("オプションボタンのアニメーション開始位置")]

    //[SerializeField] Vector3 OptionButtonPosition;

    //[Header("オプションボタンのアニメーション終了位置")]
    //[SerializeField] Vector3 OptionButtonEndPosition;


    [Header("クレジット画面のアニメーション開始位置")]
    [Tooltip("クレジット画面が画面外に配置される位置")]

    [SerializeField] Vector3 creditPanelStartPosition;

    [Header("クレジット画面の終了位置")]
    [SerializeField] Vector3 creditPanelEndPosition;

    [Header("クレジットボタンのアニメーション開始位置")]
    [SerializeField] Vector3 creditButtonStartPosition;

    [Header("クレジットボタンのアニメーション終了位置")]
    [SerializeField] Vector3 creditButtonEndPosition;

    [Header("時間経過でボタン表示するためのFloat")]
    [Tooltip("何秒後にボタン表示するか")]
    public float ButtonAnimation;


    void Start()
    {
        //パネルのオブジェクトのセットアクティブ切り替え
        mainPanel.SetActive(true);　　//タイトル画面
        CreditPanel.SetActive(false);//クレジット画面

        //ボタンのオブジェクトのセットアクティブ切り替え
        StartButton.SetActive(true);//初めからボタン
        CreditButton.SetActive(true);//クレジットボタン

        //ボタンのイベントトリガーのアクティブ切り替え
        eventTrigger_Start.enabled = true;//初めからボタンのイベントトリガー
        eventTrigger_Credit.enabled = true;//クレジットボタンのイベントトリガー

        //ボタンのアクティブ切り替え（インタラクティブ切り替え）
        start.interactable = true;//初めからボタン
        Credit.interactable = true;//クレジットボタン
    }

    public void MainView()//メイン画面に戻る関数
    {
        //オプション画面スライドアウト
        //if (OptionPanel.activeSelf)
        //{

        //    StartSlideOut();

        //}
        //OptionPanel.SetActive(false);

        //クレジット画面スライドアウト

        if (CreditPanel.activeSelf)
        {
            StartSlideOut();
        }

        mainPanel.SetActive(true);

        //時間経過でボタン表示する、ButtonAnimationで秒数指定
        Invoke(nameof(True_SetActive_Button), ButtonAnimation);

        //ボタンのアクティブ切り替え（インタラクティブ切り替え）
        start.interactable = true;//初めからボタン
        Credit.interactable = true;//クレジットボタン
                                   //option.interactable = true;

        //ボタンのイベントトリガーのアクティブ切り替え
        eventTrigger_Start.enabled = true;//初めからボタンのイベントトリガー
        eventTrigger_Credit.enabled = true;//クレジットボタンのイベントトリガー
                                           //eventTrigger_option.enabled = true;

        //ボタンのオブジェクトのセットアクティブ切り替え
        StartButton.SetActive(true);//初めからボタン
        CreditButton.SetActive(true);//クレジットボタン
    }


    public void True_SetActive_Button()//時間経過でボタン表示するための関数
    {
        //ボタンのオブジェクトのセットアクティブ切り替え
        //optionButton.SetActive(true);
        CreditButton.SetActive(true);

    }
    public void CreditView() //クレジット画面を表示
    {
        //パネルのオブジェクトのセットアクティブ切り替え
        mainPanel.SetActive(true);
        CreditPanel.SetActive(true);
        //OptionPanel.SetActive(false);

        //ボタンのオブジェクトのセットアクティブ切り替え
        StartButton.SetActive(false);
        CreditButton.SetActive(true);
        //optionButton.SetActive(false);

        //ボタンのイベントトリガーのアクティブ切り替え
        eventTrigger_Start.enabled = false;
        eventTrigger_Credit.enabled = false;
        //eventTrigger_option.enabled = false;

        //ボタンのアクティブ切り替え（インタラクティブ切り替え）
        start.interactable = false;
        Credit.interactable = false;
        //option.interactable = false;

        //クレジット画面スライドイン開始
        if (CreditPanel.activeSelf)
        {
            CreditPanel.transform.localPosition = creditPanelStartPosition;
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

        //var Option = 0f;
        var Credit = 0f;//クレジットのパネルのトランスフォーム値の変化


        //オプション画面のスライドインのアニメーション
        //while (Option <= 1.0f && OptionPanel.activeSelf)
        //{
        //    //OptionPanel.transform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.0f, 1.0f, 1.0f), size2);
        //    OptionPanel.transform.localPosition = Vector3.Lerp(new Vector3(OptionPanel_X, OptionPanel_Y, OptionPanel_Z), new Vector3(OptionPanel_End_X, OptionPanel_End_Y, OptionPanel_End_Z), size2);
        //    Option += speed * Time.deltaTime;

        //    yield return new WaitForSeconds(corrutin /** Time.deltaTime*/);
        //}

        //クレジット画面のスライドインのアニメーション

        while (Credit <= 1.0f && CreditPanel.activeSelf)
        {
            CreditPanel.transform.localPosition = Vector3.Lerp(creditPanelStartPosition, creditButtonEndPosition, Credit);

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
        var Credit = 0f;//クレジットのパネルのトランスフォーム値の変化
        //var Option = 0f;

        while (Credit <= 1.0f && CreditPanel.activeSelf)
        {
            CreditPanel.transform.localPosition = Vector3.Lerp(creditPanelStartPosition, creditPanelStartPosition, Credit);
            Credit += speed * Time.deltaTime;
            yield return new WaitForSeconds(Coroutine);
        }
        Debug.Log("通った");

        //�p�l���k���i�I�v�V�����p�l���j
        //while (size2 <= 1.0f && OptionPanel.activeSelf)
        //{
        //    //OptionPanel.transform.localScale = Vector3.Lerp(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.5f, 0.5f, 0.5f), size2);
        //    OptionPanel.transform.localPosition = Vector3.Lerp(new Vector3(OptionPanel_End_X, OptionPanel_End_Y, OptionPanel_End_Z), new Vector3(OptionPanel_X, OptionPanel_Y, OptionPanel_Z), size2);

        //    size2 += speed * Time.deltaTime;



        //    yield return new WaitForSeconds(corrutin);
        //}

        Debug.Log("通った");
        CreditPanel.SetActive(false);
        //OptionPanel.SetActive(false);

        Debug.Log("通った");


    }

    void Update()
    {
        //escape�L�[�Ή�
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            //�p�l���֌W
            //OptionPanel.SetActive(false);
            mainPanel.SetActive(true);
            CreditPanel.SetActive(false);

            //�Q�[���I�u�W�F�N�g�̃{�^��(Setactive)
            StartButton.SetActive(true);
            //optionButton.SetActive(true);
            CreditButton.SetActive(true);

            //�{�^���@�\�֘A�iInteractive)
            start.interactable = true;
            Credit.interactable = true;
            //option.interactable = true;

            //�{�^���̃C�x���g�g���K�[�֘A
            eventTrigger_Start.enabled = true;
            //eventTrigger_option.enabled = true;
            eventTrigger_Credit.enabled = true;
        }
    }
}
