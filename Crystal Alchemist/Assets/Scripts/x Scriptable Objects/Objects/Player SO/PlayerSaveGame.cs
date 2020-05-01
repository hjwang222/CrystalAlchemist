using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/Player/Save Game")]
public class PlayerSaveGame : ScriptableObject
{
    [BoxGroup("Base")]
    public string currentScene;
    [BoxGroup("Base")]
    public float timePlayed;
    [BoxGroup("Base")]
    public string characterName;
    [BoxGroup("Base")]
    public Vector2 position;

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

    [BoxGroup("Stats")]
    [SerializeField]
    private CharacterStats stats;

    [BoxGroup("Stats")]
    [SerializeField]
    private CharacterPreset defaultPreset;

    [Button]
    public void Clear()
    {
        this.currentScene = "";
        this.timePlayed = 0f;
        this.characterName = "";
        this.position = Vector2.zero;

        this.playerValue.Clear(this.stats);
        this.inventory.Clear();
        this.buttons.Clear();
        this.skillSet.Clear();

        GameUtil.setPreset(this.defaultPreset, this.playerPreset);
    }
}
