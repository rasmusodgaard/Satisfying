﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorScript : MonoBehaviour
{ // TODO: Clean up inspector workflow with tool tips and custom editor drawers
    [SerializeField]
    GridManager grid;

    [SerializeField]
    SpriteRenderer brushSizeVisualizer;

    [SerializeField]
    Color defaultTileColor = Color.gray;

    [SerializeField]
    Color defaultBackgroundColor = Color.white;

    [SerializeField]
    Color defaultPaintColor = Color.black;

    [SerializeField]
    float alphaFadeScalar;

    [SerializeField]
    [Range(0f, 1f)]
    float brushSizeVisualizerAlpha;

    [SerializeField]
    float scrollDeltaScalar = 100;

    [SerializeField]
    float brushRadiusStep;

    [SerializeField]
    float brushChangeFadeDelay;
    float brushChangeDelayTemp;

    [SerializeField]
    float minBrushSize;

    [SerializeField]
    float maxBrushSize;

    [SerializeField]
    BrushBase[] brushes;

    Camera cam;
    Color paintColor;
    Vector3 mousePosition;
    BrushBase activeBrush;
    Dictionary<BrushEnum, BrushBase> brushDictionary = new Dictionary<BrushEnum, BrushBase>();
    float brushRadius = 0.25f;

    void Awake()
    {
        cam = Camera.main;
        Camera.main.backgroundColor = defaultBackgroundColor;
        brushSizeVisualizer.transform.localScale = Vector3.one * brushRadius * 2;

        SetupBrushDictionary();
        SwitchBrush(BrushEnum.standard);
        SetColor(defaultPaintColor);
    }

    private void SetupBrushDictionary()
    {
        foreach (BrushBase brush in brushes)
        {
            brushDictionary.Add(brush.BrushType, brush);
        }
    }

    private void OnEnable()
    {
        if (brushDictionary.TryGetValue(BrushEnum.standard, out BrushBase brushBase))
        {
            var standardBrush = (StandardBrush)brushBase;
            standardBrush.SetColor += SetColor;
        }
    }

    private void OnDisable()
    {
        if (brushDictionary.TryGetValue(BrushEnum.standard, out BrushBase brushBase))
        {
            var standardBrush = (StandardBrush)brushBase;
            standardBrush.SetColor -= SetColor;
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
                activeBrush.Primary(mousePosition, brushRadius, paintColor, grid.TileTransforms);
            }
            else if (Input.GetMouseButton(1))
            {
                activeBrush.Secondary(mousePosition, brushRadius, paintColor, grid.TileTransforms);
            }
        }
        else
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            PaletteUpdate(mousePosition);
        }
    }

    private void CheckForHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!grid.ColorChangerOpen)
            {
                grid.OpenPalette();
            }
            else
            {
                grid.ClosePalette();
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
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SwitchBrush(BrushEnum.reset);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SwitchBrush(BrushEnum.sprite);
        }
    }

    // TODO: Fix bug with wrong visualizer size after making cursor smaller
    private void CheckForBrushSizeChanges()
    {
        float scaledScrollDelta = Input.mouseScrollDelta.y * scrollDeltaScalar;

        if (!Mathf.Approximately(0, scaledScrollDelta))
        {
            brushRadius += (scaledScrollDelta > 0) ? brushRadiusStep * Time.deltaTime : -brushRadiusStep * Time.deltaTime;
            brushRadius = Mathf.Clamp(brushRadius, minBrushSize, maxBrushSize);
            brushSizeVisualizer.transform.localScale = Vector3.one * brushRadius * 2;
            brushChangeDelayTemp = brushChangeFadeDelay;
            AdjustBrushSizeVisualizerAlpha(true);
        }
        else
        {
            if (brushChangeDelayTemp <= 0)
            {
                AdjustBrushSizeVisualizerAlpha(false);
            }
            else
            {
                brushChangeDelayTemp -= Time.deltaTime;
            }
        }
    }

    private void AdjustBrushSizeVisualizerAlpha(bool isIncreasingAlpha)
    {
        if (isIncreasingAlpha)
        {
            Color tempColor = brushSizeVisualizer.color;
            tempColor.a = brushSizeVisualizerAlpha;
            brushSizeVisualizer.color = tempColor;
        }
        else
        {
            if (Mathf.Approximately(brushSizeVisualizer.color.a, 0.0f))
            {
                return;
            }

            Color tempColor = brushSizeVisualizer.color;
            tempColor.a = Mathf.Clamp(tempColor.a - alphaFadeScalar * Time.deltaTime, 0.0f, brushSizeVisualizerAlpha);
            brushSizeVisualizer.color = tempColor;
        }
    }

    public void SwitchBrush(BrushEnum brush)
    {
        if (brushDictionary.TryGetValue(brush, out BrushBase value))
        {
            activeBrush = value;
            ChangeBrushCursor(value);
        }
        else
        {
            Debug.LogError("No such brush found");
        }
    }

    private void ChangeBrushCursor(BrushBase value)
    {
        Cursor.SetCursor(value.cursorTexture, value.CursorRelativeTexturePosition, CursorMode.Auto);
    }

    public void SwitchBrush(BrushBase brush)
    {
        activeBrush = brush;
        ChangeBrushCursor(brush);
    }

    public void SetColor(Color color)
    {
        paintColor = color;
    }

    private void PaletteUpdate(Vector3 mousePos)
    {
        if (activeBrush.BrushType == BrushEnum.standard)
        {
            Color newColor = new Color();
            newColor = GetPaletteColor(mousePos);

            if (Input.GetMouseButtonDown(0))
            {
                paintColor = newColor;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                cam.backgroundColor = newColor;
            }
        }
    }

    private Color GetPaletteColor(Vector3 mousePos)
    {
        Color newColor;
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

        return newColor;
    }
}