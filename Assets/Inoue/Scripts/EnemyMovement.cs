using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyMovement : EnemyAttack
{
    // �ړ����n�߂�ꏊ�A�I���̏ꏊ�A���i�̈ړ����x�A�ǐՒ��̈ړ����x�A�G�̍��G�\�Ȕ͈͂�ݒ�
    [Header("�ړ��̐�Βl")]
    [SerializeField] float moveAbs;
    private float pointA;
    private float pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private BodyPartsData upperpart;
    [SerializeField] private BodyPartsData lowerpart;

    [SerializeField] EnemyMoveAnimation moveAnimation;

    enum EnemyState
    {
        search,
        walk,
        attack,
        wait,
    }

    EnemyState enemystate;

    private bool movingToPointB = false; // �i�s����
    private Transform player; // �v���C���[�̈ʒu

    public GameMgr gamestate;

    private float timer;
    [SerializeField] float waitTime; //�U����̌㌄

    void Start()
    {
        // �v���C���[��T�����
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pointA = this.transform.position.x + moveAbs;
        pointB = this.transform.position.x - moveAbs;

    }

    void Update()
    {
        // �v���C���[�Ƃ̋������v�Z
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        switch (GameMgr.GetState())
        {
            case GameState.Main:
                switch (enemystate)
                {
                    case EnemyState.search:
                        // �v���C���[���ǐՔ͈͓��ɓ����Ă��邩�ǂ������f
                        if (distanceToPlayer < chaseRange)
                        {
                            enemystate = EnemyState.walk;
                        }
                        else
                        {
                            // �����̋���
                            float Target = movingToPointB ? pointB : pointA;
                            Vector3 target = new Vector3(Target, this.transform.position.y, this.transform.position.z);
                            MoveTowards(target, speed);

                            // �G���܂�Ԃ��n�_�ɓ��B�������ǂ������f
                            if (transform.position == target)
                            {
                                // ���B��������E
                                if (movingToPointB == true) moveAnimation.TurnToRight();
                                else moveAnimation.TurnToLeft();
                                movingToPointB = !movingToPointB;
                            }
                        }
                        break;
                    case EnemyState.walk:
                        // �v���C���[��ǐ�
                        MoveTowards(player.position, chaseSpeed);
                        // �v���C���[���U���͈͓��ɓ����Ă��邩�ǂ������f
                        if (distanceToPlayer < upperpart.AttackArea || distanceToPlayer < lowerpart.AttackArea)
                        {
                            enemystate = EnemyState.wait;
                        }
                        break;
                    case EnemyState.attack:
                        if (distanceToPlayer < upperpart.AttackArea)
                        {
                            moveAnimation.PantieStart();
                            Debug.Log("�p���`");
                            UpperEnemyAttack((float)upperpart.iPartAttack * 0.1f);
                        }
                        else if (distanceToPlayer < lowerpart.AttackArea)
                        {
                            LowerEnemyAttack((float)lowerpart.iPartAttack * 0.1f);

                        }
                        enemystate = EnemyState.search;
                        //moveAnimation.PlayerPantie();
                        break;
                    case EnemyState.wait:
                        Debug.Log("����");
                        if (timer > waitTime)
                        {
                            timer = 0;
                            enemystate = EnemyState.attack;
                            break;
                        }
                        timer += Time.deltaTime;
                        break;
                }



                break;
            case GameState.ShowText:
                break;


        }
    }
    private void MoveTowards(Vector3 target, float moveSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("�Փ˃C�x���g���m");
        Debug.Log(collision.gameObject.tag);
        Debug.Log(enemystate);
        if (collision.gameObject.CompareTag("Enemy") && enemystate == EnemyState.search)
        {
            Debug.Log("�G���m���Փ˂��A���E");
            if (movingToPointB)
            {
                moveAnimation.TurnToRight();
            }
            else
            {
                moveAnimation.TurnToLeft();
            }
            movingToPointB = !movingToPointB;
        }
    }
}
// �v���C���[�Ɍ������Ĉړ�
//�]���r(��)�̉摜���g���Ă��܂��B�{���̃I�u�W�F�N�g�ɃA�^�b�`����B
//�v���C���[(��)�̉摜���g���Ă��܂��BTag��Player�ɂȂ��Ă��Ȃ��Ɠ�����
//�e�X�g�p�Ƀv���C���[��������ƈړ����x�ς��悤�ɂ��Ă邯�ǃC���X�y�N�^�[����J�X�^���ł���


