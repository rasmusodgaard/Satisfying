using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Brush_Magnet", menuName = "ScriptableObjects/Brush_Magnet", order = 1)]
public class Brush_Magnet : Brush_Base
{
    public float magnetSpeed = 1;
    public float minSqDist = 5;
    public float maxSqDist = 75;


    public override void Primary(Vector3 _mousePosition, float _radius, Color _paintColor)
    {

        Collider2D[] results = GetPixels(_mousePosition, _radius);

        for (int i = 0; i < results.Length; i++)
        {
            float sqDist = Vector3.SqrMagnitude(_mousePosition - results[i].transform.position);
            if (sqDist > minSqDist)
            {
                Vector3 direction = ((_mousePosition - results[i].transform.position).normalized);
                results[i].transform.position += direction * magnetSpeed * Time.fixedDeltaTime;
            }

        }
    }

    public override void Secondary(Vector3 _mousePosition, float _radius, Color _paintColor)
    {
        Collider2D[] results = GetPixels(_mousePosition, _radius);
        for (int i = 0; i < results.Length; i++)
        {
            float sqDist = Vector3.SqrMagnitude(_mousePosition - results[i].transform.position);

            Vector3 direction = (_mousePosition - results[i].transform.position).normalized;
            results[i].transform.position += -direction * magnetSpeed * Time.fixedDeltaTime;

        }
    }

    private Collider2D[] GetPixels(Vector2 _mousePosition, float _radius)
    {
        Collider2D[] results = new Collider2D[10000];
        Physics2D.OverlapCircleNonAlloc(_mousePosition, _radius, results);
        return results;
    }
}
