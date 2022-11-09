using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Brush_Standard", order = 1)]
public class Brush_Standard : Brush_Base
{


    public override void Primary(Vector3 mousePosition, float radius, Color paintColor, List<Transform> tileTransforms)
    {
        List<Transform> affectedTiles = GetTilePixels(mousePosition, radius, tileTransforms);

        for (int i = 0; i < affectedTiles.Count; i++)
        {
            TileScript tile = affectedTiles[i].GetComponent<TileScript>();
            if (tile != null)
            {
                tile.ColorTile(paintColor);
            }
        }
    }
}
