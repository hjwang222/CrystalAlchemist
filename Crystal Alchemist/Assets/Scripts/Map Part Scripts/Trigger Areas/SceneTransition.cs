using UnityEngine;
using Sirenix.OdinInspector;

public class SceneTransition : MonoBehaviour
{
    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private TeleportStats stats;

    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private PlayerTeleportList teleportList;

    [BoxGroup("Required")]
    [SerializeField]
    private MenuDialogBoxLauncher dialogBox;

    private Player player;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            this.player = other.GetComponent<Player>();
            if (this.dialogBox == null) transferToScene();
            else this.dialogBox.ShowDialogBox();
        }
    }

    public void transferToScene()
    {
        if (this.player != null && this.player.values.currentState != CharacterState.respawning)
        {
            this.teleportList.SetNextTeleport(this.stats);
            GameEvents.current.DoTeleport();
        }
    }
}

