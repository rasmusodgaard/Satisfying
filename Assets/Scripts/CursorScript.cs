using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorScript : MonoBehaviour
{
    public GridManager grid;

    [SerializeField]
    Color defaultTileColor = Color.gray;

    [SerializeField]
    Color defaultBackgroundColor = Color.white;

    Color paintColor = Color.black;

    Camera cam;
    Vector3 mousePosition = new Vector3();

    [SerializeField]
    SpriteRenderer cursorSpriteRenderer;

    [SerializeField]
    SpriteRenderer brushSizeVisualizer;
    //---------------------------------------------------//
    //Brush general
    //---------------------------------------------------//
    [SerializeField]
    Dictionary<BrushEnum, BrushBase> brushDictionary = new Dictionary<BrushEnum, BrushBase>();
    BrushBase activeBrush;

    [SerializeField]
    float brushRadius = 0.25f;
    public BrushBase[] brushes;
    public float radiusDivider = 100;

    //---------------------------------------------------//
    //  ColorPicker
    //---------------------------------------------------//
    //bool colorChangerOn = false;

    //---------------------------------------------------//
    //  MagnetBrush
    //---------------------------------------------------//
    public float magnetSpeed = 1;
    public float minSqDist = 5;
    public float maxSqDist = 75;


    void Start()
    {
        cam = Camera.main;
        Cursor.visible = false;
        transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
        Camera.main.backgroundColor = Color.black;
        brushSizeVisualizer.transform.localScale = Vector3.one * brushRadius * 2;
        SetupBrushDisctionary();
        SwitchBrush(BrushEnum.standard);
    }

    private void SetupBrushDisctionary()
    {
        foreach (BrushBase brush in brushes)
        {
            brushDictionary.Add(brush.BrushType, brush);
        }
    }

    void Update()
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;

        CheckForBrushSizeChanges();
        CheckForHotkeys();
        Paint();
    }

    private void Paint()
    {
        if (grid.ColorChangerOpen == false)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                //brushes[(int)brush].Primary(mousePos, brushRadius, paintColor, grid.TileTransforms);
                activeBrush.Primary(mousePosition, brushRadius, paintColor, grid.TileTransforms);
            }
            else if (Input.GetMouseButton(1))
            {
                //brushes[(int)brush].Secondary(mousePos, brushRadius, paintColor, grid.TileTransforms);
                activeBrush.Secondary(mousePosition, brushRadius, paintColor, grid.TileTransforms);
            }
        }
        else
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            ColorChangerUpdate(mousePosition);
        }
    }

    private void CheckForHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!grid.ColorChangerOpen)
            {
                grid.OpenColorChanger();
            }
            else
            {
                grid.CloseColorChanger();
            }
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchBrush(BrushEnum.standard);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchBrush(BrushEnum.magnet);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchBrush(BrushEnum.turn);
        }
    }

    // TODO: Fade brush size visualizer in when changed and out when not
    private void CheckForBrushSizeChanges()
    {
        if (!Mathf.Approximately(0, Input.mouseScrollDelta.y))
        {
            brushRadius += Input.mouseScrollDelta.y / radiusDivider; // TODO: Make time dependant instead of mouse scroll delta
            brushRadius = Mathf.Clamp(brushRadius, 0.05f, 3);
            brushSizeVisualizer.transform.localScale = Vector3.one * brushRadius * 2;
        }
    }

    public void SwitchBrush(BrushEnum brush)
    {
        if (brushDictionary.TryGetValue(brush, out BrushBase value))
        {
            activeBrush = value;
        }
        else
        {
            Debug.LogError("No such brush found");
        }
    }

    public void Test() { }

    private void ColorChangerUpdate(Vector3 mousePos)
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Color newColor = new Color();
            if (mousePos.x < 0)
            {
                // On the left side of the screen, colors go to black from full hue by lerping brightness value
                newColor = new HSBColor(Mathf.InverseLerp(grid.ScreenWorldspaceMin.y, grid.ScreenWorldspaceMax.y, mousePos.y),
                                          1,
                                          Mathf.InverseLerp(grid.ScreenWorldspaceMin.x, 0, mousePos.x)).ToColor();
            }
            else
            {
                // On the right side of the screen, colors go to white from full hue by lerping the saturation value
                newColor = new HSBColor(Mathf.InverseLerp(grid.ScreenWorldspaceMin.y, grid.ScreenWorldspaceMax.y, mousePos.y),
                                          Mathf.InverseLerp(grid.ScreenWorldspaceMax.x, 0, mousePos.x),
                                          1).ToColor();
            }

            if (Input.GetMouseButtonDown(0))
            {
                paintColor = newColor;
                cursorSpriteRenderer.color = newColor;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                cam.backgroundColor = newColor;
            }
        }
    }
}