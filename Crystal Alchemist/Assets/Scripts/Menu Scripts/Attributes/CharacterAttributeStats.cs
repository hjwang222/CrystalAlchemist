using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private CharacterAttributeMenu mainMenu;

    [SerializeField]
    private List<GameObject> pointsList = new List<GameObject>();

    private int points;

    public void init()
    {
        getPoints();
        updateUI();
    }

    public int getPointsSpent()
    {
        return this.points;
    }

    public string GetName()
    {
        return FormatUtil.GetLocalisedText(this.gameObject.name+"_Name", LocalisationFileType.menues);
    }

    public string GetDescription()
    {
        return FormatUtil.GetLocalisedText(this.gameObject.name + "_Description", LocalisationFileType.menues);
    }

    public void setAttributes(int value)
    {
        if (getActivePoints() == value)
        {
            this.points--;
        }
        else if ((value - this.points) <= this.mainMenu.getPointsLeft())
        {
            this.points = value;
        }
        else if ((value - this.points) > this.mainMenu.getPointsLeft())
        {
            this.points += this.mainMenu.getPointsLeft();
        }

        this.mainMenu.updatePoints();
        updateUI();
        updateAttributes(this.points);
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
                this.mainMenu.playerValues.maxLife = this.mainMenu.expanderValues[value];
                if (this.mainMenu.playerValues.life > this.mainMenu.playerValues.maxLife) this.mainMenu.playerValues.life = this.mainMenu.playerValues.maxLife;
                this.mainMenu.healtSignal.Raise();
                break;
            case attributeType.manaExpander:
                this.mainMenu.playerValues.maxMana = this.mainMenu.expanderValues[value];
                if (this.mainMenu.playerValues.mana > this.mainMenu.playerValues.maxMana) this.mainMenu.playerValues.mana = this.mainMenu.playerValues.maxMana;
                this.mainMenu.manaSignal.Raise();
                break;
            case attributeType.lifeRegen:
                this.mainMenu.playerValues.lifeRegen = (float)this.mainMenu.percentageValues[value] / 100f;
                break;
            case attributeType.manaRegen:
                this.mainMenu.playerValues.manaRegen = (float)this.mainMenu.percentageValues[value] / 100f;
                break;
            case attributeType.buffPlus:
                this.mainMenu.playerValues.buffPlus = this.mainMenu.percentageValues[value];
                break;
            case attributeType.debuffMinus:
                this.mainMenu.playerValues.debuffMinus = -this.mainMenu.percentageValues[value];
                break;
        }
    }

    private void getPoints()
    {
        float value = 0;
        int result = 0;

        switch (this.type)
        {
            case attributeType.lifeExpander: value = this.mainMenu.playerValues.maxLife; break;
            case attributeType.manaExpander: value = this.mainMenu.playerValues.maxMana; break;
            case attributeType.lifeRegen: value = this.mainMenu.playerValues.lifeRegen; break;
            case attributeType.manaRegen: value = this.mainMenu.playerValues.manaRegen; break;
            case attributeType.buffPlus: value = this.mainMenu.playerValues.buffPlus; break;
            case attributeType.debuffMinus: value = this.mainMenu.playerValues.debuffMinus; break;
        }

        if (this.type == attributeType.lifeExpander || this.type == attributeType.manaExpander) result = indexOf(this.mainMenu.expanderValues, value);
        else result = indexOf(this.mainMenu.percentageValues, (value*100));

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
