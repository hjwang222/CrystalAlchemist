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

    public string name;
    public string nameEnglish;

    public string description;
    public string descriptionEnglish;

    [SerializeField]
    private CharacterAttributeMenu mainMenu;

    [SerializeField]
    private List<GameObject> pointsList = new List<GameObject>();

    private int points;

    private void Start()
    {
        getPoints();
        updateUI();
    }

    public int getPointsSpent()
    {
        return this.points;
    }

    public void setAttributes(int value)
    {
        this.points = value;

        if (getActivePoints() == value) this.points--;

        updateAttributes(this.points);
        this.mainMenu.updatePoints();
        updateUI();
    }

    public string getDescription()
    {
        string text = Utilities.Format.getLanguageDialogText(this.description, this.descriptionEnglish);
        text += "\n Aktuell " + getValue(this.points) + " von " + getValue(4);
        return text;
    }

    public string getValue(int index)
    {
        if (this.type == attributeType.lifeExpander || this.type == attributeType.manaExpander)
        {
            return this.mainMenu.expanderValues[index] + "";
        }
        else
        {
            return this.mainMenu.percentageValues[index] + "%";
        }
    }


    private void updateAttributes(int value)
    {
        switch (this.type)
        {
            case attributeType.lifeExpander:
                this.mainMenu.player.maxLife = this.mainMenu.expanderValues[value];
                if (this.mainMenu.player.life > this.mainMenu.player.maxLife) this.mainMenu.player.life = this.mainMenu.player.maxLife;
                this.mainMenu.player.callSignal(this.mainMenu.player.healthSignalUI, 1);
                break;
            case attributeType.manaExpander:
                this.mainMenu.player.maxMana = this.mainMenu.expanderValues[value];
                if (this.mainMenu.player.mana > this.mainMenu.player.maxMana) this.mainMenu.player.mana = this.mainMenu.player.maxMana;
                this.mainMenu.player.callSignal(this.mainMenu.player.manaSignalUI, 1);
                break;
            case attributeType.lifeRegen:
                this.mainMenu.player.lifeRegen = (float)this.mainMenu.percentageValues[value] / 100f;
                break;
            case attributeType.manaRegen:
                this.mainMenu.player.manaRegen = (float)this.mainMenu.percentageValues[value] / 100f;
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
            if(i+1 <= this.points) this.pointsList[i].SetActive(true);
        }
    }

    private int getActivePoints()
    {
        int result = 0;
        for (int i = 0; i < this.pointsList.Count; i++)
        {
            if (this.pointsList[i].activeInHierarchy) result = i+1;
        }
        return result;
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
