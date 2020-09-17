using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brush_Base : ScriptableObject
{
    public virtual void Primary(Vector3 _mousePosition, float _radius, Color _paintColor) { }

    public virtual void Secondary(Vector3 _mousePosition, float _radius, Color _paintColor) { }


    protected OverlapCircleResults GetPixels(Vector3 _mousePosition, float _radius)
    {
        OverlapCircleResults results = new OverlapCircleResults();
        results.colliders = new Collider2D[10000];
        results.count = Physics2D.OverlapCircleNonAlloc(_mousePosition, _radius, results.colliders);
        return results;
    }

}

public struct OverlapCircleResults
{
    public int count;
    public Collider2D[] colliders;
}
