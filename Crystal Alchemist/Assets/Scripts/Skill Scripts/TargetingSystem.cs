using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetingSystem : MonoBehaviour
{
    [HideInInspector]
    public List<Character> targets = new List<Character>();
    private List<Character> targetsInRange = new List<Character>();
    private List<GameObject> appliedIndicators = new List<GameObject>();
    private Skill skill;

    [SerializeField]
    private GameObject lockOnIndicator;

    [SerializeField]
    private GameObject rangeIndicator;

    private float duration;
    private float maxAmount;
    private TargetingMode mode;
    private int index;
    private bool showIndicator;
    private bool readyToFire = false;
    private string button = "";
    private bool selectAll = false;
    private bool inputPossible = true;
    private bool showRange = false;

    public void setParameters(Skill skill, string button)
    {
        this.skill = skill;
        this.button = button;
    }

    private void Start()
    {
        this.duration = this.skill.GetComponent<SkillTargetingSystemModule>().targetingDuration;
        this.maxAmount = this.skill.GetComponent<SkillTargetingSystemModule>().maxAmountOfTargets;
        this.mode = this.skill.GetComponent<SkillTargetingSystemModule>().targetingMode;
        this.showIndicator = this.skill.GetComponent<SkillTargetingSystemModule>().showIndicator;
        this.showRange = this.skill.GetComponent<SkillTargetingSystemModule>().showRange;
        this.skill.sender.activeLockOnTarget = this;

        if (!this.showRange) this.rangeIndicator.SetActive(false);
        else this.rangeIndicator.SetActive(true);

        StartCoroutine(delayCo());
    }

    private void Update()
    {
        if (!this.readyToFire)
        {
            if (this.mode == TargetingMode.auto)
            {
                selectAllNearestTargets();
                this.readyToFire = true;
            }
            else selectTargetManually();

            updateIndicator();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (Utilities.Collisions.checkCollision(collision, this.skill) && character != null)
        {
            if (!this.targetsInRange.Contains(character)) this.targetsInRange.Add(character);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (Utilities.Collisions.checkCollision(collision, this.skill) && character != null)
        {
            if (this.targetsInRange.Contains(character)) this.targetsInRange.Remove(character);
            if (this.targets.Contains(character)) this.targets.Remove(character);
        }
    }

    public void DestroyIt()
    {
        this.skill.sender.activeLockOnTarget = null;
        Destroy(this.gameObject);
    }

    public Skill getSkill()
    {
        return this.skill;
    }

    public bool isReadyToFire(Skill skill)
    {
        if (this.readyToFire && this.skill == skill) return true;
        return false;
    }

    private void selectTargetManually()
    {
        if (this.selectAll) selectAllNearestTargets();
        else selectNextTarget(0);

        if (this.inputPossible)
        {
            if (Input.GetButtonDown(this.button))
            {
                this.readyToFire = true;
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
        List<Character> sorted = this.targetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.skill.sender.transform.position))).ToList<Character>();

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
        List<Character> sorted = this.targetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.skill.sender.transform.position))).ToList<Character>();

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
            if (Utilities.UnityUtils.hasChildWithTag(target, this.lockOnIndicator.tag) == null)
            {
                GameObject indicator = Instantiate(this.lockOnIndicator, target.transform.position, Quaternion.identity, target.transform);
                indicator.GetComponent<LockOnFrame>().setValues(Utilities.Format.getLanguageDialogText(target.stats.characterName, target.stats.englischCharacterName));
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
