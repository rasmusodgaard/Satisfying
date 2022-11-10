using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagnetBrush", menuName = "ScriptableObjects/MagnetBrush", order = 1)]
public class MagnetBrush : BrushBase
{
    public float magnetSpeed = 1;
    public float pushSpeedMultiplier = 2;
    public float minSqrDistance = 5;
    public float maxSqrDistance = 75;

    public override void Primary(Vector3 mousePosition, float radius, Color _paintColor, List<Transform> tiles)
    {
        float sqrRadius = radius * radius;
        for (int i = 0; i < tiles.Count; i++)
        {
            Vector3 velocity = tiles[i].transform.position - mousePosition;
            float sqrDistance = Vector2.SqrMagnitude(velocity);
            if (sqrDistance <= sqrRadius)
            {
                Vector2 direction = (mousePosition - tiles[i].position).normalized;
                tiles[i].position += (Vector3)direction * magnetSpeed * Time.deltaTime;
            }
        }
    }

    public override void Secondary(Vector3 mousePosition, float radius, Color paintColor, List<Transform> tiles)
    {
        float sqrRadius = radius * radius;
        for (int i = 0; i < tiles.Count; i++)
        {
            Vector2 velocity = tiles[i].transform.position - mousePosition;
            float sqrDistance = Vector2.SqrMagnitude(velocity);
            if (sqrDistance <= sqrRadius)
            {
                Vector2 direction = (mousePosition - tiles[i].position).normalized;
                tiles[i].position += (Vector3)(-direction) * magnetSpeed * Time.deltaTime;
            }
        }
    }
}
