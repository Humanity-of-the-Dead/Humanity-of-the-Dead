
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Main,
    ShowOption,
    ShowText,
    Tutorial,
    
    

    Clear, //�N���A�\��
    BeforeBoss, // �V�����ǉ��F�{�X�풼�O
    Hint,
    AfterBoss,//�{�X��
    GameOver,
}

public class GameMgr : MonoBehaviour
{
    [SerializeField] Button OptionButton;
    [SerializeField] Button OptionReturnButton;
    [SerializeField] Button TitleButton;
    static GameState enGameState;
    static GameState previousGameState; // �O��̃Q�[���X�e�[�g��ۑ�
    float timer = 0;
    private void Start()
    {
        enGameState = GameState.Main;
        previousGameState = enGameState; // ������

    }

    private void Update()
    {


        switch (GetState())
        {
            case GameState.BeforeBoss:

                //ForceEnemiesMoveLeft();  // �G���L���������ړ�������

                break;
            case GameState.Main:
                if (Input.GetKeyDown(KeyCode.G))
                {
                    OptionButton.onClick.Invoke();

                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    //hintButton.onClick.Invoke();
                    TextDisplay textDisplay = FindAnyObjectByType<TextDisplay>();
                    textDisplay.ShowHintText();
                }
                break;
            case GameState.ShowOption:

                //���Ԓ�~
                Time.timeScale = 0.0f;

                TitleButton.onClick.AddListener(() =>
           SceneTransitionManager.instance.NextSceneButton(0));
                TitleButton.onClick.AddListener(() => ResetTime());
                ////TitleButton.onClick.AddListener(() => PlayerParameter.Instance.ResetPlayerData()
                //);
                if (Input.GetKeyDown(KeyCode.G))
                {
                    OptionReturnButton.onClick.Invoke();

                }

                break;
            case GameState.GameOver:

                if (timer > 1)
                {
                    SceneTransitionManager.instance.ReloadCurrentScene();
                    timer = 0;
                }
                timer += Time.deltaTime;

                break;

            case GameState.Hint:

                break;
        }
    }

    //�X�e�[�g�`�F���W
    public static void ChangeState(GameState newState)
    {
        previousGameState = enGameState; // ���݂̃X�e�[�g��O��̃X�e�[�g�Ƃ��ĕۑ�

        enGameState = newState;
        Debug.Log(newState);
    }
    // �X�e�[�g���ς���������m�F����֐�
    public static bool HasStateChanged()
    {
        return enGameState != previousGameState;
    }

    public static GameState GetState()
    {
        return enGameState;
    }
    public void ResetTime()
    {
        Time.timeScale = 1.0f;

    }

    //private void OnGUI()
    //{
    //    GUI.skin.label.fontSize = 30;  // �Ⴆ��30�ɐݒ�

    //    GUI.Label(new Rect(10.0f, 10.0f, Screen.width, Screen.height), enGameState.ToString());
    //}
}

