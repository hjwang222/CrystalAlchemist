using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player/Teleport List")]
public class PlayerTeleportList : ScriptableObject
{
    [SerializeField]
    private List<TeleportStats> list = new List<TeleportStats>();

    public void AddTeleport(string targetScene, Vector2 position)
    {
        TeleportStats stat = new TeleportStats(targetScene, position);
        if (!Contains(stat)) this.list.Add(stat);
    }

    public bool Contains(TeleportStats stat)
    {
        this.list.RemoveAll(item => item == null);

        for (int i = 0; i < this.list.Count; i++)
        {
            if (list[i].scene == stat.scene 
             && list[i].teleportName == stat.teleportName) return true;
        }

        return false;
    }

    public bool TeleportEnabled()
    {
        this.list.RemoveAll(item => item == null);
        return this.list.Count > 0;
    }

    public List<TeleportStats> GetStats()
    {
        return this.list;
    }
}
