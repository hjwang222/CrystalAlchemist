using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CrystalOrbMenu : MenuBehaviour
{
    [BoxGroup("Crystal Orb Menu")]
    [Required]
    [SerializeField]
    private TeleportStats lastTeleport;

    [BoxGroup("Crystal Orb Menu")]
    [Required]
    [SerializeField]
    private TeleportStats savePointInfo;

    [BoxGroup("Crystal Orb Menu")]
    [Required]
    [SerializeField]
    private Image star;
    

    public override void Start()
    {
        base.Start();
        SetStar();
    }

    public void SetStar()
    {
        if (lastTeleport.teleportName == savePointInfo.teleportName) star.gameObject.SetActive(true);
        else star.gameObject.SetActive(false);
    }

    public void SetLastTeleport()
    {
        this.lastTeleport.SetValue(this.savePointInfo);
    }
}
