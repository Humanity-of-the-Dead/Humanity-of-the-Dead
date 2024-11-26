using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UpperAttack
{
    NORMAL,
    POLICE,
    NURSE,
}

public enum DownAttack
{
    NORMAL,
    POLICE,
    NURSE,
}

public class PlayerMoveAnimation : MonoBehaviour
{
    [SerializeField, Header("����Image")] SpriteRenderer headSR;
    [SerializeField, Header("�̂̂�Image")] SpriteRenderer bodySR;
    [SerializeField, Header("�E�r��Image")] SpriteRenderer armRightSR;
    [SerializeField, Header("���r��Image")] SpriteRenderer armLeftSR;
    [SerializeField, Header("�E����Image")] SpriteRenderer handRightSR;
    [SerializeField, Header("������Image")] SpriteRenderer handLeftSR;
    [SerializeField, Header("����Image")] SpriteRenderer waistSR;
    [SerializeField, Header("�E���ڂ�Image")] SpriteRenderer legRightSR;
    [SerializeField, Header("�����ڂ�Image")] SpriteRenderer legLeftSR;
    [SerializeField, Header("�E����Image")] SpriteRenderer footRightSR;
    [SerializeField, Header("������Image")] SpriteRenderer footLeftSR;

    [Header("�S�g")] public GameObject playerRc;
    [SerializeField, Header("�r�̊p�x�A��ɉE��")]  GameObject[] arm;
    [SerializeField, Header("���ڂ̊p�x�A��ɉE��")]  GameObject[] leg;
    [SerializeField, Header("���˂̊p�x�A��ɉE��")]  GameObject[] foot;

    [Header("1�R�}�̊Ԋu�̎���")] public float timeMax;

    [Header("---�����̃A�j���[�V����---")]
    [Header("�S�g�̊p�x")] public float[] playerWalkRotation;
    [Header("�r�̊p�x")] public float[] armWalkRotation;
    [Header("�������̑O���p�x")] public float[] legWalkForwardRotation;
    [Header("���̑O���p�x")] public float[] footWalkForwardRotation;
    [Header("�������̌���p�x")] public float[] legWalkBackRotation;
    [Header("���̌���p�x")] public float[] footWalkBackRotation;
    [Header("�����̌p������")] public float timeWalk;

    [Header("---�f�t�H���g�p���`�̃A�j���[�V����---")]
    public AnimationData playerUpper;

    [Header("---�f�t�H���g�L�b�N�̃A�j���[�V����---")]
    public AnimationData playerLower;

    [Header("---�x�@���e�̃A�j���[�V����---")]
    public AnimationData policeUpper;

    [Header("---�x�@�����g�̃A�j���[�V����---")]
    public AnimationData policeLower;

    [Header("---�i�[�X�p���`�̃A�j���[�V����---")]
    public AnimationData nurseUpper;

    [Header("---�i�[�X�L�b�N�̃A�j���[�V����---")]
    public AnimationData nurseLower;

    UpperAttack upperAttack;

    DownAttack downAttack;

    //�z��̔ԍ�
    int indexNumber;

    //�̂̎�
    int shaft;

    //�����A�j���[�V�����̊p�x�̐�
    int walkLength;

    // �l�𔽓]�ɂ���t���O
    bool isActive;

    // �U�������ǂ���
    bool isAttack;

    // �����t���O(�E = false)
    bool isWalk;

    // �Î~���Ă��邩
    bool isStop;

    // �^�C�}�[
    float time;
    
    // �^�C�}�[
    float timeAttack;

    private void Start()
    {
        indexNumber = 0;
        shaft = 0;

        isAttack = false;
        isActive = false;
        isWalk = false;
        isStop = false;
        walkLength = armWalkRotation.Length - 1;
        time = 0;
        timeAttack = 0;

        upperAttack = UpperAttack.NORMAL;
        downAttack = DownAttack.NORMAL; 
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeAttack -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.D))
        {
            shaft = 0;

            //�Î~��Ԃ��獶�����Ƃ�
            if (time < 0 && isWalk)
            {
                isStop = true;
                time = timeMax * 2;
                Upright();
            }

            // �v���C���[�̌�����������E�ɕς�����Ƃ�
            isWalk = false;
           

            // ������������Ă��鎞�A�Ă΂��Ȃ�
            WalkInstance();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            shaft = 180;

            //�Î~��Ԃ��獶�����Ƃ�
            if (time < 0 && !isWalk)
            {
                isStop = true;
                time = timeMax * 2;
                Upright();
            }

            // �v���C���[�̌������E���獶�ɕς�����Ƃ�
            isWalk = true;
            
            // ������������Ă��鎞�A�Ă΂��Ȃ�
            WalkInstance();
        }

        if (timeAttack < 0)
        {
            if (Input.GetKeyDown(KeyCode.I))
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
                if(isStop)
                {
                    isStop = false;
                    WalkInstance();
                }
                else
                {
                    KeepWalk();
                }
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (isWalk)
            {
                if (isStop)
                {
                    isStop = false;
                    WalkInstance();
                }
                else
                {
                    KeepWalk();
                }
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
            Debug.LogWarning("Arm�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, -armWalkRotation[indexNumber]);
        }

        // ���̃A�j���[�V����
        if (leg == null || legWalkBackRotation == null || legWalkForwardRotation == null)
        {
            Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
            return;
        }
        else if (foot == null || footWalkBackRotation == null || footWalkForwardRotation == null) 
        {
            Debug.LogWarning("Foot�̃f�[�^���������甲���Ă�");
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
    /// �㔼�g�̃��[�V����
    /// </summary>
    void PlayerPantie()
    {
        switch(upperAttack)
        {
            case UpperAttack.NORMAL:
                // Quaternion.Euler: ��]��( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerUpper.wholeRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (playerUpper.armForwardRotation == null || playerUpper.armBackRotation == null)
                {
                    Debug.LogWarning("Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.armBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (playerUpper.legForwardRotation == null || playerUpper.legBackRotation == null)
                {
                    Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (playerUpper.footBackRotation == null || playerUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, playerUpper.footForwardRotation[indexNumber]);
                }
                break;
            case UpperAttack.POLICE:

                // Quaternion.Euler: ��]��( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, policeUpper.wholeRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (policeUpper.armForwardRotation == null || policeUpper.armBackRotation == null)
                {
                    Debug.LogWarning("�x�@Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.armBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (policeUpper.legForwardRotation == null || policeUpper.legBackRotation == null)
                {
                    Debug.LogWarning("�x�@Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (policeUpper.footBackRotation == null || policeUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("�x�@Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, policeUpper.footForwardRotation[indexNumber]);
                }
                break;
            case UpperAttack.NURSE:

                // Quaternion.Euler: ��]��( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.wholeRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (nurseUpper.armForwardRotation == null || nurseUpper.armBackRotation == null)
                {
                    Debug.LogWarning("�i�[�XArm�̃f�[�^���������甲���Ă�");    
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.armBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (nurseUpper.legForwardRotation == null || nurseUpper.legBackRotation == null)
                {
                    Debug.LogWarning("�i�[�XLeg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (nurseUpper.footBackRotation == null || nurseUpper.footForwardRotation == null)
                {
                    Debug.LogWarning("�i�[�XFoot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, nurseUpper.footForwardRotation[indexNumber]);
                }
                break;
        }
    }

    /// <summary>
    /// �L�b�N�̃A�j���[�V����
    /// </summary>
    void PlayerKick()
    {
        switch(downAttack)
        {
            case DownAttack.NORMAL:
                // Quaternion.Euler: ��]��( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerLower.wholeRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (playerLower.armForwardRotation == null || playerLower.armBackRotation == null)
                {
                    Debug.LogWarning("Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, playerLower.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, playerLower.armBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (playerLower.legBackRotation == null || playerLower.legForwardRotation == null)
                {
                    Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (playerLower.footBackRotation == null || playerLower.footForwardRotation == null)
                {
                    Debug.LogWarning("Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, playerLower.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, playerLower.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, playerLower.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, playerLower.footForwardRotation[indexNumber]);
                }
                break;
            case DownAttack.POLICE:
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, policeLower.wholeRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (policeLower.armForwardRotation == null || policeLower.armBackRotation == null)
                {
                    Debug.LogWarning("�x�@Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, policeLower.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, policeLower.armBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (policeLower.legForwardRotation == null || policeLower.legBackRotation == null)
                {
                    Debug.LogWarning("�x�@Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (policeLower.footBackRotation == null || policeLower.footForwardRotation == null)
                {
                    Debug.LogWarning("�x�@Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {   
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, policeLower.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, policeLower.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, policeLower.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, policeLower.footForwardRotation[indexNumber]);
                }
                break;
            case DownAttack.NURSE:

                // Quaternion.Euler: ��]��( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, nurseLower.wholeRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (nurseLower.armForwardRotation == null || nurseLower.armBackRotation == null)
                {
                    Debug.LogWarning("�i�[�XArm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.armBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (nurseLower.legForwardRotation == null || nurseLower.legBackRotation == null)
                {
                    Debug.LogWarning("�i�[�XLeg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (nurseLower.footBackRotation == null || nurseLower.footForwardRotation == null)
                {
                    Debug.LogWarning("�i�[�XFoot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, nurseLower.footForwardRotation[indexNumber]);
                }
                break;
        }
    }

    IEnumerator CallWalkWithDelay()
    {
        for (int i = 0; i < armWalkRotation.Length; i++)
        {
            if (!isAttack)
            {
                PlayerWalk();

                // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
                indexNumber = (indexNumber + 1) % armWalkRotation.Length;
                yield return new WaitForSeconds(timeMax);
            }
        }
    }

    IEnumerator CallPantieWithDelay()
    {
        for (int i = 0; i < playerUpper.armForwardRotation.Length; i++)
        {
            PlayerPantie();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % playerUpper.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }

        time = 0;
        isAttack = false;
    }

    IEnumerator CallKickWithDelay()
    {
        for (int i = 0; i < playerLower.armForwardRotation.Length; i++)
        {
            PlayerKick();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % playerLower.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }

        time = 0;
        isAttack = false;
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
    /// �������Ƃ��J�n�̊֐�
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
        AttackWaite();
        time = timeMax * playerUpper.armForwardRotation.Length;
        StartCoroutine(CallPantieWithDelay());
    }

    /// <summary>
    /// �L�b�N�̃A�j���[�V�����J�n����Ƃ��̊֐�
    /// </summary>
    void KickStart()
    {
        AttackWaite();
        time = timeMax * playerLower.armForwardRotation.Length;
        StartCoroutine(CallKickWithDelay());
    }

    /// <summary>
    /// �������Ƃ̏�����
    /// </summary>
    void WalkInstance()
    {
        if (time < 0)
        {
            indexNumber = 0;
            isActive = false;
            ChangeArmAnime();
            WalkStart();
        }
    }

    /// <summary>
    /// �������Ƃ��p�������Ƃ�
    /// </summary>
    void KeepWalk()
    {
        // �A�����͂���Ă��邩
        if (time - 0.05 < 0)
        {
            indexNumber = 0;
            isActive = !isActive;
            ChangeArmAnime();
            WalkStart();
        }
    }

    /// <summary>
    /// �U���̏�����
    /// </summary>
    void AttackWaite()
    {
        timeAttack = timeMax * playerLower.armForwardRotation.Length;
        isAttack = true;
        StopCoroutine(CallWalkWithDelay());
        Upright();
        indexNumber = 1;
    }

    /// <summary>
    /// ��������
    /// </summary>
    void Upright()
    {
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerWalkRotation[walkLength]);
        arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[walkLength]);
        arm[1].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[walkLength]);
        leg[0].transform.rotation = Quaternion.Euler(0, shaft, legWalkBackRotation[walkLength]);
        leg[1].transform.rotation = Quaternion.Euler(0, shaft, legWalkForwardRotation[walkLength]);
        foot[0].transform.rotation = Quaternion.Euler(0, shaft, footWalkBackRotation[walkLength]);
        foot[1].transform.rotation = Quaternion.Euler(0, shaft, footWalkForwardRotation[walkLength]);
    }

    /// <summary>
    /// �㔼�g�̃C���[�W
    /// </summary>
    /// <param name="upperBody">�摜�f�[�^�W����</param>
    public void ChangeUpperBody(BodyPartsData upperBody)
    {
        bodySR.sprite = upperBody.spBody;
        armRightSR.sprite = upperBody.spRightArm;
        armLeftSR.sprite = upperBody.spLeftArm;
        handRightSR.sprite = upperBody.spRightHand;
        handLeftSR.sprite = upperBody.spLeftHand;
    }

    /// <summary>
    /// �����g�̃C���[�W
    /// </summary>
    /// <param name="underBody">�摜�f�[�^�W����</param>
    public void ChangeUnderBody(BodyPartsData underBody)
    {
        waistSR.sprite = underBody.spWaist;
        footRightSR.sprite = underBody.spRightFoot;
        footLeftSR.sprite = underBody.spLeftFoot;
        legRightSR.sprite = underBody.spRightLeg;
        legLeftSR.sprite = underBody.spLeftLeg;
    }

    /// <summary>
    /// �㔼�g�̍U���̕ω�
    /// </summary>
    /// <param name="isName">�ڐA���镨��</param>
    public void ChangeUpperMove(UpperAttack isName)
    {
        upperAttack = isName;
    }

    /// <summary>
    /// �����g�̍U���̕ω�
    /// </summary>
    /// <param name="isName">�ڐA���镨��</param>
    public void ChangeDownMove(DownAttack isName)
    {
        downAttack = isName;
    }
}

