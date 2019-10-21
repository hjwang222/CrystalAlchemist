using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private int attributePoints;
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
        this.attributePointsMax = Utilities.Items.getAmountFromInventory(this.item, this.player.inventory, true);
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
