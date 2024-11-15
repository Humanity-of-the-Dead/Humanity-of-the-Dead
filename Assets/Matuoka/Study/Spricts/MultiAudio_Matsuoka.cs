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

    //BGM�p�̃I�[�f�B�I�\�[�X
    AudioSource bGMAudSou;
    //SE�p�̃I�[�f�B�I�\�[�X
    AudioSource sEAudSou;

    //BGM,SE,UI�̃I�[�f�B�I�~�L�T�[�O���[�v
    AudioMixerGroup bGMAudMixGro, sEAudMixGro, uIAudMixGro;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //BGM��I��ōĐ�
    public void ChooseSongsBGM(int ind) {
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

    //SE��I��ōĐ�
    public void ChooseSongsSE(int ind)
    {
        //ind��sEAudCli�̗v�f�ԍ��̂Ƃ�
        if (ind >= 0 && ind < sEAudCli.Length)
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
}
