using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { 
    Main,
    ShowOption,
    ShowText,
    Clear, //�N���A�\��
    BeforeBoss, // �V�����ǉ��F�{�X�풼�O

    AfterBOss,//�{�X��
    GameOver,
}

public class GameMgr : MonoBehaviour
{
    [SerializeField] Button OptionButton;
    [SerializeField] Button OptionReturnButton;

    static GameState enGameState;

    float timer = 0;
    private void Start()
    {
        enGameState = GameState.Main;
    }

    private void Update()
    {
        if (enGameState == GameState.BeforeBoss)
        {
            ForceEnemiesMoveLeft();  // �G���L���������ړ�������
            ChangeState(GameState.AfterBOss); // �ړ���� AfterBoss �ɐ؂�ւ���
        }
        if (enGameState == GameState.Main && Input.GetKeyDown(KeyCode.G))
        {
            OptionButton.onClick.Invoke();
            //enGameState = GameState.ShowText;
        }
        else if (enGameState == GameState.ShowOption && Input.GetKeyDown(KeyCode.G))
        {
            OptionReturnButton.onClick.Invoke();
            //enGameState = GameState.Main;
        }

        if (enGameState == GameState.GameOver)
        {
            if (timer > 1)
            {
                SceneTransitionManager.instance.ReloadCurrentScene();
                timer = 0;
            }
            timer += Time.deltaTime;
        }
    }

    //�X�e�[�g�`�F���W
    public static void ChangeState(GameState newState)
    {
        enGameState = newState;
    }
    public static GameState GetState()
    {
        return enGameState;
    }
    private void ForceEnemiesMoveLeft()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.transform.Translate(Vector3.left * 5f); // �C�ӂ̑��x�ō��ړ�
            EnemyMoveAnimation  moveAnimation =GameObject.FindAnyObjectByType<EnemyMoveAnimation>();
            moveAnimation.LeftMove();
        }

    }
}

