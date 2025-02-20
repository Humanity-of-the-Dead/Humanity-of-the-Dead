
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_spown : MonoBehaviour
{
    [SerializeField, Header("TutorialCanvasプレハブを入れてください")]
    private GameObject canvasPrefab;  // 生成するCanvasのプレハブ

    [SerializeField, Header("それぞれ用途に合わせたTutorialPanel～\nプレハブを入れてください")]
    private GameObject imagePrefab;   // 生成するImageのプレハブ

    [SerializeField, Header("チュートリアル画像のリスト\n上(Element0）から順番に表示")]
    public Sprite[] tutorialImages;  // チュートリアル画像の配列




    public GameObject canvasObject;  // 生成したキャンバスのインスタンスを保持する変数
    public GameObject newImageObject;  // 現在のImageのオブジェクト

    private int currentImageIndex = 0;  // 現在表示している画像のインデックス

   

    public void SpawnTutorial()
    {
        SpawnCanvasWithImage(tutorialImages[currentImageIndex]);
    }
    /// <summary>
    /// キャンバスを生成し、その上にImageを追加して、スプライトを設定
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





