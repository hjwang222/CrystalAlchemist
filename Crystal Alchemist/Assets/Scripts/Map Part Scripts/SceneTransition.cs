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
    private TeleportStats teleportStat;

    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private GameObjectSignal cleanUp;


    public void Awake()
    {
        if(this.fadeSignal != null) this.fadeSignal.Raise(true);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player.currentState != CharacterState.respawning)
            {                
                if(this.vcamSignal != null) this.vcamSignal.Raise();

                this.teleportStat.location = this.targetScene;
                this.teleportStat.position = this.playerPositionInNewScene;

                if (this.cleanUp != null) this.cleanUp.Raise(null);

                player.GetComponent<PlayerTeleport>().teleportPlayer(this.transitionDuration.getValue(), this.showAnimationOut, this.showAnimationIn);

            }
        }
    }
}

