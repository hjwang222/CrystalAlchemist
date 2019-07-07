using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveSystem
{
    public static void Save(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
        stream.Dispose();
    }

    public static void DeleteSave()
    {
        string path = Application.persistentDataPath + "/player.fun";
        File.Delete(path);
    }

    public static void SaveOptions()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/options.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameOptions data = new GameOptions();

        formatter.Serialize(stream, data);
        stream.Close();
        stream.Dispose();
    }

    public static PlayerData loadPlayer()
    {
        string path = Application.persistentDataPath + "/player.fun";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();
            stream.Dispose();

            return data;
        }
        else
        {
            return null;
        }
    }

    public static void loadOptions()
    {
        string path = Application.persistentDataPath + "/options.fun";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameOptions data = formatter.Deserialize(stream) as GameOptions;

            stream.Close();
            stream.Dispose();

            GlobalValues.backgroundMusicVolume = data.musicVolume;
            GlobalValues.soundEffectVolume = data.soundVolume;
        }
    }
}


