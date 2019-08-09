using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public float health;
    public float mana;
    public List<string[]> inventory = new List<string[]>();
    public List<string[]> skills = new List<string[]>();

    public float[] position;
    public string scene;

    public PlayerData(Player player, string scene)
    {
        this.health = player.life;
        this.mana = player.mana;

        this.position = new float[3];

        this.position[0] = player.transform.position.x;
        this.position[1] = player.transform.position.y;
        this.position[2] = player.transform.position.z;

        setInventory(player);
        setSkills(player);

        this.scene = scene;
    }

    private void setInventory(Player player)
    {
        this.inventory.Clear();

        foreach (Item item in player.inventory)
        {
            string[] temp = new string[2];
            temp[0] = item.gameObject.name.Replace(" (", "(").Split('(')[0];
            temp[1] = item.amount + "";
            this.inventory.Add(temp);
        }
    }

    private void setSkills(Player player)
    {
        this.skills.Clear();

        if (player.AButton != null) this.skills.Add(new string[] { "A", player.AButton.skillName });
        if (player.BButton != null) this.skills.Add(new string[] { "B", player.BButton.skillName });
        if (player.XButton != null) this.skills.Add(new string[] { "X", player.XButton.skillName });
        if (player.YButton != null) this.skills.Add(new string[] { "Y", player.YButton.skillName });
        if (player.RBButton != null) this.skills.Add(new string[] { "RB", player.RBButton.skillName });
    }
}

[System.Serializable]
public class GameOptions
{
    public float musicVolume;
    public float soundVolume;
    public string layout;
    public bool useAlternativeLanguage;

    public GameOptions()
    {
        this.musicVolume = GlobalValues.backgroundMusicVolume;
        this.soundVolume = GlobalValues.soundEffectVolume;
        this.layout = GlobalValues.layoutType.ToString().ToLower();
        this.useAlternativeLanguage = GlobalValues.useAlternativeLanguage;
    }
}


[System.Serializable]
public class InventoryData
{
    public List<Item> items;

    public InventoryData(List<Item> items)
    {
        this.items = items;
    }
}
