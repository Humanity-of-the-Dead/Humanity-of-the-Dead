using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMgr.ChangeState(GameState.BeforeBoss); // �{�X�풼�O�ɏ�ԕύX
        }
    }
}
