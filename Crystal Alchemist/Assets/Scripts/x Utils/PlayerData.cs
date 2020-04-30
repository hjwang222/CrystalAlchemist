﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public float health;
    public float mana;

    public float maxHealth;
    public float maxMana;
    public float healthRegen;
    public float manaRegen;
    public int buffplus;
    public int debuffminus;

    public List<string> keyItems = new List<string>();
    public List<string[]> inventoryItems = new List<string[]>();

    public List<string[]> abilities = new List<string[]>();

    public string characterName;
    public string race;
    public List<string[]> colorGroups = new List<string[]>();
    public List<string[]> characterParts = new List<string[]>();

    public float[] position;
    public string scene;
    public float timePlayed;


    public PlayerData(Player player, string scene)
    {
        this.health = player.values.life;
        this.mana = player.values.mana;

        this.maxHealth = player.values.maxLife;
        this.maxMana = player.values.maxMana;
        this.healthRegen = player.values.lifeRegen;
        this.manaRegen = player.values.manaRegen;
        this.buffplus = player.values.buffPlus;
        this.debuffminus = player.values.debuffMinus;

        this.position = new float[3];

        this.position[0] = player.transform.position.x;
        this.position[1] = player.transform.position.y;
        this.position[2] = player.transform.position.z;

        setInventory(player);

        this.abilities = player.GetComponent<PlayerAbilities>().buttons.saveButtonConfig();

        setPreset(player.preset);
        player.stats.SetCharacterName(this.characterName);

        this.scene = scene;
        this.timePlayed = player.secondsPlayed.getValue();
    }

    private void setInventory(Player player)
    {
        this.keyItems.Clear();
        this.inventoryItems.Clear();

        foreach (ItemGroup item in player.GetComponent<PlayerItems>().GetItemGroups())
        {
            string[] temp = new string[2];
            temp[0] = item.name;
            temp[1] = item.GetAmount() + "";
            this.inventoryItems.Add(temp);
        }

        foreach (ItemStats item in player.GetComponent<PlayerItems>().GetItemStats())
        {
            string temp = item.name;
            this.keyItems.Add(temp);
        }
    }

    private void setPreset(CharacterPreset preset)
    {
        this.characterName = preset.characterName;        
        this.race = preset.getRace().ToString();

        this.colorGroups.Clear();
        this.characterParts.Clear();

        foreach(ColorGroupData data in preset.GetColorGroupRange())
        {
            string[] temp = new string[5];
            temp[0] = data.colorGroup.ToString();
            temp[1] = data.color.r + "";
            temp[2] = data.color.g + "";
            temp[3] = data.color.b + "";
            temp[4] = data.color.a + "";
            this.colorGroups.Add(temp);
        }

        foreach (CharacterPartData data in preset.GetCharacterPartDataRange())
        {
            string[] temp = new string[2];
            temp[0] = data.parentName;
            temp[1] = data.name;
            this.characterParts.Add(temp);
        }
    }
}

[System.Serializable]
public class GameOptions
{
    public float musicVolume;
    public float soundVolume;
    public string layout;
    public bool useAlternativeLanguage;
    public bool useHealthBar;
    public bool useManaBar;

    public GameOptions()
    {
        this.musicVolume = MasterManager.settings.backgroundMusicVolume;
        this.soundVolume = MasterManager.settings.soundEffectVolume;
        this.layout = MasterManager.settings.layoutType.ToString().ToLower();
        this.useAlternativeLanguage = MasterManager.settings.useAlternativeLanguage;

        this.useHealthBar = MasterManager.settings.healthBar;
        this.useManaBar = MasterManager.settings.manaBar;
    }
}