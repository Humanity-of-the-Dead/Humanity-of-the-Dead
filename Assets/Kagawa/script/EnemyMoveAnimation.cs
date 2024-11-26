using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyMoveAnimation : MonoBehaviour
{
    [Header("�]���r")] public bool zombie;
    [Header("�S�g")] public GameObject playerRc;
    [SerializeField, Header("�r�̊p�x�A��ɉE��")] GameObject[] arm;
    [SerializeField, Header("���ڂ̊p�x�A��ɉE��")] GameObject[] leg;
    [SerializeField, Header("���˂̊p�x�A��ɉE��")] GameObject[] foot;

    [Header("1�R�}�̊Ԋu�̎���")] public float timeMax;

    [Header("---�����̃A�j���[�V����---")]
    [Header("�S�g�̊p�x")] public float[] playerWalkRotation;
    [Header("�r�̊p�x")] public float[] armWalkRotation;
    [Header("�������̑O���̊p�x")] public float[] legWalkForwardRotation;
    [Header("���̑O���̊p�x")] public float[] footWalkForwardRotation;
    [Header("�������̌���̊p�x")] public float[] legWalkBackRotation;
    [Header("���̌���̊p�x")] public float[] footWalkBackRotation;
    [Header("�����̌p������")] public float timeWalk;

    [Header("---�㔼�g�̃A�j���[�V����---")]
    public AnimationData Upper;

    [Header("---�����g�̃A�j���[�V����---")]
    public AnimationData Lower;

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
        walkLength = armWalkRotation.Length - 1;
        time = 0;
        timeAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeAttack-= Time.deltaTime;
    }

    /// <summary>
    /// �����A�j���[�V����
    /// </summary>
    public void PlayerWalk()
    {
        // Quaternion.Euler: ��]��( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerWalkRotation[indexNumber]);

        // �r�̃A�j���[�V����
        if (arm == null || armWalkRotation == null)
        {
            Debug.LogWarning("arm�̃f�[�^���������甲���Ă�");
            return;
        }
        else if(zombie)
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[indexNumber]);
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[indexNumber]);
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
    public void PlayerPantie()
    {
        // Quaternion.Euler: ��]��( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, Upper.wholeRotation[indexNumber]);

        // �r�̃A�j���[�V����
        if (Upper.armForwardRotation == null || Upper.armBackRotation == null)
        {
            Debug.LogWarning("Arm�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, Upper.armForwardRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, Upper.armBackRotation[indexNumber]);
        }

        // ���̃A�j���[�V����
        if (Upper.legForwardRotation == null || Upper.legBackRotation == null)
        {
            Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
            return;
        }
        else if (Upper.footBackRotation == null || Upper.footForwardRotation == null)
        {
            Debug.LogWarning("Foot�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            leg[0].transform.rotation = Quaternion.Euler(0, shaft, Upper.legBackRotation[indexNumber]);
            leg[1].transform.rotation = Quaternion.Euler(0, shaft, Upper.legForwardRotation[indexNumber]);
            foot[0].transform.rotation = Quaternion.Euler(0, shaft, Upper.footBackRotation[indexNumber]);
            foot[1].transform.rotation = Quaternion.Euler(0, shaft, Upper.footForwardRotation[indexNumber]);
        }
    }

    /// <summary>
    /// �����g�̃A�j���[�V����
    /// </summary>
    public void PlayerKick()
    {
        // Quaternion.Euler: ��]��( x, y, z)
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, Lower.wholeRotation[indexNumber]);

        // �r�̃A�j���[�V����
        if (Lower.armForwardRotation == null || Lower.armBackRotation == null)
        {
            Debug.LogWarning("Arm�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            arm[0].transform.rotation = Quaternion.Euler(0, shaft, Lower.armForwardRotation[indexNumber]);
            arm[1].transform.rotation = Quaternion.Euler(0, shaft, Lower.armBackRotation[indexNumber]);
        }

        // ���̃A�j���[�V����
        if (Lower.legForwardRotation == null || Lower.legBackRotation == null)
        {
            Debug.LogWarning("Leg�̃f�[�^���������甲���Ă�");
            return;
        }
        else if (Lower.footBackRotation == null || Lower.footForwardRotation == null)
        {
            Debug.LogWarning("Foot�̃f�[�^���������甲���Ă�");
            return;
        }
        else
        {
            leg[0].transform.rotation = Quaternion.Euler(0, shaft, Lower.legBackRotation[indexNumber]);
            leg[1].transform.rotation = Quaternion.Euler(0, shaft, Lower.legForwardRotation[indexNumber]);
            foot[0].transform.rotation = Quaternion.Euler(0, shaft, Lower.footBackRotation[indexNumber]);
            foot[1].transform.rotation = Quaternion.Euler(0, shaft, Lower.footForwardRotation[indexNumber]);
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
        for (int i = 0; i < Upper.armForwardRotation.Length; i++)
        {
            PlayerPantie();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % Upper.armForwardRotation.Length;
            yield return new WaitForSeconds(timeMax);
        }
        time = timeMax;
        isAttack = false;
    }

    IEnumerator CallKickWithDelay()
    {
        for (int i = 0; i < Lower.armForwardRotation.Length; i++)
        {
            PlayerKick();

            // indexNumber�̒l�𑝂₷(�z��ԍ����グ��)
            indexNumber = (indexNumber + 1) % Lower.armForwardRotation.Length;
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
        if (!zombie)
        {
            //�O�����Z�q(�e�v�f�ɑ΂��ĕϊ�������s��)
            if (isActive)
            {
                armWalkRotation = armWalkRotation.Select(value => value < 0 ? -value : value).ToArray();
            }
            else if (!isActive)
            {
                armWalkRotation = armWalkRotation.Select(value => value > 0 ? -value : value).ToArray();
            }
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
    public void PantieStart()
    {
        if (timeAttack < 0)
        {
            Debug.Log("�p���`�X�^�[�g");
            isAttack = true;
            time = timeMax * Upper.armForwardRotation.Length * 2;
            timeAttack = timeMax * Upper.armForwardRotation.Length;
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
            time = timeMax * Lower.armForwardRotation.Length;
            timeAttack = timeMax * Lower.armForwardRotation.Length;
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
        Debug.Log("�����n��");
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
        Debug.Log("�A�v���C�g");
        playerRc.transform.rotation = Quaternion.Euler(0, shaft, playerWalkRotation[walkLength]);
        arm[0].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[walkLength]);
        arm[1].transform.rotation = Quaternion.Euler(0, shaft, armWalkRotation[walkLength]);
        leg[0].transform.rotation = Quaternion.Euler(0, shaft, legWalkBackRotation[walkLength]);
        leg[1].transform.rotation = Quaternion.Euler(0, shaft, legWalkForwardRotation[walkLength]);
        foot[0].transform.rotation = Quaternion.Euler(0, shaft, footWalkBackRotation[walkLength]);
        foot[1].transform.rotation = Quaternion.Euler(0, shaft, footWalkForwardRotation[walkLength]);
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
