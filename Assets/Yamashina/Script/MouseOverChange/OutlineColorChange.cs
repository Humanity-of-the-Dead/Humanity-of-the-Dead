using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class OutlineColorChange : MonoBehaviour
{
    public Outline outline;                  // 影のShadowコンポーネント配列
    public Color hoverColor = new Color(0, 0, 0, 0.6f);  // 透明度を上げた黒色
    private Color originalColor;              // 元の影の色

    void Start()
    {
        // Outline コンポーネントの参照を取得
        outline = GetComponent<Outline>();

        // 元のOutline色を保持
        originalColor = outline.effectColor;

        // 最初はOutlineを非表示にする
        outline.enabled = false;
    }

    //アウトラインをマウスオーバー時に追加・色変更するメソッド
    public void OnPointerEnter()
    {
        // マウスオーバー時の色に変更
        outline.effectColor = hoverColor;
        outline.enabled = true;
    }
    // 元の色に戻すメソッド

    public void OnPointerExit()
    {
        // 元の色に戻す
        outline.effectColor = originalColor;
        outline.enabled = false;
    }
}









