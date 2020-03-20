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

    [BoxGroup("Activation Requirements")]
    [HideLabel]
    public Costs costs;

    private Player player;

    public void Awake()
    {
        if(this.fadeSignal != null) this.fadeSignal.Raise(true);
        if(this.dialogBox != null)
        {
            this.dialogBox.price = costs;
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

