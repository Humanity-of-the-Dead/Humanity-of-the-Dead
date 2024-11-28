using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum Status
{
    Zombie,
    Boss,
}
public enum DebugMove
{
    None,
    Walk,
    Kick,
    Pantie
}


public class EnemyMoveAnimation : MonoBehaviour
{
    [Header("�G�̎��")] public Status status;

    [Header("�f�o�b�O�p(�ʏ�ANone)")] public DebugMove debugMoves;

    [Header("�S�g")] public GameObject playerRc;
    [SerializeField, Header("�r�̊p�x�A��ɉE��")] GameObject[] arm;
    [SerializeField, Header("���̊p�x�A��ɉE��")] GameObject[] hand;
    [SerializeField, Header("���ڂ̊p�x�A��ɉE��")] GameObject[] leg;
    [SerializeField, Header("���˂̊p�x�A��ɉE��")] GameObject[] foot;

    [Header("1�R�}�̊Ԋu�̎���")] public float timeMax;

    [Header("---�����̃A�j���[�V����---")]
    public AnimationData walk;

    [Header("---�㔼�g�̃A�j���[�V����---")]
    public AnimationData upper;

    [Header("---�����g�̃A�j���[�V����---")]
    public AnimationData lower;

    //�z��̔ԍ�
    int indexNumber;

    //�̂̎�
    int shaft;

    //�����A�j���[�V�����̊p�x�̐�
    int walkLength;

    // �l�𔽓]�ɂ���t���O
    bool isActive;

    // �����Ă���������E�������Ă��邩
    bool isMirror;

    // �U�������ǂ���
    bool isAttack;

    // �����t���O(�E = false)
    bool isWalk;

    // �Î~���Ă��邩
    bool isStop;

    // �^�C�}�[
    float time;

    // �U���̃^�C�}�[
    float timeAttack;


    private void Start()
    {
        indexNumber = 0;
        shaft = 0;

        isMirror = true;
        isActive = false;
        isAttack = false;
        isWalk = true;
        isStop = false;
        walkLength = walk.armForwardRotation.Length - 1;
        time = 0;
        timeAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeAttack -= Time.deltaTime;

        if (debugMoves != DebugMove.None)
        {
            switch (debugMoves)
            {
                case DebugMove.Walk:
                    WalkInstance();
                    break;
                case DebugMove.Pantie:
                    PantieStart();
                    break;
                case DebugMove.Kick:
                    KickStart();
                    break;
            }
        }
    }
    /// <summary>
    /// �����A�j���[�V����
    /// </summary>
    public void PlayerWalk()
    {
        switch (status)
        {
            case Status.Zombie:
                // Quaternion.Euler: ��]��( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, walk.wholeRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (walk.armForwardRotation == null)
                {
                    Debug.LogWarning("Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, walk.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, walk.armForwardRotation[indexNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, walk.armForwardRotation[indexNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, walk.armForwardRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (walk.legForwardRotation == null || walk.legBackRotation == null)
                {
                    Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (walk.footBackRotation == null || walk.footForwardRotation == null)
                {
                    Debug.LogWarning("Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    if (isActive)
                    {
                        leg[0].transform.rotation = Quaternion.Euler(0, shaft, walk.legBackRotation[indexNumber]);
                        leg[1].transform.rotation = Quaternion.Euler(0, shaft, walk.legForwardRotation[indexNumber]);
                        foot[0].transform.rotation = Quaternion.Euler(0, shaft, walk.footBackRotation[indexNumber]);
                        foot[1].transform.rotation = Quaternion.Euler(0, shaft, walk.footForwardRotation[indexNumber]);
                    }
                    else
                    {
                        leg[0].transform.rotation = Quaternion.Euler(0, shaft, walk.legForwardRotation[indexNumber]);
                        leg[1].transform.rotation = Quaternion.Euler(0, shaft, walk.legBackRotation[indexNumber]);
                        foot[0].transform.rotation = Quaternion.Euler(0, shaft, walk.footForwardRotation[indexNumber]);
                        foot[1].transform.rotation = Quaternion.Euler(0, shaft, walk.footBackRotation[indexNumber]);
                    }
                }
                break;

            case Status.Boss:
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, walk.wholeRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (walk.armForwardRotation == null)
                {
                    Debug.LogWarning("Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, walk.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, -walk.armForwardRotation[indexNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, walk.armForwardRotation[indexNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, -walk.armForwardRotation[indexNumber]);
                }
                // ���̃A�j���[�V����
                if (walk.legForwardRotation == null || walk.legBackRotation == null)
                {
                    Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (walk.footBackRotation == null || walk.footForwardRotation == null)
                {
                    Debug.LogWarning("Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    if (isActive)
                    {
                        leg[0].transform.rotation = Quaternion.Euler(0, shaft, walk.legBackRotation[indexNumber]);
                        leg[1].transform.rotation = Quaternion.Euler(0, shaft, walk.legForwardRotation[indexNumber]);
                        foot[0].transform.rotation = Quaternion.Euler(0, shaft, walk.footBackRotation[indexNumber]);
                        foot[1].transform.rotation = Quaternion.Euler(0, shaft, walk.footForwardRotation[indexNumber]);
                    }
                    else
                    {
                        leg[0].transform.rotation = Quaternion.Euler(0, shaft, walk.legForwardRotation[indexNumber]);
                        leg[1].transform.rotation = Quaternion.Euler(0, shaft, walk.legBackRotation[indexNumber]);
                        foot[0].transform.rotation = Quaternion.Euler(0, shaft, walk.footForwardRotation[indexNumber]);
                        foot[1].transform.rotation = Quaternion.Euler(0, shaft, walk.footBackRotation[indexNumber]);
                    }

                }
                break;
        }
    
    }

    /// <summary>
    /// �㔼�g�̃��[�V����
    /// </summary>
    public void PlayerPantie()
    {
        switch (status)
        {
            case Status.Zombie:
                // Quaternion.Euler: ��]��( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, upper.wholeRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (upper.armForwardRotation == null || upper.armBackRotation == null)
                {
                    Debug.LogWarning("Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, upper.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, upper.armBackRotation[indexNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, upper.armForwardRotation[indexNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, upper.armBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (upper.legForwardRotation == null || upper.legBackRotation == null)
                {
                    Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (upper.footBackRotation == null || upper.footForwardRotation == null)
                {
                    Debug.LogWarning("Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, upper.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, upper.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, upper.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, upper.footForwardRotation[indexNumber]);
                }
                break;
            case Status.Boss:

                playerRc.transform.rotation = Quaternion.Euler(0, shaft, upper.wholeRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (upper.armForwardRotation == null || upper.armBackRotation == null)
                {
                    Debug.LogWarning("Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (lower.handForwardRotation == null || lower.handBackRotation == null)
                {
                    Debug.LogWarning("Hand�̃f�[�^���������甲���Ă���");
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, upper.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, upper.armBackRotation[indexNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, upper.handForwardRotation[indexNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, upper.handBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (upper.legForwardRotation == null || upper.legBackRotation == null)
                {
                    Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (upper.footBackRotation == null || upper.footForwardRotation == null)
                {
                    Debug.LogWarning("Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, upper.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, upper.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, upper.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, upper.footForwardRotation[indexNumber]);
                }
                break;
        }
    }

    /// <summary>
    /// �����g�̃A�j���[�V����
    /// </summary>
    public void PlayerKick()
    {
        switch (status)
        {
            case Status.Zombie:
                // Quaternion.Euler: ��]��( x, y, z)
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, lower.wholeRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (lower.armForwardRotation == null || lower.armBackRotation == null)
                {
                    Debug.LogWarning("Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, lower.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, lower.armBackRotation[indexNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, lower.armForwardRotation[indexNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, lower.armBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (lower.legForwardRotation == null || lower.legBackRotation == null)
                {
                    Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (lower.footBackRotation == null || lower.footForwardRotation == null)
                {
                    Debug.LogWarning("Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, lower.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, lower.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, lower.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, lower.footForwardRotation[indexNumber]);
                }
                break;
            case Status.Boss:
                playerRc.transform.rotation = Quaternion.Euler(0, shaft, lower.wholeRotation[indexNumber]);

                // �r�̃A�j���[�V����
                if (lower.armForwardRotation == null || lower.armBackRotation == null)
                {
                    Debug.LogWarning("Arm�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (lower.handForwardRotation == null || lower.handBackRotation == null)
                {
                    Debug.LogWarning("Hand�̃f�[�^���������甲���Ă���");
                }
                else
                {
                    arm[0].transform.rotation = Quaternion.Euler(0, shaft, lower.armForwardRotation[indexNumber]);
                    arm[1].transform.rotation = Quaternion.Euler(0, shaft, lower.armBackRotation[indexNumber]);
                    hand[0].transform.rotation = Quaternion.Euler(0, shaft, lower.handForwardRotation[indexNumber]);
                    hand[1].transform.rotation = Quaternion.Euler(0, shaft, lower.handBackRotation[indexNumber]);
                }

                // ���̃A�j���[�V����
                if (lower.legForwardRotation == null || lower.legBackRotation == null)
                {
                    Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
                    return;
                }
                else if (lower.footBackRotation == null || lower.footForwardRotation == null)
                {
                    Debug.LogWarning("Foot�̃f�[�^���������甲���Ă�");
                    return;
                }
                else
                {
                    leg[0].transform.rotation = Quaternion.Euler(0, shaft, lower.legBackRotation[indexNumber]);
                    leg[1].transform.rotation = Quaternion.Euler(0, shaft, lower.legForwardRotation[indexNumber]);
                    foot[0].transform.rotation = Quaternion.Euler(0, shaft, lower.footBackRotation[indexNumber]);
                    foot[1].transform.rotation = Quaternion.Euler(0, shaft, lower.footForwardRotation[indexNumber]);
                }
                break;
        }
    }

    IEnumerator CallWalkWithDelay()
    {
        for (int i = 0; i < walk.wholeRotation.Length; i++)
        {
            PlayerWalk();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % walk.wholeRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
    }

    IEnumerator CallPantieWithDelay()
    {
        for (int i = 0; i < upper.armForwardRotation.Length; i++)
        {
            PlayerPantie();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % upper.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
        time = timeMax;
        isAttack = false;
    }

    IEnumerator CallKickWithDelay()
    {
        for (int i = 0; i < lower.armForwardRotation.Length; i++)
        {
            PlayerKick();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % lower.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
        time = timeMax;
        isAttack = false;
    }

    /// <summary>
    /// �������Ƃ��p���������A�r�̔z��̒��̒l���t�ɂ���
    /// </summary>
    void ChangeArmAnime()
    {
        // �]���r�ł͂Ȃ��ꍇ�@
        if (status !=Status.Zombie)
        {
            //�O�����Z�q(�e�v�f�ɑ΂��ĕϊ�������s��)
            if (isActive)
            {
                walk.armForwardRotation = walk.armForwardRotation.Select(value => value < 0 ? -value : value).ToArray();
            }
            else if (!isActive)
            {
                walk.armForwardRotation = walk.armForwardRotation.Select(value => value > 0 ? -value : value).ToArray();
            }
        }
    }

    /// <summary>
    /// �������Ƃ��J�n�̊֐�
    /// </summary>
    void WalkStart()
    {
        time = timeMax * walk.armForwardRotation.Length;
        StartCoroutine(CallWalkWithDelay());
    }

    /// <summary>
    /// �p���`�̃A�j���[�V�����J�n����Ƃ��̊֐�
    /// </summary>
    public void PantieStart()
    {
        if (timeAttack < 0)
        {
            isAttack = true;
            time = timeMax * upper.armForwardRotation.Length * 2;
            timeAttack = timeMax * upper.armForwardRotation.Length;
            StopCoroutine(CallWalkWithDelay());
            Upright();
            indexNumber = 0;
            StartCoroutine(CallPantieWithDelay());
        }
    }

    /// <summary>
    /// �L�b�N�̃A�j���[�V�����J�n����Ƃ��̊֐�
    /// </summary>
    public void KickStart()
    {
        if (timeAttack < 0)
        {
            isAttack = true;
            time = timeMax * lower.armForwardRotation.Length;
            timeAttack = timeMax * lower.armForwardRotation.Length;
            StopCoroutine(CallWalkWithDelay());
            Upright();
            indexNumber = 0;
            StartCoroutine(CallKickWithDelay());
        }
    }

    /// <summary>
    /// �������Ƃ̏�����
    /// </summary>
    public void WalkInstance()
    {
        if (time < 0 && !isAttack)
        {
            isActive = !isActive;
            ChangeArmAnime();
            WalkStart();
        }
    }

    /// <summary>
    /// �������Ƃ��p�������Ƃ�
    /// </summary>
    public void KeepWalk()
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
    /// ��������
    /// </summary>
    public void Upright()
    {
       
    }

    /// <summary>
    /// �E�������Ƃ�
    /// </summary>
    public void RightMove()
    {
        shaft = 0;
    }

    /// <summary>
    /// ���������Ƃ�
    /// </summary>
    public void LeftMove()
    {
        shaft = 180;
    }
}
