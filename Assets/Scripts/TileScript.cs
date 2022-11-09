using UnityEngine;

public class TileScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public void Init()
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

    public Color GetColor()
    {
        return spriteRenderer.color;
    }

}
