using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int x;
    public int y;
    private Vector2 position;
    public bool isWalkable;

    public int gCost;
    private int hCost;
    public int fCost;

    private PathNode previousNode;
    private float cellSize;
    private float radius;

    private const int straightCost = 10;
    private const int diagonalCost = 14;

    public PathNode(int x, int y, float cellSize, Vector2 origin)
    {
        this.x = x;
        this.y = y;
        this.cellSize = cellSize;

        this.position = new Vector2(this.x, this.y) * this.cellSize + origin;
        this.isWalkable = true;
    }

    public void Initialize(LayerMask layerMask, float radius)
    {
        this.gCost = int.MaxValue;
        this.CalculateFCost();
        this.previousNode = null;
        this.radius = radius;

        SetWalkable(layerMask);
    }

    private void SetWalkable(LayerMask layerMask)
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = layerMask;
        filter.useLayerMask = true;
        filter.useTriggers = false;

        int result = Physics2D.OverlapCircle(this.GetVector(), GetRadius(), filter, new List<Collider2D>());
        if (result > 0) this.isWalkable = false;
    }

    public void SetStartNode(PathNode endNode)
    {
        this.gCost = 0;
        this.hCost = CalculateHCost(endNode);
        this.CalculateFCost();
    }

    public void SetNeighbornode(PathNode currentNode, PathNode endNode, int tentativeGCost)
    {
        this.previousNode = currentNode;
        this.gCost = tentativeGCost;
        this.hCost = CalculateHCost(endNode);
        this.CalculateFCost();
    }

    public void CalculateFCost()
    {
        this.fCost = this.gCost + this.hCost;
    }

    public int CalculateHCost(PathNode node)
    {
        int xDistance = Mathf.Abs(this.x - node.x);
        int yDistance = Mathf.Abs(this.y - node.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        int minimum = Mathf.Min(xDistance, yDistance);
        return diagonalCost * minimum + straightCost * remaining;
    }

    public int CalculateTentativeGCost(PathNode neighbor)
    {
        return this.gCost + this.CalculateHCost(neighbor);
    }

    public PathNode GetPreviousNode()
    {
        return this.previousNode;
    }

    public Vector2 GetVector()
    {
        return this.position + Vector2.one * this.cellSize * 0.5f;
    }

    public Vector2 GetSize()
    {
        return Vector2.one * this.cellSize;
    }

    public float GetRadius()
    {
        return (this.cellSize * this.radius *0.5f);
    }
}
