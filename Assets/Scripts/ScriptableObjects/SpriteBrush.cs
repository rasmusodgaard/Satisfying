using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Brush_Sprite", menuName = "ScriptableObjects/SpriteBrush", order = 1)]
public class SpriteBrush : BrushBase
{
    [SerializeField]
    List<Sprite> sprites;

    [ShowInInspector, ReadOnly]
    int currentSpriteIndex = 0;

    public override void Primary(Vector3 mousePosition, float radius, Color paintColor, List<Transform> tileTransforms)
    {
        List<Transform> affectedTiles = GetTilePixels(mousePosition, radius, tileTransforms);

        for (int i = 0; i < affectedTiles.Count; i++)
        {
            TileScript tileScript = affectedTiles[i].GetComponent<TileScript>();
            if (sprites.Count <= currentSpriteIndex)
            {
                currentSpriteIndex = 0;
            }

            tileScript.SetSprite(sprites[currentSpriteIndex]);
        }
    }

    public override void Secondary(Vector3 mousePosition, float radius, Color paintColor, List<Transform> tiles)
    {
        // HACK: Make cleaner check for initial button press
        if (Input.GetMouseButtonDown(1))
        {
            currentSpriteIndex++;
            if (currentSpriteIndex > sprites.Count)
            {
                currentSpriteIndex = 0;
            }
        }
    }
}
