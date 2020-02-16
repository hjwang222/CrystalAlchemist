﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererExtensionHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject characterSprite;

    [HideInInspector]
    public List<SpriteRendererExtension> colorpalettes = new List<SpriteRendererExtension>();

    private void Start()
    {
        CustomUtilities.UnityFunctions.GetChildObjects<SpriteRendererExtension>(this.characterSprite.transform, this.colorpalettes);
        init();
    }

    public void init() //TODO: REWORK!
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
