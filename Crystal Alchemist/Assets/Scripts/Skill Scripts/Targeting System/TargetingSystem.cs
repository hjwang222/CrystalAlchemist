using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D circleCollider;

    [SerializeField]
    private PolygonCollider2D viewCollider;

    private Character sender;
    public List<Character> selectedTargets = new List<Character>();
    public List<Character> allTargetsInRange = new List<Character>();
    private List<Indicator> appliedIndicators = new List<Indicator>();
    private int index;
    private bool selectAll = false;
    private bool inputPossible = true;
    private FloatValue timeLeftValue;
    private TargetingProperty properties;
    private Ability ability;
    private float timeLeft = 60f;

    #region Unity Functions

    public void Initialize(Character sender)
    {
        this.sender = sender;
    }

    public void SetTimeValue(FloatValue timeValue)
    {
        this.timeLeftValue = timeValue;
    }

    private void OnEnable()
    {
        ability.SetLockOnState();
        if (this.properties.targetingMode != TargetingMode.helper) SetColliders();

        if(this.properties.hasMaxDuration) this.timeLeft = this.properties.maxDuration;
        if(this.timeLeftValue != null) this.timeLeftValue.setValue(1f);

        StartCoroutine(delayCo());
    }

    private void Update()
    {
        this.ability.SetLockOnState();
        this.allTargetsInRange.RemoveAll(item => item.gameObject.activeInHierarchy == false);
        RotationUtil.rotateCollider(this.sender, this.viewCollider.gameObject);

        if (this.properties.targetingMode == TargetingMode.auto) selectAllNearestTargets();
        else if (this.properties.targetingMode == TargetingMode.manual) selectTargetManually();

        updateIndicator();
        updateTimer();  
    }

    private void OnDisable()
    {       
        foreach (Indicator applied in this.appliedIndicators)
        {
            Destroy(applied.gameObject);
        }
        this.appliedIndicators.Clear();
    }

    #endregion


    #region get set

    private void SetColliders()
    {
        this.circleCollider.gameObject.SetActive(false);
        this.viewCollider.gameObject.SetActive(false);

        if (this.properties.rangeType == RangeType.circle) this.circleCollider.gameObject.SetActive(true);
        else if (this.properties.rangeType == RangeType.view) this.viewCollider.gameObject.SetActive(true);

        this.circleCollider.transform.localScale = new Vector3(this.properties.range, this.properties.range, 1);
        this.viewCollider.transform.localScale = new Vector3(this.properties.range, this.properties.range, 1);
    }

    public void setParameters(Ability ability)
    {
        this.properties = ability.targetingProperty;
        this.ability = ability;
    }

    public float GetTimeLeft()
    {
        return this.timeLeft;
    }

    public List<Character> getTargets()
    {
        List<Character> targets = new List<Character>();

        foreach(Character target in this.selectedTargets)
        {
            if (target.gameObject.activeInHierarchy) targets.Add(target);
        }
        return targets;
    }

    public float getDelay()
    {
        return this.properties.multiHitDelay;
    }

    #endregion


    #region Target Functions

    private List<Character> sortTargets()
    {
        List<Character> result = this.allTargetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.sender.transform.position))).ToList<Character>();
        result.RemoveAll(item => item.gameObject.activeInHierarchy == false);
        return result;
    }

    private void selectNextTarget(int value)
    {
        this.selectedTargets.Clear();
        List<Character> sorted = sortTargets();

        if (this.allTargetsInRange.Count > 0)
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
        this.selectedTargets.Clear();
        List<Character> sorted = sortTargets();

        for (int i = 0; i < this.properties.maxAmountOfTargets && i < sorted.Count; i++)
        {
            addTarget(sorted[i]);
        }
    }

    public void removeTarget(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (CollisionUtil.checkCollision(collision, this.ability.skill, this.sender) && character != null)
        {
            if (this.allTargetsInRange.Contains(character)) this.allTargetsInRange.Remove(character);
            if (this.selectedTargets.Contains(character)) this.selectedTargets.Remove(character);
        }
    }

    public void addTarget(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (CollisionUtil.checkCollision(collision, this.ability.skill, this.sender) && character != null)
        {
            if (!this.allTargetsInRange.Contains(character)) this.allTargetsInRange.Add(character);
        }

        this.allTargetsInRange = this.allTargetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.sender.transform.position))).ToList<Character>();
    }

    private void addTarget(Character character)
    {
        if (!this.selectedTargets.Contains(character)) this.selectedTargets.Add(character);
    }

    #endregion


    #region Indicator Functions

    private void updateTimer()
    {
        if (this.properties.hasMaxDuration)
        {
            if (this.timeLeft > 0) this.timeLeft -= Time.deltaTime;
            else Deactivate();

            if (this.timeLeftValue != null) this.timeLeftValue.setValue(this.timeLeft / this.properties.maxDuration);
        }
    }

    private void updateIndicator()
    {
        if (this.properties.showIndicator)
        {
            if (this.properties.targetingMode != TargetingMode.helper)
            {
                addIndicator();
                removeIndicator();
            }
            else
            {
                this.properties.Instantiate(this.sender, null, this.appliedIndicators);
            }
        }
    }

    private void addIndicator()
    {
        foreach (Character target in this.selectedTargets)
        {
            this.properties.Instantiate(this.sender, target, this.appliedIndicators);            
        }
    }

    private void removeIndicator()
    {
        List<Indicator> tempAppliedList = new List<Indicator>();

        foreach (Indicator applied in this.appliedIndicators)
        {
            if (applied != null && !this.selectedTargets.Contains(applied.GetTarget())) Destroy(applied.gameObject);
            else tempAppliedList.Add(applied);
        }

        this.appliedIndicators = tempAppliedList;
        this.appliedIndicators.RemoveAll(item => item == null);
    }

    #endregion

    public void Deactivate()
    {
        this.ability.ResetLockOn();
        this.ability.state = AbilityState.onCooldown;
        this.gameObject.SetActive(false);
    }


    private IEnumerator delayCo()
    {
        this.inputPossible = false;
        yield return new WaitForSeconds(0.1f);
        this.inputPossible = true;
    }

}
