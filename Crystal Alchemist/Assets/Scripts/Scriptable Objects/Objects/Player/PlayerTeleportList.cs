using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player/Teleport List")]
public class PlayerTeleportList : ScriptableObject
{
    [SerializeField]
    private List<TeleportStats> list = new List<TeleportStats>();

    public TeleportStats nextTeleport;
    public TeleportStats lastTeleport;

    public void SetNextTeleport(TeleportStats stats)
    {
        this.nextTeleport = stats;
    }

    public void SetLastTeleport(TeleportStats stats)
    {
        this.lastTeleport = stats;
    }

    public void SetReturnTeleport()
    {
        if (this.lastTeleport == null) return;
        SetNextTeleport(this.lastTeleport);
    }

    public void Initialize()
    {
        this.list.RemoveAll(item => item == null);
        this.list.OrderBy(o => o.scene);
    }

    public void AddTeleport(TeleportStats stat)
    {
        if (!Contains(stat)) this.list.Add(stat);
        this.list.OrderBy(o => o.scene);
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

    public List<TeleportStats> GetStats()
    {
        return this.list;
    }

    public TeleportStats GetStats(int index)
    {        
        return this.list[index];
    }
}
