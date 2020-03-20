using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AIAggroSystem : MonoBehaviour
{
    #region attributes

    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private AI enemy;

    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private AggroStats aggroStats;

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
        RotationUtil.rotateCollider(this.enemy, this.gameObject);
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
                target = FormatUtil.getLanguageDialogText(character.stats.characterName, character.stats.englischCharacterName);
            }
        }
    }

    private void OnDisable()
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
            || this.enemy.target.currentState == CharacterState.dead
            || this.enemy.target.currentState == CharacterState.respawning))
        {
            this.aggroList.Remove(this.enemy.target);
            this.enemy.target = null;
        }

        foreach (Character character in this.aggroList.Keys)
        {
            //                       amount                         factor
            addAggro(character, this.aggroList[character][1] * (Time.deltaTime * this.enemy.timeDistortion));

            //if (this.aggroList[character][0] > 0) Debug.Log(this.characterName + " hat " + this.aggroList[character][0] + " Aggro auf" + character.characterName);

            if (this.aggroList[character][0] >= (this.aggroStats.aggroNeededToTarget /100))
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
                        StartCoroutine(switchTargetCo(this.aggroStats.targetChangeDelay, character));
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
        StartCoroutine(showClueCo(this.aggroStats.targetActiveClue, this.aggroStats.activeClueDuration));
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
            Vector3 position = new Vector3(this.enemy.transform.position.x - 0.5f, this.enemy.transform.position.y + 0.5f);
            this.activeClue = Instantiate(clue, position, Quaternion.identity, this.enemy.transform);
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
        if (CollisionUtil.checkAffections(this.enemy, this.aggroStats.affectOther, this.aggroStats.affectSame, this.aggroStats.affectNeutral, collision))
        {
            increaseAggro(collision.GetComponent<Character>(), this.aggroStats.aggroIncreaseFactor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CollisionUtil.checkAffections(this.enemy, this.aggroStats.affectOther, this.aggroStats.affectSame, this.aggroStats.affectNeutral, collision))
        {
            decreaseAggro(collision.GetComponent<Character>(), this.aggroStats.aggroDecreaseFactor);
        }
    }

    public void increaseAggroOnHit(Character newTarget, float damage)
    {
        if (newTarget != null)
        {
            addToAggroList(newTarget);

            addAggro(newTarget, (this.aggroStats.aggroOnHitIncreaseFactor));
            if (this.aggroList.Count == 1 && this.aggroStats.firstHitMaxAggro) addAggro(newTarget, (this.aggroStats.aggroNeededToTarget + (this.aggroStats.aggroDecreaseFactor * (-1))));

            if (this.aggroList[newTarget][1] == 0) setParameterOfAggrolist(newTarget, this.aggroStats.aggroDecreaseFactor);
            if (this.enemy.target == null) StartCoroutine(showClueCo(this.aggroStats.targetFoundClue, this.aggroStats.foundClueDuration));
        }
    }


    public void increaseAggro(Character newTarget, float aggroIncrease)
    {
        if (newTarget != null)
        {
            addToAggroList(newTarget);
            setParameterOfAggrolist(newTarget, aggroIncrease);
            if (this.enemy.target == null) StartCoroutine(showClueCo(this.aggroStats.targetFoundClue, this.aggroStats.foundClueDuration));
        }
    }

    public void decreaseAggro(Character newTarget, float aggroDecrease)
    {
        if (newTarget != null) setParameterOfAggrolist(newTarget, aggroDecrease);
    }

    #endregion
}
