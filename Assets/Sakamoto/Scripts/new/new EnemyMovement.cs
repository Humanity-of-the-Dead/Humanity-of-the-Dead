using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class newEnemyMovement : EnemyAttack
{
    // �ړ����n�߂�ꏊ�A�I���̏ꏊ�A���i�̈ړ����x�A�ǐՒ��̈ړ����x�A�G�̍��G�\�Ȕ͈͂�ݒ�
    [SerializeField] private float pointA;
    [SerializeField] private float pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private BodyPartsData part;

    [SerializeField] EnemyMoveAnimation moveAnimation;

    enum EnemyState 
    {
        search,
        walk,
        attack,
        wait,
    }

    EnemyState enemystate;

    private bool movingToPointB = true; // �i�s����
    private Transform player; // �v���C���[�̈ʒu

    [SerializeField] GameMgr gamestate;

    void Start()
    {
        // �v���C���[��T�����
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // �v���C���[�Ƃ̋������v�Z
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        switch (gamestate.enGameState)
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
                                if (movingToPointB == true) moveAnimation.RightMove();
                                else moveAnimation.LeftMove();
                                movingToPointB = !movingToPointB;
                            }
                        }
                        break;
                    case EnemyState.walk:
                        // �v���C���[��ǐ�
                        MoveTowards(player.position, chaseSpeed);
                        // �v���C���[���U���͈͓��ɓ����Ă��邩�ǂ������f
                        if (distanceToPlayer < attackRange)
                        {
                            enemystate = EnemyState.attack;
                        }
                        break;
                    case EnemyState.attack:
                        UpperEnemyAttack((float)part.iPartAttack);
                        break;
                    case EnemyState.wait:
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
}
// �v���C���[�Ɍ������Ĉړ�
//�]���r(��)�̉摜���g���Ă��܂��B�{���̃I�u�W�F�N�g�ɃA�^�b�`����B
//�v���C���[(��)�̉摜���g���Ă��܂��BTag��Player�ɂȂ��Ă��Ȃ��Ɠ�����
//�e�X�g�p�Ƀv���C���[��������ƈړ����x�ς��悤�ɂ��Ă邯�ǃC���X�y�N�^�[����J�X�^���ł���

