using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class newEnemyMovement : EnemyAttack
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
    [SerializeField] private Gun Juu;

    
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
        player = GameObject.Find("Player Variant").gameObject.transform;
        pointA = this.transform.position.x + moveAbs;
        pointB = this.transform.position.x - moveAbs;

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
                        moveAnimation.WalkInstance();
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
                        if(PlayerPositionFromEnemy() != movingToPointB)
                        {
                            if (movingToPointB == true) moveAnimation.RightMove();
                            else moveAnimation.LeftMove();
                            movingToPointB = !movingToPointB;

                        }
                        // �v���C���[���U���͈͓��ɓ����Ă��邩�ǂ������f
                        if ((distanceToPlayer < upperpart.AttackArea || distanceToPlayer < lowerpart.AttackArea)
                            && PlayerPositionFromEnemy() == movingToPointB)
                        {
                            enemystate = EnemyState.wait;
                        }
                        break;
                    case EnemyState.wait:
                        //moveAnimation.Upright();
                        if (timer > waitTime)
                        {
                            timer = 0;
                            if((distanceToPlayer < upperpart.AttackArea || distanceToPlayer < lowerpart.AttackArea) && PlayerPositionFromEnemy() == movingToPointB)
                            {
                                enemystate = EnemyState.attack;
                            }
                            else
                            {
                                enemystate = EnemyState.search;
                            }
                            break;
                        }
                        timer += Time.deltaTime;
                        break;
                    case EnemyState.attack:
                        if (distanceToPlayer < upperpart.AttackArea && distanceToPlayer < lowerpart.AttackArea && PlayerPositionFromEnemy() == movingToPointB)
                        {
                            //�������擾����
                            int num = UnityEngine.Random.Range(0, 2);
                            if (num == 0)
                            {
                                //�㔼�g�U��
                                moveAnimation.PantieStart();
                                if(upperpart.sPartsName == "�x�@�̏㔼�g")
                                {
                                    Vector2 ShootMoveBector = new Vector2(0, 0);
                                    //�q��playerRC�̃��[�e�[�V����Y�������Ă���
                                    // y = 0�̂Ƃ��͉E�����A0 y = 180�̂Ƃ��͍�����
                                    Debug.Log(this.gameObject.transform.GetChild(0).gameObject.transform.eulerAngles.y);
                                    if (this.gameObject.transform.GetChild(0).gameObject.transform.eulerAngles.y == 180)
                                    {
                                        ShootMoveBector.x = -1;
                                    }
                                    else
                                    {
                                        ShootMoveBector.x = 1;
                                    }

                                    Debug.Log(ShootMoveBector);
                                    Juu.Shoot(ShootMoveBector, this.transform);

                                    return;
                                }
                                UpperEnemyAttack((float)upperpart.iPartAttack);
                                MultiAudio.ins.PlaySEByName("SE_common_hit_attack");
                            }
                            if (num == 1)
                            {
                                //�����g�U��
                                moveAnimation.KickStart();
                                LowerEnemyAttack((float)lowerpart.iPartAttack);
                                MultiAudio.ins.PlaySEByName("SE_common_hit_attack");
                            }
                        }
                        if (distanceToPlayer < upperpart.AttackArea && PlayerPositionFromEnemy() == movingToPointB)
                        {
                            moveAnimation.PantieStart();
                            UpperEnemyAttack((float)upperpart.iPartAttack * 0.1f);
                            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");
                        }
                        if (distanceToPlayer < lowerpart.AttackArea && PlayerPositionFromEnemy() == movingToPointB)
                        {
                            moveAnimation.KickStart();
                            LowerEnemyAttack((float)lowerpart.iPartAttack * 0.1f);
                            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");
                        }
                        enemystate = EnemyState.search;
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
                moveAnimation.RightMove();
            }
            else
            {
                moveAnimation.LeftMove();
            }
            movingToPointB = !movingToPointB;
        }
    }

    //PlayerPositionFromEnemy�E�����Ă���{�A�������Ă���[
    bool PlayerPositionFromEnemy() 
    {
        float Direction = player.position.x - this.gameObject.transform.position.x;
        if (Direction < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
// �v���C���[�Ɍ������Ĉړ�
//�]���r(��)�̉摜���g���Ă��܂��B�{���̃I�u�W�F�N�g�ɃA�^�b�`����B
//�v���C���[(��)�̉摜���g���Ă��܂��BTag��Player�ɂȂ��Ă��Ȃ��Ɠ�����
//�e�X�g�p�Ƀv���C���[��������ƈړ����x�ς��悤�ɂ��Ă邯�ǃC���X�y�N�^�[����J�X�^���ł���

