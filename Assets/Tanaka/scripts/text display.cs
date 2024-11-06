using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textdisplay : MonoBehaviour
{
    [SerializeField]
    private TextAsset[] textAsset;   //�������̃t�@�C��(.txt)�@�z��

    [SerializeField]
    private Text text;  //��ʏ�̕���

    [SerializeField]
    private float TypingSpeed = 0.5f;  //�����̕\�����x

    private int LoadText = 0;   //�����ڂ̃e�L�X�g��ǂݍ���ł���̂�

    private int n = 0;

    [SerializeField]
    private float[] Position;

    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private GameMgr GameManager;

    [SerializeField]
    bool[] Flag;
    
    // Start is called before the first frame update
    void Start()
    {
        text.text = "";// ������
        Debug.Log(textAsset[0].text);
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.enGameState)
        {
            case GameState.Main:
                if (Player.transform.position.x > Position[1] && Flag[1] == false)
                {
                    this.gameObject.SetActive(true);    //�I�u�W�F�N�g��\��
                    Flag[1] = true;     //Flag[1]��ʂ���
                    GameManager.ChangeState(GameState.ShowText);    //GameState��ShowText�ɕς��

                    UpdateText();
                }
                break;
            case GameState.ShowText:
                if (Input.GetMouseButtonDown(0))
                {
                    this.gameObject.SetActive(false);   //�I�u�W�F�N�g���\��
                    GameManager.ChangeState(GameState.Main);
                }
                break;
        }

    }
    public void UpdateText()
    {
        if (textAsset.Length > LoadText)
        {//�e�L�X�g��LoadText�̕��\��
            text.text = textAsset[LoadText].text;


            Debug.Log(textAsset[LoadText].text);
            Debug.Log(textAsset[LoadText].text.Length); //�e�L�X�g��ɉ��������邩�f�o�b�N

            //Debug.Log(textAsset[LoadText]);
            Debug.Log(textAsset.Length);    //�S�̂̃e�L�X�g��
            Debug.Log(LoadText);            //���ݕ\������Ă���e�L�X�g�ԍ�

            LoadText++;

            //}
        }
    }
}
