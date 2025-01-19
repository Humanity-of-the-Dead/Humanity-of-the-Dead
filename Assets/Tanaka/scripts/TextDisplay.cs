using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TextDisplay : MonoBehaviour
{
   
    [System.Serializable]
    struct TextDataSet
    {
        public TextAsset[] textAsset; // Array of text files
    }

    [Header("Text Display Settings")]
    [SerializeField] private TextDataSet[] textDataSet;
    [SerializeField] private Text text;
    [SerializeField] private float typingSpeed = 1.0f;
    [SerializeField] private float textSpeed = 0.1f;
    [SerializeField] private string customNewline = "[BR]";
    [SerializeField] private GameObject textArea;
    [SerializeField] private GameObject gameClear;

    [Header("Player Settings")]
    [SerializeField] private PlayerControl player;
    [SerializeField] private float[] positionTriggers;

    private int currentDataIndex = 0;
    private int currentTextIndex = 0;
    private bool isTextFullyDisplayed = false;
    private Coroutine typingCoroutine;
    private bool[] positionFlags;
    private float clearTimer = 0;

    // Public getter for text display status
    public bool IsTextFullyDisplayed() => isTextFullyDisplayed;

    void Start()
    {
        InitializeTextDisplay();
    }

    void Update()
    {
        HandleGameState();
    }

    private void InitializeTextDisplay()
    {
        text.text = "";
        textArea.SetActive(false);
        positionFlags = new bool[positionTriggers.Length];
        player = GameObject.Find("Player Variant").GetComponent<PlayerControl>();
    }

    private void HandleGameState()
    {
        switch (GameMgr.GetState())
        {
            case GameState.Main:
                CheckPlayerPosition();
                break;

            case GameState.ShowText:
                HandleTextInput();
                break;

            case GameState.Clear:
                HandleGameClear();
                break;

            case GameState.AfterBOss:
                HandleBossDefeat();
                break;
        }
    }

    private void CheckPlayerPosition()
    {
        for (int i = 0; i < positionFlags.Length; i++)
        {
            if (!positionFlags[i] && player.transform.position.x > positionTriggers[i])
            {
                positionFlags[i] = true;
                GameMgr.ChangeState(GameState.ShowText);
                StartTextDisplay();
            }
        }
    }

    private void StartTextDisplay()
    {
        textArea.SetActive(true);
        UpdateText();
    }

    private void HandleTextInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isTextFullyDisplayed)
            {
                DisplayFullText();
            }
            else if (currentTextIndex < textDataSet[currentDataIndex].textAsset.Length - 1)
            {
                LoadNextText();
                UpdateText();
            }
            else
            {
                CloseTextArea();
                currentDataIndex++;
                currentTextIndex = 0;
            }
        }
    }

    private void HandleGameClear()
    {
        clearTimer += Time.deltaTime;

        if (clearTimer > 1)
        {
            int nextSceneIndex = Mathf.Min(SceneTransitionManager.instance.sceneInformation.GetCurrentScene() + 1, 5);

            if (!MultiAudio.ins.bgmSource.isPlaying)
            {
                SceneTransitionManager.instance.NextSceneButton(nextSceneIndex);
                clearTimer = 0;
            }
        }
    }

    private void HandleBossDefeat()
    {
        UpdateText();
        textArea.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isTextFullyDisplayed)
            {
                DisplayFullText();
            }
            else if (currentTextIndex < textDataSet[currentDataIndex].textAsset.Length - 1)
            {
                LoadNextText();
                UpdateText();
            }
            else
            {
                CloseTextArea();
                currentDataIndex++;
                currentTextIndex = 0;
            }
        }

        HandleClearAudio();
    }

    private void HandleClearAudio()
    {
        var bgm = MultiAudio.ins.bgmSource;
        bgm.Stop();
        MultiAudio.ins.PlayBGM_ByName("BGM_clear");
        bgm.loop = false;

        if (bgm.isPlaying)
        {
            gameClear.SetActive(true);
        }

        GameMgr.ChangeState(GameState.Clear);
    }

    private void UpdateText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (textDataSet[currentDataIndex].textAsset.Length > currentTextIndex)
        {
            text.text = "";
            isTextFullyDisplayed = false;
            typingCoroutine = StartCoroutine(TypingCoroutine());
        }
    }

    private IEnumerator TypingCoroutine()
    {
        string content = textDataSet[currentDataIndex].textAsset[currentTextIndex].text;

        if (!string.IsNullOrEmpty(customNewline))
        {
            content = content.Replace(customNewline, "\n");
        }

        for (int i = 0; i <= content.Length; i++)
        {
            text.text = content.Substring(0, i);
            yield return new WaitForSeconds(textSpeed);
        }

        isTextFullyDisplayed = true;
    }

    private void DisplayFullText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        string fullText = textDataSet[currentDataIndex].textAsset[currentTextIndex].text;
        text.text = !string.IsNullOrEmpty(customNewline) ? fullText.Replace(customNewline, "\n") : fullText;
        isTextFullyDisplayed = true;
    }

    private void LoadNextText()
    {
        if (currentTextIndex < textDataSet[currentDataIndex].textAsset.Length - 1)
        {
            currentTextIndex++;
        }
    }

    private void CloseTextArea()
    {
        GameMgr.ChangeState(GameState.Main);
        textArea.SetActive(false);
    }
}
