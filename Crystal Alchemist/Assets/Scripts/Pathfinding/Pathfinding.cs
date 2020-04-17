using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private float cellsize = 0.5f;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private bool showDebug = false;

    private Grid<int> grid;

    private void Start()
    {
        this.grid = new Grid<int>(this.tilemap, this.cellsize);
    }

    private void Update()
    {
        if (this.grid != null) this.grid.SetValues(this.tilemap, this.cellsize);
        if (this.showDebug) this.grid.ShowDebug();
    }


}
