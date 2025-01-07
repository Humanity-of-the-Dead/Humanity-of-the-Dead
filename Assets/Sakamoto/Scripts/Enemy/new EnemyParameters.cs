using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class newEnemyParameters : MonoBehaviour
{
    //���ʂ̑ϋv�l��ݒ�ł���
    [SerializeField]
    private int UpperHP;

    [SerializeField]
    private int LowerHP;

    //�e�X�g�p�@�G�ɗ^����_���[�W��ݒ�ł���
    [SerializeField]
    private int damage;

    ////�h���b�v����摜��ݒ�ł���
    //[SerializeField]
    //private Image deathImage;

    //�{�f�B�p�[�c
    [SerializeField]
    private BodyPartsData Upperbodypart;

    [SerializeField]
    private BodyPartsData Lowerbodypart;

    //�㔼�g�̃h���b�v�p�[�c
    [SerializeField]
    private GameObject preUpperPart;

    //�����g�̃h���b�v�p�[�c
    [SerializeField]
    private GameObject preLowerPart;

    GameObject drop;

    //�v���C���[�p�����[�^-
    public PlayerParameter scPlayerParameter;
    //�v���C���[�R���g���[��
    public GameObject PlayerControl;

    //�{�X�t���O
    [SerializeField]
    bool Boss;

    //�N���A�e�L�X�g
    [SerializeField]
    GameObject textBox;

    [SerializeField] SceneTransitionManager sceneTransitionManager;

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
    private float displayRange = 5f;
    [Header("HP�o�[��\�����Ă���A")]
    [Tooltip("�v���C���[�ƓG�L�����N�^�[�̋��������̒l�ȉ��̏ꍇ��HP�o�[��\�����܂��B\n" +
            "�l������������ƃv���C���[���߂Â��Ȃ���HP�o�[���\������Ȃ��Ȃ�A\n" +
            "�l��傫������Ɖ�������ł�HP�o�[���\������܂��B")]
    [SerializeField]
    private float hpBarDestory = 0.3f;
    private Transform player; // �v���C���[�̈ʒu

    //�q�b�g�G�t�F�N�g
    [SerializeField] GameObject hitGameObject;
    //�G�t�F�N�g�̏o���͈�
    //�㔼�g
    [SerializeField] float upperEffectXMin, upperEffectXMax, upperEffectYMin, upperEffectYMax;

    //�����g
    [SerializeField] float lowerEffectXMin, lowerEffectXMax, lowerEffectYMin, lowerEffectYMax;
    //\�G�t�F�N�g�E�E�E

    private void Start()
    {
        player = GameObject.Find("Player Variant").gameObject.transform;
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
        scPlayerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();
        sceneTransitionManager = GameObject.FindAnyObjectByType<SceneTransitionManager>();
    }


    void Update()
    {
        float DistanceToPlayer = Vector3.Distance(transform.position, player.position);
        // �v���C���[����苗���ȓ��ɂ���ꍇ��HP�o�[��\��
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
            PlayerControl.GetComponent<PlayerControl>().RemoveListItem(this.gameObject);
            Debug.Log("�㔼�g���j�󂳂ꂽ");
            //Drop(Upperbodypart, false);
            MultiAudio.ins.PlaySEByName("SE_common_breakbody");
            StartCoroutine(ShowHPBarAndDestroy(UpperHPBar, Upperbodypart, true));
        }
        if (LowerHP <= 0)
        {
            PlayerControl.GetComponent<PlayerControl>().RemoveListItem(this.gameObject);
            Debug.Log("�����g���j�󂳂ꂽ");
            //Drop(Lowerbodypart, true);
            MultiAudio.ins.PlaySEByName("SE_common_breakbody");
            StartCoroutine(ShowHPBarAndDestroy(LowerHPBar, Lowerbodypart, false))
                ;
        }
    }


    //body�ɂ�0��1��������Ă͂����Ȃ��@BA//GU/RU
    //body : 0->�㔼�g�Ƀ_���[�W
    //body : 1->�����g�Ƀ_���[�W

    public void TakeDamage(int damage, int body = 0)
    {
        //HP������d�g��
        //damage�̓e�X�g�p�̊֐�
        if (body == 0)
        {
            //�㔼�g��HP�����炷
            UpperHP -= damage;
            ShowHitEffects(body);
            UpdateHPBar(UpperHPBar, UpperHP, MaxUpperHP);
            Debug.Log(UpperHP);
            Debug.Log(MaxUpperHP);

        }

        if (body == 1)
        {
            //�����g��HP�����炷
            LowerHP -= damage;
            ShowHitEffects(body);
            UpdateHPBar(LowerHPBar, LowerHP, MaxLowerHP);
            Debug.Log(LowerHP);
            Debug.Log(MaxLowerHP);
        }
    }
    //�G��HP�o�[��ύX
    private void UpdateHPBar(Image hpBarMask, int currentHP, int maxHP)
    {
        if (hpBarMask != null)
        {
            // Fill Amount�����݂�HP�䗦�ɍX�V
            hpBarMask.fillAmount = (float)currentHP / maxHP;
        }
    }
    //�U�����q�b�g�����G�t�F�N�g(�I�u�W�F�N�g)���o��
    void ShowHitEffects(int body)
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

    void ShowDeathImage()
    {
        ////�����h���b�v�摜�ݒ肷��Ƃ�
        //if (deathImage != null)
        //{
        //    deathImage.enabled = true;
        //}
    }
    private IEnumerator ShowHPBarAndDestroy(Image hpBar, BodyPartsData part, bool typ)
    {
        if (hpBar != null)
        {

            hpBar.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f); // �p�����Ԃ͒����\
            hpBar.gameObject.SetActive(false);
        }
        Drop(part, typ);
    }
    //�h���b�v�A�C�e���𐶐�����֐��@
    //BodyPartsData part->����������ɗ^����p�����[�^�f�[�^
    //int typ->true�Ȃ�㔼�g��������:false�Ȃ牺���g��������
    //�f�t�H���g������true
    public void Drop(BodyPartsData part, bool typ = true)
    {
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

        //�v���C���[�p�����[�^�[��n��
        drop.GetComponent<newDropPart>().getPlayerManegerObjet(scPlayerParameter);

        //�e�L�X�g�{�b�N�X��n��
        drop.GetComponent<newDropPart>().getTextBox(textBox);

        //�{�X�t���O��n��
        drop.GetComponent<newDropPart>().getBossf(Boss);



        //
        drop.GetComponent<newDropPart>().getPartsData(part);
        drop.GetComponent<newDropPart>().getSceneTransition(sceneTransitionManager);
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