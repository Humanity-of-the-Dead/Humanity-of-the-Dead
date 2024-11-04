using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMoveAnimation : MonoBehaviour
{
    [Header("�S�g")]public GameObject playerRc;
    [SerializeField, Header("�r�A��ɉE��")] public GameObject[] arm;     
    [SerializeField, Header("���ځA��ɉE��")] public GameObject[] leg;   
    [SerializeField, Header("���ˁA��ɉE��")] public GameObject[] foot;

    [Header("�S�g�̊p�x")] public float[] playerRotation;
    [Header("�r�̊p�x")] public float[] armRotation;
    [Header("�������̑O���̊p�x")] public float[] legForwardRotation;
    [Header("���̑O���̊p�x")] public float[] footForwardRotation;
    [Header("�������̌���̊p�x")] public float[] legBackRotation;
    [Header("���̌���̊p�x")] public float[] footBackRotation;
    [Header("�����̌p������")] public float timeWalk;

    [Header("1�R�}�̊Ԋu�̎���")] public float timeMax;

    //�z��̔ԍ�
    int indexNumber;

    //�̂̎�
    int shaft;

    // �l�𔽓]�ɂ���t���O
    bool isActive;

    // �����Ă���������E�������Ă��邩
    bool isMirror;

    // �p�����ĕ����t���O
    bool isWalk;

    // �^�C�}�[
    float time = 0;


    private void Start()
    {
        indexNumber = 0;
        shaft = 0;

        isMirror = true;
        isActive = false;
        isWalk = false;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.D))
        {
            // �v���C���[�̌�����������E�ɕς�����Ƃ�
            if (!isMirror)
            {
                shaft = 0;
                MoveMirror();
            }
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            // �v���C���[�̌������E���獶�ɕς�����Ƃ�
            if (isMirror)
            {
                shaft = 180;
                MoveMirror();
            }
        }


        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
           
            // �A�����͂���Ă��邩
            if (time - 0.05 < 0)
            {
                isWalk = true;
                //isActive = true;
            }

            // ������������Ă��鎞�A�Ă΂��Ȃ�
            if (time < 0)
            {
                if (isWalk)
                {
                    // �z��̒��̒l���}�C�i�X�ɂ���
                    KeepWalk();
                    isWalk = false;
                }
                time = timeMax * armRotation.Length;
                StartCoroutine(CallFunctionWithDelay());
            }
        }
    }

    /// <summary>
    /// �����A�j���[�V����
    /// </summary>
    void PlayerWalk()
    {
        // Quaternion.Euler: ��]��( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerRotation[indexNumber]);

        // �r�̃A�j���[�V����
        if (arm == null || armRotation == null)
        {
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, 0, armRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, 180, armRotation[indexNumber]);
        }

        // ���̃A�j���[�V����
        if (leg == null ||foot == null)
        {
            return;
        }
        else
        {
            // �����n�߂̏ꍇ
            if (!isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, 0, legBackRotation[indexNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, 0, legForwardRotation[indexNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, 0,  footBackRotation[indexNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, 0,  footForwardRotation[indexNumber]);
            }
            //���������Ă���ꍇ
            if (isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, 0, legForwardRotation[indexNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, 0, legBackRotation[indexNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, 0, footForwardRotation[indexNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, 0, footBackRotation[indexNumber]);
            }
        }    
    }

    private IEnumerator CallFunctionWithDelay()
    {
        for (int i = 0; i < armRotation.Length; i++)
        {
            PlayerWalk();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % armRotation.Length;

            // �z��̒��̒l�����ɖ߂�
            if(isActive)
            {
                KeepWalk();
                isActive = false;
            }
            yield return new WaitForSeconds(timeMax); 
        }
    }

    
    /// <summary>
    /// �������Ƃ��p��������
    /// �z��̒��̒l���t�ɂ���
    /// </summary>
    void KeepWalk()
    {

        armRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
    }

    /// <summary>
    /// �����������ς�����Ƃ��z��̒��̒l���t�ɂ���
    /// </summary>
    void MoveMirror()
    {
        armRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
        legForwardRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
        legBackRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
        footForwardRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
        footBackRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
    }
}

