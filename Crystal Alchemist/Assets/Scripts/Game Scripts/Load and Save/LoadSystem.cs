using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadSystem : MonoBehaviour
{
    public static void loadPlayerData(Player player, string saveGameSlot)
    {
        PlayerData data = SaveSystem.loadPlayer(saveGameSlot);

        if (data != null)
        {
            player.life = data.health;
            player.mana = data.mana;

            player.maxLife = data.maxHealth;
            player.maxMana = data.maxMana;
            player.lifeRegen = data.healthRegen;
            player.manaRegen = data.manaRegen;
            player.buffPlus = data.buffplus;
            player.debuffMinus = data.debuffminus;

            player.healthSignalUI.Raise();
            player.manaSignalUI.Raise();

            player.stats.characterName = data.characterName;
            player.GetComponent<PlayerUtils>().secondsPlayed = data.timePlayed;

            player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);

            player.GetComponent<PlayerTeleport>().setLastTeleport(data.scene, new Vector3(data.position[0], data.position[1], data.position[2]));

            if (data.inventory.Count > 0)
            {
                loadInventory(data, player);                
            }            
        }
    }

    public static void loadPlayerSkills(Player player, string saveGameSlot)
    {
        PlayerData data = SaveSystem.loadPlayer(saveGameSlot);

        if (data != null && data.skills.Count > 0)
        {
            loadSkills(data, player);
        }
    }    

    public static void loadPreset(Player player, string saveGameSlot)
    {
        PlayerData data = SaveSystem.loadPlayer(saveGameSlot);

        if (data != null && data.characterParts.Count > 0)
        {
            loadPresetData(data, player);
        }
    }

    private static void loadPresetData(PlayerData data, Player player)
    {
        player.stats.characterName = data.characterName;

        CharacterPreset preset = player.preset;
        preset.characterName = data.characterName;

        if (Enum.TryParse(data.race, out Race race)) preset.setRace(race);
        
        foreach(string[] colorGroup in data.colorGroups)
        {
            string colorGroupName = colorGroup[0];
            float r = float.Parse(colorGroup[1]);
            float g = float.Parse(colorGroup[2]);
            float b = float.Parse(colorGroup[3]);
            float a = float.Parse(colorGroup[4]);

            Color color = new Color(r,g,b,a);
            if (Enum.TryParse(colorGroup[0], out ColorGroup group)) preset.AddColorGroup(group, color);
        }

        foreach(string[] characterPart in data.characterParts)
        {
            string parentName = characterPart[0];
            string name = characterPart[1];

            preset.AddCharacterPartData(parentName, name);
        }
    }







    private static void loadInventory(PlayerData data, Player player)
    {
        player.inventory.Clear();

        foreach (string[] elem in data.inventory)
        {
            GameObject prefab = Resources.Load("Prefabs/Items/" + elem[0], typeof(GameObject)) as GameObject;

            if(prefab == null) prefab = Resources.Load("Prefabs/Items/Key Items/" + elem[0], typeof(GameObject)) as GameObject;
            if (prefab == null) prefab = Resources.Load("Prefabs/Items/Attribute Points/" + elem[0], typeof(GameObject)) as GameObject;

            if (prefab != null)
            {
                GameObject instance = Instantiate(prefab);
                instance.name = prefab.name;
                Item item = instance.GetComponent<Item>();
                item.amount = Convert.ToInt32(elem[1]);
                player.collect(item, true, false);
            }
        }
    }

    private static void loadSkills(PlayerData data, Player player)
    {
        foreach (string[] elem in data.skills)
        {
            Skill skill = CustomUtilities.Skills.getSkillByName(player.skillSet, elem[1]);
            string button = elem[0];

            switch (button)
            {
                case "A": player.AButton = skill; break;
                case "B": player.BButton = skill; break;
                case "X": player.XButton = skill; break;
                case "Y": player.YButton = skill; break;
                case "RB": player.RBButton = skill; break;
            }
        }
    }
}
