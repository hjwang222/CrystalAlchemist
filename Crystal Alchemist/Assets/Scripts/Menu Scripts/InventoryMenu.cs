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


    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject regularItems;
    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject keyItems;

    private CharacterState lastState;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
        setSkillsToSlots(false);
        setSkillsToSlots(true);
        showCategory(1);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            exitMenu();
        }
        else if (Input.GetButtonDown("Inventory")) exitMenu();
    }

    private void OnEnable()
    {
        this.lastState = this.player.currentState;
        this.cursor.gameObject.SetActive(true);
        this.player.currentState = CharacterState.inMenu;
    }

    private void OnDisable()
    {
        this.cursor.gameObject.SetActive(false);
    }


    public void exitMenu()
    {
        this.player.delay(this.lastState);
        this.transform.parent.gameObject.SetActive(false);
        this.blackScreen.SetActive(false);
    }

    public void showCategory(int category)
    {
        this.regularItems.SetActive(false);
        this.keyItemsLabel.gameObject.SetActive(false);
        this.keyItems.SetActive(false);
        this.keyItemsLabel.gameObject.SetActive(false);

        switch (category)
        {
            case 1: this.keyItems.SetActive(true); this.keyItemsLabel.gameObject.SetActive(true); break;
            default: this.regularItems.SetActive(true); this.regularItemsLabel.gameObject.SetActive(true); break;
        }

    }

    private void setSkillsToSlots(bool showKeyItems)
    {
        GameObject categoryGameobject = this.regularItems;
        if (showKeyItems) categoryGameobject = this.keyItems;

        for (int i = 0; i < categoryGameobject.transform.childCount; i++)
        {
            GameObject slot = categoryGameobject.transform.GetChild(i).gameObject;
            Item item = Utilities.Items.getItemByID(this.player.inventory, slot.GetComponent<InventorySlot>().ID, showKeyItems);
            slot.GetComponent<InventorySlot>().setItemToSlot(item);
        }
    }
}
