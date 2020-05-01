using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadSystem : MonoBehaviour
{
    public static void loadPlayerData(Player player, string saveGameSlot)
    {
        PlayerData data = SaveSystem.loadPlayer(saveGameSlot);
        PlayerAbilities playerAbilities = player.GetComponent<PlayerAbilities>();
        PlayerTeleport playerTeleport = player.GetComponent<PlayerTeleport>();

        playerAbilities.buttons.ClearAbilities();

        if (data != null)
        {
            loadPreset(data, player);
            player.presetSignal.Raise();

            player.values.life = data.health;
            player.values.mana = data.mana;

            player.values.maxLife = data.maxHealth;
            player.values.maxMana = data.maxMana;
            player.values.lifeRegen = data.healthRegen;
            player.values.manaRegen = data.manaRegen;
            player.values.buffPlus = data.buffplus;
            player.values.debuffMinus = data.debuffminus;

            player.healthSignalUI.Raise();
            player.manaSignalUI.Raise();

            player.stats.SetCharacterName(data.characterName);
            //player.secondsPlayed.setValue(data.timePlayed);            

            loadInventory(data, player);
            loadPlayerSkills(data, playerAbilities);

            //playerTeleport.setLastTeleport(data.scene, new Vector3(data.position[0], data.position[1], data.position[2]), true);
            //playerTeleport.teleportPlayerToLastSavepoint(true); //letzter Savepoint, no Scene Loading            
        }
        else
        {
            //playerTeleport.setLastTeleport("", Vector2.zero, false);
            //playerTeleport.teleportPlayerToScene(true); //Starpunkt, no Scene Loading
        }
    }

    public static void loadPlayerSkills(PlayerData data, PlayerAbilities playerAbilities)
    {
        playerAbilities.skillSet.Start();

        if (data != null && data.abilities.Count > 0)
        {
            foreach(string[] elem in data.abilities)
            {
                string name = elem[1];
                string button = elem[0];

                Ability ability = playerAbilities.skillSet.getAbilityByName(name);
                playerAbilities.buttons.SetAbilityToButton(button, ability);
            }
        }
    }

    private static void loadPreset(PlayerData data, Player player)
    { 
        if (data != null && data.characterParts != null && data.characterParts.Count > 0)
        {
            loadPresetData(data, player.preset);
        }
        else
        {
            GameUtil.setPreset(player.defaultPreset, player.preset);
        }
    }

    private static void loadPresetData(PlayerData data, CharacterPreset newPreset)
    {
        //player.stats.SetCharacterName(data.characterName);

        CharacterPreset preset = newPreset;
        preset.characterName = data.characterName;

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


    private static void loadInventory(PlayerData data, Player player)
    {
        if (data.keyItems != null)
        {
            foreach (string keyItem in data.keyItems)
            {
                ItemDrop drop = MasterManager.getItemDrop(keyItem);
                if (drop != null) drop.stats.CollectIt(player);                
            }
        }

        if (data.inventoryItems != null)
        {
            foreach (string[] item in data.inventoryItems)
            {
                ItemGroup group = MasterManager.getItemGroup(item[0]);
                if (group != null) player.GetComponent<PlayerItems>().CollectInventoryItem(group, Convert.ToInt32(item[1]));
            }
        }
    }    
}
