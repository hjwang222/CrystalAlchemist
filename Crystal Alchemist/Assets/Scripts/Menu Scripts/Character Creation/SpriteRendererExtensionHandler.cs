using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpriteRendererExtensionHandler : MonoBehaviour
{
    [SerializeField]
    [Required]
    private GameObject characterSprite;

    [HideInInspector]
    public List<SpriteRendererExtension> colorpalettes = new List<SpriteRendererExtension>();

#if UNITY_EDITOR
    [Button]
    public void setExtensions()
    {
        if(this.characterSprite != null)
        {
            List<SpriteRenderer> renderers = new List<SpriteRenderer>();
            renderers.Clear();

            UnityUtil.GetChildObjects<SpriteRenderer>(this.characterSprite.transform, renderers);

            foreach(SpriteRenderer renderer in renderers)
            {
                if (renderer.gameObject.GetComponent<SpriteRendererExtension>() == null)
                {
                    renderer.gameObject.AddComponent<SpriteRendererExtension>();
                    Debug.Log("Set Extension for " + renderer.name);
                }
            }
        }
    }
#endif

    private void Start()
    {
        UnityUtil.GetChildObjects<SpriteRendererExtension>(this.characterSprite.transform, this.colorpalettes);
        init();
    }

    public void init() 
    {
        foreach (SpriteRendererExtension colorsprite in this.colorpalettes)
        {
            colorsprite.init();
        }
    }

    public void setStartColor()
    {
        foreach (SpriteRendererExtension colorsprite in this.colorpalettes)
        {
            colorsprite.setStartColor();
        }
    }

    public void resetColors()
    {
        foreach (SpriteRendererExtension colorPalette in this.colorpalettes)
        {
            colorPalette.resetColors();
        }
    }

    public void removeColor(Color color)
    {
        foreach (SpriteRendererExtension colorPalette in this.colorpalettes)
        {
            colorPalette.removeColor(color);
        }
    }

    public void enableSpriteRenderer(bool value)
    {
        this.characterSprite.SetActive(value);
        /*
        foreach (SpriteRendererExtension colorPalette in this.colorpalettes)
        {
            colorPalette.enableSpriteRenderer(value);
        }*/
    }

    public void changeColor(Color color)
    {
        foreach (SpriteRendererExtension colorPalette in this.colorpalettes)
        {
            colorPalette.changeColor(color);
        }
    }

    public void flipSprite(Vector2 direction)
    {
        foreach (SpriteRendererExtension colorPalette in this.colorpalettes)
        {
            colorPalette.flipSprite(direction);
        }
    }
}
