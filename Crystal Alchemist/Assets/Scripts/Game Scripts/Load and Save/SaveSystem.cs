using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveSystem
{
    public static void Save(PlayerSaveGame saveGame, string slot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + slot + "." + MasterManager.staticValues.saveGameFiletype;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(saveGame);

        formatter.Serialize(stream, data);
        stream.Close();
        stream.Dispose();
    }

    public static void SaveOptions()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/options."+MasterManager.staticValues.saveGameFiletype;
        FileStream stream = new FileStream(path, FileMode.Create);

        GameOptions data = new GameOptions();

        formatter.Serialize(stream, data);
        stream.Close();
        stream.Dispose();
    }

    public static PlayerData loadPlayer(string name)
    {
        if (name != null)
        {
            string path = Application.persistentDataPath + "/" + name + "." + MasterManager.staticValues.saveGameFiletype;

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                if (stream.Length <= 0) return null;

                PlayerData data = formatter.Deserialize(stream) as PlayerData;

                stream.Close();
                stream.Dispose();

                return data;
            }
        }

        return null;
    }

    public static void loadOptions()
    {
        string path = Application.persistentDataPath + "/options." + MasterManager.staticValues.saveGameFiletype;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameOptions data = formatter.Deserialize(stream) as GameOptions;

            stream.Close();
            stream.Dispose();

            MasterManager.settings.backgroundMusicVolume = data.musicVolume;
            MasterManager.settings.soundEffectVolume = data.soundVolume;
            MasterManager.settings.useAlternativeLanguage = data.useAlternativeLanguage;

            MasterManager.settings.healthBar = data.useHealthBar;
            MasterManager.settings.manaBar = data.useManaBar;

            if (data.layout == "keyboard") MasterManager.settings.layoutType = LayoutType.keyboard;
            else MasterManager.settings.layoutType = LayoutType.gamepad;
        }
    }
}


