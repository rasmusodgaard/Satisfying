using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public GridManager grid;
    public Color standard;
    public Color over;
    private Color paintColor = Color.cyan;

    private Camera cam;
    Vector3 mousePos = new Vector3();
    private SpriteRenderer cursorSprite;

    //---------------------------------------------------//
    //Brush general
    //---------------------------------------------------//
    private enum BrushEnum
    {
        standard, magnet, turn, colorChanger
    }

    private BrushEnum brush = new BrushEnum();
    private BrushEnum lastBrush = new BrushEnum();
    private BrushEnum tempBrush = new BrushEnum();

    private TileScript lastTile;
    private float radius = 0.1f;
    private ContactFilter2D contactFilter2D = new ContactFilter2D();

    public Brush_Base[] brushes;


    //---------------------------------------------------//
    //  ColorPicker
    //---------------------------------------------------//
    private bool colorChangerOn = false;


    //---------------------------------------------------//
    //  MagnetBrush
    //---------------------------------------------------//
    private List<Collider2D> magnetResults;
    public float magnetSpeed = 1;
    public float minSqDist = 5;
    public float maxSqDist = 75;

    //---------------------------------------------------//
    //  SmearBrush
    //---------------------------------------------------//

    private int tilesPaintedInStroke = 0;
    public int maxStrokes = 42;
    public float radiusDivider = 100;

    void Start()
    {
        cam = Camera.main;
        Cursor.visible = false;
        this.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
        brush = BrushEnum.standard;
        magnetResults = new List<Collider2D>();
        Camera.main.backgroundColor = Color.black;
        cursorSprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;

        BrushSize();

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
            if (Input.GetMouseButton(0))
            {
                brushes[(int)brush].Primary(mousePos, radius, paintColor);
            }
            else if (Input.GetMouseButton(1))
            {
                brushes[(int)brush].Secondary(mousePos, radius, paintColor);
            }
        }

        // Seperate logic for color changer
        else if (colorChangerOn)
        {
            ColorChangerUpdate(mousePos);
        }
    }

    private void BrushSize()
    {
        if (!Mathf.Approximately(0, Input.mouseScrollDelta.y))
        {
            radius += Input.mouseScrollDelta.y / radiusDivider;
            radius = Mathf.Clamp(radius, 0.05f, 3);
        }
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
        // else if (Input.GetKeyDown(KeyCode.S))
        // {
        //     brush = BrushEnum.smear;
        // }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            brush = BrushEnum.turn;
        }

        //Open and close color changer
        if (colorChangerOn && !grid.colorChangerOpen)
        {
            grid.OpenColorChanger();
            //brush = BrushEnum.colorChanger;
            brush = BrushEnum.standard;
        }
        else if (!colorChangerOn && grid.colorChangerOpen)
        {
            grid.CloseColorChanger();
            brush = tempBrush;
        }

        if (brush != lastBrush)
        {
            print("Changed to: " + brush);
        }

    }


    private void ColorChangerUpdate(Vector3 _mousePos)
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(_mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                TileScript tile = hit.collider.GetComponent<TileScript>();
                if (tile == null)
                {
                    print("No hit");
                }
                else
                {
                    print("Hit: " + tile.GetColor());
                }
                paintColor = tile.GetColor();
                cursorSprite.color = tile.GetColor();

            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(_mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                TileScript tile = hit.collider.GetComponent<TileScript>();

                Camera.main.backgroundColor = tile.GetColor();
            }
        }
    }
}
