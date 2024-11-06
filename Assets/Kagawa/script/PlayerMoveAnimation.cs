using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMoveAnimation : MonoBehaviour
{
    [SerializeField, Header("����Image")] SpriteRenderer[] headSR;    
    [SerializeField, Header("�r��Image�A��ɉE��")] SpriteRenderer[] armSR;
    [SerializeField, Header("����Image�A��ɉE��")] SpriteRenderer[] legSR;

    [Header("�S�g")] public GameObject playerRc;
    [SerializeField, Header("�r�̊p�x�A��ɉE��")]  GameObject[] arm;
    [SerializeField, Header("���ڂ̊p�x�A��ɉE��")]  GameObject[] leg;
    [SerializeField, Header("���˂̊p�x�A��ɉE��")]  GameObject[] foot;

    [Header("1�R�}�̊Ԋu�̎���")] public float timeMax;

    [Header("---�����̃A�j���[�V����---")]
    [Header("�S�g�̊p�x")] public float[] playerWalkRotation;
    [Header("�r�̊p�x")] public float[] armWalkRotation;
    [Header("�������̑O���̊p�x")] public float[] legWalkForwardRotation;
    [Header("���̑O���̊p�x")] public float[] footWalkForwardRotation;
    [Header("�������̌���̊p�x")] public float[] legWalkBackRotation;
    [Header("���̌���̊p�x")] public float[] footWalkBackRotation;
    [Header("�����̌p������")] public float timeWalk;

    [Header("---�p���`�̃A�j���[�V����---")]
    [Header("�S�g�̊p�x")] public float[] playerPatRotation;
    [Header("�r�̑O���p�x")] public float[] armPatForwardRotation;
    [Header("�r�̌���p�x")] public float[] armPatBackRotation;
    [Header("�������̑O���̊p�x")] public float[] legPatForwardRotation;
    [Header("���̑O���̊p�x")] public float[] footPatForwardRotation;
    [Header("�������̌���̊p�x")] public float[] legPatBackRotation;
    [Header("���̌���̊p�x")] public float[] footPatBackRotation;

    [Header("---�L�b�N�̃A�j���[�V����---")]
    [Header("�S�g�̊p�x")] public float[] playerKickRotation;
    [Header("�r�̑O���p�x")] public float[] armKickForwardRotation;
    [Header("�r�̌���p�x")] public float[] armKickBackRotation;
    [Header("�������̑O���̊p�x")] public float[] legKickForwardRotation;
    [Header("���̑O���̊p�x")] public float[] footKickForwardRotation;
    [Header("�������̌���̊p�x")] public float[] legKickBackRotation;
    [Header("���̌���̊p�x")] public float[] footKickBackRotation;

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

        // ������������Ă��鎞�A�Ă΂��Ȃ�
        if (time < 0)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                    // �v���C���[�̌�����������E�ɕς�����Ƃ�
                    isWalk = false;
                    shaft = 0;

                    isActive = false;
                    ChangeArmAnime();
                    WalkStart();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {

                // �v���C���[�̌������E���獶�ɕς�����Ƃ�
                isWalk = true;
                shaft = 180;

                isActive = false;
                ChangeArmAnime();
                WalkStart();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                PantieStart();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                KickStart();
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
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerWalkRotation[indexNumber]);

        // �r�̃A�j���[�V����
        if (arm == null || armWalkRotation == null)
        {
            Debug.LogWarning("arm�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft + 180, armWalkRotation[indexNumber]);
        }

        // ���̃A�j���[�V����
        if (leg == null || legWalkBackRotation == null || legWalkForwardRotation == null)
        {
            Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
            return;
        }
        else if (foot == null || footWalkBackRotation == null || footWalkForwardRotation == null) 
        {
            Debug.LogWarning("foot�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            // �����n�߂̏ꍇ
            if (!isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, legWalkBackRotation[indexNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, legWalkForwardRotation[indexNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, footWalkBackRotation[indexNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, footWalkForwardRotation[indexNumber]);
            }
            //���������Ă���ꍇ
            if (isActive)
            {
                leg[0].transform.rotation = Quaternion.Euler(0, shaft, legWalkForwardRotation[indexNumber]);
                leg[1].transform.rotation = Quaternion.Euler(0, shaft, legWalkBackRotation[indexNumber]);
                foot[0].transform.rotation = Quaternion.Euler(0, shaft, footWalkForwardRotation[indexNumber]);
                foot[1].transform.rotation = Quaternion.Euler(0, shaft, footWalkBackRotation[indexNumber]);
            }
        }
    }

    /// <summary>
    /// �p���`�̃��[�V����
    /// </summary>
    void PlayerPantie()
    {
        // Quaternion.Euler: ��]��( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerPatRotation[indexNumber]);

        // �r�̃A�j���[�V����
        if (arm == null || armPatForwardRotation == null || armPatBackRotation == null)
        {
            Debug.LogWarning("arm�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armPatForwardRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft + 180, armPatBackRotation[indexNumber]);
        }

        // ���̃A�j���[�V����
        if (leg == null || legPatBackRotation == null || legPatForwardRotation == null)
        {
            Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
            return;
        }
        else if (foot == null || footPatBackRotation == null || footPatForwardRotation == null)
        {
            Debug.LogWarning("foot�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            leg[0].transform.rotation = Quaternion.Euler(0, shaft, legPatBackRotation[indexNumber]);
            leg[1].transform.rotation = Quaternion.Euler(0, shaft, legPatForwardRotation[indexNumber]);
            foot[0].transform.rotation = Quaternion.Euler(0, shaft, footPatBackRotation[indexNumber]);
            foot[1].transform.rotation = Quaternion.Euler(0, shaft, footPatForwardRotation[indexNumber]);
        }
    }

    /// <summary>
    /// �L�b�N�̃A�j���[�V����
    /// </summary>
    void PlayerKick()
    {
        // Quaternion.Euler: ��]��( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerKickRotation[indexNumber]);

        // �r�̃A�j���[�V����
        if (arm == null || armKickForwardRotation == null || armKickBackRotation == null)
        {
            Debug.LogWarning("arm�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armKickForwardRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft + 180, armKickBackRotation[indexNumber]);
        }

        // ���̃A�j���[�V����
        if (leg == null || legKickBackRotation == null || legKickForwardRotation == null)
        {
            Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
            return;
        }
        else if (foot == null || footKickBackRotation == null || footKickForwardRotation == null)
        {
            Debug.LogWarning("foot�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            leg[0].transform.rotation = Quaternion.Euler(0, shaft, legKickBackRotation[indexNumber]);
            leg[1].transform.rotation = Quaternion.Euler(0, shaft, legKickForwardRotation[indexNumber]);
            foot[0].transform.rotation = Quaternion.Euler(0, shaft, footKickBackRotation[indexNumber]);
            foot[1].transform.rotation = Quaternion.Euler(0, shaft, footKickForwardRotation[indexNumber]);
        }
    }

    IEnumerator CallWalkWithDelay()
    {
        for (int i = 0; i < armWalkRotation.Length; i++)
        {
            PlayerWalk();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % armWalkRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
    }

    IEnumerator CallPantieWithDelay()
    {
        for (int i = 0; i < armPatForwardRotation.Length; i++)
        {
            PlayerPantie();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % armPatForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
    }

    IEnumerator CallKickWithDelay()
    {
        for (int i = 0; i < armKickForwardRotation.Length; i++)
        {
            PlayerKick();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % armKickForwardRotation.Length;
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
            armWalkRotation = armWalkRotation.Select(value => value > 0 ? -value : value).ToArray();
        }
        else if (!isActive)
        {
            armWalkRotation = armWalkRotation.Select(value => value < 0 ? -value : value).ToArray();
        }
    }

    /// <summary>
    /// �����n�߂̊֐�
    /// </summary>
    void WalkStart()
    {
        time = timeMax * armWalkRotation.Length;
        StartCoroutine(CallWalkWithDelay());
    }

    /// <summary>
    /// �p���`�̃A�j���[�V�����J�n����Ƃ��̊֐�
    /// </summary>
    void PantieStart()
    {
        time = timeMax * armPatForwardRotation.Length;
        StartCoroutine(CallPantieWithDelay());
    }

    /// <summary>
    /// �L�b�N�̃A�j���[�V�����J�n����Ƃ��̊֐�
    /// </summary>
    void KickStart()
    {
        time = timeMax * armKickForwardRotation.Length;
        StartCoroutine(CallKickWithDelay());
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
        for (int j = 0; j < headSR.Length; j++) 
        {
            headSR[j].sprite = head.spBody;
        }
    }

    /// <summary>
    /// �r�̃C���[�W
    /// </summary>
    /// <param name="arm">�摜�f�[�^</param>
    public void ChangeArm(BodyPartsData arm)
    {
        for (int j = 0; j < armSR.Length; j++)
        {
            armSR[j].sprite = arm.spArm;
        }
    }

    /// <summary>
    /// ���̃C���[�W
    /// </summary>
    /// <param name="leg">�摜�f�[�^</param>
    public void ChangeLeg(BodyPartsData leg)
    {
        for (int j = 0; j < legSR.Length; j++)
        {
            legSR[j].sprite = leg.spLeg;
        }
    }
}

