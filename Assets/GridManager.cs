using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject prefab;

    Vector3 min;
    Vector3 max;

    Vector3 topLeft, topRight, bottomLeft, bottomRight;
    private Vector2 gridSize;

    void Start()
    {
        min = Camera.main.ScreenToWorldPoint(Vector3.zero);
        max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        Vector2 sideLength = new Vector2(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y));
        float spriteSideLength = prefab.GetComponent<SpriteRenderer>().bounds.size.x;



        gridSize = new Vector2(Screen.width / 16, Screen.height / 16);

        topRight = max + new Vector3(0, 0, 10);
        topLeft = new Vector3(min.x, max.y, 0);
        bottomLeft = min + new Vector3(0, 0, 10);
        bottomRight = new Vector3(max.x, min.y, 0);

        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        double width = sr.sprite.bounds.size.x;
        Debug.Log("width: " + width);
        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;



        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        go.transform.localScale = Vector2.one * (float)(worldScreenWidth / width) / gridSize.x;
        float tileSize = (float)(worldScreenWidth / width) / gridSize.x;
        go.transform.localScale *= 0.9f;

        for (float y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Instantiate(go, new Vector3(min.x + (sideLength.x / gridSize.x) * 0.5f + (sideLength.x / gridSize.x * x), min.y + (sideLength.x / gridSize.x) * 0.5f + (sideLength.y / gridSize.y * y), 0), Quaternion.identity);
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
}
