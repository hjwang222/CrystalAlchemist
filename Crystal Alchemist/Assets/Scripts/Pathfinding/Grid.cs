using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;

    private TGridObject[,] gridArray;
    private Color color = Color.white;

    private Vector2 position;

    public Grid(Tilemap tilemap, float cellSize)
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

            this.gridArray = new TGridObject[width, height];
        }
    }

    public void ShowDebug()
    {
        for (int x = 0; x < this.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < this.gridArray.GetLength(1); y++)
            {
                DrawRectangle(x, y);
            }
        }
    }

    private void DrawRectangle(int x, int y)
    {
        Debug.DrawLine(GetCellPosition(x, y), GetCellPosition(x + 1, y), this.color, 0.1f);
        Debug.DrawLine(GetCellPosition(x, y), GetCellPosition(x, y + 1), this.color, 0.1f);
        Debug.DrawLine(GetCellPosition(x, y + 1), GetCellPosition(x + 1, y + 1), this.color, 0.1f);
        Debug.DrawLine(GetCellPosition(x + 1, y), GetCellPosition(x + 1, y + 1), this.color, 0.1f);
    }

    private Vector3 GetCellPosition(int x, int y)
    {
        return new Vector2(x, y) * this.cellSize + this.position;
    }

    private int[] GetXY(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int y = Mathf.FloorToInt(position.y / cellSize);
        return new int[]{ x, y };
    }
}
