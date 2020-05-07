using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

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
    public CustomCursor cursor;

    [BoxGroup("Menu Controls")]
    public bool isIngameMenu = false;

    [BoxGroup("Menu Controls")]
    [SerializeField]
    private bool disableExitButtonPress = false;

    [BoxGroup("Menu Controls")]
    [SerializeField]
    private Texture2D cursorTexture;

    [BoxGroup("Menu Controls")]
    [SerializeField]
    private UnityEvent action;

    public override void Start()
    {
        base.Start();

        if (GameEvents.current != null)
        {
            GameEvents.current.OnCancel += OnCancel;
            GameEvents.current.OnInventory += ExitMenu;
            GameEvents.current.OnPause += ExitMenu;
        }

        Cursor.SetCursor(cursorTexture, new Vector2(0, 0), CursorMode.ForceSoftware);
        Cursor.visible = true;
    }

    public virtual void OnDestroy()
    {
        if (GameEvents.current != null)
        {
            GameEvents.current.OnCancel -= OnCancel;
            GameEvents.current.OnInventory -= ExitMenu;
            GameEvents.current.OnPause -= ExitMenu;
        }
    }

    public override void OnEnable()
    {        
        if (this.cursor != null) this.cursor.gameObject.SetActive(true);

        if (this.isIngameMenu)
        {
            IngameMenuHandler.openedMenues.Add(this);

            if (IngameMenuHandler.openedMenues.Count <= 1)
            {
                Cursor.visible = true;

                IngameMenuHandler.lastState = this.playerValues.currentState;
                GameEvents.current.DoMenuOpen(CharacterState.inMenu);
                GameEvents.current.DoMenuOverlay(true);

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
            GameEvents.current.DoMenuClose(IngameMenuHandler.lastState);//avoid reclick!
            GameEvents.current.DoMenuOverlay(false);

            if (this.cursor != null) this.cursor.gameObject.SetActive(false);
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
            else ExitMenu();
        }
    }

    public virtual void OnClose()
    {
        if (!disableExitButtonPress) ExitMenu();        
    }

    public virtual void ExitMenu()
    {
        if (this.child != null && this.inputPossible) this.child.SetActive(false);
    }

    public void enableButtons(bool value)
    {
        List<Selectable> selectables = new List<Selectable>();
        UnityUtil.GetChildObjects<Selectable>(this.transform, selectables);

        foreach (Selectable selectable in selectables)
        {
            UnityUtil.SetInteractable(selectable, value);

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
