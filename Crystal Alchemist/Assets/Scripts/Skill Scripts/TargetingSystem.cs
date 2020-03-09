using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetingSystem : MonoBehaviour
{
    public List<Character> targets = new List<Character>();
    public List<Character> targetsInRange = new List<Character>();
    private List<GameObject> appliedIndicators = new List<GameObject>();

    [SerializeField]
    private GameObject lockOnIndicator;

    [SerializeField]
    private GameObject rangeIndicator;

    private float duration;
    private float maxAmount;
    private TargetingMode mode;
    private int index;
    private bool showIndicator;
    private string button = "";
    private bool selectAll = false;
    private bool inputPossible = true;
    private bool showRange = false;
    public Skill skill;
    private Character sender; 
    private Ability ability;

    public void setParameters(Ability ability, Character sender)
    {
        this.duration = ability.targetingSystem.targetingDuration;
        this.maxAmount = ability.targetingSystem.maxAmountOfTargets;
        this.mode = ability.targetingSystem.targetingMode;
        this.showIndicator = ability.targetingSystem.showIndicator;
        this.showRange = ability.targetingSystem.showRange;
        this.skill = ability.skill;
        this.ability = ability;
        this.button = "AButton";
        this.sender = sender;
    }





    private void Start()
    {
        if (!this.showRange) this.rangeIndicator.SetActive(false);
        else this.rangeIndicator.SetActive(true);

        StartCoroutine(delayCo());
    }

    private void Update()
    {
        if (this.ability.state == AbilityState.targeting)
        {
            if (this.mode == TargetingMode.auto)
            {
                selectAllNearestTargets();
                this.ability.state = AbilityState.targetLocked;
            }
            else selectTargetManually();

            updateIndicator();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        addTarget(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        addTarget(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (CustomUtilities.Collisions.checkCollision(collision, this.skill, this.sender) && character != null)
        {
            if (this.targetsInRange.Contains(character)) this.targetsInRange.Remove(character);
            if (this.targets.Contains(character)) this.targets.Remove(character);
        }
    }






    private void addTarget(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (CustomUtilities.Collisions.checkCollision(collision, this.skill, this.sender) && character != null)
        {
            if (!this.targetsInRange.Contains(character)) this.targetsInRange.Add(character);
        }

        this.targetsInRange = this.targetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.sender.transform.position))).ToList<Character>();
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

    private void selectTargetManually()
    {
        if (this.selectAll) selectAllNearestTargets();
        else selectNextTarget(0);

        if (this.inputPossible)
        {
            if (Input.GetButtonDown(this.button))
            {
                this.ability.state = AbilityState.targetLocked;
                return;
            }

            float valueX = Input.GetAxisRaw("Cursor Horizontal");
            float valueY = Input.GetAxisRaw("Cursor Vertical");

            if (valueY != 0) //switch modes
            {
                StartCoroutine(delayCo());

                if (this.selectAll) this.selectAll = false;
                else this.selectAll = true;
            }

            if (valueX != 0) //switch targets
            {
                StartCoroutine(delayCo());
                selectNextTarget((int)valueX);
            }
        }
    }

    private void selectNextTarget(int value)
    {
        this.targets.Clear();
        List<Character> sorted = this.targetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.sender.transform.position))).ToList<Character>();

        if (this.targetsInRange.Count > 0)
        {
            this.index += value;
            if (this.index >= sorted.Count) this.index = 0;
            else if (index < 0) this.index = sorted.Count - 1;

            addTarget(sorted[this.index]);
        }
    }

    private void selectAllNearestTargets()
    {
        this.targets.Clear();
        List<Character> sorted = this.targetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.sender.transform.position))).ToList<Character>();

        for (int i = 0; i < this.maxAmount && i < sorted.Count; i++)
        {
            addTarget(sorted[i]);
        }
    }

    private void addTarget(Character character)
    {
        if (!this.targets.Contains(character)) this.targets.Add(character);
    }

    private void updateIndicator()
    {
        if (this.showIndicator)
        {
            addIndicator();
            removeIndicator();
        }
    }

    private void addIndicator()
    {
        foreach (Character target in this.targets)
        {
            if (CustomUtilities.UnityUtils.hasChildWithTag(target, this.lockOnIndicator.tag) == null)
            {
                GameObject indicator = Instantiate(this.lockOnIndicator, target.transform.position, Quaternion.identity, target.transform);
                indicator.GetComponent<LockOnFrame>().setValues(CustomUtilities.Format.getLanguageDialogText(target.stats.characterName, target.stats.englischCharacterName));
                this.appliedIndicators.Add(indicator);
            }
        }
    }

    private void removeIndicator()
    {
        List<GameObject> tempAppliedList = new List<GameObject>();

        foreach (GameObject applied in this.appliedIndicators)
        {
            Character character = applied.transform.parent.gameObject.GetComponent<Character>();

            if (applied != null && character != null && !this.targets.Contains(character)) Destroy(applied);
            else tempAppliedList.Add(applied);
        }

        this.appliedIndicators = tempAppliedList;
    }

    private IEnumerator delayCo()
    {
        this.inputPossible = false;
        yield return new WaitForSeconds(0.1f);
        this.inputPossible = true;
    }

    private void OnDestroy()
    {
        foreach (GameObject applied in this.appliedIndicators)
        {
            Destroy(applied);
        }
        this.appliedIndicators.Clear();
    }
}
