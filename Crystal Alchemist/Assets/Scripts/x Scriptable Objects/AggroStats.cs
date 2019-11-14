using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Stats/AggroStats")]
public class AggroStats : ScriptableObject
{
    [FoldoutGroup("Aggro Attributes", expanded: false)]
    public bool firstHitMaxAggro = true;


    [FoldoutGroup("Aggro Attributes", expanded: false)]
    [Range(0, 120)]
    public float aggroIncreaseFactor = 25;

    [FoldoutGroup("Aggro Attributes", expanded: false)]
    [Range(0, 120)]
    public float aggroOnHitIncreaseFactor = 25;

    [FoldoutGroup("Aggro Attributes", expanded: false)]
    [Range(-120, 0)]
    public float aggroDecreaseFactor = -25;

    [FoldoutGroup("Aggro Attributes", expanded: false)]
    public float aggroNeededToTarget = 100;

    [FoldoutGroup("Aggro Attributes", expanded: false)]
    public float targetChangeDelay = 0f;


    [FoldoutGroup("Aggro Object Attributes", expanded: false)]
    public GameObject targetFoundClue;

    [FoldoutGroup("Aggro Object Attributes", expanded: false)]
    public float foundClueDuration = 1;

    [FoldoutGroup("Aggro Object Attributes", expanded: false)]
    public GameObject targetActiveClue;

    [FoldoutGroup("Aggro Object Attributes", expanded: false)]
    public float activeClueDuration = 1;


    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Spieler")]
    public bool affectOther = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Gegner")]
    public bool affectSame = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Gegner")]
    public bool affectNeutral = false;
}
