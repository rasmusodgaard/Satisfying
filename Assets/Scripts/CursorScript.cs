using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorScript : MonoBehaviour
{
    public GridManager grid;
    public Color defaultColor = Color.gray;
    private Color paintColor = Color.black;

    private Camera cam;
    Vector3 mousePos = new Vector3();

    [SerializeField]
    private SpriteRenderer cursorSpriteRenderer;

    [SerializeField]
    private SpriteRenderer brushSizeVisualizer;
    //---------------------------------------------------//
    //Brush general
    //---------------------------------------------------//
    Dictionary<BrushEnum, BrushBase> brushDictionary = new Dictionary<BrushEnum, BrushBase>();

    private BrushEnum brush = new BrushEnum();
    private BrushEnum lastBrush = new BrushEnum();
    private BrushEnum tempBrush = new BrushEnum();

    private float brushRadius = 0.1f;
    public BrushBase[] brushes;
    public float radiusDivider = 100;

    float brushVisializationTargetAlpha = 100;

    //---------------------------------------------------//
    //  ColorPicker
    //---------------------------------------------------//
    private bool colorChangerOn = false;


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
        this.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
        brush = BrushEnum.standard;
        Camera.main.backgroundColor = Color.black;
        brushSizeVisualizer.transform.localScale = Vector3.one * brushRadius * 2;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;

        CheckForBrushSizeChanges();
        SwitchBrush();
    }

    void LateUpdate()
    {
        //Make sure that a brush that is not in the brushes array is not selected
        if ((int)brush >= brushes.Length)
        {
            print("Out of bounds brush selection");
            return;
        }

        // General brush behaviour
        if (!colorChangerOn)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                brushes[(int)brush].Primary(mousePos, brushRadius, paintColor, grid.TileTransforms);
            }
            else if (Input.GetMouseButton(1))
            {
                brushes[(int)brush].Secondary(mousePos, brushRadius, paintColor, grid.TileTransforms);
            }
        }

        // Seperate logic for color changer
        else if (colorChangerOn)
        {
            ColorChangerUpdate(mousePos);
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

    }

    private void SwitchBrush()
    {
        lastBrush = brush;

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!colorChangerOn)
            {
                tempBrush = lastBrush;
            }
            colorChangerOn = !colorChangerOn;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            brush = BrushEnum.standard;
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            brush = BrushEnum.magnet;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            brush = BrushEnum.turn;
        }

        // TODO: Make change to color changer code easier to read
        //Open and close color changer
        if (colorChangerOn && !grid.ColorChangerOpen)
        {
            grid.OpenColorChanger();
            //brush = BrushEnum.colorChanger;
            brush = BrushEnum.standard;
        }
        else if (!colorChangerOn && grid.ColorChangerOpen)
        {
            grid.CloseColorChanger();
            brush = tempBrush;
        }

        if (brush != lastBrush)
        {
            print("Changed to: " + brush);
        }
    }


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