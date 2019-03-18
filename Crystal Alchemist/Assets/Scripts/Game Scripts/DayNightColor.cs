using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DayNightColor : MonoBehaviour
{
    public Globals global;
    private SpriteRenderer spRenderer;
    private Tilemap spTilemap;

    // Start is called before the first frame update
    void Start()
    {
        this.spRenderer = this.GetComponent<SpriteRenderer>();
        this.spTilemap = this.GetComponent<Tilemap>();

        if (this.spRenderer != null) this.spRenderer.color = this.global.color;
        if (this.spTilemap != null) this.spTilemap.color = this.global.color;
    }

    public void updateColor()
    {
        if (this.spRenderer != null) this.spRenderer.color = this.global.color;
        if (this.spTilemap != null) this.spTilemap.color = this.global.color;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
