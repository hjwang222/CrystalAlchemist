using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class SceneTransition : MonoBehaviour
{
    [Header("New Scene Variables")]
    [Tooltip("Name der nächsten Map")]
    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private string targetScene;

    [Tooltip("Spawnpunkt des Spielers")]
    [SerializeField]
    [BoxGroup("Required")]
    [Required]
    private Vector2 playerPositionInNewScene;

    [BoxGroup("Transition")]
    [SerializeField]
    private bool showTransition = false;

    [ShowIf("showTransition", true)]
    [BoxGroup("Transition")]
    [SerializeField]
    private SimpleSignal vcamSignal;

    [ShowIf("showTransition", true)]
    [BoxGroup("Transition")]
    [Required]
    [SerializeField]
    private BoolSignal fadeSignal;

    [ShowIf("showTransition", true)]
    [BoxGroup("Transition")]
    [Required]
    [SerializeField]
    private FloatValue transitionDuration;

    [ShowIf("showTransition", true)]
    [BoxGroup("Transition")]
    [SerializeField]
    private bool showAnimationIn = false;

    [ShowIf("showTransition", true)]
    [BoxGroup("Transition")]
    [SerializeField]
    private bool showAnimationOut = false;

    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private TeleportStats nextTeleport;

    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private GameObjectSignal cleanUp;

    [BoxGroup("Required")]
    [SerializeField]
    private MenuDialogBoxLauncher dialogBox;

    [FoldoutGroup("Activation Requirements", expanded: false)]
    [EnumToggleButtons]
    [Tooltip("Was benötigt wird um zu öffnen")]
    public ResourceType currencyNeeded = ResourceType.none;

    [FoldoutGroup("Activation Requirements", expanded: false)]
    [ShowIf("currencyNeeded", ResourceType.item)]
    [Tooltip("Benötigtes Item")]
    public Item item;

    [FoldoutGroup("Activation Requirements", expanded: false)]
    [HideIf("currencyNeeded", ResourceType.none)]
    [Range(0, CustomUtilities.maxIntInfinite)]
    public int price = 0;

    private Player player;

    public void Awake()
    {
        if(this.fadeSignal != null) this.fadeSignal.Raise(true);
        if(this.dialogBox != null)
        {
            this.dialogBox.currencyNeeded = this.currencyNeeded;
            this.dialogBox.itemNeeded = this.item;
            this.dialogBox.price = this.price;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            this.player = other.GetComponent<Player>();
            if (this.dialogBox == null) transferToScene();
            else this.dialogBox.raiseDialogBox();
        }
    }

    public void transferToScene()
    {
        if (this.player != null && this.player.currentState != CharacterState.respawning)
        {
            if (this.vcamSignal != null) this.vcamSignal.Raise();

            this.nextTeleport.location = this.targetScene;
            this.nextTeleport.position = this.playerPositionInNewScene;

            if (this.cleanUp != null) this.cleanUp.Raise(null);

            this.player.GetComponent<PlayerTeleport>().teleportPlayerToNextScene(this.transitionDuration.getValue(), this.showAnimationOut, this.showAnimationIn);
        }
    }
}

