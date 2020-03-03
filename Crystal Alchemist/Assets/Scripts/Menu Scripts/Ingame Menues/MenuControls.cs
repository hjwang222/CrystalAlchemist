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

    [BoxGroup("Menu Controls")]
    [SerializeField]
    private Texture2D cursorTexture;

    public override void Start()
    {
        base.Start();
        Cursor.SetCursor(cursorTexture, new Vector2(0, 0), CursorMode.ForceSoftware);
        Cursor.visible = true;
    }

    public override void OnEnable()
    {
        if (this.cursor != null) this.cursor.gameObject.SetActive(true);
        if (this.blackScreen != null) this.blackScreen.SetActive(true);

        if (this.isIngameMenu)
        {
            Cursor.visible = true;
            this.player = this.playerStats.player;
            this.lastState = this.player.currentState;
            this.player.currentState = CharacterState.inMenu;
            if(this.musicVolumeSignal != null) this.musicVolumeSignal.Raise(GlobalValues.getMusicInMenu());
        }

        base.OnEnable();
    }

    public override void OnDisable()
    {
        if (this.cursor.infoBox != null) this.cursor.infoBox.Hide();

        if (this.isIngameMenu)
        {
            Cursor.visible = false;
            if (this.player != null) this.player.delay(this.lastState);
            if (this.cursor != null) this.cursor.gameObject.SetActive(false);
            if (this.blackScreen != null) this.blackScreen.SetActive(false);
            if (this.musicVolumeSignal != null) this.musicVolumeSignal.Raise(GlobalValues.backgroundMusicVolume);
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

    public void enableButtons(bool value)
    {
        List<Selectable> selectables = new List<Selectable>();
        CustomUtilities.UnityFunctions.GetChildObjects<Selectable>(this.transform, selectables);

        foreach (Selectable selectable in selectables)
        {
            selectable.interactable = value;
            ButtonExtension buttonExtension = selectable.GetComponent<ButtonExtension>();
            if (buttonExtension != null)
            {
                buttonExtension.enabled = value;
                buttonExtension.setFirst();
            }
        }
    }
}
