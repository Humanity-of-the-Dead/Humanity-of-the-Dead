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
    //SEを名前で呼び出せるように、名前と要素番号を紐付ける変数
    Dictionary<string, AudioClip> sEAudCliDic;

    //BGM用のオーディオソース
    AudioSource bGMAudSou;
    //SE用のオーディオソース
    AudioSource sEAudSou;

    //BGM,SE,UIのオーディオミキサーグループ
    [SerializeField]AudioMixerGroup bGMAudMixGro, sEAudMixGro, uIAudMixGro;

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

        //初期化
        sEAudCliDic = new Dictionary<string, AudioClip>();
        //SEのオーディオクリップと要素番号を紐づける
        foreach(AudioClip clip in sEAudCli)
        {
            sEAudCliDic[clip.name]= clip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //BGMを要素番号で選んで再生
    public void ChooseSongsBGM_Num(int ind) {
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

    //SEを要素番号で選んで再生
    public void ChooseSongsSE_Num(int ind)
    {
        //indがsEAudCliの要素番号のとき
        if (ind >= 0 && ind < sEAudCli.Length)
        {
            //SEの再生
            SoundAnSE(sEAudCli[ind]);
        }
        else
        {
            Debug.LogWarning("SE index out of range");
        }
    }

    //SEを名前で選んで再生
    public void ChooseSongsSE_Name(string name)
    {
        //その名前のSEがあるか判断しつつ、そのSEのオーディオクリップを取得
        if (sEAudCliDic.TryGetValue(name,out AudioClip clip))
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

    //SEの再生
    void SoundAnSE(AudioClip clip)
    {
        if (clip != null)
        {
            //clip名にの行頭がUIのとき
            if (clip.name.StartsWith("UI"))
            {
                sEAudSou.outputAudioMixerGroup = uIAudMixGro;
                Debug.Log("行頭がUI");
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
            //警告メッセージ
            Debug.LogWarning("SE clip not set");
        }
    }
}
