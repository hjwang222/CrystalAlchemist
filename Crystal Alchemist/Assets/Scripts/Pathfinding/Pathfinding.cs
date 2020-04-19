using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private float cellsize = 0.5f;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    public bool showDebug = false;

    private PathfindingGrid grid;
    private List<PathNode> openList = new List<PathNode>();
    private List<PathNode> closeList = new List<PathNode>();

    private const int straightCost = 10;
    private const int diagonalCost = 14;

    private void Start()
    {
        this.grid = new PathfindingGrid(this.tilemap, this.cellsize);
        this.grid.ShowDebug();
    }

    public List<Vector2> FindPath(Vector2 startPosition, Vector2 endPosition)
    {
        int[] startgrid = this.grid.GetXY(startPosition);
        int[] endgrid = this.grid.GetXY(endPosition);
        List<PathNode> path = FindPath(startgrid[0], startgrid[1], endgrid[0], endgrid[1]);

        if (path != null)
        {
            List<Vector2> result = new List<Vector2>();
            foreach (PathNode node in path)
            {
                result.Add(this.grid.GetCellPosition(node.x, node.y) * this.cellsize + Vector2.one * this.cellsize * 0.5f);
            }
            return result;
        }
        else return null;
    }


    private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = this.grid.GetNode(startX, startY);
        PathNode endNode = this.grid.GetNode(endX, endY);

        this.closeList.Clear();
        this.openList.Clear();

        this.openList.Add(startNode);

        //Initialize all nodes
        for (int x = 0; x < this.grid.GetWidth(); x++)
        {
            for (int y = 0; y < this.grid.GetHeight(); y++)
            {
                PathNode node = this.grid.GetNode(x, y);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.previousNode = null;
            }
        }

        //Set Startnode
        startNode.gCost = 0;
        startNode.hCost = CalculateHCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(this.openList);
            if (currentNode == endNode)
            {
                //reached final node
                return GetPath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (PathNode node in GetNeighborNodes(currentNode))
            {
                if (closeList.Contains(node)) continue;
                if (!node.isWalkable)
                {
                    this.closeList.Add(node);
                    continue;
                }

                int tentantiveGCost = currentNode.gCost + CalculateHCost(currentNode, node);
                if (tentantiveGCost < node.gCost)
                {
                    node.previousNode = currentNode;
                    node.gCost = tentantiveGCost;
                    node.hCost = CalculateHCost(node, endNode);
                    node.CalculateFCost();

                    if (!this.openList.Contains(node)) this.openList.Add(node);
                }
            }
        }

        //no path found;
        return null;
    }

    private List<PathNode> GetNeighborNodes(PathNode node)
    {
        List<PathNode> result = new List<PathNode>();

        if (node.x - 1 >= 0)
        {
            result.Add(this.grid.GetNode(node.x - 1, node.y));
            if (node.y - 1 >= 0) result.Add(this.grid.GetNode(node.x - 1, node.y - 1));
            if (node.y + 1 < this.grid.GetHeight()) result.Add(this.grid.GetNode(node.x - 1, node.y + 1));
        }
        if (node.x + 1 < this.grid.GetWidth())
        {
            result.Add(this.grid.GetNode(node.x + 1, node.y));
            if (node.y - 1 >= 0) result.Add(this.grid.GetNode(node.x + 1, node.y - 1));
            if (node.y + 1 < this.grid.GetHeight()) result.Add(this.grid.GetNode(node.x + 1, node.y + 1));
        }
        if (node.y - 1 >= 0)
        {
            result.Add(this.grid.GetNode(node.x, node.y - 1));
        }
        if (node.y + 1 < this.grid.GetHeight())
        {
            result.Add(this.grid.GetNode(node.x, node.y + 1));
        }

        return result;
    }

    private List<PathNode> GetPath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateHCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        int minimum = Mathf.Min(xDistance, yDistance);
        return diagonalCost * minimum + straightCost * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode result = pathNodeList[0];

        foreach (PathNode node in pathNodeList)
        {
            if (node.fCost < result.fCost) result = node;
        }

        return result;
    }

}
