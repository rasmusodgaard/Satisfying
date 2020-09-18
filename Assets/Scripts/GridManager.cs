using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GridManager : MonoBehaviour
{
    //----------------------------------------------//
    //                    public                    //
    //----------------------------------------------//

    public GameObject pixelPrefab;
    public int divider = 32;
    [ShowInInspector, ReadOnly]
    public bool colorChangerOpen = false;

    //----------------------------------------------//
    //                   private                    //
    //----------------------------------------------//

    private Vector3 min;
    private Vector3 max;
    private Vector3 topLeft, topRight, bottomLeft, bottomRight;

    private Vector2 gridSize;
    private Vector2 sideLength;

    private float spriteSideLength;
    private TileScript[,] tileScripts;
    private float tileSize;

    private Color[,] temp;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
        temp = new Color[(int)gridSize.x, (int)gridSize.y];
    }

    void Start()
    {
        GetScreenInfo();
        DrawGrid();
    }

    private void DrawGrid()
    {
        tileScripts = new TileScript[(int)gridSize.x, (int)gridSize.y];
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject clone = Instantiate(pixelPrefab,
                                               new Vector3(min.x + (sideLength.x / gridSize.x * 0.5f) + (sideLength.x / gridSize.x * x),
                                                           min.y + (sideLength.y / gridSize.y * 0.5f) + (sideLength.y / gridSize.y * y), 0),
                                               Quaternion.identity);
                clone.transform.localScale = Vector2.one * tileSize;
                clone.transform.parent = this.transform;
                tileScripts[x, y] = clone.GetComponent<TileScript>();
            }
        }
    }

    private void GetScreenInfo()
    {
        //Get min and max points of screen
        min = cam.ScreenToWorldPoint(Vector3.zero);
        max = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        //Isolating the corners
        topRight = max + new Vector3(0, 0, 10);
        topLeft = new Vector3(min.x, max.y, 0);
        bottomLeft = min + new Vector3(0, 0, 10);
        bottomRight = new Vector3(max.x, min.y, 0);


        sideLength = new Vector2(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y));
        spriteSideLength = pixelPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        gridSize = new Vector2(Screen.width / divider, Screen.height / divider);

        double worldScreenHeight = cam.orthographicSize * 2.0;
        double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        tileSize = (float)(worldScreenWidth / spriteSideLength) / gridSize.x;

        print("Grid: " + gridSize);
    }


    void Update()
    {
        // DrawBoundaries();

        if (Input.GetKeyDown(KeyCode.P))
        {
            string date = System.DateTime.Now.ToString();
            date = date.Replace("/", "-");
            date = date.Replace(" ", "_");
            date = date.Replace(":", "-");
            ScreenCapture.CaptureScreenshot("MasterPeace - " + date + ".png", 2);
        }
    }

    public void OpenColorChanger()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                temp[x, y] = tileScripts[x, y].GetColor();
                Color final = new Color();
                if (x < gridSize.x / 2)
                {
                    final = new HSBColor(y / gridSize.y, 1, x / gridSize.x * 2).ToColor();
                }
                else
                {
                    final = new HSBColor(y / gridSize.y, Mathf.InverseLerp(gridSize.x, gridSize.x / 2, x), 1).ToColor();
                }

                tileScripts[x, y].ColorTile(final);
            }
        }
        colorChangerOpen = true;
    }

    public void CloseColorChanger()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                tileScripts[x, y].ColorTile(temp[x, y]);
            }
        }
        colorChangerOpen = false;
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
        Vector3 pos = cam.WorldToScreenPoint(cursorPos) / divider;
        Vector2Int pos2D = new Vector2Int(Mathf.FloorToInt(pos.x - (spriteSideLength / 2)), Mathf.FloorToInt(pos.y - (spriteSideLength / 2)));
        print("Pos2D: " + pos2D);
        return tileScripts[pos2D.x, pos2D.y];
    }
}
