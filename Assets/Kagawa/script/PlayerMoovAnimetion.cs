using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMoveAnimetion : MonoBehaviour
{
    [Header("�S�g")]public GameObject playerRc;
    [SerializeField, Header("�r�A��ɉE��")] public GameObject[] arm;     
    [SerializeField, Header("���ځA��ɉE��")] public GameObject[] leg;   
    [SerializeField, Header("���ˁA��ɉE��")] public GameObject[] foot;

    [Header("�S�g�̊p�x")] public float[] playerRotation;
    [Header("�r�̊p�x")] public float[] armRotation;
    [Header("�������̑O���̊p�x")] public float[] legForwardRotation;
    [Header("�������̌���̊p�x")] public float[] legBackRotation;
    [Header("���̑O���̊p�x")] public float[] footForwardRotation;
    [Header("���̌����̊p�x")] public float[] footBackRotation;


    int i = 0;
    int j = 0;

    bool isActive;
    float time = 0;

    [Header("����")] public float timeMax;

    private void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            interval();
            time = timeMax;
        }

    }

    void interval()
    {

        playerRc.transform.rotation = Quaternion.Euler(0, 0, playerRotation[j]);
        if (arm == null || armRotation == null)
        {
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, 0, armRotation[j]);
            arm[1].transform.rotation = Quaternion.Euler(0, 180, armRotation[j]);
        }


        if (leg == null ||foot == null)
        {
            Debug.Log("asa");
            return;
        }
        else
        {
            if (!isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, 0, legBackRotation[j]);
                leg[1].transform.rotation = Quaternion.Euler(0, 0, legForwardRotation[j]);
                foot[0].transform.rotation = Quaternion.Euler(0, 0,  footBackRotation[j]);
                foot[1].transform.rotation = Quaternion.Euler(0, 0,  footForwardRotation[j]);
            }
            if (isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, 0, legForwardRotation[j]);
                leg[1].transform.rotation = Quaternion.Euler(0, 0, legBackRotation[j]);
                foot[0].transform.rotation = Quaternion.Euler(0, 0, footForwardRotation[j]);
                foot[1].transform.rotation = Quaternion.Euler(0, 0, footBackRotation[j]);
            }
        }



        //// j�̒l�𑝂₷
        j = (j + 1) % armRotation.Length;

        // �z��ԍ���0�ɖ߂����Ƃ��z��̒l���}�C�i�X�ɕς���
        if (j == 0 && !isActive)
        {
            armRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
            isActive = true;
            return;
        }
        else if (j == 0 && isActive)
        {
            armRotation = armRotation.Select(value => value < 0 ? -value : value).ToArray();
            isActive = false;
        }
    }
}

