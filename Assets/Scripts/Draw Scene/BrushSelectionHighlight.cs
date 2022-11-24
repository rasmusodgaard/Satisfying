using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushSelectionHighlight : SerializedMonoBehaviour
{
    [SerializeField]
    Dictionary<BrushEnum, Image> dictionaryOfButtons;

    [SerializeField]
    public Color highligtColor;

    [SerializeField]
    GameObject PaletteUI;

    public void SetBrushSelectionHighlight(BrushEnum selectedBrush)
    {
        foreach (var button in dictionaryOfButtons)
        {
            button.Value.color = Color.white;
        }

        if (selectedBrush == BrushEnum.standard)
        {
            PaletteUI.SetActive(true);
        }
        else
        {
            PaletteUI.SetActive(false);
        }

        dictionaryOfButtons[selectedBrush].color = highligtColor;
    }
}
