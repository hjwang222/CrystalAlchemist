using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum aoeShareType
{
    none,
    less,
    exact,
    more
}

public class SkillStack : SkillMechanicHit
{
    [BoxGroup("Mechanics")]
    [SerializeField]
    private int amountNeeded;

    [BoxGroup("Mechanics")]
    [SerializeField]
    private aoeShareType type;

    private float timeLeft = 0;
    private List<Character> listOfCharacters = new List<Character>();

    private void Start()
    {
        this.skill.SetTriggerActive(1);
    }

    private void OnDestroy()
    {
        this.percentage = percentageByAmount();
        this.hitAllCharacters();
    }

    private float calculatePercentage()
    {
        float percentage = (100 / this.amountNeeded * (this.amountNeeded - this.listOfCharacters.Count));

        percentage = Mathf.Round(percentage / 25) * 25;

        if (percentage > 100) percentage = 100;
        else if (percentage < 0) percentage = 0;

        return percentage;
    }

    private float percentageByAmount()
    {
        if (this.type == aoeShareType.exact && this.amountNeeded == this.listOfCharacters.Count)
        {
            return 0; //no damage, if exact amount
        }
        else if (this.type == aoeShareType.less && this.listOfCharacters.Count <= this.amountNeeded)
        {
            return 0; //no damage, if amount or less
        }
        else if (this.type == aoeShareType.more)
        {
            return calculatePercentage(); //damage based on how many ppl share
        }

        return 100; //full damage
    }

    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        if (CustomUtilities.Collisions.checkCollision(hittedCharacter, this.skill))
        {
            Character character = hittedCharacter.GetComponent<Character>();
            if (character != null && !this.listOfCharacters.Contains(character)) this.listOfCharacters.Add(character);
        }
    }

    private void OnTriggerExit2D(Collider2D hittedCharacter)
    {
        if (CustomUtilities.Collisions.checkCollision(hittedCharacter, this.skill))
        {
            Character character = hittedCharacter.GetComponent<Character>();
            if (character != null && this.listOfCharacters.Contains(character)) this.listOfCharacters.Remove(character);
        }
    }
}
