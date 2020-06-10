using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillStack : SkillHitTrigger
{
    public enum ShareType
    {
        less,
        exact,
        more
    }

    [BoxGroup("Mechanics")]
    [SerializeField]
    private int amountNeeded;

    [BoxGroup("Mechanics")]
    [SerializeField]
    private ShareType type;

    private List<Character> listOfCharacters = new List<Character>();

    private bool hitIt = false; 

    public int GetAmount()
    {
        return this.listOfCharacters.Count;
    }

    public void HitIt() => this.hitIt = true;

    private float CalculateDamage()
    {
        if ((this.type == ShareType.exact && GetAmount() == this.amountNeeded)
         || (this.type == ShareType.less && GetAmount() <= this.amountNeeded)
         || (this.type == ShareType.more && GetAmount() >= this.amountNeeded)) return 0; 

        return 100; //full damage
    }

    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        if (CollisionUtil.checkCollision(hittedCharacter, this.skill))
        {
            Character character = hittedCharacter.GetComponent<Character>();
            if (character != null)
            {
                if (!this.hitIt && !this.listOfCharacters.Contains(character)) this.listOfCharacters.Add(character);
                else if (this.hitIt) this.skill.hitIt(character, CalculateDamage());
            }             
        }
    }

    private void OnTriggerExit2D(Collider2D hittedCharacter)
    {
        if (CollisionUtil.checkCollision(hittedCharacter, this.skill))
        {
            Character character = hittedCharacter.GetComponent<Character>();
            if (character != null && !this.hitIt && this.listOfCharacters.Contains(character)) this.listOfCharacters.Remove(character);            
        }
    }
}
