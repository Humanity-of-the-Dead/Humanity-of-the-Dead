using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMoveAnimation : MonoBehaviour
{
    [SerializeField, Header("����Image")] SpriteRenderer headSR;
    [SerializeField, Header("�r��Image�A��ɉE��")] SpriteRenderer armSR;
    [SerializeField, Header("����Image�A��ɉE��")] SpriteRenderer legSR;

    [Header("�S�g")] public GameObject playerRc;
    [SerializeField, Header("�r�̊p�x�A��ɉE��")] GameObject[] arm;
    [SerializeField, Header("���ڂ̊p�x�A��ɉE��")] GameObject[] leg;
    [SerializeField, Header("���˂̊p�x�A��ɉE��")] GameObject[] foot;

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

    // �����t���O(�E = false)
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
            // ������������Ă��鎞�A�Ă΂��Ȃ�
            if (time < 0)
            {
                // �v���C���[�̌�����������E�ɕς�����Ƃ�
                isWalk = false;
                shaft = 0;

                isActive = false;
                ChangeArmAnime();
                WalkStart();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // ������������Ă��鎞�A�Ă΂��Ȃ�
            if (time < 0)
            {
                // �v���C���[�̌������E���獶�ɕς�����Ƃ�
                isWalk = true;
                shaft = 180;

                isActive = false;
                ChangeArmAnime();
                WalkStart();
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (!isWalk)
            {
                KeepWalk();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (isWalk)
            {
                KeepWalk();
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
            Debug.LogWarning("arm�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft + 180, armRotation[indexNumber]);
        }

        // ���̃A�j���[�V����
        if (leg == null || legBackRotation == null || legForwardRotation == null)
        {
            Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
            return;
        }
        else if (foot == null || footBackRotation == null || footForwardRotation == null)
        {
            Debug.LogWarning("foot�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            // �����n�߂̏ꍇ
            if (!isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, legBackRotation[indexNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, legForwardRotation[indexNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, footBackRotation[indexNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, footForwardRotation[indexNumber]);
            }
            //���������Ă���ꍇ
            if (isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, legForwardRotation[indexNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, legBackRotation[indexNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, footForwardRotation[indexNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, footBackRotation[indexNumber]);
            }
        }
    }

    IEnumerator CallFunctionWithDelay()
    {
        for (int i = 0; i < armRotation.Length; i++)
        {
            PlayerWalk();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % armRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
    }


    /// <summary>
    /// �������Ƃ��p���������A�r�̔z��̒��̒l���t�ɂ���
    /// </summary>
    void ChangeArmAnime()
    {
        //�O�����Z�q(�e�v�f�ɑ΂��ĕϊ�������s��)
        if (isActive)
        {
            armRotation = armRotation.Select(value => value > 0 ? -value : value).ToArray();
        }
        else if (!isActive)
        {
            armRotation = armRotation.Select(value => value < 0 ? -value : value).ToArray();
        }
    }

    /// <summary>
    /// �����n�߂̊֐�
    /// </summary>
    void WalkStart()
    {
        time = timeMax * armRotation.Length;
        StartCoroutine(CallFunctionWithDelay());
    }

    /// <summary>
    /// �������Ƃ��p�������Ƃ�
    /// </summary>
    void KeepWalk()
    {
        // �A�����͂���Ă��邩
        if (time - 0.05 < 0)
        {
            isActive = !isActive;
            ChangeArmAnime();
            WalkStart();
        }
    }

    /// <summary>
    /// ���̃C���[�W
    /// </summary>
    /// <param name="head">�摜�f�[�^</param>
    public void ChangeHead(BodyPartsData head)
    {

        headSR.sprite = head.spHand;
    }

    /// <summary>
    /// �r�̃C���[�W
    /// </summary>
    /// <param name="arm">�摜�f�[�^</param>
    public void ChangeArm(BodyPartsData arm)
    {
        armSR.sprite = arm.spArm;
    }

    /// <summary>
    /// ���̃C���[�W
    /// </summary>
    /// <param name="leg">�摜�f�[�^</param>
    public void ChangeLeg(BodyPartsData leg)
    {
        legSR.sprite = leg.spLeg;
    }
}