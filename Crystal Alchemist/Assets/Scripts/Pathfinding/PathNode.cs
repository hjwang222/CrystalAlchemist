using UnityEngine;

public class PathNode
{
    public int x;
    public int y;
    public Vector2 position;
    public bool isWalkable;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode previousNode;

    public PathNode(int x, int y, float cellSize, Vector2 origin)
    {
        this.x = x;
        this.y = y;

        this.position = new Vector2(this.x, this.y) * cellSize + origin;

        this.isWalkable = true;
    }

    public void CalculateFCost()
    {
        this.fCost = this.gCost + this.hCost;
    }

    public Vector2 GetCellPosition()
    {
        return this.position;
    }

    public int[] GetXY(Vector2 origin, float cellSize)
    {
        int x = Mathf.FloorToInt((this.position - origin).x / cellSize);
        int y = Mathf.FloorToInt((this.position - origin).y / cellSize);
        return new int[] { x, y };
    }
}
