using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class PlayerTargetingSystem : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject lockOnIndicator;

    [SerializeField]
    private GameObject rangeIndicator;

    [SerializeField]
    private CircleCollider2D circleCollider;

    [SerializeField]
    private PolygonCollider2D viewCollider;

    [SerializeField]
    private TextMeshPro durationTime;

    private List<Character> targets = new List<Character>();
    private List<Character> targetsInRange = new List<Character>();
    private List<GameObject> appliedIndicators = new List<GameObject>();
    private int index;
    private bool selectAll = false;
    private bool inputPossible = true;

    private LockOnSystem properties;
    private Skill skill;
    private float timeLeft = 60f;

    #region Unity Functions

    private void OnEnable()
    {
        this.circleCollider.gameObject.SetActive(false);
        this.viewCollider.gameObject.SetActive(false);

        if (this.properties.rangeType == RangeType.circle) this.circleCollider.gameObject.SetActive(true);
        else if (this.properties.rangeType == RangeType.view) this.viewCollider.gameObject.SetActive(true);

        this.circleCollider.transform.localScale = new Vector3(this.properties.range, this.properties.range, 1);
        this.viewCollider.transform.localScale = new Vector3(this.properties.range, this.properties.range, 1);

        if (!this.properties.showRange) this.rangeIndicator.SetActive(false);
        else this.rangeIndicator.SetActive(true);

        this.timeLeft = this.properties.maxDuration;

        StartCoroutine(delayCo());
    }

    private void Update()
    {
        RotationUtil.rotateCollider(this.player, this.viewCollider.gameObject);

        if (this.properties.targetingMode == TargetingMode.auto) selectAllNearestTargets();
        else selectTargetManually();

        updateIndicator();

        if (this.timeLeft > 0) this.timeLeft -= Time.deltaTime;
        else this.gameObject.SetActive(false);

        this.durationTime.text = FormatUtil.setDurationToString(this.timeLeft);        
    }

    private void OnDisable()
    {
        foreach (GameObject applied in this.appliedIndicators)
        {
            Destroy(applied);
        }
        this.appliedIndicators.Clear();
    }

    #endregion


    #region get set

    public void setParameters(Ability ability)
    {
        this.properties = ability.targetingSystem;
        this.skill = ability.skill;
    }

    public List<Character> getTargets()
    {
        return this.targets;
    }

    public float getDelay()
    {
        return this.properties.multiHitDelay;
    }

    #endregion


    #region Target Functions

    private void selectNextTarget(int value)
    {
        this.targets.Clear();
        List<Character> sorted = this.targetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.player.transform.position))).ToList<Character>();

        if (this.targetsInRange.Count > 0)
        {
            this.index += value;
            if (this.index >= sorted.Count) this.index = 0;
            else if (index < 0) this.index = sorted.Count - 1;

            addTarget(sorted[this.index]);
        }
    }

    private void selectTargetManually()
    {
        if (this.selectAll) selectAllNearestTargets();
        else selectNextTarget(0);

        if (this.inputPossible)
        {
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

    private void selectAllNearestTargets()
    {
        this.targets.Clear();
        List<Character> sorted = this.targetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.player.transform.position))).ToList<Character>();

        for (int i = 0; i < this.properties.maxAmountOfTargets && i < sorted.Count; i++)
        {
            addTarget(sorted[i]);
        }
    }

    public void removeTarget(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (CollisionUtil.checkCollision(collision, this.skill, this.player) && character != null)
        {
            if (this.targetsInRange.Contains(character)) this.targetsInRange.Remove(character);
            if (this.targets.Contains(character)) this.targets.Remove(character);
        }
    }

    public void addTarget(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (CollisionUtil.checkCollision(collision, this.skill, this.player) && character != null)
        {
            if (!this.targetsInRange.Contains(character)) this.targetsInRange.Add(character);
        }

        this.targetsInRange = this.targetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.player.transform.position))).ToList<Character>();
    }

    private void addTarget(Character character)
    {
        if (!this.targets.Contains(character)) this.targets.Add(character);
    }

    #endregion


    #region Indicator Functions

    private void updateIndicator()
    {
        if (this.properties.showIndicator)
        {
            addIndicator();
            removeIndicator();
        }
    }

    private void addIndicator()
    {
        foreach (Character target in this.targets)
        {
            if (UnityUtil.hasChildWithTag(target, this.lockOnIndicator.tag) == null)
            {
                GameObject indicator = Instantiate(this.lockOnIndicator, target.transform.position, Quaternion.identity, target.transform);
                indicator.GetComponent<LockOnFrame>().setValues(FormatUtil.getLanguageDialogText(target.stats.characterName, target.stats.englischCharacterName));
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

    #endregion


    private IEnumerator delayCo()
    {
        this.inputPossible = false;
        yield return new WaitForSeconds(0.1f);
        this.inputPossible = true;
    }

}
