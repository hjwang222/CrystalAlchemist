using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class AIAggroSystem : MonoBehaviour
{
    #region attributes

    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private AI enemy;


    [FoldoutGroup("Aggro Attributes", expanded: false)]
    [SerializeField]
    private bool firstHitMaxAggro = true;


    [FoldoutGroup("Aggro Attributes", expanded: false)]
    [SerializeField]
    [Range(0, 120)]
    private float aggroIncreaseFactor = 25;

    [FoldoutGroup("Aggro Attributes", expanded: false)]
    [SerializeField]
    [Range(0, 120)]
    private float aggroOnHitIncreaseFactor = 25;

    [FoldoutGroup("Aggro Attributes", expanded: false)]
    [SerializeField]
    [Range(-120, 0)]
    private float aggroDecreaseFactor = -25;

    [FoldoutGroup("Aggro Attributes", expanded: false)]
    [SerializeField]
    private float aggroNeededToTarget = 100;

    [FoldoutGroup("Aggro Attributes", expanded: false)]
    [SerializeField]
    private float targetChangeDelay = 0f;


    [FoldoutGroup("Aggro Object Attributes", expanded: false)]
    [SerializeField]
    private GameObject targetFoundClue;

    [FoldoutGroup("Aggro Object Attributes", expanded: false)]
    [SerializeField]
    private float foundClueDuration = 1;

    [FoldoutGroup("Aggro Object Attributes", expanded: false)]
    [SerializeField]
    private GameObject targetActiveClue;

    [FoldoutGroup("Aggro Object Object Attributes", expanded: false)]
    [SerializeField]
    private float activeClueDuration = 1;


    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt nur auf sich selbst")]
    public bool affectSelf = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Spieler")]
    public bool affectOther = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Gegner")]
    public bool affectSame = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Gegner")]
    public bool affectNeutral = false;

    private GameObject activeClue;
    private Dictionary<Character, float[]> aggroList = new Dictionary<Character, float[]>();

    #endregion


    #region Start und Update

    private void Start()
    {
        this.aggroList.Clear();
    }

    private void Update()
    {
        generateAggro();
    }

    #endregion


    #region Aggro-System

    public void addAggro(Character newTarget, float aggro)
    {
        if (newTarget != null && this.aggroList.ContainsKey(newTarget)) this.aggroList[newTarget][0] += (float)(aggro / 100);
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
        this.enemy.target = null;
        this.hideClue();
        this.aggroList.Clear();
    }

    public void generateAggro()
    {
        List<Character> charactersToRemove = new List<Character>();

        if(this.enemy.target != null 
            && (!this.enemy.target.gameObject.activeInHierarchy 
            || this.enemy.target.currentState == CharacterState.dead))
        {
            this.aggroList.Remove(this.enemy.target);
            this.enemy.target = null;
        }

        foreach (Character character in this.aggroList.Keys)
        {
            //                       amount                         factor
            addAggro(character, this.aggroList[character][1] * (Time.deltaTime * this.enemy.timeDistortion));

            //if (this.aggroList[character][0] > 0) Debug.Log(this.characterName + " hat " + this.aggroList[character][0] + " Aggro auf" + character.characterName);

            if (this.aggroList[character][0] >= (this.aggroNeededToTarget/100))
            {
                //this.aggroList[character][0] = 1f; //aggro                             

                //Aggro max, Target!
                if (this.enemy.target == null)
                {
                    StartCoroutine(switchTargetCo(0, character));
                }
                else
                {
                    if(this.aggroList[this.enemy.target][1] < this.aggroList[character][1])
                    {
                        StartCoroutine(switchTargetCo(this.targetChangeDelay, character));
                    }
                }
            }
            else if (this.aggroList[character][0] <= 0)
            {
                this.aggroList[character][0] = 0; //aggro   

                //Aggro lost, Target lost
                if (this.enemy.target == character) this.enemy.target = null;
                charactersToRemove.Add(character);
                hideClue();
            }
        }

        foreach (Character character in charactersToRemove)
        {
            this.removeFromAggroList(character);
        }
    }


    private IEnumerator switchTargetCo(float delay, Character character)
    {
        yield return new WaitForSeconds(delay);
        this.enemy.target = character;
        StartCoroutine(showClueCo(this.targetActiveClue, this.activeClueDuration));
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
        float aggroAmount = 0f;
        float factor = 0f;

        if (!this.aggroList.ContainsKey(character))
        {   
            this.aggroList.Add(character, new float[] { aggroAmount, factor });            
        }
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
        if (Utilities.Collisions.checkAffections(this.enemy, this.affectOther, this.affectSame, this.affectNeutral, collision))
        {
            increaseAggro(collision.GetComponent<Character>(), this.aggroIncreaseFactor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Utilities.Collisions.checkAffections(this.enemy, this.affectOther, this.affectSame, this.affectNeutral, collision))
        {
            decreaseAggro(collision.GetComponent<Character>(), this.aggroDecreaseFactor);
        }
    }

    public void increaseAggroOnHit(Character newTarget, float damage)
    {
        if (newTarget != null)
        {
            addToAggroList(newTarget);

            addAggro(newTarget, (this.aggroOnHitIncreaseFactor*damage));
            if (this.aggroList.Count == 1 && this.firstHitMaxAggro) addAggro(newTarget, this.aggroNeededToTarget);

            if (this.aggroList[newTarget][1] == 0) setParameterOfAggrolist(newTarget, this.aggroDecreaseFactor);
            if (this.enemy.target == null) StartCoroutine(showClueCo(this.targetFoundClue, this.foundClueDuration));
        }
    }


    public void increaseAggro(Character newTarget, float aggroIncrease)
    {
        if (newTarget != null)
        {
            addToAggroList(newTarget);
            setParameterOfAggrolist(newTarget, aggroIncrease);
            if (this.enemy.target == null) StartCoroutine(showClueCo(this.targetFoundClue, this.foundClueDuration));
        }
    }

    public void decreaseAggro(Character newTarget, float aggroDecrease)
    {
        if (newTarget != null) setParameterOfAggrolist(newTarget, aggroDecrease);
    }

    #endregion
}
