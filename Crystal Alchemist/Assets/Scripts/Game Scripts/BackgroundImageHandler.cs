using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImageHandler : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void SetSprite(Sprite sprite)
    {
        if (this.spriteRenderer != null) this.spriteRenderer.sprite = sprite;
    }

    public void SetScale(float value)
    {
        this.transform.localScale = new Vector3(value, value);
    }

    public void SetPosition(float value)
    {
        this.transform.localPosition = new Vector3(0, value, 1);
    }
}
