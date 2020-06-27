using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class MiniMap : MenuBehaviour
{
    [BoxGroup]
    [SerializeField]
    private TeleportStats lastTeleport;

    [BoxGroup]
    [SerializeField]
    private TextMeshProUGUI teleportName;

    [BoxGroup]
    [SerializeField]
    private Image icon;

    public override void Start()
    {
        base.Start();

        if (this.lastTeleport == null) this.teleportName.text = "-";
        this.teleportName.text = this.lastTeleport.GetTeleportName();
        this.icon.sprite = this.lastTeleport.icon;
    }
}
