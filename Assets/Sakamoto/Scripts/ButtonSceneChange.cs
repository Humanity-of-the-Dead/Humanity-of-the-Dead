using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonSceneChange : MonoBehaviour
{
    [SerializeField] Button OptionReturnButton;
    // Update is called once per frame
    void Update()
    {
        MultiAudio.ins.PlayBGM_ByName("BGM_credit");


        MultiAudio.ins.bgmSource.loop = false; if (Input.GetKeyDown(KeyCode.G))
        {
            OptionReturnButton.onClick.Invoke();
            //enGameState = GameState.ShowText;
        }
    }

    public void goTitle()
    {
        SceneTransitionManager.instance.NextSceneButton(0);
    }
}
