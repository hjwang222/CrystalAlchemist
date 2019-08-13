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

public class AoESkill : StandardSkill
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

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    private bool followTarget;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    [ShowIf("followTarget", true)]
    private float followTimeMin;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    [ShowIf("followTarget", true)]
    private bool useRandomTime;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    [ShowIf("followTarget", true)]
    [ShowIf("useRandomTime", true)]
    private float followTimeMax;

    private float timeLeft = 0;
    private bool hurtCharacter = false;
    private List<Character> listOfCharacters = new List<Character>();

    public override void init()
    {
        base.init();

        this.SetTriggerActive(1);
        this.joinCollider.enabled = true;
        if (this.hurtCollider != null) this.hurtCollider.enabled = false;

        this.timeLeft = followTimeMax;
        if (this.useRandomTime) this.timeLeft = Random.Range(followTimeMin, followTimeMax);
        if (this.timeLeft > this.delay) this.timeLeft = this.delay - 0.01f;
    }

    private void moveTowardsTarget(Vector3 position)
    {
        if (this.myRigidbody != null)
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, position, this.speed * (Time.deltaTime));
            Vector2 direction = position - this.transform.position;

            this.myRigidbody.MovePosition(temp);
            this.myRigidbody.velocity = Vector2.zero;
        }
    }

    public override void doOnUpdate()
    {
        base.doOnUpdate();

        if (this.followTarget)
        {
            if (this.timeLeft > 0)
            {
                this.timeLeft -= Time.deltaTime;

                if (Vector3.Distance(this.target.transform.position, this.transform.position) > 0.1f)
                {
                    moveTowardsTarget(this.target.transform.position);
                }
            }
        }

        if (this.delayTimeLeft <= 0)
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

    public override void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        if (Utilities.Collisions.checkCollision(hittedCharacter, this))
        {
            if (!this.hurtCharacter)
            {
                Character character = hittedCharacter.GetComponent<Character>();
                if (character != null && !this.listOfCharacters.Contains(character)) this.listOfCharacters.Add(character);
            }
            else
            {
                float percentage = percentageByAmount();
                landAttack(hittedCharacter, percentage);
            }
        }
    }

    public override void OnTriggerStay2D(Collider2D hittedCharacter)
    {

    }

    public override void OnTriggerExit2D(Collider2D hittedCharacter)
    {
        if (!this.hurtCharacter && Utilities.Collisions.checkCollision(hittedCharacter, this))
        {
            Character character = hittedCharacter.GetComponent<Character>();
            if (character != null && this.listOfCharacters.Contains(character)) this.listOfCharacters.Remove(character);
        }
    }
}
