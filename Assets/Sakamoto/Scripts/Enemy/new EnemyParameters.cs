using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    //�{�X�t���O
    [SerializeField, Header("�{�X���ǂ����A�`�F�b�N�������Ă���Ȃ�{�X")]
    public bool Boss;

    [SerializeField,Header("�G���ł��Ă����̒e�̃_���[�W��")] float bulletDamage ;    //  //�N���A�e�L�X�g
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

        playerControl = GameObject.Find("Player Variant").GetComponent<PlayerControl>();
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

        // ���ʂ��j�󂳂ꂽ�ۂ�HP�o�[����u�\��
        if (UpperHP <= 0)
        {
            playerControl.RemoveListItem(this.gameObject);
            //Debug.Log("�㔼�g���j�󂳂ꂽ");
            //Drop(Upperbodypart, false);
            StartCoroutine(ShowHPBarAndDestroy(UpperHPBar, Lowerbodypart, false));

        }
        if (LowerHP <= 0)
        {
            playerControl.RemoveListItem(this.gameObject);
            //Debug.Log("�����g���j�󂳂ꂽ");
            //Drop(Lowerbodypart, true);
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
            ShowHitEffects(body);
            Debug.Log(hitGameObject);

            UpdateHPBar(UpperHPBar, UpperHP, MaxUpperHP);
            MultiAudio.ins.PlaySEByName("SE_common_hit_attack");

            Debug.Log(UpperHP);
            Debug.Log(MaxUpperHP);

        }

        if (body == 1)
        {
            //�����g��HP�����炷
            LowerHP -= (int)damage;
            ShowHitEffects(body);
            Debug.Log(hitGameObject);
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
    //�U�����q�b�g�����G�t�F�N�g(�I�u�W�F�N�g)���o��
    public override void ShowHitEffects(int body)
    {
        //���̃I�u�W�F�N�g�̍��W
        Vector3 enemyVector3 = new Vector3(transform.position.x,transform.position.y);

        

        //�㔼�g�̏ꍇ
        if (body == 0)
        {
            //�I�u�W�F�N�g���o�����[�J�����W
            Vector3 effectVec2Upper = new Vector3(
                Random.Range(upperEffectXMin, upperEffectXMax),
                Random.Range(upperEffectYMin, upperEffectYMax));

            //�I�u�W�F�N�g���o��
            Instantiate(hitGameObject, effectVec2Upper + enemyVector3, Quaternion.identity);
            // Debug.Log("effectVec2+thisVec2="+effectVec2+tihsVec2)
            // Debug.Log("hit effect");
        }

        if (body == 1)
        {
            //�I�u�W�F�N�g���o�����[�J�����W
            Vector3 effectVec3Lower = new Vector2(
                Random.Range(lowerEffectXMin, lowerEffectXMax),
                Random.Range(lowerEffectYMin, lowerEffectYMax));

            Instantiate(hitGameObject, effectVec3Lower + enemyVector3, Quaternion.identity);
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
        }
        else
        {
            //�v���n�u���C���X�^���X��
            drop = Instantiate(preLowerPart);
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
            TakeDamage(bulletDamage, 0);
        }
    }

    //�h���b�v�̋�������ĂȂ������ʂɏo�邾���Ȃ̂Œ��߂���
    //�|���ꂽ��̂���������v���O�������K�v
    //���̎��_���Ɨ����h���b�v���Ă��܂��̂ŏC������
    //����Image�����邱�ƂɂȂ��Ă邯�ǁA������Sprite������悤�ɂ�����

    //���̃v���O�����̓������e�X�g�p�ɉ�������

    //�_���[�W��get��set

}