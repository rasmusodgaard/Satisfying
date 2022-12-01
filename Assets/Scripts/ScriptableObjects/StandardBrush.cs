using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StandardBrush", menuName = "ScriptableObjects/StandardBrush", order = 1)]
public class StandardBrush : BrushBase
{
    [SerializeField]
    float perimeterRadius;

    public delegate void SetColorDelegate(Color color);
    public event SetColorDelegate SetColor;

    public override void Primary(Vector3 mousePosition, float radius, Color paintColor, List<Transform> tileTransforms)
    {
        List<Transform> affectedTiles = GetTilePixels(mousePosition, radius, tileTransforms);

        for (int i = 0; i < affectedTiles.Count; i++)
        {
            TileScript tile = affectedTiles[i].GetComponent<TileScript>();
            if (tile != null)
            {
                tile.SetColor(paintColor);
            }
        }
    }

    public override void Secondary(Vector3 mousePosition, float radius, Color paintColor, List<Transform> tileTransforms)
    {
        List<Transform> affectedTiles = GetTilePixels(mousePosition, perimeterRadius, tileTransforms);

        for (int i = 0; i < affectedTiles.Count; i++)
        {
            TileScript tile = affectedTiles[i].GetComponent<TileScript>();
            if (tile.GetSpriteRenderer.bounds.Contains((Vector2)mousePosition))
            {
                SetColor?.Invoke(tile.GetColor());
            }
        }

    }
}
