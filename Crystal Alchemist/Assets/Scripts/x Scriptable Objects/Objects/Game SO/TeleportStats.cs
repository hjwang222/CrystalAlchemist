using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player/Teleport Stats")]
public class TeleportStats : ScriptableObject, ISerializationCallbackReceiver
{
    public string teleportName = "";
    public string scene;
    public Vector2 position;
    public bool showAnimationIn = false;
    public bool showAnimationOut = false;

    public void Clear()
    {
        this.teleportName = "";
        this.scene = "Haus";
        this.position = Vector2.zero;
        this.showAnimationIn = true;
        this.showAnimationOut = false;
    }

    public TeleportStats(string targetScene, Vector2 position)
    {
        SetValue(targetScene, position, true, true);
    }

    public void SetValue(string targetScene, Vector2 position, bool showIn, bool showOut)
    {
        this.scene = targetScene;
        this.position = position;
        this.showAnimationIn = showIn;
        this.showAnimationOut = showOut;
    }

    public void SetValue(string targetScene, Vector2 position)
    {
        SetValue(targetScene, position, true, false);
    }

    public void SetValue(TeleportStats stats)
    {
        SetValue(stats.scene, stats.position, true, false);
    }

    public void SetValue(TeleportStats stats, bool showIn, bool showOut)
    {
        SetValue(stats.scene, stats.position, showIn, showOut);
    }

    public void OnAfterDeserialize()
    {
        position = Vector2.zero;
    }

    public void OnBeforeSerialize() { }
}
