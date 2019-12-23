using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public GridManager grid;
    public Color standard;
    public Color over;
    private Color paintColor = Color.cyan;

    private int tilesPaintedInStroke = 0;
    public int maxStrokes = 10;

    private TileScript lastTile;

    private Camera cam;
    void Start()
    {
        cam = Camera.main;
        Cursor.visible = false;
        this.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    void Update()
    {

        this.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            tilesPaintedInStroke = 0;
        }

        if (Input.GetMouseButton(0))
        {
            // TileScript tile = grid.GetTile(this.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                TileScript tile = hit.collider.GetComponent<TileScript>();
                if (tile != lastTile)
                {
                    print("painted");
                    tile.ColorTile(Color.Lerp(paintColor, Color.grey, (float)tilesPaintedInStroke / (float)maxStrokes));
                    tilesPaintedInStroke = Mathf.Clamp(tilesPaintedInStroke, 0, maxStrokes) + 1;
                }
                lastTile = tile;
            }

        }
    }
}
