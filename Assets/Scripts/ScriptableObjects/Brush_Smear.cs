using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush_Smear : Brush_Base
{

    private int tilesPaintedInStroke = 0;
    public int maxStrokes = 42;
    public float radiusDivider = 100;

    public override void Primary(Vector3 _mousePosition, float _radius, Color _paintColor)
    {


        // // TileScript tile = grid.GetTile(this.transform.position);
        // RaycastHit2D hit = Physics2D.Raycast(_mousePosition, Vector2.zero);

        // if (hit.collider != null)
        // {
        //     TileScript tile = hit.collider.GetComponent<TileScript>();
        //     if (tile != lastTile && tile != null)
        //     {
        //         tile.ColorTile(Color.Lerp(paintColor, Color.grey, (float)tilesPaintedInStroke / (float)maxStrokes));
        //         tilesPaintedInStroke = Mathf.Clamp(tilesPaintedInStroke, 0, maxStrokes) + 1;
        //     }
        //     lastTile = tile;
        // // }
    }

    public override void Secondary(Vector3 _mousePosition, float _radius, Color _paintColor)
    { }
}
