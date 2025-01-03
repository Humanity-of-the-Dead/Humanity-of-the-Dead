using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlighter : MonoBehaviour
{
    public Button operationButton; // 操作説明ボタン
    public Button volumeButton;    // 音量ボタン
    public Color selectedColor = Color.white; // 選択状態の色
    public Color unselectedColor = Color.gray; // 非選択状態の色

    private void Start()
    {
        // 初期状態を設定
        ResetButtonColors();
        SetButtonColor(operationButton, selectedColor); // 操作説明ボタンを選択状態にする
        operationButton.Select();
    }

    private void SetButtonColor(Button button, Color color)
    {
        // ボタンに対応する画像の色を変更
        var image = button.GetComponent<Image>();
        if (image != null)
        {
            image.color = color;
        }
    }

    public void OnButtonSelected(Button selectedButton)
    {
        // 選択されたボタン以外を灰色に、選択されたボタンを白色に設定
        if (selectedButton == operationButton)
        {
            SetButtonColor(operationButton, selectedColor);
            SetButtonColor(volumeButton, unselectedColor);
        }
        else if (selectedButton == volumeButton)
        {
            SetButtonColor(operationButton, unselectedColor);
            SetButtonColor(volumeButton, selectedColor);
        }

        // 選択したボタンを明示的に選択状態にする
        selectedButton.Select();
    }

    private void ResetButtonColors()
    {
        // 両方のボタンをリセット（灰色に設定）
        SetButtonColor(operationButton, selectedColor);
        SetButtonColor(volumeButton, unselectedColor);
    }
}
