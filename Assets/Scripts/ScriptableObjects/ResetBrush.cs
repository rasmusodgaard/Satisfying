using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResetBrush", menuName = "ScriptableObjects/ResetBrush", order = 1)]
public class ResetBrush : BrushBase
{
    public override void Primary(Vector3 mousePosition, float radius, Color paintColor, List<Transform> tileTransforms)
    {
        List<Transform> affectedTiles = GetTilePixels(mousePosition, radius, tileTransforms);

        for (int i = 0; i < affectedTiles.Count; i++)
        {
            TileScript tile = affectedTiles[i].GetComponent<TileScript>();
            if (tile != null)
            {
                tile.ResetTransformation();
            }
        }
    }

    public override void Secondary(Vector3 mousePosition, float radius, Color paintColor, List<Transform> tileTransforms)
    {
        List<Transform> affectedTiles = GetTilePixels(mousePosition, radius, tileTransforms);
        for (int i = 0; i < affectedTiles.Count; i++)
        {
            TileScript tile = affectedTiles[i].GetComponent<TileScript>();
            if (tile != null)
            {
                tile.ResetColor();
            }
        }

    }
}
