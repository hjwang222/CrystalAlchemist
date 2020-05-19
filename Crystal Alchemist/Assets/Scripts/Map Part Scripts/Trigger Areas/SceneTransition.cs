﻿using UnityEngine;
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
            if (this.vcamSignal != null) this.vcamSignal.Raise();

            this.nextTeleport.SetValue(this.targetScene, this.playerPositionInNewScene, this.showAnimationIn, this.showAnimationOut);
            this.player.GetComponent<PlayerTeleport>().SwitchScene();
        }
    }
}
