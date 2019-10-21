using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class CharacterAttributeMenu : MonoBehaviour
{
    [HideInInspector]
    public int[] percentageValues = new int[] { 0, 25, 50, 75, 100 };
    [HideInInspector]
    public int[] expanderValues = new int[] { 1, 3, 5, 7, 9 };
    [HideInInspector]
    public Player player;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private Item item;

    [SerializeField]
    private TextMeshProUGUI pointsField;

    [SerializeField]
    private List<CharacterAttributeStats> statObjects = new List<CharacterAttributeStats>();


    [BoxGroup("Signals")]
    [SerializeField]
    private FloatSignal musicVolumeSignal;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private myCursor cursor;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private GameObject blackScreen;


    private int attributePoints;
    private CharacterState lastState;
    private int attributePointsMax;

    private void Start()
    {
        this.attributePoints = Utilities.Items.getAmountFromInventory(this.item, this.player.inventory, false);
        this.attributePointsMax = Utilities.Items.getAmountFromInventory(this.item, this.player.inventory, true);

        foreach (CharacterAttributeStats statObject in this.statObjects)
        {
            this.attributePoints -= statObject.getPointsSpent();
        }
    }

    private void OnEnable()
    {
        this.player = this.playerStats.player;
        this.attributePointsMax = Utilities.Items.getAmountFromInventory(this.item, this.player.inventory, true);

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

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (this.cursor.infoBox.gameObject.activeInHierarchy) this.cursor.infoBox.Hide();
            else exitMenu();
        }
        else if (Input.GetButtonDown("Inventory")) exitMenu();
    }

    public void exitMenu()
    {
        this.cursor.infoBox.Hide();
        this.player.delay(this.lastState);
        this.transform.parent.gameObject.SetActive(false);
        this.blackScreen.SetActive(false);
    }

    public int getAvailablePoints()
    {
        return this.attributePoints;
    }

    public void updateAllStats(int value)
    {
        this.attributePoints -= value;

        foreach (CharacterAttributeStats statObject in this.statObjects)
        {
            statObject.updateButton();
        }

        updateUI();
    }

    private void updateUI()
    {
        this.pointsField.text = Utilities.Format.formatString(this.attributePoints, this.attributePointsMax);
    }
}
