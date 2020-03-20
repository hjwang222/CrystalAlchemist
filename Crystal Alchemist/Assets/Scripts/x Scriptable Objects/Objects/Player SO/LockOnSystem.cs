using UnityEngine;
using Sirenix.OdinInspector;

public enum TargetingMode
{
    manual,
    auto
}

[CreateAssetMenu(menuName = "Game/Player/Targeting System")]
public class LockOnSystem : ScriptableObject
{
    [BoxGroup("Zielerfassung")]
    [Tooltip("Manual = Spieler kann Ziele auswählen, Single = näheste Ziel, Multi = Alle in Range, Auto = Sofort ohne Bestätigung")]
    [EnumToggleButtons]
    public TargetingMode targetingMode = TargetingMode.manual;

    [BoxGroup("Zielerfassung")]
    public int maxAmountOfTargets = 1;

    [BoxGroup("Zielerfassung")]
    public float targetingDuration = 6f;

    [BoxGroup("Zielerfassung")]
    [Tooltip("In welchen Intervallen die Ziele getroffen werden sollen")]
    [Range(0, 10)]
    public float multiHitDelay = 0;

    [BoxGroup("Zielerfassung")]
    [Tooltip("Soll die Reichweite bei der Zielerfassung angezeigt werden")]
    public bool showRange = false;

    [BoxGroup("Zielerfassung")]
    [Tooltip("Soll die Reichweite bei der Zielerfassung angezeigt werden")]
    public bool showIndicator = false;
}
