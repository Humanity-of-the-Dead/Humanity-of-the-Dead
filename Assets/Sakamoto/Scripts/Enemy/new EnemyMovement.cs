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
    [Tooltip("�e�L�����N�^�[�������Ă���㔼�g�p�[�c")]

    [SerializeField] private BodyPartsData upperPart;
    [Tooltip("�e�L�����N�^�[�������Ă��鉺���g�p�[�c")]

    [SerializeField] private BodyPartsData lowerPart;
    private Gun gun;

    private newEnemyParameters newEnemyParameters;
    private EnemyMoveAnimation enemyMoveAnimation;

    private float pointA, pointB;//�J�n�ʒu�ƏI���ʒu


    private enum EnemyState { Search, Walk, Attack, Wait }

    private EnemyState enemyState = EnemyState.Search;

    private bool movingToPointB = false; // �i�s����
    private PlayerControl player; // �v���C���[�̈ʒu
    private float timer;//�U����̎���
    [Tooltip("�{�X�̈ړ��\�ȍŏ�X���W")]
    [SerializeField] private float bossMinX = -8f;
    [Tooltip("�{�X�̈ړ��\�ȍő�X���W")]
    [SerializeField] private float bossMaxX = 8f;
    void Start()
    {
        // �v���C���[��T�����
        player = GameObject.Find("Player Variant").GetComponent<PlayerControl>();
        enemyMoveAnimation = GetComponent<EnemyMoveAnimation>();
        pointA = transform.position.x + moveDistance;
        pointB = transform.position.x - moveDistance;
        newEnemyParameters = GetComponent<newEnemyParameters>();
        if (upperPart.sPartsName == "�x�@�̏㔼�g")
        {
            gun = GetComponent<Gun>();
        }
       
    }

    void Update()
    {
        // �v���C���[�Ƃ̋������v�Z
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log(distanceToPlayer.ToString());
        switch (GameMgr.GetState())
        {
            case GameState.Main:
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
                            float Target = movingToPointB ? pointB : pointA;
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
                                if (movingToPointB == true) enemyMoveAnimation.RightMove();
                                else enemyMoveAnimation.LeftMove();
                                movingToPointB = !movingToPointB;
                            }
                        }
                        break;
                    case EnemyState.Walk:
                        // �v���C���[��ǐ�
                        MoveTowards(player.transform.position, chaseSpeed);
                        if (PlayerPositionFromEnemy() != movingToPointB)
                        {
                            if (movingToPointB == true) enemyMoveAnimation.RightMove();
                            else enemyMoveAnimation.LeftMove();
                            movingToPointB = !movingToPointB;

                        }
                        // �v���C���[���U���͈͓��ɓ����Ă��邩�ǂ������f
                        if ((distanceToPlayer < upperPart.AttackArea || distanceToPlayer < lowerPart.AttackArea)
                            && PlayerPositionFromEnemy() == movingToPointB)
                        {
                            enemyState = EnemyState.Wait;
                        }
                        break;
                    case EnemyState.Wait:
                        //moveAnimation.Upright();
                        if (timer > waitTime)
                        {
                            timer = 0;
                            if ((distanceToPlayer < upperPart.AttackArea || distanceToPlayer < lowerPart.AttackArea) && PlayerPositionFromEnemy() == movingToPointB)
                            {
                                enemyState = EnemyState.Attack;
                            }
                            else
                            {
                                enemyState = EnemyState.Search;
                            }
                            break;
                        }
                        timer += Time.deltaTime;
                        break;
                    case EnemyState.Attack:
                        if (distanceToPlayer < upperPart.AttackArea || distanceToPlayer < lowerPart.AttackArea &&
                            PlayerPositionFromEnemy() == movingToPointB)
                        {
                            //�������擾����
                            int num = Random.Range(0, 2);
                            if (num == 0)
                            {
                                //�㔼�g�U��
                                enemyMoveAnimation.PantieStart();
                                OnUpperAttackAnimationFinished();

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
                                        gun.Shoot(ShootMoveVector, transform);
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

                                    case "�G���G�̏㔼�g":
                                        //��l���㔼�g�̍U������SE��炷
                                        MultiAudio.ins.PlaySEByName(
                                            "SE_hero_attack_upper");

                                        Debug.Log("�G���G���㔼�g�ōU��");

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
                            if (num == 1)
                            {
                                //�����g�U��
                                enemyMoveAnimation.KickStart();
                                OnLowerAttackAnimationFinished();
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

                                    case "�G���G�̉����g":
                                        //��l�������g�̍U������SE��炷
                                        MultiAudio.ins.PlaySEByName(
                                            "SE_hero_attack_lower");

                                        Debug.Log("�G���������g�ōU��");
                                        break;

                                    case "�Ō�t�̉����g":
                                        //�i�[�X�����g�̍U������SE��炷
                                        MultiAudio.ins.PlaySEByName(
                                            "SE_nurse_attack_lower");

                                        Debug.Log("�Ō�t�������g�ōU��");

                                        break;
                                }

                            }
                        }

                        enemyState = EnemyState.Search;
                        //moveAnimation.PlayerPantie();
                        break;

                }



                break;
            case GameState.ShowText:
                break;


        }
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
            if (movingToPointB)
            {
                enemyMoveAnimation?.RightMove();
            }
            else
            {
                enemyMoveAnimation?.LeftMove();
            }
            movingToPointB = !movingToPointB;
        }
    }

    //PlayerPositionFromEnemy�E�����Ă���{�A�������Ă���[
    bool PlayerPositionFromEnemy()
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
        UpperEnemyAttack((float)lowerPart.iPartAttack);

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
}
// �v���C���[�Ɍ������Ĉړ�
//�]���r(��)�̉摜���g���Ă��܂��B�{���̃I�u�W�F�N�g�ɃA�^�b�`����B
//�v���C���[(��)�̉摜���g���Ă��܂��BTag��Player�ɂȂ��Ă��Ȃ��Ɠ�����
//�e�X�g�p�Ƀv���C���[��������ƈړ����x�ς��悤�ɂ��Ă邯�ǃC���X�y�N�^�[����J�X�^���ł���

