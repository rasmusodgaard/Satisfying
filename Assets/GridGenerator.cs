using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    Vector3 screenSizeMax;
    Vector3 screenSizeMin;

    Vector3 topLeft, topRight, bottomLeft, bottomRight;

    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        topLeft = Vector3.zero;
        topRight = Vector3.zero;
        bottomLeft = Vector3.zero;
        bottomRight = Vector3.zero;


        screenSizeMin = Vector3.zero;
        screenSizeMax = new Vector3(Screen.width, Screen.height, 0);

        print("min: " + screenSizeMin);
        print("max: " + screenSizeMax);
        screenSizeMin = Camera.main.ScreenToWorldPoint(Vector3.zero);
        screenSizeMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Instantiate(prefab, screenSizeMax, Quaternion.identity);
        print("min: " + screenSizeMin);
        print("max: " + screenSizeMax);
        topLeft = screenSizeMin;
        topRight = new Vector3(screenSizeMax.x, screenSizeMin.y, 0);
        bottomLeft = new Vector3(screenSizeMin.x, screenSizeMax.y, 0);
        bottomRight = screenSizeMax;

        print("tl: " + topLeft);
        print("tr: " + topRight);
        print("bl: " + bottomLeft);
        print("br: " + bottomRight);

    }

    // Update is called once per frame
    void Update()
    {
        DrawBoundaryRays();
    }

    private void DrawBoundaryRays()
    {
        Debug.DrawLine(topLeft, bottomRight, Color.red);
        // Debug.DrawLine(topRight, bottomRight, Color.blue);
        // Debug.DrawLine(bottomRight, bottomLeft, Color.red);
        // Debug.DrawLine(bottomLeft, topLeft, Color.blue);
    }
}
