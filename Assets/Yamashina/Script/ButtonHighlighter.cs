
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlighter : MonoBehaviour
{
    public Color selectedColor = Color.white;
    public Color unselectedColor = Color.gray;

    private Button lastSelectedButton;

    void Update()
    {
        // 現在選択されているUIオブジェクトを取得
        var selectedObject = EventSystem.current.currentSelectedGameObject;

        if (selectedObject != null && selectedObject.TryGetComponent<Button>(out var selectedButton))
        {
            // 前回のボタンをリセット
            if (lastSelectedButton != null && lastSelectedButton != selectedButton)
            {
                ResetButtonColor(lastSelectedButton);
            }

            // 現在選択中のボタンの色を変更
            SetButtonColor(selectedButton, selectedColor);

            // 現在のボタンを記憶
            lastSelectedButton = selectedButton;
        }
    }

    private void SetButtonColor(Button button, Color color)
    {
        var image = button.GetComponent<Image>();
        if (image != null)
        {
            image.color = color;
        }
    }

    private void ResetButtonColor(Button button)
    {
        var image = button.GetComponent<Image>();
        if (image != null)
        {
            image.color = unselectedColor;
        }
    }
}
