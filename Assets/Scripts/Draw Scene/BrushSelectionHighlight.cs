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

    public void SetBrushSelectionHighlight(BrushEnum selectedBrush)
    {
        foreach (var button in dictionaryOfButtons)
        {
            button.Value.color = Color.white;
        }

        dictionaryOfButtons[selectedBrush].color = highligtColor;
    }
}
