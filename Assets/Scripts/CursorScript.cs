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
    private bool colorChangerOn = false;

    private TileScript lastTile;
    private float radius = 0.1f;
    private ContactFilter2D contactFilter2D = new ContactFilter2D();



    //---------------------------------------------------//
    //ColorPicker
    //---------------------------------------------------//

    //---------------------------------------------------//
    //MagnetBrush
    //---------------------------------------------------//
    private List<Collider2D> magnetResults;

    //---------------------------------------------------//
    //SmearBrush
    //---------------------------------------------------//

    private int tilesPaintedInStroke = 0;
    public int maxStrokes = 10;

    void Start()
    {
        cam = Camera.main;
        Cursor.visible = false;
        this.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
        brush = BrushEnum.turn;
        magnetResults = new List<Collider2D>();
    }

    void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePos;

        SwitchBrush();

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

            // TileScript tile = grid.GetTile(this.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(_mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                TileScript tile = hit.collider.GetComponent<TileScript>();
                if (tile != lastTile && tile != null)
                {
                    print("painted");
                    tile.ColorTile(paintColor);
                }
                lastTile = tile;
            }

        }
    }
}
