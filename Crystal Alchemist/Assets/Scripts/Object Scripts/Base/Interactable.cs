using UnityEngine;
using Sirenix.OdinInspector;

public class Interactable : MonoBehaviour
{
    #region Attribute
    [BoxGroup("Activation Requirements")]
    [HideLabel]
    public Costs costs;

    [BoxGroup("ContextMenu")]
    [SerializeField]
    private bool customActionButton = false;

    [BoxGroup("ContextMenu")]
    [ShowIf("customActionButton")]
    [SerializeField]
    private string ID;

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

    private void Update() => DoOnUpdate();

    public string translationID
    {
        get { return this.ID; }
        set { this.ID = value; }
    }

    public virtual void DoOnUpdate() { }

    private void OnSubmit()
    {
        if (PlayerCanInteract()) DoOnSubmit();
    }

    public virtual void DoOnSubmit() { }

    private void OnDestroy() => GameEvents.current.OnSubmit -= OnSubmit;

    private void OnDisable() { if (this.context != null) this.context.gameObject.SetActive(false); }
    

    #endregion

    #region Context Clue Funktionen

    public void CheckInteraction(Player player)
    {
        if (player != null)
        {
            if (this.player != player) this.player = player;
            this.isPlayerInRange = true;
            this.isPlayerLookingAtIt = PlayerIsLooking();
        }

        if (PlayerCanInteract())
        {
            if (this.player.values.currentState != CharacterState.interact)
            {
                if (this.customActionButton) MasterManager.actionButtonText.SetValue(this.ID);
                else MasterManager.actionButtonText.SetValue(string.Empty);

                this.context.gameObject.SetActive(true);
                this.player.values.currentState = CharacterState.interact;
            }
        }
        else
        {
            if (this.player.values.currentState == CharacterState.interact)
            {
                this.context.gameObject.SetActive(false);
                this.player.values.currentState = CharacterState.idle;
            }
        }
    }

    public void DeactivateInteraction()
    {
        if (this.player != null)
        {
            this.context.gameObject.SetActive(false);
            this.player.values.currentState = CharacterState.idle;
            this.player = null;
        }

        this.isPlayerInRange = false;
        this.isPlayerLookingAtIt = false;
        this.context.gameObject.SetActive(false);
    }

    private void interact(Collider2D characterCollisionBox)
    {
        if (!characterCollisionBox.isTrigger)
        {
            Player player = characterCollisionBox.GetComponent<Player>();
            CheckInteraction(player);            
        }
    }

    private bool PlayerCanInteract()
    {
        return (this.player != null            
            && this.isPlayerInRange
            && this.isPlayerLookingAtIt
            && this.player.values.CanInteract());
    }

    public virtual bool PlayerIsLooking()
    {
        if (this.isPlayerInRange && CollisionUtil.checkIfGameObjectIsViewed(this.player, this.gameObject)) return true;
        return false;
    }

    private void OnTriggerStay2D(Collider2D characterCollisionBox) => interact(characterCollisionBox);

    private void OnTriggerEnter2D(Collider2D characterCollisionBox) => interact(characterCollisionBox);

    private void OnTriggerExit2D(Collider2D characterCollisionBox)
    {
        if (!characterCollisionBox.isTrigger) DeactivateInteraction();
    }

    public void ShowDialog(DialogTextTrigger trigger, ItemStats stats)
    {
        if (this.GetComponent<DialogSystem>() != null) this.GetComponent<DialogSystem>().showDialog(this.player, this, trigger, stats);
    }

    public void ShowDialog(DialogTextTrigger trigger)
    {
        if(this.GetComponent<DialogSystem>() != null) this.GetComponent<DialogSystem>().showDialog(this.player, this, trigger);
    }

    public void ShowDialog()
    {
        if (this.GetComponent<DialogSystem>() != null) this.GetComponent<DialogSystem>().showDialog(this.player, this);
    }

    public void ShowMenuDialog()
    {
        if (this.GetComponent<MenuDialogBoxLauncher>() != null) this.GetComponent<MenuDialogBoxLauncher>().ShowDialogBox();
    }
    #endregion
}
