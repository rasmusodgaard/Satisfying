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

        OverlapCircleResults results = GetPixels(_mousePosition, _radius);

        for (int i = 0; i < results.count; i++)
        {
            float sqDist = Vector3.SqrMagnitude(_mousePosition - results.colliders[i].transform.position);
            if (sqDist > minSqDist)
            {
                Vector3 direction = ((_mousePosition - results.colliders[i].transform.position).normalized);
                results.colliders[i].transform.position += direction * magnetSpeed * Time.fixedDeltaTime;
            }

        }
    }

    public override void Secondary(Vector3 _mousePosition, float _radius, Color _paintColor)
    {
        OverlapCircleResults results = GetPixels(_mousePosition, _radius);
        for (int i = 0; i < results.count; i++)
        {
            float sqDist = Vector3.SqrMagnitude(_mousePosition - results.colliders[i].transform.position);

            Vector3 direction = (_mousePosition - results.colliders[i].transform.position).normalized;
            results.colliders[i].transform.position += -direction * magnetSpeed * Time.fixedDeltaTime;

        }
    }
}
