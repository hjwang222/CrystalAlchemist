using UnityEngine;
using Sirenix.OdinInspector;

public class MenuControls : PreventDeselection
{
    [HideInInspector]
    public Player player;

    [SerializeField]
    [BoxGroup("Mandatory")]
    private PlayerStats playerStats;

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

    public virtual void OnEnable()
    {
        this.player = this.playerStats.player;

        this.lastState = this.player.currentState;
        this.cursor.gameObject.SetActive(true);
        this.player.currentState = CharacterState.inMenu;

        this.musicVolumeSignal.Raise(GlobalValues.getMusicInMenu());
    }

    public virtual void OnDisable()
    {
        this.cursor.gameObject.SetActive(false);
        this.musicVolumeSignal.Raise(GlobalValues.backgroundMusicVolume);
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Cancel"))
        {
            if (this.cursor.infoBox.gameObject.activeInHierarchy) this.cursor.infoBox.Hide();
            else exitMenu();
        }
        else if (Input.GetButtonDown("Inventory") || Input.GetButtonDown("Pause")) exitMenu();
    }

    public void exitMenu()
    {
        if(this.cursor.infoBox != null) this.cursor.infoBox.Hide();
        this.player.delay(this.lastState);
        this.transform.parent.gameObject.SetActive(false);
        if(this.blackScreen != null) this.blackScreen.SetActive(false);
    }
}
