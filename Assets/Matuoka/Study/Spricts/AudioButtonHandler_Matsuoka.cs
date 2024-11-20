using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioButtonHandler_Matsuoka : MonoBehaviour, IPointerEnterHandler
{
    [Header("�ԍ������O��")]
    [Tooltip("�N���b�N�p")][SerializeField] bool isNum_Click;
    [Tooltip("�}�E�X����ɗ����p")][SerializeField] bool isNum_OnMou;

    [Header("���̔z��ԍ�")]
    [Tooltip("�N���b�N�p")][SerializeField] int ind_Click = -1;
    [Tooltip("�}�E�X����ɗ����p")][SerializeField] int ind_OnMou = -1;

    [Header("���̖��O")]
    [Tooltip("�N���b�N�p")][SerializeField] string name_Click;
    [Tooltip("�}�E�X����ɗ����p")][SerializeField] string name_OnMou;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�{�^���������ꂽ�Ƃ��ASE��炷
    public void OnClickButtonPlayOnShotSE()
    {
        if (isNum_Click)
        {
            if (ind_Click >= 0) MultiAudio_Matsuoka.ins.ChooseSongsSE(ind_Click);
        }
        //else �����ɖ��O���g���Ė炷����
    }

    //�{�^���̏�Ƀ}�E�X���������ASE���Ȃ炷
    public void OnPointerEnter(PointerEventData eveData)
    {
        if (isNum_OnMou)
        {
            if(ind_OnMou >= 0)MultiAudio_Matsuoka.ins.ChooseSongsSE(ind_OnMou);
        }
        //else �����ɖ��O���g���Ė炷����
    }
}
