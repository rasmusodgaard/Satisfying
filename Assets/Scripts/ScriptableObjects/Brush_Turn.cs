using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Brush_Turn", menuName = "ScriptableObjects/Brush_Turn", order = 1)]
public class Brush_Turn : Brush_Base
{
    public override void Primary(Vector3 _mousePosition, float _radius, Color _paintColor)
    {
        OverlapCircleResults results = GetPixels(_mousePosition, _radius);
        for (int i = 0; i < results.count; i++)
        {
            results.colliders[i].transform.LookAt(_mousePosition);
        }

    }

    public override void Secondary(Vector3 _mousePosition, float _radius, Color _paintColor)
    {
        OverlapCircleResults results = GetPixels(_mousePosition, _radius);
        for (int i = 0; i < results.count; i++)
        {
            results.colliders[i].transform.rotation = Quaternion.identity;
        }
    }
}
