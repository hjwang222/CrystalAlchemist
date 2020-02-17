using UnityEngine;
using UnityEngine.Rendering;

public class SnakeTail : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private Player player;

    [SerializeField]
    private bool isHead = false;

    private float maxDistance = 0f;
    private Vector2 startPosition;

    private void Start()
    {
        this.maxDistance = Vector2.Distance(this.parent.transform.position, this.transform.position);
        this.startPosition = this.transform.position;
    }

    private void Disable()
    {
       if(this.startPosition != Vector2.zero) this.transform.position = this.startPosition;
    }

    void Update()
    {
        if (this.player != null && this.player.myRigidbody != null)
        {
            float distance = Vector2.Distance(this.parent.transform.position, this.transform.position);

            if (distance > this.maxDistance)
            {
                Vector2 direction = ((Vector2)this.parent.transform.position - (Vector2)this.transform.position).normalized;
                this.GetComponent<Rigidbody2D>().velocity = (direction * this.player.myRigidbody.velocity.magnitude);
            }
            else
            {
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            int modify = 0;

            if (this.parent.transform.position.y > this.transform.position.y) modify = 1;
            else modify = -1;

            if (this.parent != null && this.parent.GetComponent<SortingGroup>() != null)
                this.GetComponent<SortingGroup>().sortingOrder = this.parent.GetComponent<SortingGroup>().sortingOrder + modify;

            if (this.isHead && this.parent.GetComponent<SortingGroup>() != null)
                this.parent.GetComponent<SortingGroup>().sortingOrder = modify;
        }
    }
}
