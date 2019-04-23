using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float mana;
    public float[] position;
    public string scene;

    public PlayerData(Player player)
    {
        this.health = player.life;
        this.mana = player.mana;
        this.position = new float[3];

        this.position[0] = player.transform.position.x;
        this.position[1] = player.transform.position.y;
        this.position[2] = player.transform.position.z;

        this.scene = player.getScene();
    }
}
