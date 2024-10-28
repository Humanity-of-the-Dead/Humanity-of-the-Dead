using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BodyChangeManager : MonoBehaviour
{   
    [SerializeField, Header("��")] SpriteRenderer headSR;    //��
    [SerializeField, Header("�r�A��ɉE��")] SpriteRenderer armSR;     //�r
    [SerializeField, Header("���A��ɉE��")] SpriteRenderer legSR;     //��

    [SerializeField] List<Sprite> head;        //��
    [SerializeField] List<Sprite> arm;         //�r
    [SerializeField] List<Sprite> leg;         //��

    // �F�̃��X�g�i�C�ӂ̐��̐F��ݒ�\�j
    public List<Color> colors = new List<Color>();

    // ���݂̐F�̃C���f�b�N�X
    private int currentColorIndex = 0;

    int i = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            ChangeHead(i);
            i++;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeArm(i);
            i++;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeLeg(i);
            i++;
        }
    }

    void ChangeHead(int headNumber)
    {
        if (headNumber > head.Count)
        {
            Debug.LogWarning("����Ȓl�͂Ȃ���");
            return;
        }

        headSR.sprite = head[headNumber];
    }


    void ChangeArm(int armNumber)
    {
        if (armNumber > arm.Count)
        {
            Debug.LogWarning("����Ȓl�͂Ȃ���");
            return;
        }

        armSR.sprite = arm[armNumber];
    }


    void ChangeLeg(int legNumber)
    {
        if (legNumber > leg.Count)
        {
            Debug.LogWarning("����Ȓl�͂Ȃ���");
            return;
        }

        legSR.sprite = leg[legNumber];
    }

}
