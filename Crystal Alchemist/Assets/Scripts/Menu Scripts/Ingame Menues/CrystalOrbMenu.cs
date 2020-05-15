using Sirenix.OdinInspector;
using UnityEngine;

public class CrystalOrbMenu : MenuManager
{
    [BoxGroup("Crystal Orb Menu")]
    [Required]
    [SerializeField]
    private TeleportStats lastTeleport;

    [BoxGroup("Crystal Orb Menu")]
    [Required]
    [SerializeField]
    private TeleportStats savePointInfo;

    public void SetLastTeleport()
    {
        this.lastTeleport.SetValue(this.savePointInfo);
    }

}
