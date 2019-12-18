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
    void Start()
    {
        min = Camera.main.ScreenToWorldPoint(Vector3.zero);
        max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        topRight = max + new Vector3(0, 0, 10);
        topLeft = new Vector3(min.x, max.y, 0);
        bottomLeft = min + new Vector3(0, 0, 10);
        bottomRight = new Vector3(max.x, min.y, 0);
        print("tr: " + topRight);
        print("tl: " + topLeft);
        print("bl: " + bottomLeft);
        print("br: " + bottomRight);



        float side = Mathf.Abs(min.x - max.x);
        float scale = side / prefab.GetComponent<SpriteRenderer>().bounds.size.x;
        print("side: " + side + " scale: " + scale);
        for (float i = min.x; i <= max.x; i += scale)
        {
            Instantiate(prefab, new Vector3(i + (scale / 2), 0.0f, 0.0f), Quaternion.identity);
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
