using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RangeTriggered : MonoBehaviour
{
    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Spieler")]
    [SerializeField]
    private bool affectOther = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Gegner")]
    [SerializeField]
    private bool affectSame = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Gegner")]
    [SerializeField]
    private bool affectNeutral = false;

    [SerializeField]
    private AI npc;

    public bool isTriggered = false;

    private void Update()
    {
        RotationUtil.rotateCollider(this.npc, this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CollisionUtil.checkAffections(this.npc, this.affectOther, this.affectSame, this.affectNeutral, collision)) this.isTriggered = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CollisionUtil.checkAffections(this.npc, this.affectOther, this.affectSame, this.affectNeutral, collision)) this.isTriggered = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CollisionUtil.checkAffections(this.npc, this.affectOther, this.affectSame, this.affectNeutral, collision)) this.isTriggered = false;
    }
}
