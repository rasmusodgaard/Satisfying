using UnityEngine;

public class TileScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public SpriteRenderer GetSpriteRenderer => spriteRenderer;
    Color initColor;
    Vector3 initPosition;
    Sprite initSprite;

    public void Init(Color defaultColor)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initColor = defaultColor;
        SetColor(defaultColor);
        initPosition = transform.localPosition;
        initSprite = spriteRenderer.sprite;
    }

    public void ResetTransformation(Sprite sprite = null)
    {
        transform.position = initPosition;
        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = (sprite == null) ? initSprite : sprite;
    }

    public void ResetColor()
    {
        SetColor(initColor);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public Color GetColor()
    {
        return spriteRenderer.color;
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }

    public bool SetColor(Color color)
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

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetAllTransformations(Vector3 position, Quaternion rotation, Sprite sprite)
    {
        transform.position = position;
        transform.rotation = rotation;
        spriteRenderer.sprite = sprite;
    }
}
