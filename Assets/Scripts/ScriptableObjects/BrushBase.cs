using System.Collections.Generic;
using UnityEngine;

public abstract class BrushBase : ScriptableObject
{
    [SerializeField]
    BrushEnum brushType;

    [SerializeField]
    Vector3 cursorRelativePosition;

    public Sprite cursorIcon;

    public Vector3 CursorRelativePosition => cursorRelativePosition;

    public BrushEnum BrushType => brushType;

    public virtual void Primary(Vector3 mousePosition, float radius, Color _paintColor, List<Transform> tiles) { }

    public virtual void Secondary(Vector3 mousePosition, float radius, Color _paintColor, List<Transform> tiles) { }

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
