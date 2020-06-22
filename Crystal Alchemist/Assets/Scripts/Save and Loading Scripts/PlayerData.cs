using System.Collections.Generic;

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
    public List<object[]> teleportPoints = new List<object[]>();
    public List<string[]> abilities = new List<string[]>();

    public string characterName;
    public string race;
    public List<string[]> colorGroups = new List<string[]>();
    public List<string[]> characterParts = new List<string[]>();

    public float[] startPosition;
    public string startScene;
    public string startName;

    public float[] lastPosition;
    public string lastScene;
    public string lastName;

    public float timePlayed;

    public PlayerData(PlayerSaveGame saveGame)
    {
        this.health = saveGame.playerValue.life;
        this.mana = saveGame.playerValue.mana;

        this.maxHealth = saveGame.playerValue.maxLife;
        this.maxMana = saveGame.playerValue.maxMana;
        this.healthRegen = saveGame.playerValue.lifeRegen;
        this.manaRegen = saveGame.playerValue.manaRegen;
        this.buffplus = saveGame.playerValue.buffPlus;
        this.debuffminus = saveGame.playerValue.debuffMinus;

        setInventory(saveGame.inventory);
        setPreset(saveGame.playerPreset);

        this.abilities = saveGame.buttons.saveButtonConfig();
        this.timePlayed = saveGame.timePlayed.GetValue();
        this.characterName = saveGame.GetCharacterName();

        SetStartTeleport(saveGame.startSpawnPoint);
        SetLastTeleport(saveGame.lastTeleport);
        SetTeleportList(saveGame.teleportList);
    }

    private void setInventory(PlayerInventory inventory)
    {
        this.keyItems.Clear();
        this.inventoryItems.Clear();

        foreach (ItemGroup item in inventory.inventoryItems)
        {
            string[] temp = new string[2];
            temp[0] = item.name;
            temp[1] = item.GetAmount() + "";
            this.inventoryItems.Add(temp);
        }

        foreach (ItemStats item in inventory.keyItems)
        {
            string temp = item.name;
            this.keyItems.Add(temp);
        }
    }

    private void setPreset(CharacterPreset preset)
    {     
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

    private void SetTeleportList(PlayerTeleportList list)
    {
        this.teleportPoints.Clear();

        foreach(TeleportStats stat in list.GetStats())
        {
            object[] teleportLine = new object[4];
            teleportLine[0] = stat.teleportName;
            teleportLine[1] = stat.scene;
            teleportLine[2] = stat.position.x;
            teleportLine[3] = stat.position.y;

            this.teleportPoints.Add(teleportLine);
        }
    }

    private void SetStartTeleport(TeleportStats stats)
    {
        this.startScene = stats.scene;
        this.startName = stats.teleportName;
        this.startPosition = new float[] { stats.position.x, stats.position.y };
    }

    private void SetLastTeleport(TeleportStats stats)
    {
        this.lastScene = stats.scene;
        this.lastName = stats.teleportName;
        this.lastPosition = new float[] { stats.position.x, stats.position.y };
    }
}

[System.Serializable]
public class GameOptions
{
    public float musicVolume;
    public float soundVolume;

    public string layout;
    public string language;

    public bool useHealthBar;
    public bool useManaBar;

    public GameOptions()
    {
        this.musicVolume = MasterManager.settings.backgroundMusicVolume;
        this.soundVolume = MasterManager.settings.soundEffectVolume;

        this.layout = MasterManager.settings.layoutType.ToString().ToLower();
        this.language = MasterManager.settings.language.ToString();

        this.useHealthBar = MasterManager.settings.healthBar;
        this.useManaBar = MasterManager.settings.manaBar;
    }
}