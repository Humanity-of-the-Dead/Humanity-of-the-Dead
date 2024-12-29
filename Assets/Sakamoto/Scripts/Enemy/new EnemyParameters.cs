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
    public 
    PlayerParameter scPlayerParameter;
    //�v���C���[�R���g���[��
    public GameObject PlayerControl;

    //�{�X�t���O
    [SerializeField]
    bool Boss;

    //�N���A�e�L�X�g
    [SerializeField]
    GameObject textBox;

    [SerializeField]SceneTransitionManager sceneTransitionManager;
    
    //�q�b�g�G�t�F�N�g
    [SerializeField] GameObject hitGameObject;
    //�G�t�F�N�g�̏o���͈�
    //�㔼�g
    [SerializeField]float upperEffectXMin, upperEffectXMax,upperEffectYMin,upperEffectYMax;

    //�����g
    [SerializeField]float lowerEffectXMin, lowerEffectXMax,lowerEffectYMin,lowerEffectYMax;
    //\�G�t�F�N�g�E�E�E
    
    private void Start()
    {
        scPlayerParameter = GameObject.Find("PlParameter").GetComponent<PlayerParameter>();
        sceneTransitionManager =GameObject.FindAnyObjectByType<SceneTransitionManager>();
    }
    void Update()
    {
        //�����ϋv�l��0�ɂȂ�����h���b�v����
        if (UpperHP <= 0)
        {
            PlayerControl.GetComponent<PlayerControl>().RemoveListItem(this.gameObject);
            Debug.Log("�����g����������");
            Drop(Lowerbodypart,false);
            MultiAudio.ins.PlaySEByName("SE_common_breakbody");
        }
        if (LowerHP <= 0)
        {
            PlayerControl.GetComponent<PlayerControl>().RemoveListItem(this.gameObject);
            Debug.Log("�㔼�g����������");
            Drop(Upperbodypart,true);
            MultiAudio.ins.PlaySEByName("SE_common_breakbody");
        }
    }
    //body�ɂ�0��1��������Ă͂����Ȃ��@BA//GU/RU
    //body : 0->�㔼�g�Ƀ_���[�W
    //body : 1->�����g�Ƀ_���[�W

    public void TakeDamage(int damage, int body = 0)
    {
        //HP������d�g��
        //damage�̓e�X�g�p�̊֐�
    if(body == 0)
        {
            //�㔼�g��HP�����炷
            UpperHP -= damage;
            ShowHitEffects(body);
        }

    if(body == 1)
        {
            //�����g��HP�����炷
            LowerHP -= damage;
            ShowHitEffects(body);
        }
    }

    //�U�����q�b�g�����G�t�F�N�g(�I�u�W�F�N�g)���o��
    void ShowHitEffects(int body)
    {
        //���̃I�u�W�F�N�g�̍��W
        Vector2 tihsVec2 = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

        //�G�t�F�N�g
        GameObject obj=null;
        
        //�㔼�g�̏ꍇ
        if (body == 0)
        {
            //�I�u�W�F�N�g���o�����[�J�����W
            Vector2 effectVec2=new Vector2(
                Random.Range(upperEffectXMin,upperEffectXMax),
                Random.Range(upperEffectYMin,upperEffectYMax));

            //�I�u�W�F�N�g���o��
            obj=Instantiate(hitGameObject,effectVec2+tihsVec2,Quaternion.identity);
            // Debug.Log("effectVec2+thisVec2="+effectVec2+tihsVec2)
            // Debug.Log("hit effect");
        }

        if (body == 1)
        {
            //�I�u�W�F�N�g���o�����[�J�����W
            Vector2 effectVec2=new Vector2(
                Random.Range(lowerEffectXMin,lowerEffectXMax),
                Random.Range(lowerEffectYMin,lowerEffectYMax));

            obj=Instantiate(hitGameObject,effectVec2+tihsVec2,Quaternion.identity);
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

    //�h���b�v�A�C�e���𐶐�����֐��@
    //BodyPartsData part->����������ɗ^����p�����[�^�f�[�^
    //int typ->true�Ȃ�㔼�g��������:false�Ȃ牺���g��������
    //�f�t�H���g������true
    public void Drop(BodyPartsData part , bool typ = true)
    {
        if(typ == true)
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