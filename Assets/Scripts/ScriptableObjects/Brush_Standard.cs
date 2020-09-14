using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Brush_Standard", order = 1)]
public class Brush_Standard : Brush_Base
{


    public override void Primary(Vector3 _mousePosition, float _radius, Color _paintColor)
    {
        Collider2D[] hoverList = new Collider2D[3000];
        int count = Physics2D.OverlapCircleNonAlloc(_mousePosition, _radius, hoverList);

        for (int i = 0; i < count; i++)
        {
            TileScript tile = hoverList[i].GetComponent<TileScript>();
            if (tile != null)
            {
                tile.ColorTile(_paintColor);
            }
        }
    }
}
