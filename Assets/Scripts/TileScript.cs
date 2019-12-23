using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // void OnMouseEnter()
    // {
    //     spriteRenderer.color = Color.gray;
    // }

    // void OnMouseExit()
    // {
    //     spriteRenderer.color = Color.white;
    // }

    public bool ColorTile(Color color)
    {
        if (spriteRenderer.color == color)
        {
            return false;
        }
        else
        {
            spriteRenderer.color = color;
            return true;
        }
    }

}
