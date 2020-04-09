using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(SpriteRenderer))]
public class Indicator : MonoBehaviour
{
    private Character sender;
    private Character target;
    private Color color;

    [SerializeField]
    [Required]
    private SpriteRenderer spriteRenderer;

    public virtual void Initialize(Character sender, Character target)
    {
        this.sender = sender;
        this.target = target;
    }

    public void SetColor(Color color)
    {
        this.color = color;
        this.spriteRenderer.color = this.color;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return this.spriteRenderer;
    }

    public Character GetSender()
    {
        return this.sender;
    }

    public Character GetTarget()
    {
        return this.target;
    }

    public Color GetColor()
    {
        return this.color;
    }

    public virtual void Start()
    {
        //this.tag = "Indicator";
    }

    public virtual void Update()
    {
        
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }
}
