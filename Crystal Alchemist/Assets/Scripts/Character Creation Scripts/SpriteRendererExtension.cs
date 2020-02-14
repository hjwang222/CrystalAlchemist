using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererExtension : MonoBehaviour
{
    [SerializeField]
    private bool canChangeColor = true;

    public SpriteRenderer spriteRenderer;

    public List<Color> colors = new List<Color>();

    private void Awake()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void init()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        if (this.colors.Count == 0) this.colors.Add(this.spriteRenderer.color);
    }

    public void resetColors()
    {
        if (this.canChangeColor)
        {
            if (this.colors.Count > 0) this.spriteRenderer.color = this.colors[0];
            this.colors.Clear();
            this.changeColor(this.spriteRenderer.color);
        }
    }

    public void removeColor(Color color)
    {
        if (this.canChangeColor)
        {
            if (this.colors.Count > 1) this.colors.Remove(color); //dont remove first color
            this.spriteRenderer.color = this.colors[this.colors.Count - 1];
        }
    }

    public void enableSpriteRenderer(bool value)
    {
        this.spriteRenderer.enabled = value;
    }

    public void changeColor(Color color)
    {
        if (this.canChangeColor)
        {
            if (this.colors.Contains(color))
            {
                this.spriteRenderer.color = this.colors[this.colors.IndexOf(color)];
            }
            else
            {
                this.colors.Add(color);
                this.spriteRenderer.color = this.colors[this.colors.Count - 1];
            }
        }
    }

    public void setStartColor()
    {
        if (this.canChangeColor)
        {
            this.spriteRenderer.color = this.colors[0];
        }
    }

    public void flipSprite(Vector2 direction)
    {
        if (direction.x < 0) this.spriteRenderer.flipX = true;
        else this.spriteRenderer.flipX = false;
    }
}
