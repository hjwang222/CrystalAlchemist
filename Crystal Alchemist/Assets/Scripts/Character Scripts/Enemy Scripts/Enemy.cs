using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Enemy : Character
{
    [HideInInspector]
    public Character target;

    [FoldoutGroup("Enemy Attributes", expanded: false)]
    [SerializeField]
    [Range(0, 10)]
    private float aggroIncreaseFactor = 0.25f;

    [FoldoutGroup("Enemy Attributes", expanded: false)]
    [SerializeField]
    [Range(0, 10)]
    private float aggroDecreaseFactor = 0.25f;

    [FoldoutGroup("Enemy Attributes", expanded: false)]
    [SerializeField]
    private GameObject targetFoundClue;

    [FoldoutGroup("Enemy Attributes", expanded: false)]
    [SerializeField]
    private float foundClueDuration = 1;

    [FoldoutGroup("Enemy Attributes", expanded: false)]
    [SerializeField]
    private GameObject targetActiveClue;

    [FoldoutGroup("Enemy Attributes", expanded: false)]
    [SerializeField]
    private float activeClueDuration = 1;

    private GameObject activeClue;
    private Dictionary<Character, float[]> aggroList = new Dictionary<Character, float[]>();

    #region Start

    private void Start()
    {
        init();
        Utilities.SetAnimatorParameter(this.animator, "isWakeUp", true);
    }

    #endregion


    public void Update()
    {
        //this.myRigidbody.velocity = Vector2.zero;
        generateAggro();
    }


    #region Aggro-System

    public void generateAggro()
    {
        List<Character> charactersToRemove = new List<Character>();

        foreach (Character character in this.aggroList.Keys)
        {
            this.aggroList[character][0] += this.aggroList[character][1] * (Time.deltaTime * this.timeDistortion);

            //if (this.aggroList[character][0] > 0) Debug.Log(this.characterName + " hat " + this.aggroList[character][0] + " Aggro auf" + character.characterName);

            if (this.aggroList[character][0] >= 1)
            {
                this.aggroList[character][0] = 1; //aggro
                this.aggroList[character][1] = 0; //factor

                //Aggro max, Target!
                if (this.target == null)
                {
                    this.target = character;
                    StartCoroutine(showClueCo(this.targetActiveClue, this.activeClueDuration));
                }
            }
            else if (this.aggroList[character][0] <= 0)
            {
                this.aggroList[character][0] = 0; //aggro
                this.aggroList[character][1] = 0; //factor

                //Aggro lost, Target lost
                if (this.target == character) this.target = null;
                charactersToRemove.Add(character);
                hideClue();
            }
        }

        foreach (Character character in charactersToRemove)
        {
            this.removeFromAggroList(character);
        }
    }

    private IEnumerator showClueCo(GameObject clue, float duration)
    {
        hideClue();
        showClue(clue);
        yield return new WaitForSeconds(duration);
        if (duration > 0) hideClue();
    }

    private void showClue(GameObject clue)
    {
        if (clue != null && this.activeClue == null)
        {
            Vector3 position = new Vector3(this.transform.position.x - 0.5f, this.transform.position.y + 0.5f);
            this.activeClue = Instantiate(clue, position, Quaternion.identity, this.transform);
        }
    }

    private void hideClue()
    {
        if (this.activeClue != null)
        {
            Destroy(this.activeClue);
            this.activeClue = null;
        }
    }

    private void addToAggroList(Character character)
    {
        if (!this.aggroList.ContainsKey(character)) this.aggroList.Add(character, new float[] { 0, 0 });
    }

    private void removeFromAggroList(Character character)
    {
        if (this.aggroList.ContainsKey(character)) this.aggroList.Remove(character);
    }

    private void setParameterOfAggrolist(Character character, float amount)
    {
        this.aggroList[character][1] = amount; //set factor of increase/decreasing aggro
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        increaseAggro(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        decreaseAggro(collision);
    }

    public void increaseAggro(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {

            addToAggroList(collision.GetComponent<Character>());
            setParameterOfAggrolist(collision.GetComponent<Character>(), 1 * this.aggroIncreaseFactor);
            if (this.target == null) StartCoroutine(showClueCo(this.targetFoundClue, this.foundClueDuration));
        }
    }

    public void decreaseAggro(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            setParameterOfAggrolist(collision.GetComponent<Character>(), -1 * this.aggroDecreaseFactor);
        }
    }

    #endregion


    #region Animation, StateMachine




    private void setAnimFloat(Vector2 setVector)
    {
        this.direction = setVector;

        Utilities.SetAnimatorParameter(this.animator, "moveX", setVector.x);
        Utilities.SetAnimatorParameter(this.animator, "moveY", setVector.y);
    }

    public void changeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) setAnimFloat(Vector2.right);
            else if (direction.x < 0) setAnimFloat(Vector2.left);
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0) setAnimFloat(Vector2.up);
            else if (direction.y < 0) setAnimFloat(Vector2.down);
        }
    }

    public void changeState(CharacterState newState)
    {
        if (this.currentState != newState) this.currentState = newState;
    }

    #endregion

}
