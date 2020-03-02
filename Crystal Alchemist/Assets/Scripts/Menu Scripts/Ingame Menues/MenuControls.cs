using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuControls : BasicMenu
{
    [HideInInspector]
    public Player player;

    [SerializeField]
    [BoxGroup("Mandatory")]
    [Required]
    private PlayerStats playerStats;

    [BoxGroup("Mandatory")]
    public GameObject child;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private FloatSignal musicVolumeSignal;

    [BoxGroup("Mandatory")]
    [Required]
    public myCursor cursor;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private GameObject blackScreen;

    [HideInInspector]
    public CharacterState lastState;

    [BoxGroup("Menu Controls")]
    public bool isIngameMenu = false;

    [BoxGroup("Menu Controls")]
    [SerializeField]
    private bool disableExitButtonPress = false;

    [BoxGroup("Menu Controls")]
    [SerializeField]
    private MenuDialogBoxLauncher exitDialogBox;

    public override void OnEnable()
    {
        if (this.cursor != null) this.cursor.gameObject.SetActive(true);
        if (this.blackScreen != null) this.blackScreen.SetActive(true);

        if (this.isIngameMenu)
        {
            this.player = this.playerStats.player;
            this.lastState = this.player.currentState;
            this.player.currentState = CharacterState.inMenu;
            this.musicVolumeSignal.Raise(GlobalValues.getMusicInMenu());
        }

        base.OnEnable();
    }

    public override void OnDisable()
    {
        if (this.cursor.infoBox != null) this.cursor.infoBox.Hide();

        if (this.isIngameMenu)
        {
            if (this.player != null) this.player.delay(this.lastState);
            if (this.cursor != null) this.cursor.gameObject.SetActive(false);
            if (this.blackScreen != null) this.blackScreen.SetActive(false);
            this.musicVolumeSignal.Raise(GlobalValues.backgroundMusicVolume);
        }

        base.OnDisable();
    }

    public override void Update()
    {
        base.Update();

        if (!disableExitButtonPress)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (this.cursor.infoBox.gameObject.activeInHierarchy) this.cursor.infoBox.Hide();
                else showExitDialogBox();
            }
            else if (Input.GetButtonDown("Inventory") || Input.GetButtonDown("Pause")) showExitDialogBox();
        }
    }

    public void showExitDialogBox()
    {
        if (this.exitDialogBox != null) this.exitDialogBox.raiseDialogBox();        
        else exitMenu();
    }

    public virtual void exitMenu()
    { 
        if (this.child != null) this.child.SetActive(false);
    }
}
