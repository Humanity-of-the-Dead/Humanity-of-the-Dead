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
    [Header("�S�g�̊p�x")] public float[] playerPatRotation;
    [Header("�r�̑O���p�x")] public float[] armPatForwardRotation;
    [Header("�r�̌���p�x")] public float[] armPatBackRotation;
    [Header("�������̉��p�x")] public float[] legPatForwardRotation;
    [Header("���̉��p�x")] public float[] footPatForwardRotation;
    [Header("�������̎�O�p�x")] public float[] legPatBackRotation;
    [Header("���̎�O�p�x")] public float[] footPatBackRotation;

    [Header("---�f�t�H���g�L�b�N�̃A�j���[�V����---")]
    [Header("�S�g�̊p�x")] public float[] playerKickRotation;
    [Header("�r�̑O���p�x")] public float[] armKickForwardRotation;
    [Header("�r�̌���p�x")] public float[] armKickBackRotation;
    [Header("�������̉��p�x")] public float[] legKickForwardRotation;
    [Header("���̉��p�x")] public float[] footKickForwardRotation;
    [Header("�������̎�O�p�x")] public float[] legKickBackRotation;
    [Header("���̎�O�p�x")] public float[] footKickBackRotation;

    [Header("---�x�@���e�̃A�j���[�V����---")]
    [Header("�S�g�̊p�x")] public float[]         poPlayerPatRotation;
    [Header("�r�̎�O�p�x")] public float[]       poArmPatForwardRotation;
    [Header("�r�̉��p�x")] public float[]       poArmPatBackRotation;
    [Header("�������̉��p�x")] public float[] poLegPatForwardRotation;
    [Header("���̉��p�x")] public float[]     poFootPatForwardRotation;
    [Header("�������̎�O�p�x")] public float[] poLegPatBackRotation;
    [Header("���̎�O�p�x")] public float[]     poFootPatBackRotation;

    [Header("---�i�[�X�p���`�̃A�j���[�V����---")]
    [Header("�S�g�̊p�x")] public float[]         nuPlayerPatRotation;
    [Header("�r�̎�O�p�x")] public float[]       nuArmPatForwardRotation;
    [Header("�r�̉��p�x")] public float[]       nuArmPatBackRotation;
    [Header("�������̉��p�x")] public float[] nuLegPatForwardRotation;
    [Header("���̉��p�x")] public float[]     nuFootPatForwardRotation;
    [Header("�������̎�O�p�x")] public float[] nuLegPatBackRotation;
    [Header("���̎�O�p�x")] public float[]     nuFootPatBackRotation;

    [Header("---�i�[�X�L�b�N�̃A�j���[�V����---")]
    [Header("�S�g�̊p�x")] public float[]         nuPlayerKickRotation;
    [Header("�r�̎�O���p�x")] public float[]       nuArmKickForwardRotation;
    [Header("�r�̉��p�x")] public float[]       nuArmKickBackRotation;
    [Header("�������̉��p�x")] public float[] nuLegKickForwardRotation;
    [Header("���̉��p�x")] public float[]     nuFootKickForwardRotation;
    [Header("�������̎�O�p�x")] public float[] nuLegKickBackRotation;
    [Header("���̎�O�p�x")] public float[]     nuFootKickBackRotation;

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
            Debug.LogWarning("arm�̃f�[�^���������甲���Ă�");
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
    /// �㔼�g�̃��[�V����
    /// </summary>
    void PlayerPantie()
    {
        switch(upperAttack)
        {
            case UpperAttack.NORMAL:
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
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, armPatBackRotation[indexNumber]);
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
                break;
            case UpperAttack.POLICE:

                // Quaternion.Euler: ��]��( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, poPlayerPatRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (armPatForwardRotation == null || poArmPatBackRotation == null)
                {
                    Debug.LogWarning("�x�@��Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, poArmPatForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, poArmPatBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (poLegPatBackRotation == null || poLegPatForwardRotation == null)
                {
                    Debug.LogWarning("�x�@��Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (poFootPatBackRotation == null || poFootPatForwardRotation == null)
                {
                    Debug.LogWarning("�x�@��Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, poLegPatBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, poLegPatForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, poFootPatBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, poFootPatForwardRotation[indexNumber]);
                }
                break;
            case UpperAttack.NURSE:

                // Quaternion.Euler: ��]��( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, nuPlayerPatRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (nuArmPatForwardRotation == null || nuArmPatBackRotation == null)
                {
                    Debug.LogWarning("�i�[�X�̏㔼�g�U����Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, nuArmPatForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, nuArmPatBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (nuLegPatBackRotation == null || nuLegPatForwardRotation == null)
                {
                    Debug.LogWarning("�i�[�X�̏㔼�g�U����Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (nuFootPatBackRotation == null || nuFootPatForwardRotation == null)
                {
                    Debug.LogWarning("�i�[�X�̏㔼�g�U����Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, nuLegPatBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, nuLegPatForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, nuFootPatBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, nuFootPatForwardRotation[indexNumber]);
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
            case DownAttack.POLICE:

                // Quaternion.Euler: ��]��( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerKickRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (armKickForwardRotation == null || armKickBackRotation == null)
                {
                    Debug.LogWarning("arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, armKickForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, armKickBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (legKickBackRotation == null || legKickForwardRotation == null)
                {
                    Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (footKickBackRotation == null || footKickForwardRotation == null)
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
                break;

            case DownAttack.NURSE:

                // Quaternion.Euler: ��]��( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, nuPlayerKickRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (nuArmKickForwardRotation == null || nuArmKickBackRotation == null)
                {
                    Debug.LogWarning("�i�[�X�̉����g�U����Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, nuArmKickForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, nuArmKickBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (nuLegKickBackRotation == null || nuLegKickForwardRotation == null)
                {
                    Debug.LogWarning("�i�[�X�̉����g�U����Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (nuFootKickBackRotation == null || nuFootKickForwardRotation == null)
                {
                    Debug.LogWarning("�i�[�X�̉����g�U����Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, nuLegKickBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, nuLegKickForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, nuFootKickBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, nuFootKickForwardRotation[indexNumber]);
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
        for (int i = 0; i < armPatForwardRotation.Length; i++)
        {
            PlayerPantie();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % armPatForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }

        time = 0;
        isAttack = false;
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
        time = timeMax * armPatForwardRotation.Length;
        StartCoroutine(CallPantieWithDelay());
    }

    /// <summary>
    /// �L�b�N�̃A�j���[�V�����J�n����Ƃ��̊֐�
    /// </summary>
    void KickStart()
    {
        AttackWaite();
        time = timeMax * armKickForwardRotation.Length;
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
        timeAttack = timeMax * armKickBackRotation.Length;
        isAttack = true;
        StopCoroutine(CallWalkWithDelay());
        Upright();
        indexNumber = 0;
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

