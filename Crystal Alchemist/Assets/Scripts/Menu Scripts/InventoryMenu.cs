using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField]
    [BoxGroup("Mandatory")]
    private PlayerStats playerStats;

    private Player player;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private myCursor cursor;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI regularItemsLabel;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI keyItemsLabel;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private GameObject blackScreen;


    [BoxGroup("Signals")]
    [SerializeField]
    private SimpleSignal skillMenuSignal;

    [BoxGroup("Signals")]
    [SerializeField]
    private FloatSignal musicVolumeSignal;


    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject regularItems;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject keyItems;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject quickmenu;

    private CharacterState lastState;

    private void Awake()
    {
        this.player = this.playerStats.player;
    }

    private void Start()
    {
        this.player = this.playerStats.player;
        loadInventory();
        showCategory(0);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (this.cursor.infoBox.gameObject.activeInHierarchy) this.cursor.infoBox.Hide();
            else exitMenu();
        }
        else if (Input.GetButtonDown("Inventory")) exitMenu();
    }

    private void OnEnable()
    {
        loadInventory();

        this.lastState = this.player.currentState;
        this.cursor.gameObject.SetActive(true);
        this.player.currentState = CharacterState.inMenu;

        this.musicVolumeSignal.Raise(GlobalValues.getMusicInMenu());
    }

    private void OnDisable()
    {
        this.cursor.gameObject.SetActive(false);

        this.musicVolumeSignal.Raise(GlobalValues.backgroundMusicVolume);
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
        this.blackScreen.SetActive(false);
        this.player.currentState = this.lastState;
        item.keyItemSignal.Raise();
    }

    public void openMap()
    {
        this.transform.parent.gameObject.SetActive(false);
        this.blackScreen.SetActive(false);
        this.player.currentState = this.lastState;
        //raise Map Signal
    }

    public void exitMenu()
    {
        this.cursor.infoBox.Hide();
        this.player.delay(this.lastState);
        this.transform.parent.gameObject.SetActive(false);
        this.blackScreen.SetActive(false);
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
                item = Utilities.Items.getItemByID(this.player.inventory, ID, showKeyItems);

                slot.GetComponent<InventorySlot>().setItemToSlot(item);
            }
        }
    }
}
