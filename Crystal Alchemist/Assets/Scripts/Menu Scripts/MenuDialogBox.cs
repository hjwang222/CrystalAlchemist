﻿using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public enum DialogBoxType
{
    yesNo,
    ok
}

public class MenuDialogBox : MenuControls
{
    [BoxGroup("DialogBox")]
    [SerializeField]
    private TextMeshProUGUI textfield;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private ButtonExtension YesButton;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private ButtonExtension NoButton;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private ButtonExtension OKButton;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private MiniGamePrice priceField;

    private UnityEvent actionYes; 
    private MenuControls lastMainMenu;

    private ResourceType currencyNeeded = ResourceType.none;
    private Item itemNeeded;
    private int price;

    [HideInInspector]
    public string dialogText;

    public void setActive(GameObject launcherObject)
    {
        this.priceField.gameObject.SetActive(false);
        MenuDialogBoxLauncher launcher = launcherObject.GetComponent<MenuDialogBoxLauncher>();

        this.YesButton.gameObject.SetActive(false);
        this.NoButton.gameObject.SetActive(false);
        this.OKButton.gameObject.SetActive(false);

        if (launcher != null)
        {
            this.price = launcher.price;
            this.itemNeeded = launcher.itemNeeded;
            this.currencyNeeded = launcher.currencyNeeded;

            this.child.SetActive(true);
            this.actionYes = launcher.actionOnConfirm;
            this.textfield.text = launcher.dialogText;

            if(launcher.dialogBoxType == DialogBoxType.ok)
            {
                this.OKButton.gameObject.SetActive(true);
            }
            else
            {
                this.YesButton.gameObject.SetActive(true);
                this.NoButton.gameObject.SetActive(true);

                if (launcher.currencyNeeded != ResourceType.none)
                {
                    this.priceField.gameObject.SetActive(true);
                    bool canActivateYES = this.priceField.updatePrice(this.currencyNeeded, this.itemNeeded, this.price, this.player);
                    this.YesButton.GetComponent<Selectable>().interactable = canActivateYES;
                }

                if (launcher.setYesButtonFirst) this.YesButton.setFirst();
                else this.NoButton.setFirst();
            }

            this.lastMainMenu = launcher.parentMenu;
            if (this.lastMainMenu != null) this.lastMainMenu.enableButtons(false);
        }
    }

    public void Yes()
    {        
        this.closeDialog();
        if (this.actionYes != null)
        {
            CustomUtilities.Items.reduceCurrency(this.currencyNeeded, this.itemNeeded, this.player, this.price);
            this.actionYes.Invoke();
        }
    }

    public void No()
    {
        this.closeDialog();
    }

    private void closeDialog()
    {
        if (this.lastMainMenu != null) this.lastMainMenu.enableButtons(true);   
        this.exitMenu();
    }

    public void SetText(string text)
    {
        this.textfield.text = text;
    }

    public void setYes(UnityEvent action)
    {
        this.actionYes = action;
    }
}
