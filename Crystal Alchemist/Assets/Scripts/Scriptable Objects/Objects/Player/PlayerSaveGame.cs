using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/Player/Save Game")]
public class PlayerSaveGame : ScriptableObject
{
    [BoxGroup("Time")]
    public FloatValue timePlayed;
    [BoxGroup("Time")]
    public TimeValue time;

    [BoxGroup("Player")]
    public StringValue characterName;
    [BoxGroup("Player")]
    public CharacterPreset playerPreset;
    [BoxGroup("Player")]
    public CharacterValues playerValue;
    [BoxGroup("Player")]
    public PlayerInventory inventory;
    [BoxGroup("Player")]
    public PlayerButtons buttons;
    [BoxGroup("Player")]
    public PlayerSkillset skillSet;

    [BoxGroup("Teleport")]
    public TeleportStats startSpawnPoint;
    [BoxGroup("Teleport")]
    public TeleportStats lastTeleport;
    [BoxGroup("Teleport")]
    public PlayerTeleportList teleportList;

    [BoxGroup("Stats")]
    [SerializeField]
    private CharacterStats stats;

    [BoxGroup("Stats")]
    [SerializeField]
    private CharacterPreset defaultPreset;

    [Button]
    public void Clear()
    {
        this.time.Clear();
        this.timePlayed.setValue(0f);
        this.characterName.SetValue("Hero");
        this.playerValue.Clear(this.stats);
        this.inventory.Clear();
        this.buttons.Clear();
        this.skillSet.Clear();
        this.startSpawnPoint.Clear();
        this.lastTeleport.Clear();

        GameUtil.setPreset(this.defaultPreset, this.playerPreset);
    }

    public void SetCharacterName(string characterName) => this.characterName.SetValue(characterName);

    public string GetCharacterName() { return this.characterName.getValue(); }
}
