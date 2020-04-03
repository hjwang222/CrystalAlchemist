using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum PrimitiveType
{
    line,
    triangle,
    rectangle,
    pentagon,
    hexagon,
    octagon,
    circle, 
    polygon
}

[RequireComponent(typeof(LineRenderer))]
public class DrawPrimitive : MonoBehaviour
{
    [SerializeField]
    private PrimitiveType type = PrimitiveType.circle;

    [SerializeField]
    [Range(0,25)]
    private float radius = 3f;

    [SerializeField]
    [Range(0, 10)]
    private float lineWidth = 0.06f;

    [SerializeField]
    private Color color = Color.white;

    [SerializeField]
    private Material material;

    [SerializeField]
    [ShowIf("type", PrimitiveType.polygon)]
    private PolygonCollider2D collider;

    private int segments = 0;

    private LineRenderer line = null;

    private void Start()
    {
        this.line = this.GetComponent<LineRenderer>();        
        this.line.useWorldSpace = false;
    }

    private void FixedUpdate()
    {
        Render();
    }

    private void Render()
    {
        setProperties();
        if (this.type == PrimitiveType.polygon) drawCollider();
        else drawPrimitive();
    }

    private int setSegments()
    {
        switch (this.type)
        {
            case PrimitiveType.line: return 2;
            case PrimitiveType.triangle: return 3;
            case PrimitiveType.rectangle: return 4;
            case PrimitiveType.pentagon: return 5;
            case PrimitiveType.hexagon: return 6;
            case PrimitiveType.octagon: return 8;
            case PrimitiveType.circle: return 45;
            default: return 0;
        }
    }

    private void setColor()
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(this.color, 0.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(this.color.a, 0.0f) }
        );
        this.line.colorGradient = gradient;
        this.line.material = this.material;
    }

    private void setProperties()
    {
        setColor();
        this.line.startWidth = this.lineWidth;
        this.line.endWidth = this.lineWidth;
        
    }

    private void drawPrimitive()
    {
        this.segments = setSegments();

        int pointCount = this.segments + 1;
        this.line.positionCount = pointCount;
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / this.segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0);
        }

        this.line.SetPositions(points);
    }

    private void drawCollider()
    {
        List<Vector3> points = new List<Vector3>();

        foreach (Vector2 point in this.collider.points)
        {
            points.Add(new Vector3(point.x, point.y, 0));
        }

        points.Add(this.collider.points[0]);

        this.line.positionCount = points.Count;
        this.line.SetPositions(points.ToArray());
    }
}
