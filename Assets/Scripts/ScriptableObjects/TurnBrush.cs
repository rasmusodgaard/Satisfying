using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurnBrush", menuName = "ScriptableObjects/TurnBrush", order = 1)]
public class TurnBrush : BrushBase
{
    public override void Primary(Vector3 mousePosition, float radius, Color paintColor, List<Transform> tileTransforms)
    {
        List<Transform> results = GetTilePixels(mousePosition, radius, tileTransforms);
        for (int i = 0; i < results.Count; i++)
        {
            results[i].LookAt(mousePosition);
        }

    }

    public override void Secondary(Vector3 mousePosition, float radius, Color paintColor, List<Transform> tileTransforms)
    {
        List<Transform> results = GetTilePixels(mousePosition, radius, tileTransforms);
        for (int i = 0; i < results.Count; i++)
        {
            results[i].rotation = Quaternion.identity;
        }
    }
}
