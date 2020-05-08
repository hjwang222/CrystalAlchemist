using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class InventoryMenu : MenuControls
{
    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private PlayerInventory inventory;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI regularItemsLabel;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI keyItemsLabel;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject regularItems;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject currencyItems;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject keyItems;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject quickmenu;

    public override void Start()
    {
        base.Start();
        showCategory(0);
        loadInventory();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        loadInventory();
    }

    public override void Cancel()
    {
        if (this.keyItems.activeInHierarchy) this.showCategory(0);
        else base.Cancel();
    }

    private void loadInventory()
    {
        setItemsToSlots(this.regularItems, false);
        setItemsToSlots(this.keyItems, true);
        setItemsToSlots(this.quickmenu, true);
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
        this.currencyItems.SetActive(false);
        this.keyItems.SetActive(false);
        this.keyItemsLabel.gameObject.SetActive(false);

        switch (category)
        {
            case 1: this.keyItems.SetActive(true); this.keyItemsLabel.gameObject.SetActive(true); this.currencyItems.SetActive(true); break;
            default: this.regularItems.SetActive(true); this.regularItemsLabel.gameObject.SetActive(true); break;
        }
    }

    private void setItemsToSlots(GameObject categoryGameobject, bool showKeyItems)
    {
        for (int i = 0; i < categoryGameobject.transform.childCount; i++)
        {
            GameObject slot = categoryGameobject.transform.GetChild(i).gameObject;
            InventorySlot iSlot = slot.GetComponent<InventorySlot>();

            int ID = iSlot.getID();

            if (showKeyItems)
            {
                ItemStats item = this.inventory.GetKeyItem(ID);
                slot.GetComponent<InventorySlot>().setItemToSlot(item);
            }
            else
            {
                ItemGroup item = this.inventory.GetInventoryItem(ID);
                slot.GetComponent<InventorySlot>().setItemToSlot(item);
            }
        }
    }
}
