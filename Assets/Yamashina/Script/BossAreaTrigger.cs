
using UnityEngine;

public class BossAreaTrigger : MonoBehaviour
{
    private PlayerControl targetPosition;


    // ���e�͈́i�^�[�Q�b�g�ʒu�Ƃ̋����j
    [SerializeField,Header("�v���C���[���{�X�O�̃G���A�n�_�ɂǂꂾ���߂Â����瓞�B�Ƃ݂Ȃ��������߂�l")] private float threshold = 1.0f;
    private bool isDelayedStateChange = false;

    private void Start()
    {
        targetPosition = GameObject.Find("Player Variant").GetComponent<PlayerControl>();

    }
    void Update()
    {
        //Debug.Log(IsPlayerAtTarget());
        // �v���C���[���w��̈ʒu�ɓ��B���������`�F�b�N
        if (GameMgr.GetState() == GameState.Main && IsPlayerAtTarget())
        {
            GameMgr.ChangeState(GameState.BeforeBoss);
        }
    }

    // �v���C���[���^�[�Q�b�g�ʒu�ɓ��B���Ă��邩�𔻒�
    private bool IsPlayerAtTarget()
    {
        float distance = Vector3.Distance(new Vector2(transform.position.x, transform.position.z),
    new Vector2(targetPosition.transform.position.x, targetPosition.transform.position.z));
        //Debug.Log(transform.position + "�{�X�O�G���A ");
        //Debug.Log(targetPosition.transform.position + "�v���C���[�̈ʒu " );

        return distance <= threshold;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameMgr.ChangeState(GameState.BeforeBoss); // �{�X�풼�O�ɏ�ԕύX
        }
    }
}
