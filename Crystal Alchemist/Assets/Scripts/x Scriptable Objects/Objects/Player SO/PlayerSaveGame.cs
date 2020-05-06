using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/Player/Save Game")]
public class PlayerSaveGame : ScriptableObject
{
    [BoxGroup("Base")]
    public float timePlayed;

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
    [BoxGroup("Player")]
    public TeleportStats nextTeleport;

    [BoxGroup("Stats")]
    [SerializeField]
    private CharacterStats stats;

    [BoxGroup("Stats")]
    [SerializeField]
    private CharacterPreset defaultPreset;

    [Button]
    public void Clear()
    {        
        this.timePlayed = 0f;

        this.stats.SetCharacterName("Hero");
        this.playerValue.Clear(this.stats);
        this.inventory.Clear();
        this.buttons.Clear();
        this.skillSet.Clear();
        this.nextTeleport.Clear();

        GameUtil.setPreset(this.defaultPreset, this.playerPreset);
    }

    public void SetCharacterName(string characterName) => this.stats.SetCharacterName(characterName);

    public string GetCharacterName() { return this.stats.GetCharacterName(); }
}
