
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
        // ���ݑI������Ă���UI�I�u�W�F�N�g���擾
        var selectedObject = EventSystem.current.currentSelectedGameObject;

        if (selectedObject != null && selectedObject.TryGetComponent<Button>(out var selectedButton))
        {
            // �O��̃{�^�������Z�b�g
            if (lastSelectedButton != null && lastSelectedButton != selectedButton)
            {
                ResetButtonColor(lastSelectedButton);
            }

            // ���ݑI�𒆂̃{�^���̐F��ύX
            SetButtonColor(selectedButton, selectedColor);

            // ���݂̃{�^�����L��
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
