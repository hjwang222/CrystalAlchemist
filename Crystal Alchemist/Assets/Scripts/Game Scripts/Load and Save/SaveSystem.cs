using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveSystem
{
    public static void Save(Player player, string scene, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + name + "." + GlobalGameObjects.staticValues.saveGameFiletype;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player, scene);

        formatter.Serialize(stream, data);
        stream.Close();
        stream.Dispose();
    }

    public static void SaveOptions()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/options."+GlobalGameObjects.staticValues.saveGameFiletype;
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
            string path = Application.persistentDataPath + "/" + name + "." + GlobalGameObjects.staticValues.saveGameFiletype;

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
        string path = Application.persistentDataPath + "/options." + GlobalGameObjects.staticValues.saveGameFiletype;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameOptions data = formatter.Deserialize(stream) as GameOptions;

            stream.Close();
            stream.Dispose();

            GlobalGameObjects.settings.backgroundMusicVolume = data.musicVolume;
            GlobalGameObjects.settings.soundEffectVolume = data.soundVolume;
            GlobalGameObjects.settings.useAlternativeLanguage = data.useAlternativeLanguage;

            GlobalGameObjects.settings.healthBar = data.useHealthBar;
            GlobalGameObjects.settings.manaBar = data.useManaBar;

            if (data.layout == "keyboard") GlobalGameObjects.settings.layoutType = LayoutType.keyboard;
            else GlobalGameObjects.settings.layoutType = LayoutType.gamepad;
        }
    }
}


