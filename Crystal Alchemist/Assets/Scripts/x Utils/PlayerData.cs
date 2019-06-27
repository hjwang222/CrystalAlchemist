using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public float health;
    public float mana;
    public List<string[]> inventory = new List<string[]>();
    public List<GameObject> temp = new List<GameObject>();

    public float[] position;
    public string scene;

    public PlayerData(Player player)
    {
        this.health = player.life;
        this.mana = player.mana;

        this.position = new float[3];

        this.position[0] = player.transform.position.x;
        this.position[1] = player.transform.position.y;
        this.position[2] = player.transform.position.z;

        this.inventory.Clear();

        foreach(Item item in player.inventory)
        {
            string[] temp = new string[2];
            temp[0] = item.gameObject.name.Split('(')[0];
            temp[1] = item.amount + "";
            this.temp.Add(item.gameObject);
            this.inventory.Add(temp);
        }

        this.scene = player.getScene();
    }
}

[System.Serializable]
public class GameOptions
{
    public float musicVolume;
    public float soundVolume;

    public GameOptions()
    {
        this.musicVolume = GlobalValues.backgroundMusicVolume;
        this.soundVolume = GlobalValues.soundEffectVolume;
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
