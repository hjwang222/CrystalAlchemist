using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuControls : BasicMenu
{
    [Required]
    public CharacterValues playerValues;

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

        if (GameEvents.current != null)
        {
            GameEvents.current.OnCancel += OnCancel;
            GameEvents.current.OnInventory += showExitDialogBox;
            GameEvents.current.OnPause += showExitDialogBox;
        }

        Cursor.SetCursor(cursorTexture, new Vector2(0, 0), CursorMode.ForceSoftware);
        Cursor.visible = true;
    }

    private void OnDestroy()
    {
        if (GameEvents.current != null)
        {
            GameEvents.current.OnCancel -= OnCancel;
            GameEvents.current.OnInventory -= showExitDialogBox;
            GameEvents.current.OnPause -= showExitDialogBox;
        }
    }

    public override void OnEnable()
    {      
        if (this.cursor != null) this.cursor.gameObject.SetActive(true);
        if (this.blackScreen != null) this.blackScreen.SetActive(true);

        if (this.isIngameMenu)
        {
            IngameMenuHandler.openedMenues.Add(this);

            if (IngameMenuHandler.openedMenues.Count <= 1)
            {
                Cursor.visible = true;

                IngameMenuHandler.lastState = this.playerValues.currentState;
                this.playerValues.setStateAfterMenuClose(CharacterState.inMenu);

                if (this.musicVolumeSignal != null) this.musicVolumeSignal.Raise(MasterManager.settings.getMusicInMenu());
            }
        }

        base.OnEnable();
    }

    public override void OnDisable()
    {
        IngameMenuHandler.openedMenues.Remove(this);

        if (this.isIngameMenu && IngameMenuHandler.openedMenues.Count <= 0)
        {
            Cursor.visible = false;

            this.playerValues.setStateAfterMenuClose(IngameMenuHandler.lastState); //avoid reclick!

            if (this.cursor != null) this.cursor.gameObject.SetActive(false);
            if (this.blackScreen != null) this.blackScreen.SetActive(false);
            if (this.musicVolumeSignal != null) this.musicVolumeSignal.Raise(MasterManager.settings.backgroundMusicVolume);
        }

        if (this.cursor.infoBox != null) this.cursor.infoBox.Hide();
        base.OnDisable();
    }

    public override void Update()
    {
        base.Update();        
    }

    public virtual void OnCancel()
    {
        if (!disableExitButtonPress)
        {
            if (this.cursor.infoBox.gameObject.activeInHierarchy) this.cursor.infoBox.Hide();
            else showExitDialogBox();
        }
    }

    public virtual void OnClose()
    {
        if (!disableExitButtonPress) showExitDialogBox();        
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
        UnityUtil.GetChildObjects<Selectable>(this.transform, selectables);

        foreach (Selectable selectable in selectables)
        {
            selectable.interactable = value;

            if (value)
            {
                ButtonExtension buttonExtension = selectable.GetComponent<ButtonExtension>();
                if (buttonExtension != null)
                {
                    buttonExtension.enabled = value;
                    buttonExtension.setFirst();
                }
            }
        }
    }
}
