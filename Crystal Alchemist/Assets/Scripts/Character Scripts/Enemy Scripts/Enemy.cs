using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class Enemy : Character
{
    [HideInInspector]
    public Character target;

    [FoldoutGroup("Enemy Attributes", expanded: false)]
    [SerializeField]
    [Range(0, 120)]
    private float aggroIncreaseFactor = 25;

    [FoldoutGroup("Enemy Attributes", expanded: false)]
    [SerializeField]
    [Range(0, 120)]
    private float aggroOnHitIncreaseFactor = 25;

    [FoldoutGroup("Enemy Attributes", expanded: false)]
    [SerializeField]
    [Range(-120, 0)]
    private float aggroDecreaseFactor = -25;

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
        this.aggroList.Clear();
        this.target = null;
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "isWakeUp", true);
    }

    #endregion


    public new void Update()
    {
        base.Update();
        generateAggro();
    }


    #region Aggro-System

    public void addAggro(Character newTarget, float aggro)
    {
        if(newTarget != null && this.aggroList.ContainsKey(newTarget)) this.aggroList[newTarget][0] += (float)(aggro/100);
    }

    public void getHighestAggro(out float aggro, out string target)
    {
        aggro = 0;
        target = "";

        foreach (Character character in this.aggroList.Keys)
        {
            float currentAggro = this.aggroList[character][0];

            if (currentAggro > aggro)
            {
                aggro = currentAggro;
                target = Utilities.Format.getLanguageDialogText(character.characterName, character.englischCharacterName);
            }
        }
    }

    public void clearAggro()
    {
        this.target = null;
        this.hideClue();
        this.aggroList.Clear();
    }

    public void generateAggro()
    {
        List<Character> charactersToRemove = new List<Character>();

        foreach (Character character in this.aggroList.Keys)
        {
            //                       amount                         factor
            addAggro(character, this.aggroList[character][1] * (Time.deltaTime * this.timeDistortion));

            //if (this.aggroList[character][0] > 0) Debug.Log(this.characterName + " hat " + this.aggroList[character][0] + " Aggro auf" + character.characterName);

            if (this.aggroList[character][0] >= 1f)
            {
                this.aggroList[character][0] = 1f; //aggro                             

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
        //Aggro, Increase, Factor
        if (!this.aggroList.ContainsKey(character)) this.aggroList.Add(character, new float[] { 0, 0 });
    }

    private void removeFromAggroList(Character character)
    {
        if (this.aggroList.ContainsKey(character)) this.aggroList.Remove(character);
    }

    private void setParameterOfAggrolist(Character character, float amount)
    {
        if (this.aggroList.ContainsKey(character))
        {
            this.aggroList[character][1] = amount; //set factor of increase/decreasing aggro            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            increaseAggro(collision.GetComponent<Character>(), this.aggroIncreaseFactor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            decreaseAggro(collision.GetComponent<Character>(), this.aggroDecreaseFactor);
        }
    }

    public void increaseAggroOnHit(Character newTarget)
    {
        if (newTarget != null)
        {
            addToAggroList(newTarget);            
            addAggro(newTarget, this.aggroOnHitIncreaseFactor);
            if(this.aggroList[newTarget][1] == 0) setParameterOfAggrolist(newTarget, this.aggroDecreaseFactor);
            if (this.target == null) StartCoroutine(showClueCo(this.targetFoundClue, this.foundClueDuration));
        }
    }


    public void increaseAggro(Character newTarget, float aggroIncrease)
    {
        if (newTarget != null)
        {
            addToAggroList(newTarget);
            setParameterOfAggrolist(newTarget, aggroIncrease);
            if (this.target == null) StartCoroutine(showClueCo(this.targetFoundClue, this.foundClueDuration));
        }
    }

    public void decreaseAggro(Character newTarget, float aggroDecrease)
    {
        if (newTarget != null) setParameterOfAggrolist(newTarget, aggroDecrease);
    }

    #endregion


    #region Animation, StateMachine


    private void setAnimFloat(Vector2 setVector)
    {
        this.direction = setVector;

        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveX", setVector.x);
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveY", setVector.y);
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
