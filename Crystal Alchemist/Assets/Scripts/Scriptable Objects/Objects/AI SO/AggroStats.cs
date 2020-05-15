using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/AI/Aggro Stats")]
public class AggroStats : ScriptableObject
{
    [BoxGroup("Aggro Attributes")]
    public bool firstHitMaxAggro = true;

    [BoxGroup("Aggro Attributes")]
    [Range(0, 100)]
    public float aggroIncreaseFactor = 25;

    [BoxGroup("Aggro Attributes")]
    [Range(0, 100)]
    public float aggroOnHitIncreaseFactor = 25;

    [BoxGroup("Aggro Attributes")]
    [Range(-100, 0)]
    public float aggroDecreaseFactor = -25;

    [BoxGroup("Aggro Attributes")]
    [Range(0, 100)]
    public float aggroNeededToTarget = 100;

    [BoxGroup("Aggro Attributes")]
    public float targetChangeDelay = 0f;


    [BoxGroup("Aggro Object Attributes")]
    public float foundClueDuration = 1;

    [BoxGroup("Aggro Object Attributes")]
    public float activeClueDuration = 1;


    [BoxGroup("Wirkungsbereich")]
    [Tooltip("wirkt auf alle Spieler")]
    public bool affectOther = false;

    [BoxGroup("Wirkungsbereich")]
    [Tooltip("wirkt auf alle Gegner")]
    public bool affectSame = false;

    [BoxGroup("Wirkungsbereich")]
    [Tooltip("wirkt auf alle Gegner")]
    public bool affectNeutral = false;
}
