using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float mana;

    public int coins;
    public int keys;
    public int crystals;

    public int wood;
    public int stone;
    public int metal;

    public float[] position;
    public string scene;

    public PlayerData(Player player)
    {
        this.health = player.life;
        this.mana = player.mana;

        this.coins = player.coins;
        this.keys = player.keys;
        this.crystals = player.crystals;

        this.wood = player.wood;
        this.stone = player.stone;
        this.metal = player.metal;

        this.position = new float[3];

        this.position[0] = player.transform.position.x;
        this.position[1] = player.transform.position.y;
        this.position[2] = player.transform.position.z;

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
