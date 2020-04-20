using System.Collections.Generic;
using UnityEngine;

public class PathSeeker : MonoBehaviour
{
    private List<PathNode> openList = new List<PathNode>();
    private List<PathNode> closeList = new List<PathNode>();
    private PathfindingGrid grid;

    public List<Vector2> FindPath(Vector2 startPosition, Vector2 endPosition)
    {
        this.grid = Pathfinding.Instance.grid; 

        if (this.grid != null)
        {
            List<PathNode> path = FindPath(grid.GetNode(startPosition), grid.GetNode(endPosition));

            if (path != null)
            {
                List<Vector2> result = new List<Vector2>();
                foreach (PathNode node in path)
                {
                    result.Add(node.GetVector());
                }
                return result;
            }
        }

        return null;
    }

    private List<PathNode> FindPath(PathNode start, PathNode end)
    {
        PathNode startNode = start;
        PathNode endNode = end;

        this.closeList.Clear();
        this.openList.Clear();

        this.openList.Add(startNode);
        Pathfinding.Instance.InitializeNodes();

        //Set Startnode
        startNode.SetStartNode(endNode);

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

            foreach (PathNode neighbor in GetNeighborNodes(currentNode))
            {
                if (closeList.Contains(neighbor)) continue;
                if (neighbor != endNode && !neighbor.isWalkable)
                {
                    this.closeList.Add(neighbor);
                    continue;
                }

                int tentantiveGCost = currentNode.CalculateTentativeGCost(neighbor);
                if (tentantiveGCost < neighbor.gCost)
                {
                    neighbor.SetNeighbornode(currentNode, endNode, tentantiveGCost);
                    if (!this.openList.Contains(neighbor)) this.openList.Add(neighbor);
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

        while (currentNode.GetPreviousNode() != null)
        {
            path.Add(currentNode.GetPreviousNode());
            currentNode = currentNode.GetPreviousNode();
        }

        path.Reverse();
        return path;
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
