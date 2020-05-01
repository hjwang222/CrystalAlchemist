using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player/Save Game")]
public class PlayerSaveGame : ScriptableObject
{
    public string slot;
    public string currentScene;
    public float timePlayed;
    public CharacterPreset preset;
    public Vector2 position;
}
