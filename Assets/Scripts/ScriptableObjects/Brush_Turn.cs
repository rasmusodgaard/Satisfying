using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Brush_Turn", menuName = "ScriptableObjects/Brush_Turn", order = 1)]
public class Brush_Turn : Brush_Base
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
