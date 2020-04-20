using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    //Dynamic Obstacles
    //Grid List

    [SerializeField]
    [Required]
    [BoxGroup("Grid")]
    private Tilemap tileMap;

    [SerializeField]
    [ShowIf("tileMap")]
    [BoxGroup("Grid")]
    private float cellsize = 1f;

    [SerializeField]
    [ShowIf("tileMap")]
    [BoxGroup("Grid")]
    [Range(0,1)]
    private float radius = 0.9f;

    [SerializeField]
    [ShowIf("tileMap")]
    [BoxGroup("Grid")]
    private LayerMask filter;

    [SerializeField]
    [BoxGroup("Debug")]
    public bool showDebug = true;

    [SerializeField]
    [BoxGroup("Debug")]
    [ShowIf("showDebug")]
    private Color gridColor = Color.blue;

    [SerializeField]
    [BoxGroup("Debug")]
    [ShowIf("showDebug")]
    private Color notWalkable = Color.red;

    public PathfindingGrid grid;

    public static Pathfinding Instance { get; private set; }

    private void OnDrawGizmos()
    {
        if (this.showDebug) GizmoUtil.PathfinderGizmo(this.grid, this.gridColor, this.notWalkable);      
    }

    [Button]
    private void Bake()
    {
        this.grid = new PathfindingGrid(this.tileMap, this.cellsize);
        Instance = this;

        InitializeNodes();
    }

    private void Start()
    {
        Bake();
    }

    public void InitializeNodes()
    {
        //Initialize all nodes
        for (int x = 0; x < this.grid.GetWidth(); x++)
        {
            for (int y = 0; y < this.grid.GetHeight(); y++)
            {
                PathNode node = this.grid.GetNode(x, y);
                node.Initialize(this.filter, this.radius);
            }
        }
    }
}
