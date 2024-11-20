using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //�^�C�}�[
    float fTimer;
    //�^�C�}�[�̍ő�l
    [SerializeField] float fTimerMax;
    void Start()
    {
        fTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //�^�C�}�[�̍ő�l�𒴂������\���ɂ���
        if(fTimer > fTimerMax)
        {
            this.gameObject.SetActive(false);
            fTimer = 0;
        }
        fTimer += Time.deltaTime;
    }
}
