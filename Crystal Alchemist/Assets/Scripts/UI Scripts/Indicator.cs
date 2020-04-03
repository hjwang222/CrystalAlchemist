using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(SpriteRenderer))]
public class Indicator : MonoBehaviour
{
    private Character sender;
    private Character target;


    public void Initialize(Character sender, Character target)
    {
        this.sender = sender;
        this.target = target;
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
        
    }

    public virtual void Update()
    {
        
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }
}
