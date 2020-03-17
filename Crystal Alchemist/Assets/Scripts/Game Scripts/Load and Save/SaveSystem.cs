using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveSystem
{
    public static void Save(Player player, string scene, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + name + "." + GlobalValues.saveGameFiletype;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player, scene);

        formatter.Serialize(stream, data);
        stream.Close();
        stream.Dispose();
    }

    public static void SaveOptions()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/options."+GlobalValues.saveGameFiletype;
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
            string path = Application.persistentDataPath + "/" + name + "." + GlobalValues.saveGameFiletype;

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
        string path = Application.persistentDataPath + "/options." + GlobalValues.saveGameFiletype;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameOptions data = formatter.Deserialize(stream) as GameOptions;

            stream.Close();
            stream.Dispose();

            GlobalValues.backgroundMusicVolume = data.musicVolume;
            GlobalValues.soundEffectVolume = data.soundVolume;
            GlobalValues.useAlternativeLanguage = data.useAlternativeLanguage;

            GlobalValues.healthBar = data.useHealthBar;
            GlobalValues.manaBar = data.useManaBar;

            if (data.layout == "keyboard") GlobalValues.layoutType = LayoutType.keyboard;
            else GlobalValues.layoutType = LayoutType.gamepad;
        }
    }
}


