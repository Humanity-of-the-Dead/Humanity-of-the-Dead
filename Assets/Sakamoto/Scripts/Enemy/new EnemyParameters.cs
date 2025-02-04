using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class newEnemyParameters : CharacterStats
{
    //���ʂ̑ϋv�l��ݒ�ł���
    [SerializeField, Header("�G�̏㔼�gHP")]
    private int UpperHP;
    [SerializeField, Header("�G�̉����gHP")]
    private int LowerHP;



    //�{�f�B�p�[�c
    [SerializeField, Header("�G�̏㔼�g�f�[�^")]
    private BodyPartsData Upperbodypart;

    [SerializeField, Header("�G�̉����g�f�[�^")]
    private BodyPartsData Lowerbodypart;

    //�㔼�g�̃h���b�v�p�[�c
    [SerializeField, Header("�G�̏㔼�g�h���b�v�p�[�c")]
    private GameObject preUpperPart;

    //�����g�̃h���b�v�p�[�c
    [SerializeField, Header("�G�̉����g�h���b�v�p�[�c")]
    private GameObject preLowerPart;


    [SerializeField, Header("�v���C���[�R���g���[���̃X�N���v�g���\n�����œ��邽�߉�������Ȃ�")]

    //�v���C���[�R���g���[��
    public PlayerControl playerControl;

    private PlayerMoveAnimation playerMoveAnimation;

    //�{�X�t���O
    [SerializeField, Header("�{�X���ǂ����A�`�F�b�N�������Ă���Ȃ�{�X")]
    public bool Boss;

    [SerializeField, Header("�����ʂւ̍U�����K�v���H�`�F�b�N�������Ă���Ȃ�K�v")]
    public bool needsAttackingBothParts;

    [SerializeField, Header("�G���ł��Ă����̒e�̃_���[�W��")] float bulletDamage;    //  //�N���A�e�L�X�g
                                                                         //  [SerializeField]
                                                                         //private  GameObject textBox;



    //�G��HP�Q�[�W�֘A
    [SerializeField, Header("�G�̏㔼�g�Q�[�W�̃C���[�W�R���|�[�l���g��ݒ�\n�K��UpperHP_Bar�������Ă��邱�Ƃ��m�F")]
    private Image UpperHPBar;

    [SerializeField, Header("�G�̉����g�Q�[�W�̃C���[�W�R���|�[�l���g��ݒ�\n�K��LowerHP_Bar�������Ă��邱�Ƃ��m�F")]
    private Image LowerHPBar;
    // HP�o�[�S�̂̃R���e�i 
    [SerializeField, Header("�G�̃Q�[�W�̃I�u�W�F�N�g��ݒ�\n�K��HPBar_Object�������Ă��邱�Ƃ��m�F")]
    private GameObject HPBarContainer;
    //�e�G�L�����̍ő�HP
    private int MaxUpperHP;
    private int MaxLowerHP;

    [SerializeField, Header("HP�o�[��\�����鋗��")]
    [Tooltip("�v���C���[�ƓG�L�����N�^�[�̋��������̒l�ȉ��̏ꍇ��HP�o�[��\�����܂��B\n" +
             "�l������������ƃv���C���[���߂Â��Ȃ���HP�o�[���\������Ȃ��Ȃ�A\n" +
             "�l��傫������Ɖ�������ł�HP�o�[���\������܂��B")]
    private float displayRange = 0.3f;
    [SerializeField, Header("�G��|���Ă���HP�o�[��������܂ł̕b��")]
    [Tooltip("HP��0�̏�Ԃ�HP���\������Ă���̃J�E���g�ł��B")]
    private float hpBarDestroy = 0.3f;
    //private Transform player; // �v���C���[�̈ʒu
    //�_�ŃG�t�F�N�g
    private Renderer[] enemyRenderer;
    // �_�Ŏ���[s]
    [SerializeField] private float enemyCycle = 1;
    // ���ł̃f���[�e�B��(1�Ŋ��S��ON�A0�Ŋ��S��OFF)
    [SerializeField, Range(0, 1)] private float _dutyRate = 0.5f;

    private double _time;
    //public bool isDropInstantiated = false;
    private void Start()
    {
        MaxLowerHP = LowerHP;
        MaxUpperHP = UpperHP;
        // HP�o�[���\���ɐݒ�
        if (HPBarContainer != null)
        {
            HPBarContainer.SetActive(false);
        }
        else
        {
            Debug.LogWarning("HPBarContainer��null");
        }
        enemyRenderer = transform.GetComponentsInChildren<Renderer>();
        playerControl = GameObject.Find("Player Variant").GetComponent<PlayerControl>();
        playerMoveAnimation = playerControl.GetComponent<PlayerMoveAnimation>();
        //Debug.Log(playerControl);

    }


    void Update()
    {
        float DistanceToPlayer = Vector3.Distance(transform.position, playerControl.transform.position);
        // �v���C���[����苗���ȓ��ɂ���ꍇ��HP�o�[��\��playerControl
        if (DistanceToPlayer < displayRange)
        {
            if (HPBarContainer != null)
            {
                HPBarContainer.SetActive(true);
            }
        }
        else
        {
            if (HPBarContainer != null)
            {
                HPBarContainer.SetActive(false);
            }
        }

        AdjustHpIfNeededAttackingBothParts();

        // ���ʂ��j�󂳂ꂽ�ۂ�HP�o�[����u�\��
        if (UpperHP <= 0)
        {

            playerControl.RemoveListItem(this.gameObject);
            playerMoveAnimation.ShowHitEffects(0, transform.position, true);
            FlashObject();

            //Debug.Log("�㔼�g���j�󂳂ꂽ");
            //Drop(Upperbodypart, false);
            StartCoroutine(ShowHPBarAndDestroy(UpperHPBar, Lowerbodypart, false));



        }
        if (LowerHP <= 0)
        {

            playerControl.RemoveListItem(this.gameObject);
            playerMoveAnimation.ShowHitEffects(1, transform.position, true);

            //Debug.Log("�����g���j�󂳂ꂽ");
            //Drop(Lowerbodypart, true);
            FlashObject();

            StartCoroutine(ShowHPBarAndDestroy(LowerHPBar, Upperbodypart, true));




        }
        //if (GameMgr.GetState() == GameState.ShowText&&!Boss)
        //{
        //    Destroy(this.gameObject);   
        //}

    }


    //body�ɂ�0��1��������Ă͂����Ȃ��@BA//GU/RU
    //body : 0->�㔼�g�Ƀ_���[�W
    //body : 1->�����g�Ƀ_���[�W

    public override void TakeDamage(float damage, int body = 0)
    {
        //HP������d�g��
        //damage�̓e�X�g�p�̊֐�
        if (body == 0)
        {
            //�㔼�g��HP�����炷
            UpperHP -= (int)damage;
            //ShowHitEffects(body);
            playerMoveAnimation.ShowHitEffects(body, transform.position);

            UpdateHPBar(UpperHPBar, UpperHP, MaxUpperHP);
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");

            //Debug.Log(UpperHP);
            //Debug.Log(MaxUpperHP);

        }

        if (body == 1)
        {
            //�����g��HP�����炷
            LowerHP -= (int)damage;
            //ShowHitEffects(body);
            playerMoveAnimation.ShowHitEffects(body, transform.position);
            UpdateHPBar(LowerHPBar, LowerHP, MaxLowerHP);
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");

            Debug.Log(LowerHP);
            Debug.Log(MaxLowerHP);
        }
    }
    //�G��HP�o�[��ύX
    private void UpdateHPBar(Image hpBarMask, float currentHP, float maxHP)
    {
        if (hpBarMask != null)
        {
            // Fill Amount�����݂�HP�䗦�ɍX�V
            hpBarMask.fillAmount = currentHP / maxHP;
        }
    }

    private void FlashObject()
    {
        _time += Time.deltaTime;

        // ����cycle�ŌJ��Ԃ��l�̎擾
        // 0�`cycle�͈̔͂̒l��������
        var repeatValue = Mathf.Repeat((float)_time, enemyCycle);
        bool currentState = enemyRenderer.Length > 0 && enemyRenderer[0].enabled; // �ŏ���Renderer�̏�Ԃ����
        foreach (Renderer r in enemyRenderer)
        {

            // ��������time�ɂ����閾�ŏ�Ԃ𔽉f
            // �f���[�e�B���ON/OFF�̊�����ύX���Ă���
            r.enabled = !currentState; // ���ׂĂ�Renderer�̕\����Ԃ𔽓]
        }
    }

    private IEnumerator ShowHPBarAndDestroy(Image hpBar, BodyPartsData part, bool typ)
    {
        if (hpBar != null)
        {

            hpBar.gameObject.SetActive(true);
            yield return new WaitForSeconds(hpBarDestroy); // �p�����Ԃ͒����\
            hpBar.gameObject.SetActive(false);
        }
        Drop(part, typ);
        MultiAudio.ins.PlaySEByName("SE_common_breakbody");

    }
    //�h���b�v�A�C�e���𐶐�����֐��@
    //BodyPartsData part->����������ɗ^����p�����[�^�f�[�^
    //int typ->true�Ȃ�㔼�g��������:false�Ȃ牺���g��������
    //�f�t�H���g������true
    public void Drop(BodyPartsData part, bool typ = true)
    {
        GameObject drop = null;
        if (typ == true)
        {
            //�v���n�u���C���X�^���X��
            drop = Instantiate(preUpperPart);
            //isDropInstantiated = true;

        }
        else
        {
            //�v���n�u���C���X�^���X��
            drop = Instantiate(preLowerPart);
            //isDropInstantiated = true;

        }

        //���������p�[�c�����g�̏ꏊ�Ɏ����Ă���
        drop.transform.position = transform.position;




        //�{�X�t���O��n��
        drop.GetComponent<newDropPart>().getBossf(Boss);



        //
        drop.GetComponent<newDropPart>().getPartsData(part);
        //�����̃Q�[���I�u�W�F�N�g������
        GlobalEnemyManager.Instance.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShoot"))
        {
            bulletDamage = (float)collision.gameObject.GetComponent<Bullet>().attack;
            TakeDamage(bulletDamage, 0);
        }
    }

    // �����ʂւ̍U�����K�v�ȏꍇ��HP�̒������s�Ȃ�
    private void AdjustHpIfNeededAttackingBothParts()
    {
        if (needsAttackingBothParts)
        {
            // ����HP��1�̎��A���̕��ʂ�HP�����؂������̂Ƃ���
            // �㔼�gHP��0�ɂȂ������A�������gHP�����؂��Ă��Ȃ��Ƃ�
            if (UpperHP <= 0 && LowerHP > 1)
            {
                // HP��j���O�Ŏ~�߂�
                UpperHP = 1;
            }
            // �����g
            else if (LowerHP <= 0 && UpperHP > 1)
            {
                // HP��j���O�Ŏ~�߂�
                LowerHP = 1;
            }
        }
    }

    //�h���b�v�̋�������ĂȂ������ʂɏo�邾���Ȃ̂Œ��߂���
    //�|���ꂽ��̂���������v���O�������K�v
    //���̎��_���Ɨ����h���b�v���Ă��܂��̂ŏC������
    //����Image�����邱�ƂɂȂ��Ă邯�ǁA������Sprite������悤�ɂ�����

    //���̃v���O�����̓������e�X�g�p�ɉ�������

    //�_���[�W��get��set

}