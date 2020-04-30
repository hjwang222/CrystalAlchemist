using UnityEngine;
using Sirenix.OdinInspector;


public class Interactable : MonoBehaviour
{
    #region Attribute
    [BoxGroup("Activation Requirements")]
    [HideLabel]
    public Costs costs;

    [BoxGroup("Sound")]
    [Tooltip("Standard-Soundeffekt")]
    public AudioClip soundEffect;

    [HideInInspector]
    public bool isPlayerInRange = false;
    [HideInInspector]
    public bool isPlayerLookingAtIt = false;
    [HideInInspector]
    public Player player;
    [HideInInspector]
    public ContextClue context;

    #endregion


    #region Start Funktionen (init, ContextClue, Item set bzw. Lootregeln)

    public virtual void Start()
    {
        GameEvents.current.OnSubmit += OnSubmit;
        this.context = Instantiate(MasterManager.contextClue, this.transform.position, Quaternion.identity, this.transform);
    }

    private void Update()
    {
        doOnUpdate();
    }

    public virtual void doOnUpdate()
    {

    }

    private void OnSubmit()
    {
        if (this.player != null
            && this.isPlayerInRange
            && this.isPlayerLookingAtIt
            && this.player.values.currentState == CharacterState.interact)
        {
            doSomethingOnSubmit();            
        }
    }

    public virtual void doSomethingOnSubmit()
    {

    }

    private void OnDestroy()
    {
        GameEvents.current.OnSubmit -= OnSubmit;
    }


    #endregion

    #region Context Clue Funktionen

    private void interact(Collider2D characterCollisionBox)
    {
        if (!characterCollisionBox.isTrigger)
        {
            Player player = characterCollisionBox.GetComponent<Player>();

            if (player != null)
            {
                if (this.player != player) this.player = player;
                this.isPlayerInRange = true;

                checkifLooking(this.player);
            }
        }
    }

    private void checkifLooking(Character character)
    {
        if (character != null
            && (character.values.currentState == CharacterState.interact
             || character.values.currentState == CharacterState.idle
             || character.values.currentState == CharacterState.walk))
        {
            if (this.isPlayerInRange
                && CollisionUtil.checkIfGameObjectIsViewed(character, this.gameObject))
            {
                player.values.currentState = CharacterState.interact;
                this.context.gameObject.SetActive(true);
                this.isPlayerLookingAtIt = true;
            }
            else
            {
                player.values.currentState = CharacterState.idle;
                this.context.gameObject.SetActive(false);
                this.isPlayerLookingAtIt = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D characterCollisionBox)
    {
        interact(characterCollisionBox);
    }

    private void OnTriggerEnter2D(Collider2D characterCollisionBox)
    {
        interact(characterCollisionBox);
    }

    private void OnTriggerExit2D(Collider2D characterCollisionBox)
    {
        if (!characterCollisionBox.isTrigger)
        {
            Player player = characterCollisionBox.GetComponent<Player>();

            if (player != null && player.values.currentState == CharacterState.interact)
            {
                player.values.currentState = CharacterState.idle;
                if (this.player == player) this.player = null;
            }

            this.isPlayerInRange = false;
            this.isPlayerLookingAtIt = false;
            this.context.gameObject.SetActive(false);
        }
    }
    #endregion
}
