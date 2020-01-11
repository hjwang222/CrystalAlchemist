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

//TODO: Show Timer
//TODO: Override Cast

public class SkillStack : SkillExtension
{   
    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    [Required]
    private Collider2D joinCollider;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    [Required]
    private Collider2D hurtCollider;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    private int amountNeeded;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    private aoeShareType type;

    private float timeLeft = 0;
    private bool hurtCharacter = false;
    private List<Character> listOfCharacters = new List<Character>();

    private void Start()
    {
        this.skill.SetTriggerActive(1);
        this.joinCollider.enabled = true;
        if (this.hurtCollider != null) this.hurtCollider.enabled = false;

        //this.timeLeft = followTimeMax;
        //if (this.useRandomTime) this.timeLeft = Random.Range(followTimeMin, followTimeMax);
        if (this.timeLeft > this.skill.delay) this.timeLeft = this.skill.delay - 0.01f;
    }

    private void Update()
    {
        if (this.skill.delayTimeLeft <= 0)
        {
            this.hurtCharacter = true;

            if (this.hurtCollider != null)
            {
                this.joinCollider.enabled = false;
                this.hurtCollider.enabled = true;
            }
        }
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
            if (!this.hurtCharacter)
            {
                Character character = hittedCharacter.GetComponent<Character>();
                if (character != null && !this.listOfCharacters.Contains(character)) this.listOfCharacters.Add(character);
            }
            else
            {
                float percentage = percentageByAmount();
                this.skill.hitIt(hittedCharacter, percentage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D hittedCharacter)
    {
        if (!this.hurtCharacter && CustomUtilities.Collisions.checkCollision(hittedCharacter, this.skill))
        {
            Character character = hittedCharacter.GetComponent<Character>();
            if (character != null && this.listOfCharacters.Contains(character)) this.listOfCharacters.Remove(character);
        }
    }
}
