using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brush_Base : ScriptableObject
{
    public virtual void Primary(Vector3 _mousePosition, float _radius, Color _paintColor) { }

    public virtual void Secondary(Vector3 _mousePosition, float _radius, Color _paintColor) { }

}
