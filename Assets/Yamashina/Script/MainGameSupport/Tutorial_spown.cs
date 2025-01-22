
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
    private Sprite[] tutorialImages;  // �`���[�g���A���摜�̔z��



    private GameObject canvasObject;  // ���������L�����o�X�̃C���X�^���X��ێ�����ϐ�
    private GameObject newImageObject;  // ���݂�Image�̃I�u�W�F�N�g

    private int currentImageIndex = 0;  // ���ݕ\�����Ă���摜�̃C���f�b�N�X

    void Start()
    {
        //SpawnCanvasWithImage(tutorialImages[0]);
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
        Image imageComponent = newImageObject.gameObject.transform.GetChild(0).GetComponent<Image>();

        if (imageComponent != null)
        {
            imageComponent.sprite = sprite;
        }



        Button nextButton = canvasObject.transform.Find("ChangeImage")?.GetComponent<Button>();
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => ShowNextTutorialImage());
        }

        Button prevButton = canvasObject.transform.Find("ChangeImage_Return")?.GetComponent<Button>();
        if (prevButton != null)
        {
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(() => ShowPreviousTutorialImage());
        }

        Button destroyButton = canvasObject.transform.Find("Destroy")?.GetComponent<Button>();
        if (destroyButton != null)
        {
            destroyButton.onClick.RemoveAllListeners();
            destroyButton.onClick.AddListener(() => DestroyCanvasWithImage());
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





