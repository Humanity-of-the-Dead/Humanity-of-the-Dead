using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textdisplay : MonoBehaviour
{
    [SerializeField]
    private TextAsset[] textAsset;   //�������̃t�@�C��(.txt)�@�z��

    [SerializeField]
    private Text text;  //��ʏ�̕���

    private int LoadText = 0;   //�����ڂ̃e�L�X�g��ǂݍ���ł���̂�

    private int n = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        text.text = "";// textAsset.text;
        Debug.Log(textAsset[0].text);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            //for (int i = 0; i < textAsset.Length; i++)
            //{
             
            if (textAsset.Length > LoadText )
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
}
