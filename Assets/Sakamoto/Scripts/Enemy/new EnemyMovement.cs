using UnityEngine;

public class newEnemyMovement : MonoBehaviour
{
    // �ړ����n�߂�ꏊ�A�I���̏ꏊ�A���i�̈ړ����x�A�ǐՒ��̈ړ����x�A�G�̍��G�\�Ȕ͈͂�ݒ�

    [Header("�G�l�~�[�̈ړ��֘A�̐ݒ�")]
    [Tooltip("�ړ��̐�Βl")]
    [SerializeField] private float moveDistance = 5f;
    [Tooltip("���i�̈ړ����x")]
    [SerializeField] private float normalSpeed = 2f;
    [Tooltip("�ǐՒ��̈ړ����x")]
    [SerializeField] private float chaseSpeed = 4f;
    [Tooltip("�G�̍��G�\�Ȕ͈�")]
    [SerializeField] private float chaseRange = 5f;
    [Tooltip("�G�̍U���\�Ȕ͈�")]
    [SerializeField] private float attackRange = 2f;
    [Tooltip("�G�̍U���ҋ@����")]
    [SerializeField] private float waitTime = 1f;

    [Header("�G�l�~�[�̊�{�ݒ�")]
    [Tooltip("�㔼�g�p�[�c�̍U���͂ƍU���͈͂𔽉f")]

    [SerializeField] private BodyPartsData upperPart;
    [Tooltip("�����g�p�[�c�̍U���͂ƍU���͈͂𔽉f")]

    [SerializeField] private BodyPartsData lowerPart;
    private Gun gun;

    private newEnemyParameters newEnemyParameters;
    private EnemyMoveAnimation enemyMoveAnimation;

    public enum EnemyState { Search, Walk, AttackIdle, Attack, AfterAttack, Wait, IsDead }

    protected EnemyState enemyState = EnemyState.Search;

    private float pointA, pointB;   // �J�n�ʒu�ƏI���ʒu(A: �ړ��\�͈͂̉E�[/ B: ���[)
    private bool isMovingToPointB = true; // �i�s����
    public PlayerControl player; // �v���C���[�̈ʒu
    private float timer;//�U����̎���
    [Tooltip("�{�X�̈ړ��\�ȍŏ�X���W")]
    [SerializeField] private float bossMinX = -8f;
    [Tooltip("�{�X�̈ړ��\�ȍő�X���W")]
    [SerializeField] private float bossMaxX = 8f;
    protected virtual void Start()
    {

        // �v���C���[��T�����
        player = FindFirstObjectByType<PlayerControl>();




        enemyMoveAnimation = GetComponent<EnemyMoveAnimation>();

        pointA = transform.position.x + moveDistance;
        pointB = transform.position.x - moveDistance;
        newEnemyParameters = GetComponent<newEnemyParameters>();
        if (upperPart.sPartsName == "�x�@�̏㔼�g")
        {
            gun = GetComponent<Gun>();
        }

    }

    protected virtual void Update()
    {
        // �v���C���[�Ƃ̋������v�Z
        //Debug.Log(distanceToPlayer.ToString());
        switch (GameMgr.GetState())
        {
            case GameState.Main:

                EnemyAction();


                break;

            case GameState.ShowText:
            case GameState.Hint:
                break;


        }
    }
    public EnemyState GetEnemyState()
    {
        return enemyState;
    }
    public void SetEnemyState(EnemyState newState)
    {
        enemyState = newState;
    }
    private void MoveTowards(Vector3 target, float moveSpeed)
    {
        // �{�X�̈ړ��͈͂𐧌�
        if (newEnemyParameters.Boss)
        {
            target.x = Mathf.Clamp(target.x, bossMinX, bossMaxX);
        }
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log($"Collided with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

            //Debug.Log("�G���m���Փ˂��A���E");
            if (isMovingToPointB)
            {
                enemyMoveAnimation?.TurnToRight();
            }
            else
            {
                enemyMoveAnimation?.TurnToLeft();
            }
            isMovingToPointB = !isMovingToPointB;
        }
    }

    private bool IsAttackableRangeAndDirection()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return (distanceToPlayer < upperPart.AttackArea || distanceToPlayer < lowerPart.AttackArea) && IsLeftFromPlayer() == isMovingToPointB;
    }

    // 
    bool IsLeftFromPlayer()
    {
        float Direction = player.transform.position.x - gameObject.transform.position.x;
        if (Direction < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnUpperAttackAnimationFinished()
    {
        // �㔼�g�U������
        UpperEnemyAttack((float)upperPart.iPartAttack);

    }

    private void OnLowerAttackAnimationFinished()
    {
        LowerEnemyAttack((float)lowerPart.iPartAttack);
    }
    void UpperEnemyAttack(float upperDamage)
    {

        IDamageable damageable = PlayerParameter.Instance.GetComponent<IDamageable>();
        damageable?.TakeDamage(upperDamage, 0);
        Debug.Log("�㔼�g�U���_���[�W���f");


    }
    void LowerEnemyAttack(float lowerDamage)
    {
        IDamageable damageable = PlayerParameter.Instance.GetComponent<IDamageable>();
        damageable?.TakeDamage(lowerDamage, 1);
        //Debug.Log("�����g�U���_���[�W���f");

    }

    private void AttackIdle()
    {
        enemyState = EnemyState.AttackIdle;
        enemyMoveAnimation.AttakIdleStart();
    }

    // �㔼�g�ł̍U����SE����
    private void AttackWithSeUpper()
    {
        Debug.Log("AttackWithSeUpper");
        //�㔼�g�U��
        enemyMoveAnimation.PantieStart();

        if (upperPart.sPartsName != "�x�@�̏㔼�g" && IsAttackableRangeAndDirection())
        {
            OnUpperAttackAnimationFinished();
        }

        //�U���҂̏㔼�g���m�F
        switch (upperPart.sPartsName)
        {
            case "�x�@�̏㔼�g":
                Vector2 ShootMoveVector = (player.transform.position - enemyMoveAnimation.playerRc.transform.position).normalized;
                float enemyRotationY = enemyMoveAnimation.playerRc.transform.eulerAngles.y;
                Debug.Log(enemyRotationY);
                if (enemyRotationY == 180)
                {
                    // �������̏ꍇ�A�����x�N�g����x�𔽓]
                    ShootMoveVector.x = -Mathf.Abs(ShootMoveVector.x);
                }
                else
                {
                    // �E�����̏ꍇ�A�����x�N�g����x�͂��̂܂�
                    ShootMoveVector.x = Mathf.Abs(ShootMoveVector.x);
                }

                // �e���g���Ēe�𔭎�
                gun.Shoot(ShootMoveVector, transform, upperPart.iPartAttack);
                //�x�@���̏㔼�g�ōU������SE��炷
                MultiAudio.ins.PlaySEByName(
                    "SE_policeofficer_attack_upper");

                Debug.Log("�x�����㔼�g�ōU��");

                break;

            case "�{�X�̏㔼�g":
                //���X�{�X�㔼�g�̍U������SE��炷
                MultiAudio.ins.PlaySEByName(
                    "SE_lastboss_attack_upper");

                Debug.Log("�{�X���㔼�g�ōU��");

                break;

            case "�L�m�R�̏㔼�g":
                //��l���㔼�g�̍U������SE��炷
                MultiAudio.ins.PlaySEByName(
                    "SE_hero_attack_upper");

                Debug.Log("�L�m�R�G�����㔼�g�ōU��");

                break;

            case "�Ō�t�̏㔼�g":
                //�i�[�X�㔼�g�̍U������SE��炷
                MultiAudio.ins.PlaySEByName(
                    "SE_nurse_attack_upper");

                Debug.Log("�Ō�t���㔼�g�ōU��");

                break;
        }

        //MultiAudio.ins.PlaySEByName("SE_common_hit_attack");
    }

    // �����g�ł̍U����SE����
    private void AttackWithSeLower()
    {
        Debug.Log("AttackWithSeLower");
        //�����g�U��
        enemyMoveAnimation.KickStart();
        if (IsAttackableRangeAndDirection())
        {
            OnLowerAttackAnimationFinished();
        }
        //�U���҂̉����g���m�F
        switch (lowerPart.sPartsName)
        {
            case "�x�@�̉����g":
                //�x�@�������g�̍U������SE���Ȃ炷
                MultiAudio.ins.PlaySEByName(
                    "SE_policeofficer_attack_lower");

                Debug.Log("�x���������g�ōU��");
                break;

            case "�{�X�̉����g":
                //���X�{�X�����g�̍U������SE��炷
                MultiAudio.ins.PlaySEByName(
                    "SE_lastboss_attack_lower");

                Debug.Log("�{�X�������g�ōU��");

                break;

            case "�L�m�R�̉����g":
                //��l�������g�̍U������SE��炷
                MultiAudio.ins.PlaySEByName(
                    "SE_hero_attack_lower");

                Debug.Log("�L�m�R�G���������g�ōU��");
                break;

            case "�Ō�t�̉����g":
                //�i�[�X�����g�̍U������SE��炷
                MultiAudio.ins.PlaySEByName(
                    "SE_nurse_attack_lower");

                Debug.Log("�Ō�t�������g�ōU��");

                break;
        }
    }
    protected virtual void EnemyAction()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (enemyState)
        {
            case EnemyState.Search:
                enemyMoveAnimation.WalkInstance();
                // �v���C���[���ǐՔ͈͓��ɓ����Ă��邩�ǂ������f
                if (distanceToPlayer < chaseRange)
                {
                    enemyState = EnemyState.Walk;
                }
                else
                {
                    // �����̋���
                    float Target = isMovingToPointB ? pointB : pointA;
                    Vector3 target = new Vector3(Target, transform.position.y, transform.position.z);
                    // �{�X�̏ꍇ�A�ړ��͈͂𐧌�
                    if (newEnemyParameters.Boss)
                    {
                        target.x = Mathf.Clamp(target.x, bossMinX, bossMaxX);
                    }
                    MoveTowards(target, normalSpeed);

                    // �G���܂�Ԃ��n�_�ɓ��B�������ǂ������f
                    if (transform.position == target)
                    {
                        // ���B��������E
                        if (isMovingToPointB == true) enemyMoveAnimation.TurnToRight();
                        else enemyMoveAnimation.TurnToLeft();
                        isMovingToPointB = !isMovingToPointB;
                    }
                }
                break;
            case EnemyState.Walk:
                // �v���C���[��ǐ�
                enemyMoveAnimation.WalkInstance();
                MoveTowards(player.transform.position, chaseSpeed);
                if (IsLeftFromPlayer() != isMovingToPointB)
                {
                    if (isMovingToPointB == true) enemyMoveAnimation.TurnToRight();
                    else enemyMoveAnimation.TurnToLeft();
                    isMovingToPointB = !isMovingToPointB;

                }
                // �v���C���[���U���͈͓��ɓ����Ă��邩�ǂ������f
                if ((distanceToPlayer < upperPart.AttackArea || distanceToPlayer < lowerPart.AttackArea)
                    && IsLeftFromPlayer() == isMovingToPointB)
                {
                    enemyState = EnemyState.Wait;
                }
                break;
            case EnemyState.Wait:
                //moveAnimation.Upright();
                if (timer > waitTime)
                {
                    timer = 0;
                    if (IsAttackableRangeAndDirection())
                    {
                        AttackIdle();
                    }
                    else
                    {
                        enemyState = EnemyState.Search;
                    }
                    break;
                }
                timer += Time.deltaTime;
                break;
            case EnemyState.AttackIdle:
                // �\������A�j���[�V�������͉������Ȃ�
                // �A�j���[�V�����֐����ŃA�j���[�V�������EnemyState�ؑ�
                break;
            case EnemyState.Attack:
                // �㔼�g�����g�ǂ��炩����������U���͈͓��̏ꍇ
                if (distanceToPlayer <= upperPart.AttackArea && distanceToPlayer > lowerPart.AttackArea)
                {
                    AttackWithSeUpper();
                }
                else if (distanceToPlayer > upperPart.AttackArea && distanceToPlayer <= lowerPart.AttackArea)
                {
                    AttackWithSeLower();
                }
                // �ǂ�����͈͓��̏ꍇ
                else if (distanceToPlayer <= upperPart.AttackArea && distanceToPlayer <= lowerPart.AttackArea)
                {
                    // �������擾����
                    int num = Random.Range(0, 2);
                    if (num == 0)
                    {
                        AttackWithSeUpper();
                    }
                    else
                    {
                        AttackWithSeLower();
                    }
                }
                else
                {
                    // �ǂ�����͈͊O�Ȃ�͈͂̍L����
                    if (upperPart.AttackArea >= lowerPart.AttackArea)
                    {
                        AttackWithSeUpper();
                    }
                    else
                    {
                        AttackWithSeLower();
                    }
                }
                enemyState = EnemyState.AfterAttack;
                break;
            case EnemyState.AfterAttack:
                // �U���A�j���[�V�������͉������Ȃ�
                // �A�j���[�V�����֐����ŃA�j���[�V�������EnemyState�ؑ�
                break;
            case EnemyState.IsDead:
                break;
        }
    }
}

// �v���C���[�Ɍ������Ĉړ�
//�]���r(��)�̉摜���g���Ă��܂��B�{���̃I�u�W�F�N�g�ɃA�^�b�`����B
//�v���C���[(��)�̉摜���g���Ă��܂��BTag��Player�ɂȂ��Ă��Ȃ��Ɠ�����
//�e�X�g�p�Ƀv���C���[��������ƈړ����x�ς��悤�ɂ��Ă邯�ǃC���X�y�N�^�[����J�X�^���ł���

