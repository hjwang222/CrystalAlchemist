using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(SpriteRenderer))]
public class Indicator : MonoBehaviour
{
    private Character sender;
    private Character target;

    [SerializeField]
    [Required]
    private SpriteRenderer spriteRenderer;

    public virtual void Initialize(Character sender, Character target)
    {
        this.sender = sender;
        this.target = target;
    }

    public virtual void SetColor(Color color)
    {
        this.spriteRenderer.color = color;
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
