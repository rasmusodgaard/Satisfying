using System.Collections.Generic;
using UnityEngine;

public abstract class BrushBase : ScriptableObject
{
    [SerializeField]
    BrushEnum brushType;

    [SerializeField]
    Vector3 cursorRelativeSpritePosition;

    public Sprite cursorIconSprite;

    [SerializeField]
    Vector3 cursorRelativeTexturePosition;

    public Texture2D cursorTexture;

    public Vector3 CursorRelativeSpritePosition => cursorRelativeSpritePosition;
    public Vector3 CursorRelativeTexturePosition => cursorRelativeTexturePosition;



    public BrushEnum BrushType => brushType;

    // TODO: Send TileScripts instead of tileTranforms for optimization reasons
    public virtual void Primary(Vector3 mousePosition, float radius, Color paintColor, List<Transform> tileTransforms) { }

    public virtual void Secondary(Vector3 mousePosition, float radius, Color paintColor, List<Transform> tiles) { }

    public List<Transform> GetTilePixels(Vector2 mousePosition, float radius, List<Transform> tiles)
    {
        List<Transform> output = new List<Transform>();
        float sqrRadius = radius * radius;
        for (int i = 0; i < tiles.Count; i++)
        {
            Vector2 velocity = (Vector2)tiles[i].position - mousePosition;
            float sqrDistance = Vector2.SqrMagnitude(velocity);
            if (sqrDistance <= sqrRadius)
            {
                output.Add(tiles[i]);
            }
        }
        return output;
    }
}
