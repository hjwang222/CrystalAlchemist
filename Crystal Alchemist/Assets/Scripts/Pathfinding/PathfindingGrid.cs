using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingGrid
{
    private int width;
    private int height;
    private float cellSize;

    public PathNode[,] gridArray;

    private Vector2 position;    

    public PathfindingGrid(Tilemap tilemap, float cellSize)
    {
        SetValues(tilemap, cellSize);
    }    

    public void SetValues(Tilemap tilemap, float cellSize)
    {
        if (cellSize > 0)
        {
            this.width = (int)(tilemap.size.x / cellSize);
            this.height = (int)(tilemap.size.y / cellSize);
            this.position = tilemap.localBounds.min;
            this.cellSize = cellSize;

            this.gridArray = new PathNode[width, height];
            Initialize();
        }
    }

    private void Initialize()
    {
        for (int x = 0; x < this.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < this.gridArray.GetLength(1); y++)
            {
                this.gridArray[x, y] = new PathNode(x, y, this.cellSize, this.position);
            }
        }
    }

    public PathNode GetNode(int x, int y)
    {
        return this.gridArray[x, y];        
    }

    public PathNode GetNode(Vector2 position)
    {
        int x = Mathf.FloorToInt((position - this.position).x / this.cellSize);
        int y = Mathf.FloorToInt((position - this.position).y / this.cellSize);
        return GetNode(x, y);
    }

    public int GetWidth()
    {
        return this.width;
    }

    public int GetHeight()
    {
        return this.height;
    }
}
