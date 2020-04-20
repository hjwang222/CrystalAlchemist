using UnityEngine;

public class GizmoUtil : MonoBehaviour
{
    public static void PathfinderGizmo(PathfindingGrid grid, Color gridColor, Color collisionColor)
    {
        Gizmos.color = gridColor;

        if (grid != null && grid.gridArray != null)
        {
            for (int x = 0; x < grid.gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < grid.gridArray.GetLength(1); y++)
                {
                    PathNode node = grid.gridArray[x, y];
                    if (node != null)
                    {
                        Gizmos.DrawWireCube(node.GetVector(), node.GetSize());
                        Gizmos.DrawWireSphere(node.GetVector(), node.GetRadius());
                        if (!node.isWalkable)
                            Gizmos.DrawIcon(node.GetVector(), "Pathfinding/NotWalkable", false, collisionColor);
                    }
                }
            }
        }
    }
}
