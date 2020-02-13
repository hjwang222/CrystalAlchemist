using UnityEngine;
using UnityEngine.Rendering;

public class SnakeTail : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private Player player;

    private float maxDistance = 0f;

    private void Start()
    {
        this.maxDistance = Vector2.Distance(this.parent.transform.position, this.transform.position);
    }

    void Update()
    {
        float distance = Vector2.Distance(this.parent.transform.position, this.transform.position);

        if(distance > this.maxDistance)
        {            
            Vector2 direction = ((Vector2)this.parent.transform.position - (Vector2)this.transform.position).normalized;
            this.GetComponent<Rigidbody2D>().velocity = (direction * this.player.myRigidbody.velocity.magnitude);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        int modify = 0;

        if(this.parent.transform.position.y > this.transform.position.y) modify = 1;       
        else modify = - 1;        

        if(this.parent.GetComponent<SpriteRenderer>() != null)
            this.GetComponent<SpriteRenderer>().sortingOrder = this.parent.GetComponent<SpriteRenderer>().sortingOrder + modify;

        if (this.parent.GetComponent<SortingGroup>() != null)
            this.parent.GetComponent<SortingGroup>().sortingOrder = modify;
    }
}
