using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMoveItemHit : MonoBehaviour
{
    [SerializeField]
    private Skill skill;

    private Item item;

    private void Update()
    {
        if (this.item != null && this.item.GetComponent<Rigidbody2D>() != null)
        {
            this.item.GetComponent<Rigidbody2D>().MovePosition(this.skill.transform.position);
        }
    }

    public void moveItem(Collider2D hittedCharacter)
    {
        Item hittedItem = hittedCharacter.GetComponent<Item>();
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
