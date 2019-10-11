using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMoveItemHit : SkillHitTrigger
{
    private Item moveItem;

    private void Update()
    {
        if (this.moveItem != null && this.moveItem.GetComponent<Rigidbody2D>() != null)
        {
            this.moveItem.GetComponent<Rigidbody2D>().MovePosition(this.skill.transform.position);
        }
    }

    public void moveThings(Collider2D hittedCharacter)
    {
        if (this.skill.sender != null
            && hittedCharacter.tag != this.skill.sender.tag
            && !hittedCharacter.isTrigger
            && !hittedCharacter.CompareTag("Object"))
        {
            if(this.GetComponent<SkillBoomerang>() != null) this.GetComponent<SkillBoomerang>().durationThenBackToSender = 0;
        }

        if (this.skill.sender != null
            && hittedCharacter.tag != this.skill.sender.tag
            && hittedCharacter.CompareTag("Item"))
        {
            if (this.GetComponent<SkillBoomerang>() != null) this.GetComponent<SkillBoomerang>().durationThenBackToSender = 0;

            Item hittedItem = hittedCharacter.GetComponent<Item>();
            if (hittedItem != null && this.moveItem == null)
                this.moveItem = hittedItem;
        }
    }

    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        moveThings(hittedCharacter);
    }

    private void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        //got Hit -> Back to Target
        moveThings(hittedCharacter);
    }
}
