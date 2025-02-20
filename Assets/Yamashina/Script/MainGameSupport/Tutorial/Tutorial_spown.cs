
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_spown : MonoBehaviour
{
    [SerializeField, Header("TutorialCanvas�v���n�u�����Ă�������")]
    private GameObject canvasPrefab;  // ��������Canvas�̃v���n�u

    [SerializeField, Header("���ꂼ��p�r�ɍ��킹��TutorialPanel�`\n�v���n�u�����Ă�������")]
    private GameObject imagePrefab;   // ��������Image�̃v���n�u

    [SerializeField, Header("�`���[�g���A���摜�̃��X�g\n��(Element0�j���珇�Ԃɕ\��")]
    public Sprite[] tutorialImages;  // �`���[�g���A���摜�̔z��




    public GameObject canvasObject;  // ���������L�����o�X�̃C���X�^���X��ێ�����ϐ�
    public GameObject newImageObject;  // ���݂�Image�̃I�u�W�F�N�g

    private int currentImageIndex = 0;  // ���ݕ\�����Ă���摜�̃C���f�b�N�X

   

    public void SpawnTutorial()
    {
        SpawnCanvasWithImage(tutorialImages[currentImageIndex]);
    }
    /// <summary>
    /// �L�����o�X�𐶐����A���̏��Image��ǉ����āA�X�v���C�g��ݒ�
    /// </summary>
    public void SpawnCanvasWithImage(Sprite sprite)
    {
        if (canvasObject != null)
        {
            Destroy(canvasObject);
        }
        canvasObject = Instantiate(canvasPrefab);
        newImageObject = Instantiate(imagePrefab, canvasObject.transform);
        Image imageComponent = newImageObject.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();

        if (imageComponent != null)
        {
            imageComponent.sprite = sprite;
        }







        newImageObject.transform.SetAsLastSibling();
       
    }



    public void ShowNextTutorialImage()
    {
        if (currentImageIndex < tutorialImages.Length - 1)
        {
            currentImageIndex++;
            ChangeImage(tutorialImages[currentImageIndex]);

        }
    }

    public void ShowPreviousTutorialImage()
    {
        if (currentImageIndex > 0)
        {
            currentImageIndex--;
            ChangeImage(tutorialImages[currentImageIndex]);
        }
    }

    public void DestroyCanvasWithImage()
    {
        if (canvasObject != null)
        {
            Destroy(canvasObject);
            if (Tutorial.GetState() != Tutorial_State.Option)
            {

                GameMgr.ChangeState(GameState.Main);
            }
            ShowNextTutorialImage();
        }
    }

    public void ChangeImage(Sprite sprite)
    {
        if (newImageObject != null)
        {
            Image imageComponent = newImageObject.gameObject.transform.GetChild(0).GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = sprite;
            }
        }
    }

}





