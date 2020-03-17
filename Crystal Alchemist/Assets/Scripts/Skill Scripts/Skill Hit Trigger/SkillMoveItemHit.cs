using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMoveItemHit : MonoBehaviour
{
    [SerializeField]
    private Skill skill;

    private Collectable item;

    private void Update()
    {
        if (this.item != null && this.item.GetComponent<Rigidbody2D>() != null)
        {
            this.item.GetComponent<Rigidbody2D>().MovePosition(this.skill.transform.position);
        }
    }

    public void moveItem(Collider2D hittedCharacter)
    {
        Collectable hittedItem = hittedCharacter.GetComponent<Collectable>();
        if (hittedItem != null && this.item == null)
            this.item = hittedItem;
    }

    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        moveItem(hittedCharacter);
    }

    private void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        //got Hit -> Back to Target
        moveItem(hittedCharacter);
    }
}
