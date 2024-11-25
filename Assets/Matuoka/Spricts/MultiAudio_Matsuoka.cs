using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class MultiAudio_Matsuoka: MonoBehaviour
{
    //BGM�̃I�[�f�B�I�N���b�v
    [SerializeField] AudioClip[] bGMAudCli;
    //SE�̃I�[�f�B�I�N���b�v
    [SerializeField] AudioClip[] sEAudCli;
    //SE�𖼑O�ŌĂяo����悤�ɁA���O�Ɨv�f�ԍ���R�t����ϐ�
    Dictionary<string, AudioClip> sEAudCliDic;

    //BGM�p�̃I�[�f�B�I�\�[�X
    AudioSource bGMAudSou;
    //SE�p�̃I�[�f�B�I�\�[�X
    AudioSource sEAudSou;

    //BGM,SE,UI�̃I�[�f�B�I�~�L�T�[�O���[�v
    [SerializeField]AudioMixerGroup bGMAudMixGro, sEAudMixGro, uIAudMixGro;

    //�V���O���g��
    public static MultiAudio_Matsuoka ins;

    private void Awake()
    {
        //�V���O���g����
        if (ins != null)
        {
            Destroy(gameObject);
        }
        else
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        //\�V���O���g����
    }

    // Start is called before the first frame update
    void Start()
    {
        //BGM�̃I�[�f�B�I�\�[�X���擾
        bGMAudSou=GameObject.FindWithTag("BGM").GetComponent<AudioSource>();
        //SE�̃I�[�f�B�I�\�[�X�擾
        sEAudSou=GameObject.FindWithTag("SE").GetComponent<AudioSource>();

        //AudioSource�ɃI�[�f�B�I�~�L�T�[�O���[�v�����蓖�Ă�
        if(bGMAudSou != null )bGMAudSou.outputAudioMixerGroup = bGMAudMixGro;
        if (sEAudSou != null) sEAudSou.outputAudioMixerGroup = sEAudMixGro;

        //������
        sEAudCliDic = new Dictionary<string, AudioClip>();
        //SE�̃I�[�f�B�I�N���b�v�Ɨv�f�ԍ���R�Â���
        foreach(AudioClip clip in sEAudCli)
        {
            sEAudCliDic[clip.name]= clip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //BGM��v�f�ԍ��őI��ōĐ�
    public void ChooseSongsBGM_Num(int ind) {
        //ind��bGMAudCli�̗v�f�ԍ��̂Ƃ�
        if (ind >= 0 && ind < bGMAudCli.Length)
        {
            bGMAudSou.clip = bGMAudCli[ind];

            if (bGMAudSou != null)
            {
                bGMAudSou.Play();
                Debug.Log("Playing BGM:" + bGMAudSou.clip.name);
            }
            else
            {
                //�x�����b�Z�[�W
                Debug.LogWarning("BGM clip not set");
            }
        }
        else
        {
            Debug.LogWarning("BGM index out of range");
        }
    }

    //SE��v�f�ԍ��őI��ōĐ�
    public void ChooseSongsSE_Num(int ind)
    {
        //ind��sEAudCli�̗v�f�ԍ��̂Ƃ�
        if (ind >= 0 && ind < sEAudCli.Length)
        {
            //SE�̍Đ�
            SoundAnSE(sEAudCli[ind]);
        }
        else
        {
            Debug.LogWarning("SE index out of range");
        }
    }

    //SE�𖼑O�őI��ōĐ�
    public void ChooseSongsSE_Name(string name)
    {
        //���̖��O��SE�����邩���f���A����SE�̃I�[�f�B�I�N���b�v���擾
        if (sEAudCliDic.TryGetValue(name,out AudioClip clip))
        {
            sEAudSou.clip = sEAudCli[ind];

            if (sEAudSou != null)
            {
                //clip���ɂ̍s����UI�̂Ƃ�
                if (sEAudCli[ind].name.StartsWith("UI"))
                {
                    sEAudSou.outputAudioMixerGroup = uIAudMixGro;
                    Debug.Log("�s����UI");
                }
                else
                {
                    sEAudSou.outputAudioMixerGroup= sEAudMixGro;
                }

                sEAudSou.PlayOneShot(sEAudSou.clip);
                Debug.Log("Playing SE:" + sEAudSou.clip.name);
            }
            else
            {
                //�x�����b�Z�[�W
                Debug.LogWarning("SE clip not set");
            }
        }
        else
        {
            Debug.LogWarning("SE index out of range");
        }
    }

    //SE�̍Đ�
    void SoundAnSE(AudioClip clip)
    {
        if (clip != null)
        {
            //clip���ɂ̍s����UI�̂Ƃ�
            if (clip.name.StartsWith("UI"))
            {
                sEAudSou.outputAudioMixerGroup = uIAudMixGro;
                Debug.Log("�s����UI");
            }
            else
            {
                sEAudSou.outputAudioMixerGroup = sEAudMixGro;
            }
    
            sEAudSou.PlayOneShot(sEAudSou.clip);
            Debug.Log("Playing SE:" + sEAudSou.clip.name);
        }
        else
        {
            //�x�����b�Z�[�W
            Debug.LogWarning("SE clip not set");
        }
    }
}
