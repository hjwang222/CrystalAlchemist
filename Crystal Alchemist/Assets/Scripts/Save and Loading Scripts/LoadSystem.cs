using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadSystem
{
    public static void loadPlayerData(PlayerSaveGame saveGame, string slot)
    {
        PlayerData data = SaveSystem.loadPlayer(slot);

        if (data != null)
        {
            LoadPreset(data, saveGame.playerPreset);
            LoadBasicValues(data, saveGame.playerValue);

            saveGame.time.Clear();
            saveGame.timePlayed.SetValue(data.timePlayed);
            saveGame.SetCharacterName(data.characterName);

            loadInventory(data, saveGame.inventory);
            loadPlayerSkills(data, saveGame.buttons, saveGame.skillSet);
            LoadProgress(data, saveGame.progress);

            saveGame.startSpawnPoint.SetValue(data.startScene, data.startPosition);
            saveGame.lastTeleport.SetValue(data.lastScene, data.lastPosition);

            LoadTeleportList(data, saveGame.teleportList);
        }
    }

    private static void LoadProgress(PlayerData data, PlayerGameProgress progress)
    {
        progress.Clear();
        progress.Set(data.progress);
    }

    private static void LoadTeleportList(PlayerData data, PlayerTeleportList list)
    {
        if (data.teleportPoints == null) return;
        foreach(object[] teleportData in data.teleportPoints)
        {
            TeleportStats stats = new TeleportStats((string)teleportData[0], (string)teleportData[1], 
                                         new Vector2((float)teleportData[2], (float)teleportData[3]));

            list.AddTeleport(stats);
        }
    }

    private static void LoadBasicValues(PlayerData data, CharacterValues playerValue)
    {
        playerValue.life = data.health;
        playerValue.mana = data.mana;

        playerValue.maxLife = data.maxHealth;
        playerValue.maxMana = data.maxMana;
        playerValue.lifeRegen = data.healthRegen;
        playerValue.manaRegen = data.manaRegen;
        playerValue.buffPlus = data.buffplus;
        playerValue.debuffMinus = data.debuffminus;
    }

    public static void loadPlayerSkills(PlayerData data, PlayerButtons buttons, PlayerSkillset skillSet)
    {
        skillSet.Initialize();

        if (data != null && data.abilities.Count > 0)
        {
            foreach(string[] elem in data.abilities)
            {
                string name = elem[1];
                string button = elem[0];

                Ability ability = skillSet.getAbilityByName(name);
                buttons.SetAbilityToButton(button, ability);
            }
        }
    }

    private static void LoadPreset(PlayerData data, CharacterPreset savedPreset)
    { 
        if (data != null && data.characterParts != null && data.characterParts.Count > 0)
        {
            loadPresetData(data, savedPreset); //set Preset
        }
    }

    private static void loadPresetData(PlayerData data, CharacterPreset newPreset)
    {
        CharacterPreset preset = newPreset;

        if (Enum.TryParse(data.race, out Race race)) preset.setRace(race);

        List<ColorGroupData> colorGroups = new List<ColorGroupData>();

        foreach(string[] colorGroup in data.colorGroups)
        {
            string colorGroupName = colorGroup[0];
            float r = float.Parse(colorGroup[1]);
            float g = float.Parse(colorGroup[2]);
            float b = float.Parse(colorGroup[3]);
            float a = float.Parse(colorGroup[4]);

            Color color = new Color(r,g,b,a);
            if (Enum.TryParse(colorGroup[0], out ColorGroup group)) colorGroups.Add(new ColorGroupData(group, color));
        }

        preset.AddColorGroupRange(colorGroups);

        List<CharacterPartData> parts = new List<CharacterPartData>();

        foreach (string[] characterPart in data.characterParts)
        {
            string parentName = characterPart[0];
            string name = characterPart[1];

            parts.Add(new CharacterPartData(parentName, name));
        }

        preset.AddCharacterPartDataRange(parts);
    }


    private static void loadInventory(PlayerData data, PlayerInventory inventory)
    {
        if (data.keyItems != null)
        {
            foreach (string keyItem in data.keyItems)
            {
                ItemDrop master = MasterManager.getItemDrop(keyItem);
                if (master != null)
                {
                    ItemDrop drop = master.Instantiate(1);
                    inventory.collectItem(drop.stats);
                    MonoBehaviour.Destroy(drop);
                }
            }
        }

        if (data.inventoryItems != null)
        {
            foreach (string[] item in data.inventoryItems)
            {
                ItemGroup master = MasterManager.getItemGroup(item[0]);
                if (master != null) inventory.collectItem(master, Convert.ToInt32(item[1]));                
            }
        }
    }    
}
