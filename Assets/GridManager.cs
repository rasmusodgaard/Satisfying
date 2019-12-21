using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject prefab;
    public int divider = 32;

    Vector3 min;
    Vector3 max;

    Vector3 topLeft, topRight, bottomLeft, bottomRight;
    private Vector2 gridSize;
    private Vector2 sideLength;
    private float spriteSideLength;
    private TileScript[,] tileScripts;

    void Start()
    {
        min = Camera.main.ScreenToWorldPoint(Vector3.zero);
        max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        sideLength = new Vector2(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y));
        spriteSideLength = prefab.GetComponent<SpriteRenderer>().bounds.size.x;


        gridSize = new Vector2(Screen.width / divider, Screen.height / divider);
        tileScripts = new TileScript[(int)gridSize.x, (int)gridSize.y];

        print("grid: " + gridSize);

        topRight = max + new Vector3(0, 0, 10);
        topLeft = new Vector3(min.x, max.y, 0);
        bottomLeft = min + new Vector3(0, 0, 10);
        bottomRight = new Vector3(max.x, min.y, 0);

        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        double width = sr.sprite.bounds.size.x;
        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        GameObject go = prefab;
        float tileSize = (float)(worldScreenWidth / width) / gridSize.x;
        go.transform.localScale = Vector2.one * tileSize;
        go.transform.localScale *= 0.9f;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject clone = Instantiate(go, new Vector3(min.x + (sideLength.x / gridSize.x) * 0.5f + (sideLength.x / gridSize.x * x), min.y + (sideLength.x / gridSize.x) * 0.5f + (sideLength.y / gridSize.y * y), 0), Quaternion.identity);
                tileScripts[x, y] = clone.GetComponent<TileScript>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawBoundaries();
    }

    private void DrawBoundaries()
    {
        Debug.DrawLine(topLeft, topRight, Color.cyan);
        Debug.DrawLine(topRight, bottomRight, Color.red);
        Debug.DrawLine(bottomRight, bottomLeft, Color.magenta);
        Debug.DrawLine(bottomLeft, topLeft, Color.blue);

    }

    public TileScript GetTile(Vector3 cursorPos)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(cursorPos) / divider;
        Vector2Int pos2D = new Vector2Int(Mathf.FloorToInt(pos.x - (spriteSideLength / 2)), Mathf.FloorToInt(pos.y - (spriteSideLength / 2)));
        print("Pos2D: " + pos2D);
        return tileScripts[pos2D.x, pos2D.y];
    }
}
