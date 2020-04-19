using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingGrid
{
    private int width;
    private int height;
    private float cellSize;

    private PathNode[,] gridArray;

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

            for (int x = 0; x < this.gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < this.gridArray.GetLength(1); y++)
                {
                    this.gridArray[x, y] = new PathNode(x, y, this.cellSize, this.position);
                }
            }
        }
    }

    public void ShowDebug()
    {
        for (int x = 0; x < this.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < this.gridArray.GetLength(1); y++)
            {
                DrawRectangle(this.gridArray[x,y]);
            }
        }
    }



    private void DrawRectangle(PathNode node)
    {
        Color color = Color.white;
        int x = node.x;
        int y = node.y;

        if (!node.isWalkable) color = Color.red;

        Debug.DrawLine(GetCellPosition(x, y), GetCellPosition(x + 1, y), color, 100f);
        Debug.DrawLine(GetCellPosition(x, y), GetCellPosition(x, y + 1), color, 100f);
        Debug.DrawLine(GetCellPosition(x, y + 1), GetCellPosition(x + 1, y + 1), color, 100f);
        Debug.DrawLine(GetCellPosition(x + 1, y), GetCellPosition(x + 1, y + 1), color, 100f);
        //CreateWorldText(GetCellPosition(x, y)+"", null, GetCellPosition(x,y) + new Vector2(cellSize, cellSize) * .5f, 20, color, TextAnchor.MiddleCenter);
    }

    /*
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 10)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        transform.localScale = new Vector2(0.1f, 0.1f);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }*/

    public PathNode GetNode(int x, int y)
    {
        return this.gridArray[x, y];        
    }

    public int GetWidth()
    {
        return this.width;
    }

    public int GetHeight()
    {
        return this.height;
    }

    public Vector2 GetCellPosition(int x, int y)
    {
        return new Vector2(x, y) * this.cellSize + this.position;
    }

    public int[] GetXY(Vector2 position)
    {
        int x = Mathf.FloorToInt((position - this.position).x / cellSize);
        int y = Mathf.FloorToInt((position - this.position).y / cellSize);
        return new int[]{ x, y };
    }
}
