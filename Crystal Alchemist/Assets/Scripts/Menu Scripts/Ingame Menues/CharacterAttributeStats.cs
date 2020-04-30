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

    public string attributeName;
    public string nameEnglish;

    public string description;
    public string descriptionEnglish;

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

    public string getDescription()
    {
        string text = FormatUtil.getLanguageDialogText(this.description, this.descriptionEnglish);
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
                this.mainMenu.player.values.maxLife = this.mainMenu.expanderValues[value];
                if (this.mainMenu.player.values.life > this.mainMenu.player.values.maxLife) this.mainMenu.player.values.life = this.mainMenu.player.values.maxLife;
                this.mainMenu.player.callSignal(this.mainMenu.player.healthSignalUI, 1);
                break;
            case attributeType.manaExpander:
                this.mainMenu.player.values.maxMana = this.mainMenu.expanderValues[value];
                if (this.mainMenu.player.values.mana > this.mainMenu.player.values.maxMana) this.mainMenu.player.values.mana = this.mainMenu.player.values.maxMana;
                this.mainMenu.player.callSignal(this.mainMenu.player.manaSignalUI, 1);
                break;
            case attributeType.lifeRegen:
                this.mainMenu.player.values.lifeRegen = (float)this.mainMenu.percentageValues[value] / 100f;
                break;
            case attributeType.manaRegen:
                this.mainMenu.player.values.manaRegen = (float)this.mainMenu.percentageValues[value] / 100f;
                break;
            case attributeType.buffPlus:
                this.mainMenu.player.values.buffPlus = this.mainMenu.percentageValues[value];
                break;
            case attributeType.debuffMinus:
                this.mainMenu.player.values.debuffMinus = -this.mainMenu.percentageValues[value];
                break;
        }
    }

    private void getPoints()
    {
        float value = 0;
        int result = 0;

        switch (this.type)
        {
            case attributeType.lifeExpander: value = this.mainMenu.player.values.maxLife; break;
            case attributeType.manaExpander: value = this.mainMenu.player.values.maxMana; break;
            case attributeType.lifeRegen: value = this.mainMenu.player.values.lifeRegen; break;
            case attributeType.manaRegen: value = this.mainMenu.player.values.manaRegen; break;
            case attributeType.buffPlus: value = this.mainMenu.player.values.buffPlus; break;
            case attributeType.debuffMinus: value = this.mainMenu.player.values.debuffMinus; break;
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
