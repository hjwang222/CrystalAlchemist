using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
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
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();

        showCategory(0);
    }

    private void Start()
    {
        if(this.player == null) this.player = GameObject.FindWithTag("Player").GetComponent<Player>();        
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
        setSkillsToSlots(this.regularItems, false);
        setSkillsToSlots(this.keyItems, true);
        setSkillsToSlots(this.quickmenu, true);

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

    public void openSkillMenu()
    {
        this.transform.parent.gameObject.SetActive(false);
        this.blackScreen.SetActive(false);
        this.player.currentState = this.lastState;
        this.skillMenuSignal.Raise();
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

    private void setSkillsToSlots(GameObject categoryGameobject, bool showKeyItems)
    {
        for (int i = 0; i < categoryGameobject.transform.childCount; i++)
        {
            GameObject slot = categoryGameobject.transform.GetChild(i).gameObject;
            InventorySlot iSlot = slot.GetComponent<InventorySlot>();
            Item item = null;  

            //if (iSlot.getFeature() != ItemFeature.none) item = Utilities.Items.getItemByFeature(this.player.inventory, iSlot.getFeature());
            //else 
            item = Utilities.Items.getItemByID(this.player.inventory, iSlot.getID(), showKeyItems);

            slot.GetComponent<InventorySlot>().setItemToSlot(item);
        }
    }
}
