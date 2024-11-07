using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    [SerializeField]    SceneTransitionManager sceneTransitionManager;
    [SerializeField] SceneInformation sceneInformation;
    // Start is called before the first frame update
    void Start()
    {
        //sceneTransitionManager = GetComponent<SceneTransitionManager>();    
    }

    // Update is called once per frame
    void Update()
    {
        sceneTransitionManager.SceneChange(SceneInformation.SCENE.StageOne);
        sceneInformation.SetPreviousScene((SceneInformation.SCENE)SceneManager.GetActiveScene().buildIndex);

    }
}
