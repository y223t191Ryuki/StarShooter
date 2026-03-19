using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonLegacyTextColorChanger : MonoBehaviour,
    ISelectHandler, IDeselectHandler
{
    public Text buttonText;
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(160, 255, 255);

    void Reset()
    {
        // 自動取得（Buttonの子にTextがある前提）
        buttonText = GetComponentInChildren<Text>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = normalColor;
    }
}