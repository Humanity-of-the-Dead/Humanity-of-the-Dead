using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // �ړ����n�߂�ꏊ�A�I���̏ꏊ�A���i�̈ړ����x�A�ǐՒ��̈ړ����x�A�G�̍��G�\�Ȕ͈͂�ݒ�
    [SerializeField] private Vector3 pointA;
    [SerializeField] private Vector3 pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float chaseRange = 5f;

    private bool movingToPointB = true; // �i�s����
    private Transform player; // �v���C���[�̈ʒu

    void Start()
    {
        // �v���C���[��T�����
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // �v���C���[�Ƃ̋������v�Z
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �v���C���[���ǐՔ͈͓��ɓ����Ă��邩�ǂ������f
        if (distanceToPlayer < chaseRange)
        {
            // �v���C���[��ǐ�
            MoveTowards(player.position, chaseSpeed);
        }
        else
        {
            // �����̋���
            Vector3 target = movingToPointB ? pointB : pointA;
            MoveTowards(target, speed);

            // �G���܂�Ԃ��n�_�ɓ��B�������ǂ������f
            if (transform.position == target)
            {
                // ���B��������E
                movingToPointB = !movingToPointB;
            }
        }
    }

    // �v���C���[�Ɍ������Ĉړ�
    private void MoveTowards(Vector3 target, float moveSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }
}

//�]���r(��)�̉摜���g���Ă��܂��B�{���̃I�u�W�F�N�g�ɃA�^�b�`����B
//�v���C���[(��)�̉摜���g���Ă��܂��BTag��Player�ɂȂ��Ă��Ȃ��Ɠ�����
//�e�X�g�p�Ƀv���C���[��������ƈړ����x�ς��悤�ɂ��Ă邯�ǃC���X�y�N�^�[����J�X�^���ł���

