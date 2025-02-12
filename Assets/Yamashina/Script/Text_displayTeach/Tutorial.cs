using UnityEngine;
using UnityEngine.UI;
public class Tutorial : TextDisplay
{
    private Tutorial_spown tutorial_Spawn;


    protected override void Start()
    {
        base.Start();
        tutorial_Spawn = FindAnyObjectByType<Tutorial_spown>();
    }
    protected override void Update()
    {

        switch (GameMgr.GetState())
        {

            case GameState.Main:
                base.Update();

                break;
            case GameState.ShowText:
                base.Update();
                if (!TextArea.activeSelf)
                {
                    GameMgr.ChangeState(GameState.Tutorial);
                    tutorial_Spawn.SpawnTutorial();



                }
                break;
            case GameState.Tutorial:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    tutorial_Spawn.DestroyCanvasWithImage();
                }
                break;
            case GameState.Clear:
                base.Update();  
                break;




        }
    }

}
