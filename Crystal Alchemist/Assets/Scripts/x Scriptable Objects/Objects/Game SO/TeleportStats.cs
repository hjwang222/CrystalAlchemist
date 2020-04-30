using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player/Teleport Stats")]
public class TeleportStats : ScriptableObject, ISerializationCallbackReceiver
{
    public string teleportName = "";
    public string scene;
    public Vector2 position;
    public bool lastTeleportSet = false;
    public bool showAnimationIn = false;
    public bool showAnimationOut = false;

    public TeleportStats(string targetScene, Vector2 position)
    {
        SetValue(targetScene, position, true, true);
    }

    public void SetValue(string targetScene, Vector2 position, bool showIn, bool showOut)
    {
        this.scene = targetScene;
        this.position = position;
        this.showAnimationOut = showIn;
        this.showAnimationIn = showOut;
    }

    public void OnAfterDeserialize()
    {
        position = Vector2.zero;
    }

    public void OnBeforeSerialize() { }
}
