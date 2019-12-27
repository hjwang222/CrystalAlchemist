using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class InventoryMenu : MenuControls
{
    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI regularItemsLabel;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI keyItemsLabel;

    [BoxGroup("Signals")]
    [SerializeField]
    private SimpleSignal skillMenuSignal;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject regularItems;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject keyItems;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject quickmenu;

    private void Start()
    {
        showCategory(0);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        loadInventory();
    }


    private void loadInventory()
    {      
        setItemsToSlots(this.regularItems, false);
        setItemsToSlots(this.keyItems, true);
        setItemsToSlots(this.quickmenu, true);
    }

    public void openSkillMenu(Item item)
    {
        this.transform.parent.gameObject.SetActive(false);
        this.player.currentState = this.lastState;
        item.keyItemSignal.Raise();
    }

    public void openMap()
    {
        this.transform.parent.gameObject.SetActive(false);
        this.player.currentState = this.lastState;
        //raise Map Signal
    }

    public void switchCategory()
    {
        if (this.regularItems.activeInHierarchy) showCategory(1);
        else showCategory(0);
    }

    public void showCategory(int category)
    {
        this.regularItems.SetActive(false);
        this.regularItemsLabel.gameObject.SetActive(false);
        this.keyItems.SetActive(false);
        this.keyItemsLabel.gameObject.SetActive(false);

        switch (category)
        {
            case 1: this.keyItems.SetActive(true); this.keyItemsLabel.gameObject.SetActive(true); break;
            default: this.regularItems.SetActive(true); this.regularItemsLabel.gameObject.SetActive(true); break;
        }
    }

    private void setItemsToSlots(GameObject categoryGameobject, bool showKeyItems)
    {
        if (this.player != null)
        {
            for (int i = 0; i < categoryGameobject.transform.childCount; i++)
            {
                GameObject slot = categoryGameobject.transform.GetChild(i).gameObject;
                InventorySlot iSlot = slot.GetComponent<InventorySlot>();
                Item item = null;

                //if (iSlot.getFeature() != ItemFeature.none) item = Utilities.Items.getItemByFeature(this.player.inventory, iSlot.getFeature());
                //else 

                int ID = iSlot.getID();
                item = CustomUtilities.Items.getItemByID(this.player.inventory, ID, showKeyItems);

                slot.GetComponent<InventorySlot>().setItemToSlot(item);
            }
        }
    }
}
