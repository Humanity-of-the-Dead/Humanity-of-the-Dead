using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameter : MonoBehaviour
{
    [Header("1��������̂ɂ����鎞��")]
    [SerializeField] int iDownTime;
    float fTimer; //�^�C�}�[

    [SerializeField] int iHumanity;     //�l�Ԑ�
    [SerializeField] int iUpperHP;      //�㔼�g��HP
    [SerializeField] int iLowerHP;      //�����g��HP
    // Start is called before the first frame update

    private void Start()
    {
        //�^�C�}�[������
        int fTimer = 0;
    }
    private void Update()
    {
        //�^�C�}�[�̒l��iDownTime�𒴂�����p�����[�^��1���炷
        if(fTimer > iDownTime)
        {
            iHumanity--;
            iUpperHP--;
            iLowerHP--;
            //�^�C�}�[��0�ɖ߂�
            fTimer = 0;
            return;
        }
        fTimer += Time.deltaTime; 
    }

    //�l�Ԑ��̎擾
    public int getHumanity()
    {
        return iHumanity;
    }
    //�㔼�gHP�̎擾
    public int getUpperHP() { 
        return iUpperHP;
    }
    //�����gHP�̎擾
    public int getLowerHP()
    {
        return iLowerHP;
    }
}
