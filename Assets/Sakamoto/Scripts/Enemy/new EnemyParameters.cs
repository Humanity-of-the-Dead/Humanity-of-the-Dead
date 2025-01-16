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
    private bool Boss;

  //  //�N���A�e�L�X�g
  //  [SerializeField]
  //private  GameObject textBox;


    //�G��HP�Q�[�W�֘A
    [SerializeField]
    private Image UpperHPBar;

    [SerializeField]
    private Image LowerHPBar;
    // HP�o�[�S�̂̃R���e�i 
    [SerializeField]
    private GameObject HPBarContainer;
    //�e�G�L�����̍ő�HP
    private int MaxUpperHP;
    private int MaxLowerHP;

    // HP�o�[��\�����鋗��
    // ���̒l�́A�G�L�����N�^�[�ƃv���C���[�̋��������͈͓̔��ɓ������Ƃ���HP�o�[��\�����邽�߂̂��̂ł��B
    // ��̓I�ɂ́A�v���C���[�ƓG�L�����N�^�[�̈ʒu�Ԃ̋����� displayRange �ɐݒ肳�ꂽ���l��菬�����ꍇ�AHP�o�[���\������܂��B
    // �t�ɁA���̋����𒴂����HP�o�[�͔�\���ɂȂ�܂��B
    // �����̒l������������ƁA�v���C���[�ɋ߂Â��Ȃ���HP�o�[���\������Ȃ��Ȃ�A
    // �@�傫������Ɖ�������ł�HP�o�[���\�������悤�ɂȂ�܂��B
    [Header("HP�o�[��\�����鋗��")]
    [Tooltip("�v���C���[�ƓG�L�����N�^�[�̋��������̒l�ȉ��̏ꍇ��HP�o�[��\�����܂��B\n" +
             "�l������������ƃv���C���[���߂Â��Ȃ���HP�o�[���\������Ȃ��Ȃ�A\n" +
             "�l��傫������Ɖ�������ł�HP�o�[���\������܂��B")]
    [SerializeField]
    private float displayRange = 0.3f;
    [Header("�G��|���Ă���HP�o�[��������܂ł̕b��")]
    [Tooltip("HP��0�̏�Ԃ�HP���\������Ă���̃J�E���g�ł��B")]
    [SerializeField]
    private float hpBarDestory = 0.3f;
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
        Debug.Log(playerControl);

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
        Vector2 tihsVec2 = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

        //�G�t�F�N�g
        GameObject obj = null;

        //�㔼�g�̏ꍇ
        if (body == 0)
        {
            //�I�u�W�F�N�g���o�����[�J�����W
            Vector2 effectVec2 = new Vector2(
                Random.Range(upperEffectXMin, upperEffectXMax),
                Random.Range(upperEffectYMin, upperEffectYMax));

            //�I�u�W�F�N�g���o��
            obj = Instantiate(hitGameObject, effectVec2 + tihsVec2, Quaternion.identity);
            // Debug.Log("effectVec2+thisVec2="+effectVec2+tihsVec2)
            // Debug.Log("hit effect");
        }

        if (body == 1)
        {
            //�I�u�W�F�N�g���o�����[�J�����W
            Vector2 effectVec2 = new Vector2(
                Random.Range(lowerEffectXMin, lowerEffectXMax),
                Random.Range(lowerEffectYMin, lowerEffectYMax));

            obj = Instantiate(hitGameObject, effectVec2 + tihsVec2, Quaternion.identity);
        }
    }


    private IEnumerator ShowHPBarAndDestroy(Image hpBar, BodyPartsData part, bool typ)
    {
        if (hpBar != null)
        {

            hpBar.gameObject.SetActive(true);
            yield return new WaitForSeconds(hpBarDestory); // �p�����Ԃ͒����\
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
        drop.transform.position = this.transform.position;

        
        //�e�L�X�g�{�b�N�X��n��
        //drop.GetComponent<newDropPart>().getTextBox(textBox);

        //�{�X�t���O��n��
        drop.GetComponent<newDropPart>().getBossf(Boss);



        //
        drop.GetComponent<newDropPart>().getPartsData(part);
        //�����̃Q�[���I�u�W�F�N�g������
        GlobalEnemyManager.Instance.RemoveEnemy(this.gameObject);
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShoot"))
        {
            TakeDamage(1, 0);
        }
    }

    //�h���b�v�̋�������ĂȂ������ʂɏo�邾���Ȃ̂Œ��߂���
    //�|���ꂽ��̂���������v���O�������K�v
    //���̎��_���Ɨ����h���b�v���Ă��܂��̂ŏC������
    //����Image�����邱�ƂɂȂ��Ă邯�ǁA������Sprite������悤�ɂ�����

    //���̃v���O�����̓������e�X�g�p�ɉ�������

    //�_���[�W��get��set

}