using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //----------------------------------------------//
    //                    public                    //
    //----------------------------------------------//

    public GameObject pixelPrefab;
    public int divider = 32;
    public bool ColorChangerOpen { get => colorChangerOpen; private set => colorChangerOpen = value; }
    public List<Transform> TileTransforms { get; private set; }

    //----------------------------------------------//
    //                   private                    //
    //----------------------------------------------//

    private bool colorChangerOpen = false;
    private Vector3 min;
    private Vector3 max;
    private Vector3 topLeft, topRight, bottomLeft, bottomRight;

    private Vector2 gridSize;
    private Vector2 sideLength;

    private float spriteSideLength;
    private TileScript[,] tileScripts;
    private float tileSize;

    private Color[,] currentImageBackup;

    private Camera cam;


    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        GetScreenInfo();
        DrawGrid();
    }

    private void DrawGrid()
    {
        tileScripts = new TileScript[Mathf.RoundToInt(gridSize.x), Mathf.RoundToInt((int)gridSize.y)];
        TileTransforms = new List<Transform>();
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
                TileTransforms.Add(clone.transform);
            }
        }
    }

    private void GetScreenInfo()
    {
        //Get min and max points of screen
        min = cam.ScreenToWorldPoint(Vector3.zero);
        max = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        sideLength = new Vector2(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y));
        spriteSideLength = pixelPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        gridSize = new Vector2(Screen.width / divider, Screen.height / divider);
        currentImageBackup = new Color[(int)gridSize.x, (int)gridSize.y];

        double worldScreenHeight = cam.orthographicSize * 2.0;
        double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        tileSize = (float)Math.Round(0.0005 + (worldScreenWidth / spriteSideLength) / gridSize.x, 3);
        print("Grid: " + gridSize + ". TileSize: " + tileSize);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenShotOfImage();
        }
    }

    private static void TakeScreenShotOfImage()
    {
        string date = System.DateTime.Now.ToString();
        date = date.Replace("/", "-");
        date = date.Replace(" ", "_");
        date = date.Replace(":", "-");
        ScreenCapture.CaptureScreenshot("Creations\\MasterPeace - " + date + ".png", 2); // TODO: Handle image download from webbuild
    }

    public void OpenColorChanger()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                currentImageBackup[x, y] = tileScripts[x, y].GetColor();
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
                tileScripts[x, y].ColorTile(currentImageBackup[x, y]);
            }
        }
        colorChangerOpen = false;
    }
}
