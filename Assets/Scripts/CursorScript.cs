using System;
using System.Collections;
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

    //---------------------------------------------------//
    //Brush general
    //---------------------------------------------------//
    private enum BrushEnum
    {
        standard, magnet, smear, turn, colorChanger
    }

    private BrushEnum brush = new BrushEnum();
    private BrushEnum lastBrush = new BrushEnum();
    private BrushEnum tempBrush = new BrushEnum();

    private TileScript lastTile;
    private float radius = 0.1f;
    private ContactFilter2D contactFilter2D = new ContactFilter2D();



    //---------------------------------------------------//
    //ColorPicker
    //---------------------------------------------------//
    private bool colorChangerOn = false;

    //---------------------------------------------------//
    //StandardBrush
    //---------------------------------------------------//
    private List<Collider2D> hoverList = new List<Collider2D>();
    private List<Collider2D> tempList = new List<Collider2D>();

    //---------------------------------------------------//
    //MagnetBrush
    //---------------------------------------------------//
    private List<Collider2D> magnetResults;
    public float magnetSpeed = 1;
    public float minSqDist = 5;
    public float maxSqDist = 75;

    //---------------------------------------------------//
    //SmearBrush
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
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePos;

        BrushSize();

        SwitchBrush();

    }

    void FixedUpdate()
    {
        switch (brush)
        {
            case BrushEnum.colorChanger:
                ColorChangerUpdate(mousePos);
                break;

            case BrushEnum.standard:
                StandardUpdate(mousePos);
                break;

            case BrushEnum.smear:
                SmearUpdate(mousePos);
                break;

            case BrushEnum.turn:
                TurnUpdate(mousePos);
                break;

            case BrushEnum.magnet:
                MagnetUpdate(mousePos);
                break;
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
        else if (Input.GetKeyDown(KeyCode.S))
        {
            brush = BrushEnum.smear;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            brush = BrushEnum.turn;
        }

        //Open and close color changer
        if (colorChangerOn && !grid.colorChangerOpen)
        {
            grid.OpenColorChanger();
            brush = BrushEnum.colorChanger;
            print("Open");
        }
        else if (!colorChangerOn && grid.colorChangerOpen)
        {
            grid.CloseColorChanger();
            brush = tempBrush;
            print("Close");
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

                paintColor = tile.GetColor();
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

    private void TurnUpdate(Vector3 _mousePos)
    {
        if (Input.GetMouseButton(0))
        {
            magnetResults.Clear();
            Physics2D.OverlapCircle(_mousePos, radius, contactFilter2D, magnetResults);
            foreach (var item in magnetResults)
            {
                item.transform.LookAt(_mousePos);
            }
        }
        else if (Input.GetMouseButton(1))
        {
            magnetResults.Clear();
            Physics2D.OverlapCircle(_mousePos, radius, contactFilter2D, magnetResults);
            foreach (var item in magnetResults)
            {
                item.transform.rotation = Quaternion.identity;
            }
        }
    }

    private void MagnetUpdate(Vector3 _mousePos)
    {
        //pull
        if (Input.GetMouseButton(0))
        {
            magnetResults.Clear();
            Physics2D.OverlapCircle(_mousePos, radius, contactFilter2D, magnetResults);
            foreach (var item in magnetResults)
            {
                float sqDist = Vector3.SqrMagnitude(_mousePos - item.transform.position);
                if (sqDist > minSqDist)
                {
                    Vector3 direction = (_mousePos - item.transform.position).normalized;
                    item.transform.position += direction * magnetSpeed * Time.fixedDeltaTime;
                }
            }
        }
        //push
        else if (Input.GetMouseButton(1))
        {
            magnetResults.Clear();
            Physics2D.OverlapCircle(_mousePos, radius, contactFilter2D, magnetResults);
            foreach (var item in magnetResults)
            {
                float sqDist = Vector3.SqrMagnitude(_mousePos - item.transform.position);

                Vector3 direction = (_mousePos - item.transform.position).normalized;
                item.transform.position += -direction * magnetSpeed * Time.fixedDeltaTime;

            }
        }
    }

    private void SmearUpdate(Vector3 _mousePos)
    {
        if (Input.GetMouseButtonDown(0))
        {
            tilesPaintedInStroke = 0;
        }

        if (Input.GetMouseButton(0))
        {

            // TileScript tile = grid.GetTile(this.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(_mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                TileScript tile = hit.collider.GetComponent<TileScript>();
                if (tile != lastTile && tile != null)
                {
                    print("painted");
                    tile.ColorTile(Color.Lerp(paintColor, Color.grey, (float)tilesPaintedInStroke / (float)maxStrokes));
                    tilesPaintedInStroke = Mathf.Clamp(tilesPaintedInStroke, 0, maxStrokes) + 1;
                }
                lastTile = tile;
            }

        }
    }

    private void StandardUpdate(Vector3 _mousePos)
    {
        if (Input.GetMouseButton(0))
        {

            // // TileScript tile = grid.GetTile(this.transform.position);
            // RaycastHit2D hit = Physics2D.Raycast(_mousePos, Vector2.zero);

            // if (hit.collider != null)
            // {
            //     TileScript tile = hit.collider.GetComponent<TileScript>();
            //     if (tile != lastTile && tile != null)
            //     {
            //         print("painted");
            //         tile.ColorTile(paintColor);
            //     }
            //     lastTile = tile;
            // }

            hoverList.Clear();
            Physics2D.OverlapCircle(_mousePos, radius, contactFilter2D, hoverList);
            foreach (var item in hoverList)
            {
                TileScript tile = item.GetComponent<TileScript>();
                if (tile != null)
                {
                    print("painted");
                    tile.ColorTile(paintColor);
                }
            }
        }
    }
}
