using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System.Collections.Generic;

public enum DialogBoxType
{
    yesNo,
    ok
}

public class MenuDialogBox : MenuManager
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

    [BoxGroup("DialogBox")]
    [SerializeField]
    private PlayerInventory inventory;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private MenuDialogBoxInfo info;

    private UnityEvent OnConfirm; 
    private GameObject lastMainMenu;
    private Costs price;

    public override void Start()
    {
        base.Start();
        Initialize();
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void Initialize()
    {
        init();
        this.OnConfirm = this.info.OnConfirm;
        this.lastMainMenu = this.info.parent;
        this.textfield.text = this.info.text;

        if (this.info.type == DialogBoxType.ok) this.OKButton.gameObject.SetActive(true);        
        else
        {
            this.YesButton.gameObject.SetActive(true);
            this.NoButton.gameObject.SetActive(true);
            setPrice(this.info.costs);
        }
        
        if (this.lastMainMenu != null) this.EnableButtons(false, this.lastMainMenu);
    }

    private void init()
    {
        this.YesButton.gameObject.SetActive(false);
        this.NoButton.gameObject.SetActive(false);
        this.OKButton.gameObject.SetActive(false);
        this.price = null;
        this.priceField.gameObject.SetActive(false);
        UnityUtil.SetInteractable(this.YesButton.GetComponent<Selectable>(), true);
    }

    private void setPrice(Costs cost)
    {
        this.price = cost;
        if (this.price != null && this.price.resourceType != CostType.none)
        {
            this.priceField.gameObject.SetActive(true);
            bool enabled = this.priceField.updatePrice(this.inventory, this.price);
            UnityUtil.SetInteractable(this.YesButton.GetComponent<Selectable>(), enabled);
        }
    }

    public void Yes()
    {        
        this.closeDialog();
        if (this.OnConfirm != null)
        {
            GameEvents.current.DoReduce(this.price);
            this.OnConfirm.Invoke();
        }
    }

    public void No() => this.closeDialog();    

    private void closeDialog()
    {
        if (this.lastMainMenu != null) this.EnableButtons(true, this.lastMainMenu);
        this.ExitMenu();
    }

    private void EnableButtons(bool value, GameObject parent)
    {
        List<Selectable> selectables = new List<Selectable>();
        UnityUtil.GetChildObjects<Selectable>(parent.transform, selectables);

        foreach (Selectable selectable in selectables)
        {
            UnityUtil.SetInteractable(selectable, value);

            if (value)
            {
                ButtonExtension buttonExtension = selectable.GetComponent<ButtonExtension>();
                if (buttonExtension != null)
                {
                    buttonExtension.enabled = value;
                    buttonExtension.SetFirst();
                }
            }
        }
    }
}
