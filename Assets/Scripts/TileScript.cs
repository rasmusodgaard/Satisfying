using UnityEngine;

public class TileScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color initColor;
    Vector3 initPosition;

    public void Init(Color defaultColor)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initColor = defaultColor;
        initPosition = transform.localPosition;

    }

    public void ResetTransform()
    {
        transform.position = initPosition;
        transform.rotation = Quaternion.identity;
    }

    public void ResetColor()
    {
        ColorTile(initColor);
    }

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
