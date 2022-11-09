using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //----------------------------------------------//
    //                    public                    //
    //----------------------------------------------//

    public GameObject pixelPrefab;
    public int gridSizeDivider = 32;
    public List<Transform> TileTransforms { get; private set; }
    public bool ColorChangerOpen => colorChangerOpen;
    public Vector2 GridSize => gridSize;

    public Vector3 ScreenWorldspaceMin => screenWorldspaceMin;
    public Vector3 ScreenWorldspaceMax => screenWorldspaceMax;

    //----------------------------------------------//
    //                   private                    //
    //----------------------------------------------//

    Color defaultColor = Color.gray;

    private bool colorChangerOpen = false;
    private Vector3 screenWorldspaceMin;
    private Vector3 screenWorldspaceMax;

    private Vector2 gridSize;
    private Vector2 sideLengths;

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

    // TODO: Make asynchronous solution
    private void DrawGrid()
    {
        tileScripts = new TileScript[Mathf.RoundToInt(gridSize.x), Mathf.RoundToInt((int)gridSize.y)];
        TileTransforms = new List<Transform>();
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject clone = Instantiate(pixelPrefab,
                                               new Vector3(screenWorldspaceMin.x + (sideLengths.x / gridSize.x * 0.5f) + (sideLengths.x / gridSize.x * x),
                                                           screenWorldspaceMin.y + (sideLengths.y / gridSize.y * 0.5f) + (sideLengths.y / gridSize.y * y), 0),
                                               Quaternion.identity);
                clone.transform.localScale = Vector2.one * tileSize;
                clone.transform.parent = this.transform;
                TileScript cloneTile = clone.GetComponent<TileScript>();
                cloneTile.Init();
                cloneTile.ColorTile(defaultColor);
                tileScripts[x, y] = cloneTile;
                TileTransforms.Add(clone.transform);
            }
        }
    }

    private void GetScreenInfo()
    {
        screenWorldspaceMin = cam.ScreenToWorldPoint(Vector3.zero);
        screenWorldspaceMax = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        sideLengths = new Vector2(Mathf.Abs(screenWorldspaceMax.x - screenWorldspaceMin.x), Mathf.Abs(screenWorldspaceMax.y - screenWorldspaceMin.y));
        spriteSideLength = pixelPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        gridSize = new Vector2(Screen.width / gridSizeDivider, Screen.height / gridSizeDivider);
        currentImageBackup = new Color[(int)gridSize.x, (int)gridSize.y];

        double worldScreenHeight = cam.orthographicSize * 2.0;
        double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        tileSize = (float)Math.Round(0.005 + (worldScreenWidth / spriteSideLength) / gridSize.x, 2);
        print("Grid: " + gridSize + ". TileSize: " + tileSize);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            TakeScreenShotOfImage();
        }
    }

    // TODO: Remove cursor from picture
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
