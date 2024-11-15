using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class MultiAudio_Matsuoka: MonoBehaviour
{
    //BGMのオーディオクリップ
    [SerializeField] AudioClip[] bGMAudCli;
    //SEのオーディオクリップ
    [SerializeField] AudioClip[] sEAudCli;

    //BGM用のオーディオソース
    AudioSource bGMAudSou;
    //SE用のオーディオソース
    AudioSource sEAudSou;

    //BGM,SE,UIのオーディオミキサーグループ
    AudioMixerGroup bGMAudMixGro, sEAudMixGro, uIAudMixGro;

    //シングルトン
    public static MultiAudio_Matsuoka ins;

    private void Awake()
    {
        //シングルトン化
        if (ins != null)
        {
            Destroy(gameObject);
        }
        else
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        //\シングルトン化
    }

    // Start is called before the first frame update
    void Start()
    {
        //BGMのオーディオソースを取得
        bGMAudSou=GameObject.FindWithTag("BGM").GetComponent<AudioSource>();
        //SEのオーディオソース取得
        sEAudSou=GameObject.FindWithTag("SE").GetComponent<AudioSource>();

        //AudioSourceにオーディオミキサーグループを割り当てる
        if(bGMAudSou != null )bGMAudSou.outputAudioMixerGroup = bGMAudMixGro;
        if (sEAudSou != null) sEAudSou.outputAudioMixerGroup = sEAudMixGro;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //BGMを選んで再生
    public void ChooseSongsBGM(int ind) {
        //indがbGMAudCliの要素番号のとき
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
                //警告メッセージ
                Debug.LogWarning("BGM clip not set");
            }
        }
        else
        {
            Debug.LogWarning("BGM index out of range");
        }
    }

    //SEを選んで再生
    public void ChooseSongsSE(int ind)
    {
        //indがsEAudCliの要素番号のとき
        if (ind >= 0 && ind < sEAudCli.Length)
        {
            sEAudSou.clip = sEAudCli[ind];

            if (sEAudSou != null)
            {
                //clip名にの行頭がUIのとき
                if (sEAudCli[ind].name.StartsWith("UI"))
                {
                    sEAudSou.outputAudioMixerGroup = uIAudMixGro;
                    Debug.Log("行頭がUI");
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
                //警告メッセージ
                Debug.LogWarning("SE clip not set");
            }
        }
        else
        {
            Debug.LogWarning("SE index out of range");
        }
    }
}
