using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DayNightColor : MonoBehaviour
{
    private SpriteRenderer spRenderer;
    private Tilemap spTilemap;

    // Start is called before the first frame update
    void Start()
    {
        this.spRenderer = this.GetComponent<SpriteRenderer>();
        this.spTilemap = this.GetComponent<Tilemap>();

        if (this.spRenderer != null) this.spRenderer.color = GlobalValues.color;
        if (this.spTilemap != null) this.spTilemap.color = GlobalValues.color;
    }

    public void updateColor()
    {
        if (this.spRenderer != null) this.spRenderer.color = GlobalValues.color;
        if (this.spTilemap != null) this.spTilemap.color = GlobalValues.color;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
