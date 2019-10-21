using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Sirenix.OdinInspector;

public enum attributeType
{
    lifeExpander,
    manaExpander,
    lifeRegen,
    manaRegen,
    buffPlus,
    debuffMinus
}

public class CharacterAttributeStats : MonoBehaviour
{
    [SerializeField]
    private attributeType type;

    public Image icon;

    public TextMeshProUGUI attributeNameField;

    public string description;
    public string descriptionEnglish;

    [SerializeField]
    private GameObject buttonReduce;

    [SerializeField]
    private GameObject buttonIncrease;

    [SerializeField]
    private CharacterAttributeMenu mainMenu;

    [SerializeField]
    private List<GameObject> pointsList = new List<GameObject>();

    private int points;

    private void Start()
    {
        getPoints();
        updateUI();
        updateButton();
    }

    public int getPointsSpent()
    {
        return this.points;
    }

    public void setAttributes(int value)
    {
        this.points += value;
        if (this.points < 0) this.points = 0;
        else if (this.points > 4) this.points = 4;

        updateAttributes(this.points);
        this.mainMenu.updateAllStats(value);
        updateUI();
    }

    public void updateButton()
    {
        if(this.mainMenu.getAvailablePoints() <= 0 || this.points >= 4) this.buttonIncrease.SetActive(false);
        else this.buttonIncrease.SetActive(true);

        if (this.points <= 0) this.buttonReduce.SetActive(false);
        else this.buttonReduce.SetActive(true);
    }

    private void updateAttributes(int value)
    {
        switch (this.type)
        {
            case attributeType.lifeExpander:
                this.mainMenu.player.maxLife = this.mainMenu.expanderValues[value];
                this.mainMenu.player.callSignal(this.mainMenu.player.healthSignalUI, 1);
                break;
            case attributeType.manaExpander:
                this.mainMenu.player.maxMana = this.mainMenu.expanderValues[value];
                this.mainMenu.player.callSignal(this.mainMenu.player.manaSignalUI, 1);
                break;
            case attributeType.lifeRegen:
                this.mainMenu.player.lifeRegen = 100 / this.mainMenu.percentageValues[value];
                break;
            case attributeType.manaRegen:
                this.mainMenu.player.manaRegen = 100 / this.mainMenu.percentageValues[value];
                break;
            case attributeType.buffPlus:
                this.mainMenu.player.buffPlus = this.mainMenu.percentageValues[value];
                break;
            case attributeType.debuffMinus:
                this.mainMenu.player.debuffMinus = -this.mainMenu.percentageValues[value];
                break;
        }
    }

    private void getPoints()
    {
        float value = 0;
        int result = 0;

        switch (this.type)
        {
            case attributeType.lifeExpander: value = this.mainMenu.player.maxLife; break;
            case attributeType.manaExpander: value = this.mainMenu.player.maxMana; break;
            case attributeType.lifeRegen: value = this.mainMenu.player.lifeRegen; break;
            case attributeType.manaRegen: value = this.mainMenu.player.manaRegen; break;
            case attributeType.buffPlus: value = this.mainMenu.player.buffPlus; break;
            case attributeType.debuffMinus: value = this.mainMenu.player.debuffMinus; break;
        }

        if (this.type == attributeType.lifeExpander || this.type == attributeType.manaExpander) result = indexOf(this.mainMenu.expanderValues, value);
        else result = indexOf(this.mainMenu.percentageValues, value);

        this.points = result;
    }

    private void updateUI()
    {
        for(int i = 0; i< this.pointsList.Count; i++)
        {
            this.pointsList[i].SetActive(false);
            if(i <= this.points) this.pointsList[i].SetActive(true);
        }
    }

    private int indexOf(int[] array, float value)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if (array[i] >= (int)value) return i;
        }

        return 0;
    }
}
