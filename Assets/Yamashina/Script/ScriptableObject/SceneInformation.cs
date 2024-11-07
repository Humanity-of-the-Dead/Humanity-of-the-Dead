using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SceneInformation", menuName = "ScriptableObjects/StageInformation")]

public class SceneInformation : ScriptableObject
{
    // �S�ẴV�[��
    // �S�ẴV�[��
    [System.Serializable]
    public enum SCENE
    {
        Title,      // �^�C�g��
        StageOne,   // �X�e�[�W�P
        StageTwo,   // �X�e�[�W�Q
        StageThree, // �X�e�[�W�R

    }
    // �X�e�[�W�̃V�[��
    [System.Serializable]
    public enum STAGE
    {
        One,      // �X�e�[�W�P
        Two,      // �X�e�[�W�Q
        Three,    // �X�e�[�W�R
    }

    [SerializeField] public SceneObject[] sceneObject;
    [SerializeField] public string[] sceneNames;// �V�[���̖��O
    [SerializeField] private SCENE previousScene;

    public SceneObject GetSceneObject(SCENE scene)
    {
        return sceneObject[(int)scene];
    }

    public string GetSceneName(SCENE scene)
    {
        return sceneNames[(int)scene];
    }

    public SCENE GetPreviousScene() { return previousScene; }

    public void SetPreviousScene(SCENE scene) { previousScene = scene; }
}


