using UnityEngine;
using Sirenix.OdinInspector;

public class MenuControls : BasicMenu
{
    [HideInInspector]
    public Player player;

    [SerializeField]
    [BoxGroup("Mandatory")]
    [Required]
    private PlayerStats playerStats;

    [SerializeField]
    [BoxGroup("Mandatory")]
    private GameObject child;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private FloatSignal musicVolumeSignal;

    [BoxGroup("Mandatory")]
    [Required]
    public myCursor cursor;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private GameObject blackScreen;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private GameObject exitDialogBox;

    [HideInInspector]
    public CharacterState lastState;

    public bool disableMenuControls = false;

    public override void OnEnable()
    {
        if (!this.disableMenuControls)
        {
            this.player = this.playerStats.player;

            this.lastState = this.player.currentState;
            this.cursor.gameObject.SetActive(true);
            this.player.currentState = CharacterState.inMenu;

            this.musicVolumeSignal.Raise(GlobalValues.getMusicInMenu());
        }

        if (this.exitDialogBox != null) this.exitDialogBox.SetActive(false);

        base.OnEnable();
    }

    public override void OnDisable()
    {
        this.cursor.gameObject.SetActive(false);
        this.musicVolumeSignal.Raise(GlobalValues.backgroundMusicVolume);

        base.OnDisable();
    }

    public override void Update()
    {
        if (!this.disableMenuControls)
        {
            base.Update();

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
        if (this.exitDialogBox != null && !this.exitDialogBox.activeInHierarchy) this.exitDialogBox.SetActive(true);
        else if (this.exitDialogBox == null) exitMenu();
    }

    public void exitMenu()
    {
        if (this.cursor.infoBox != null) this.cursor.infoBox.Hide();
        if (this.player != null) this.player.delay(this.lastState);
        if (this.child != null) this.child.SetActive(false);
        if (this.blackScreen != null) this.blackScreen.SetActive(false);
    }
}
